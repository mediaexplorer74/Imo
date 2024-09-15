// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.Managers.NotificationsManager
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using Coding4Fun.Toolkit.Controls;
using ImoSilverlightApp.Connection;
using ImoSilverlightApp.Helpers;
using ImoSilverlightApp.Storage.Models;
using NLog;
using System;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Media;


namespace ImoSilverlightApp.Managers
{
  internal class NotificationsManager
  {
    private static readonly Logger log = LogManager.GetLogger(typeof (NotificationsManager).Name);
    private bool showDisconnected;

    public event EventHandler RequestedPlaySound;

    public NotificationsManager()
    {
      IMO.ConversationsManager.PropertyChanged += new PropertyChangedEventHandler(this.ConversationsManager_PropertyChanged);
      IMO.ConversationsManager.IMMessageReceived += new EventHandler<EventArg<Message>>(this.ConversationsManager_IMMessageReceived);
      IMO.Network.Connected += new EventHandler<EventArg<ConnectionData>>(this.Network_Connected);
      IMO.Network.Disconnected += new EventHandler<EventArgs>(this.Network_Disconnected);
    }

    private void showDisconnectedTimer_Tick(object sender, EventArgs e)
    {
      this.UpdateTrayIcon(IMO.ConversationsManager.UnreadConversationsCount);
    }

    private void Network_Connected(object sender, EventArg<ConnectionData> e)
    {
      this.showDisconnected = false;
      this.UpdateTrayIcon(IMO.ConversationsManager.UnreadConversationsCount);
    }

    private void Network_Disconnected(object sender, EventArgs e)
    {
      this.showDisconnected = true;
      Utils.DelayExecute(1500, (Action) (() => this.UpdateTrayIcon(IMO.ConversationsManager.UnreadConversationsCount)));
    }

    private void OnRequestedPlaySound()
    {
      if (this.RequestedPlaySound == null)
        return;
      this.RequestedPlaySound((object) this, EventArgs.Empty);
    }

    private void ConversationsManager_IMMessageReceived(object sender, EventArg<Message> e)
    {
      IMO.ConversationsManager.GetOrCreateConversation(e.Arg.ConvBuid);
      this.OnRequestedPlaySound();
      int num = IMO.ApplicationSettings.ShowNotifications ? 1 : 0;
    }

    public void ShowMessageNotification(string buid, string alias, string message)
    {
      try
      {
        string str1 = alias;
        string str2 = message;
        ToastPrompt toastPrompt = new ToastPrompt();
        toastPrompt.Title = str1;
        toastPrompt.Message = str2;
        toastPrompt.Background = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 27, (byte) 161, (byte) 226));
        toastPrompt.Tap += (EventHandler<GestureEventArgs>) ((s, e) => IMO.NavigationManager.NavigateToConversation(buid));
        toastPrompt.Show();
      }
      catch (Exception ex)
      {
        NotificationsManager.log.Error(ex, 103, nameof (ShowMessageNotification));
      }
    }

    private void ConversationsManager_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      if (!(e.PropertyName == "UnreadConversationsCount"))
        return;
      int conversationsCount = IMO.ConversationsManager.UnreadConversationsCount;
      this.UpdateTrayIcon(conversationsCount);
      this.SetTaskbarNotifications(conversationsCount);
    }

    public void UpdateTrayIcon(int unreadConversationsCount = 0)
    {
    }

    public void SetTaskbarNotifications(int notificationsCount)
    {
    }
  }
}
