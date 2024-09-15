// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.MessageFactory
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Helpers;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;


namespace ImoSilverlightApp
{
  public class MessageFactory
  {
    private static readonly string TAG = nameof (MessageFactory);

    public static JObject CreateMessage(
      string method,
      JObject data,
      string to,
      Action<JToken> callback,
      Action onDispatcherAckCallback)
    {
      string ssid = IMO.Dispatcher.GetSSID();
      JObject jobject = new JObject();
      jobject.Add(nameof (method), (JToken) method);
      jobject.Add(nameof (data), (JToken) data);
      if (callback != null)
      {
        string str = IMO.Dispatcher.StoreCallback(callback);
        jobject.Add("request_id", (JToken) str);
      }
      if (onDispatcherAckCallback != null)
        IMO.Dispatcher.StoreAckCallback(onDispatcherAckCallback);
      return new JObject()
      {
        {
          "from",
          (JToken) new JObject()
          {
            {
              "system",
              (JToken) "client"
            },
            {
              "ssid",
              (JToken) ssid
            }
          }
        },
        {
          nameof (to),
          (JToken) new JObject()
          {
            {
              "system",
              (JToken) to
            }
          }
        },
        {
          nameof (data),
          (JToken) jobject
        }
      };
    }

    public static JObject CreateNameChannelMessage()
    {
      string str1 = Utils.EncryptSecretKey(Sym.GetSecretKey());
      string str2 = Utils.GetRandomString(5) + ".0";
      JObject jobject1 = new JObject();
      jobject1.Add("imo-api-level", (JToken) 0);
      jobject1.Add("ua", (JToken) UserAgentHelper.GetUA());
      jobject1.Add("compression", (JToken) JArray.Parse("['zlib']"));
      jobject1.Add("no_b64", (JToken) true);
      jobject1.Add("key2", (JToken) str1);
      JObject jobject2 = new JObject();
      jobject2.Add("name", (JToken) str2);
      JObject nameChannelMessage = new JObject();
      nameChannelMessage.Add("method", (JToken) "name_channel");
      nameChannelMessage.Add("headers", (JToken) jobject1);
      nameChannelMessage.Add("data", (JToken) jobject2);
      string randomPadding = IMO.ImoDNSManager.GetRandomPadding();
      if (randomPadding != null)
        nameChannelMessage.Add("padding", (JToken) randomPadding);
      return nameChannelMessage;
    }

    internal static JToken CreateFirstMessageHeaders()
    {
      JObject jobject = new JObject();
      jobject.Add("UDID", (JToken) IMO.ApplicationSettings.Udid);
      jobject.Add("iat", (JToken) IMO.ApplicationSettings.Cookie);
      JObject firstMessageHeaders = new JObject();
      firstMessageHeaders.Add("route_num", (JToken) IMO.Network.GetRouteNumber());
      firstMessageHeaders.Add("c", (JToken) jobject);
      firstMessageHeaders.Add("ua", (JToken) UserAgentHelper.GetUA());
      if (IMO.ApplicationSettings.CountryCode != null)
        firstMessageHeaders.Add("sim_iso", (JToken) IMO.ApplicationSettings.CountryCode);
      string networkTypeAndSubtype = Utils.GetNetworkTypeAndSubtype();
      firstMessageHeaders.Add("connection_type", (JToken) (networkTypeAndSubtype ?? "null"));
      return (JToken) firstMessageHeaders;
    }

    public static void AddSsidUidProto(Dictionary<string, object> data)
    {
      data.Add("ssid", (object) IMO.Dispatcher.GetSSID());
      data.Add("uid", (object) IMO.User.Uid);
      data.Add("proto", (object) "imo");
    }
  }
}
