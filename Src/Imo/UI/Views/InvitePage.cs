// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.UI.Views.InvitePage
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Helpers;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;


namespace ImoSilverlightApp.UI.Views
{
  public class InvitePage : ImoPage
  {
    internal Microsoft.Phone.Shell.ApplicationBar appBar;
    internal Grid LayoutRoot;
    internal ContactSelectorView selectorControl;
    private bool _contentLoaded;

    public InvitePage()
    {
      this.InitializeComponent();
      this.DataContext = (object) (this.ViewModel = new InvitePageViewModel((FrameworkElement) this));
      this.selectorControl.ViewModel.SelectedItems.CollectionChanged += new NotifyCollectionChangedEventHandler(this.SelectedItems_CollectionChanged);
    }

    private void SelectedItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      this.ViewModel.IsInviteEnabled = this.selectorControl.ViewModel.SelectedItems.Count > 0;
    }

    private InvitePageViewModel ViewModel { get; set; }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
      IMO.ApplicationSettings.LastInvitePageShown = Utils.GetTimestamp();
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
    }

    private void Cancel_Click(object sender, EventArgs e)
    {
      IMO.NavigationManager.NavigateBackOrExit();
    }

    private async void Invite_Click(object sender, EventArgs e)
    {
      this.LogInvites(this.selectorControl.ViewModel.SelectedItems.Count);
      await IMO.PhonebookManager.SendSms((IList<string>) this.selectorControl.ViewModel.SelectedItems.Select<SelectorItemBase, string>((Func<SelectorItemBase, string>) (x => (x as SelectorPhonebookContactItem).PhoneNumber)).ToList<string>());
      IMO.NavigationManager.NavigateToHome();
    }

    private void LogInvites(int count)
    {
      IMO.MonitorLog.Log("invites", new JObject()
      {
        {
          "invite_click",
          (JToken) 1
        },
        {
          "invite_click_count",
          (JToken) count
        }
      });
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/ImoSilverlightApp;component/UI/Views/InvitePage.xaml", UriKind.Relative));
      this.appBar = (Microsoft.Phone.Shell.ApplicationBar) this.FindName("appBar");
      this.LayoutRoot = (Grid) this.FindName("LayoutRoot");
      this.selectorControl = (ContactSelectorView) this.FindName("selectorControl");
    }
  }
}
