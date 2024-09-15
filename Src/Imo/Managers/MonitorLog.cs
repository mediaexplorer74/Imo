// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.Managers.MonitorLog
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;


namespace ImoSilverlightApp.Managers
{
  internal class MonitorLog : BaseManager
  {
    public void Log(string nnamespace, string eevent, Action ackCallback = null)
    {
      this.Log(nnamespace, eevent, (object) 1, ackCallback);
    }

    public void Log(string nnamespace, string key, object value, Action ackCallback = null)
    {
      this.Log(nnamespace, new JObject()
      {
        {
          key,
          JToken.FromObject(value)
        }
      }, ackCallback);
    }

    public void Log(
      string nnamespace,
      JObject eventsMap,
      Action ackCallback = null,
      bool addNamespacePrefix = true)
    {
      BaseManager.Send("monitor", "log_event", new Dictionary<string, object>()
      {
        {
          "events",
          (object) new JArray()
          {
            (JToken) new JObject()
            {
              {
                "namespace",
                (JToken) ((addNamespacePrefix ? "winphone_" : "") + nnamespace)
              },
              {
                "data",
                (JToken) eventsMap
              }
            }
          }
        },
        {
          "ssid",
          (object) IMO.Dispatcher.GetSSID()
        }
      }, onDispatcherAckCallback: ackCallback);
    }

    internal void LogSignOff(string reason, Action ackCallback)
    {
      this.Log("account", new JObject()
      {
        {
          "sign_off",
          (JToken) 1
        },
        {
          "sign_off_" + reason,
          (JToken) 1
        }
      }, ackCallback);
    }

    internal void LogWithSource(string nnamespace, string eevent, string source)
    {
      this.Log(nnamespace, new JObject()
      {
        {
          eevent,
          (JToken) 1
        },
        {
          eevent + "_" + source,
          (JToken) 1
        }
      });
    }

    internal void LogQuit(string reason, Action ackCallback)
    {
      this.Log("application", new JObject()
      {
        {
          "quit",
          (JToken) 1
        },
        {
          "quit_" + reason,
          (JToken) 1
        }
      }, ackCallback);
    }

    internal void LogFail(string nnamespace, string reason)
    {
      this.Log(nnamespace, new JObject()
      {
        {
          "fail",
          (JToken) 1
        },
        {
          "fail_reason",
          (JToken) reason
        }
      });
    }

    internal void LogWithReason(string nnamespace, string eevent, string reason)
    {
      this.Log(nnamespace, new JObject()
      {
        {
          eevent,
          (JToken) 1
        },
        {
          eevent + "_" + reason,
          (JToken) 1
        }
      });
    }
  }
}
