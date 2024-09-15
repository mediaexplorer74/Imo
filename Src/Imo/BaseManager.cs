// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.BaseManager
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;


namespace ImoSilverlightApp
{
  public class BaseManager
  {
    protected static void Send(
      string to,
      string methodName,
      Dictionary<string, object> data = null,
      Action<JToken> successCallback = null,
      Action onDispatcherAckCallback = null)
    {
      BaseManager.Send(to, methodName, JObject.FromObject((object) data), successCallback, onDispatcherAckCallback);
    }

    protected static void Send(
      string to,
      string methodName,
      JObject data = null,
      Action<JToken> successCallback = null,
      Action onDispatcherAckCallback = null)
    {
      IMO.Dispatcher.EnqueueOutgoingMessage(MessageFactory.CreateMessage(methodName, data, to, successCallback, onDispatcherAckCallback));
    }
  }
}
