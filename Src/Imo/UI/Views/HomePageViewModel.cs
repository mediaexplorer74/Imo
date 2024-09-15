// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.UI.Views.HomePageViewModel
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Managers;
using ImoSilverlightApp.Storage.Models;
using System.ComponentModel;
using System.Windows;


namespace ImoSilverlightApp.UI.Views
{
  public class HomePageViewModel : ViewModelBase
  {
    public HomePageViewModel(FrameworkElement element)
      : base(element)
    {
    }

    public User User => IMO.User;

    public bool ShowInCallItem => IMO.AVManager.IsInCall;

    protected override void OnLoaded(object sender, RoutedEventArgs e)
    {
      base.OnLoaded(sender, e);
      IMO.AVManager.PropertyChanged += new PropertyChangedEventHandler(this.OnCallStateChanged);
      this.OnCallStateChanged((object) null, new PropertyChangedEventArgs("CallController"));
      IMO.ConversationsManager.PropertyChanged += new PropertyChangedEventHandler(this.ConversationsManager_PropertyChanged);
      this.OnPropertyChanged("UnreadConversationsCountUI");
    }

    protected override void OnUnloaded(object sender, RoutedEventArgs e)
    {
      IMO.AVManager.PropertyChanged -= new PropertyChangedEventHandler(this.OnCallStateChanged);
      IMO.ConversationsManager.PropertyChanged -= new PropertyChangedEventHandler(this.ConversationsManager_PropertyChanged);
      base.OnUnloaded(sender, e);
    }

    private void OnCallStateChanged(object sender, PropertyChangedEventArgs e)
    {
      if (!(e.PropertyName == "CallController"))
        return;
      this.OnPropertyChanged("ShowInCallItem");
    }

    public ConversationsManager ConversationsManager => IMO.ConversationsManager;

    private void ConversationsManager_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      if (!(e.PropertyName == "UnreadConversationsCount"))
        return;
      this.OnPropertyChanged("HasUnreadConversations");
      if (IMO.ConversationsManager.UnreadConversationsCount <= 0)
        return;
      this.OnPropertyChanged("UnreadConversationsCountUI");
    }

    public string UnreadConversationsCountUI
    {
      get => IMO.ConversationsManager.UnreadConversationsCount.ToString();
    }
  }
}
