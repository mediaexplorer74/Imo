// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.Session
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Helpers;
using System;
using System.Collections.Generic;
using System.Windows;


namespace ImoSilverlightApp
{
  internal class Session : BaseManager
  {
    private DateTime lastSentUserActivity = DateTime.MinValue;
    private DateTime lastUserActivity = DateTime.MinValue;

    public void CookieLogin(string reason, bool isActive = false)
    {
      Dictionary<string, object> data = new Dictionary<string, object>();
      data.Add(nameof (reason), (object) reason);
      data.Add("timestamp", (object) Utils.GetTimestamp());
      data.Add("ssid", (object) Dispatcher.Instance.GetSSID());
      data.Add("last_unread_ts_nano", (object) -1);
      if (isActive)
        data.Add("active", (object) true);
      else
        data.Add("active", (object) ((App) Application.Current).IsActive);
      data.Add("lang", (object) "en-US");
      if (IMO.ApplicationSettings.CookieUid != null)
        data.Add("uid", (object) IMO.ApplicationSettings.CookieUid);
      BaseManager.Send("session", "cookie_login", data);
    }

    public void SetSessionActivity(bool active)
    {
      BaseManager.Send("session", "set_session_activity", new Dictionary<string, object>()
      {
        {
          "ssid",
          (object) Dispatcher.Instance.GetSSID()
        },
        {
          nameof (active),
          (object) true
        },
        {
          "lang",
          (object) "en-US"
        }
      });
    }

    public void SendKeepALive()
    {
      BaseManager.Send("dispatcher", "keep_alive", new Dictionary<string, object>()
      {
        {
          "ssid",
          (object) Dispatcher.Instance.GetSSID()
        }
      });
    }

    public void UpdateLastUserActivity() => this.lastUserActivity = DateTime.Now;

    public void NotifyUserIsActive()
    {
      if ((DateTime.Now - this.lastSentUserActivity).TotalSeconds < 59.0 || (DateTime.Now - this.lastUserActivity).TotalSeconds > 60.0)
        return;
      BaseManager.Send("session", "observed_user_activity", new Dictionary<string, object>()
      {
        {
          "ssid",
          (object) IMO.Dispatcher.GetSSID()
        }
      });
      this.lastSentUserActivity = DateTime.Now;
    }
  }
}
