// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.Managers.FeedbackManager
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Helpers;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;


namespace ImoSilverlightApp.Managers
{
  internal class FeedbackManager : BaseManager
  {
    public void SendFeedback(string name, string email, string message, Action<JToken> callback)
    {
      Dictionary<string, object> data = new Dictionary<string, object>();
      string ssid = IMO.Dispatcher.GetSSID();
      data.Add("ssid", (object) ssid);
      data.Add("email_address", (object) email);
      data.Add(nameof (message), (object) message);
      data.Add("metadata", (object) new Dictionary<string, object>()
      {
        {
          "lang",
          (object) CultureInfo.CurrentCulture.DisplayName
        },
        {
          "location",
          (object) RegionInfo.CurrentRegion.DisplayName
        },
        {
          "contact_name",
          (object) name
        },
        {
          "ssid",
          (object) ssid
        },
        {
          "ua",
          (object) UserAgentHelper.GetUA()
        }
      });
      BaseManager.Send("feedback", "send_feedback", data, callback);
    }
  }
}
