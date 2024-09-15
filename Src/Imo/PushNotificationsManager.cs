// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.PushNotificationsManager
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Helpers;
using ImoSilverlightApp.Storage.Models;
using Microsoft.Phone.Notification;
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Windows.Input;


namespace ImoSilverlightApp
{
  public class PushNotificationsManager : BaseManager
  {
    private static readonly Logger log = LogManager.GetLogger(typeof (PushNotificationsManager).Name);
    private HttpNotificationChannel httpChannel;

    public void HandleAppActivated()
    {
      if (IMO.ApplicationProperties.IsSignedOn)
        this.InitPushChannel();
      else
        IMO.AccountManager.SignedOn += new EventHandler<EventArg<SignOnData>>(this.AccountManager_SignedOn);
    }

    private void AccountManager_SignedOn(object sender, EventArg<SignOnData> e)
    {
      IMO.AccountManager.SignedOn -= new EventHandler<EventArg<SignOnData>>(this.AccountManager_SignedOn);
      this.InitPushChannel();
    }

    private void InitPushChannel()
    {
      try
      {
        bool flag = false;
        if (this.httpChannel == null)
        {
          this.httpChannel = HttpNotificationChannel.Find("ImoSilverlightApp.PUSH_AND_VOIP");
          if (this.httpChannel == null)
          {
            this.httpChannel = new HttpNotificationChannel("ImoSilverlightApp.PUSH_AND_VOIP");
            flag = true;
          }
          else
            PushNotificationsManager.SendNotificationChannel(this.httpChannel.ChannelUri.AbsoluteUri, IMO.ApplicationSettings.Udid, IMO.Dispatcher.GetSSID(), successCallback: (Action<JToken>) (t =>
            {
              IMO.ApplicationSettings.NotificationChannel = this.httpChannel.ChannelUri.AbsoluteUri;
              IMO.MonitorLog.Log("set_push_channel", "success");
            }));
        }
        this.httpChannel.ChannelUriUpdated += new EventHandler<NotificationChannelUriEventArgs>(this.PushChannel_ChannelUriUpdated);
        this.httpChannel.ErrorOccurred += new EventHandler<NotificationChannelErrorEventArgs>(this.PushChannel_ErrorOccurred);
        this.httpChannel.HttpNotificationReceived += new EventHandler<HttpNotificationEventArgs>(this.PushChannel_HttpNotificationReceived);
        this.httpChannel.ShellToastNotificationReceived += new EventHandler<NotificationEventArgs>(this.HttpChannel_ShellToastNotificationReceived);
        if (!flag)
          return;
        this.httpChannel.Open();
      }
      catch (Exception ex)
      {
        PushNotificationsManager.log.Error(ex, 79, nameof (InitPushChannel));
      }
    }

    public void HandleAppDeactivated()
    {
      if (this.httpChannel != null)
      {
        this.httpChannel.ChannelUriUpdated -= new EventHandler<NotificationChannelUriEventArgs>(this.PushChannel_ChannelUriUpdated);
        this.httpChannel.ErrorOccurred -= new EventHandler<NotificationChannelErrorEventArgs>(this.PushChannel_ErrorOccurred);
        this.httpChannel.HttpNotificationReceived -= new EventHandler<HttpNotificationEventArgs>(this.PushChannel_HttpNotificationReceived);
        this.httpChannel.ShellToastNotificationReceived -= new EventHandler<NotificationEventArgs>(this.HttpChannel_ShellToastNotificationReceived);
      }
      IMO.AccountManager.SignedOn -= new EventHandler<EventArg<SignOnData>>(this.AccountManager_SignedOn);
    }

    private void PushChannel_ChannelUriUpdated(object sender, NotificationChannelUriEventArgs e)
    {
      PushNotificationsManager.SendNotificationChannel(e.ChannelUri.AbsoluteUri, IMO.ApplicationSettings.Udid, IMO.Dispatcher.GetSSID(), successCallback: (Action<JToken>) (t =>
      {
        IMO.ApplicationSettings.NotificationChannel = e.ChannelUri.AbsoluteUri;
        IMO.MonitorLog.Log("set_push_channel", "success");
      }));
    }

    private void PushChannel_ErrorOccurred(object sender, NotificationChannelErrorEventArgs e)
    {
      PushNotificationsManager.log.Error("Error sending push channel " + (object) e.ErrorType + ": " + e.Message + " Code:" + (object) e.ErrorCode + "," + (object) e.ErrorAdditionalData, 107, nameof (PushChannel_ErrorOccurred));
      IMO.MonitorLog.Log("set_push_channel", "fail");
    }

    private void PushChannel_HttpNotificationReceived(object sender, HttpNotificationEventArgs e)
    {
    }

    private void HttpChannel_ShellToastNotificationReceived(object sender, NotificationEventArgs e)
    {
      Utils.BeginInvokeOnUI((Action) (() =>
      {
        if (e.Collection == null || !e.Collection.ContainsKey("wp:Param") || !e.Collection.ContainsKey("wp:Text1") || !e.Collection.ContainsKey("wp:Text2"))
          return;
        string str = e.Collection["wp:Param"];
        int num = str.IndexOf("buid=");
        if (num == -1)
          return;
        string buid = str.Substring(num + "buid=".Length);
        if (string.IsNullOrEmpty(buid))
          return;
        Conversation conversation = IMO.ConversationsManager.GetOrCreateConversation(buid);
        if (IMO.ConversationsManager.CurrentOpenConversation == conversation || conversation.Contact.IsMuted)
          return;
        string alias = e.Collection["wp:Text1"];
        string message = e.Collection["wp:Text2"];
        IMO.NotificationsManager.ShowMessageNotification(buid, alias, message);
      }));
    }

    private void Toast_Tap(object sender, GestureEventArgs e)
    {
      throw new NotImplementedException();
    }

    public static void SendNotificationChannel(
      string channel,
      string udid,
      string ssid,
      bool isVoip = false,
      Action<JToken> successCallback = null)
    {
      BaseManager.Send("windows", "set_push_channel", new JObject()
      {
        [nameof (ssid)] = (JToken) ssid,
        [nameof (udid)] = (JToken) udid,
        [nameof (channel)] = (JToken) channel,
        ["is_voip"] = (JToken) isVoip
      }, successCallback);
    }
  }
}
