// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.UI.Views.HomePage
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Helpers;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Networking.Voip;
using Microsoft.Phone.Scheduler;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;


namespace ImoSilverlightApp.UI.Views
{
  public class HomePage : ImoPage
  {
    private static readonly string INCOMING_CALL_TASK_NAME = "ImoSilverlightApp.IncomingCallTask";
    private VoipHttpIncomingCallTask incomingCallTask;
    private const string keepAliveTaskName = "ImoSilverlightApp.KeepAliveTask";
    private VoipKeepAliveTask keepAliveTask;
    internal Microsoft.Phone.Shell.ApplicationBar appBar;
    internal Grid LayoutRoot;
    internal Pivot pivot;
    internal PivotItem contactsPivotItem;
    internal PivotItem chatsPivotItem;
    private bool _contentLoaded;

    public HomePage()
    {
      this.InitializeComponent();
      this.Loaded += new RoutedEventHandler(this.HomePage_Loaded);
      this.DataContext = (object) new HomePageViewModel((FrameworkElement) this);
    }

    private void HomePage_Loaded(object sender, RoutedEventArgs e)
    {
      if (ImoPage.DisableAnimations)
      {
        ImoPage.DisableAnimations = false;
        this.EnableAnimations();
      }
      try
      {
        this.InitHttpNotificationTask();
      }
      catch (Exception ex)
      {
      }
      try
      {
        this.InitKeepAliveTask();
      }
      catch (Exception ex)
      {
      }
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
      base.OnNavigatedTo(e);
      if (!IMO.ApplicationSettings.HasAcceptedGdpr && IMO.ApplicationSettings.IsEuCountry())
      {
        IMO.NavigationManager.NavigateToGDPRAccept();
      }
      else
      {
        while (this.NavigationService.CanGoBack)
          this.NavigationService.RemoveBackEntry();
        if (IMO.ApplicationSettings.IsChatsTabActive)
          this.pivot.SelectedItem = (object) this.chatsPivotItem;
        this.MaybeShowInvitePopup();
      }
    }

    private async void MaybeShowInvitePopup()
    {
      long timestamp = Utils.GetTimestamp();
      long lastInvitePageShown = IMO.ApplicationSettings.LastInvitePageShown;
      if (TimeSpan.FromMilliseconds((double) (timestamp - lastInvitePageShown)).TotalDays < 2.0)
        return;
      long invitePopupShown = IMO.ApplicationSettings.LastInvitePopupShown;
      if (TimeSpan.FromMilliseconds((double) (timestamp - invitePopupShown)).TotalDays <= 6.0)
        return;
      IMO.ApplicationSettings.LastInvitePopupShown = timestamp;
      if (await ImoMessageBox.Show("imo is more fun with friends. Would you like to invite them now?", ImoMessageBoxButton.OKCancel) != ImoMessageBoxResult.OK)
        return;
      IMO.NavigationManager.NavigateToInvitePage();
    }

    public void InitHttpNotificationTask()
    {
      if (this.incomingCallTask != null && this.incomingCallTask.IsScheduled)
        return;
      this.incomingCallTask = ScheduledActionService.Find(HomePage.INCOMING_CALL_TASK_NAME) as VoipHttpIncomingCallTask;
      if (this.incomingCallTask != null)
      {
        if (this.incomingCallTask.IsScheduled)
          return;
        ScheduledActionService.Remove(HomePage.INCOMING_CALL_TASK_NAME);
      }
      this.incomingCallTask = new VoipHttpIncomingCallTask(HomePage.INCOMING_CALL_TASK_NAME, "ImoSilverlightApp.PUSH_AND_VOIP");
      this.incomingCallTask.Description = "Incoming call task";
      ScheduledActionService.Add((ScheduledAction) this.incomingCallTask);
    }

    public void InitKeepAliveTask()
    {
      if (this.keepAliveTask != null && this.keepAliveTask.IsScheduled)
        return;
      this.keepAliveTask = ScheduledActionService.Find("ImoSilverlightApp.KeepAliveTask") as VoipKeepAliveTask;
      if (this.keepAliveTask != null)
      {
        if (this.keepAliveTask.IsScheduled)
          return;
        ScheduledActionService.Remove("ImoSilverlightApp.KeepAliveTask");
      }
      this.keepAliveTask = new VoipKeepAliveTask("ImoSilverlightApp.KeepAliveTask");
      this.keepAliveTask.Interval = new TimeSpan(12, 0, 0);
      this.keepAliveTask.Description = "keep-alive task";
      ScheduledActionService.Add((ScheduledAction) this.keepAliveTask);
    }

    private void Settings_Click(object sender, EventArgs e)
    {
      IMO.NavigationManager.NavigateToProfileSettings();
    }

    private void Search_Click(object sender, EventArgs e)
    {
      this.NavigationService.Navigate(new Uri("/UI/Views/SearchContactsPage.xaml", UriKind.Relative));
    }

    private void InCallItem_Click(object sender, EventArgs e)
    {
      IMO.NavigationManager.NavigateToAVCallPage();
    }

    private void VideoCapture_Click(object sender, EventArgs e)
    {
      this.NavigationService.Navigate(new Uri("/UI/Views/VideoCapturePage.xaml", UriKind.Relative));
    }

    private void MarkAllAsRead_Click(object sender, EventArgs e)
    {
      IMO.ConversationsManager.MarkAllAsRead();
    }

    private void pivotSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      IMO.ApplicationSettings.IsChatsTabActive = e.AddedItems[0] as PivotItem == this.chatsPivotItem;
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/ImoSilverlightApp;component/UI/Views/HomePage.xaml", UriKind.Relative));
      this.appBar = (Microsoft.Phone.Shell.ApplicationBar) this.FindName("appBar");
      this.LayoutRoot = (Grid) this.FindName("LayoutRoot");
      this.pivot = (Pivot) this.FindName("pivot");
      this.contactsPivotItem = (PivotItem) this.FindName("contactsPivotItem");
      this.chatsPivotItem = (PivotItem) this.FindName("chatsPivotItem");
    }
  }
}
