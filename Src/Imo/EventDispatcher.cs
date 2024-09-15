// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.EventDispatcher
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Collections;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;


namespace ImoSilverlightApp
{
  internal class EventDispatcher
  {
    private static EventDispatcher instance;
    private Queue<TimestampedObject<JObject>> processedMessages;
    private const int SAVED_MESSAGES_COUNT = 100;

    private EventDispatcher() => this.processedMessages = new Queue<TimestampedObject<JObject>>();

    public static EventDispatcher Instance
    {
      get
      {
        if (EventDispatcher.instance == null)
          EventDispatcher.instance = new EventDispatcher();
        return EventDispatcher.instance;
      }
    }

    public IEnumerable<TimestampedObject<JObject>> GetProcessedMessages()
    {
      return (IEnumerable<TimestampedObject<JObject>>) this.processedMessages.ToList<TimestampedObject<JObject>>();
    }

    public event EventHandler<EventArg<TimestampedObject<JObject>>> IncommingMessage;

    private void OnProcessMessage(JObject message)
    {
      TimestampedObject<JObject> timestampedObject = new TimestampedObject<JObject>(message);
      this.processedMessages.Enqueue(timestampedObject);
      if (this.processedMessages.Count > 100)
        this.processedMessages.Dequeue();
      EventHandler<EventArg<TimestampedObject<JObject>>> incommingMessage = this.IncommingMessage;
      if (incommingMessage == null)
        return;
      incommingMessage((object) this, new EventArg<TimestampedObject<JObject>>(timestampedObject));
    }

    public void ProcessMessage(JObject mObj)
    {
      this.OnProcessMessage(mObj);
      JObject jobject = mObj.Value<JObject>((object) "data");
      string s = jobject.Value<string>((object) "type");
      // ISSUE: reference to a compiler-generated method
      switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(s))
      {
        case 109168545:
          if (!(s == "blist"))
            return;
          IMO.ContactsManager.HandleMessage(jobject);
          return;
        case 693094236:
          if (!(s == "account"))
            return;
          break;
        case 830805427:
          if (!(s == "groupav"))
            return;
          goto label_22;
        case 1495603374:
          if (!(s == "av"))
            return;
          goto label_22;
        case 1778093175:
          if (!(s == "mobile"))
            return;
          break;
        case 2569433988:
          if (!(s == "imo_directory"))
            return;
          IMO.QueryEngine.HandleMessage(jobject);
          return;
        case 3262359788:
          if (!(s == "avconv"))
            return;
          goto label_22;
        case 3277802743:
          if (!(s == "session"))
            return;
          break;
        case 3963642631:
          if (!(s == "conv"))
            return;
          IMO.ConversationsManager.HandleMessage(jobject);
          return;
        case 4203432024:
          if (!(s == "imo_dns"))
            return;
          IMO.ImoDNSManager.HandleMessage(jobject);
          return;
        default:
          return;
      }
      IMO.AccountManager.HandleMessage(jobject);
      return;
label_22:
      IMO.AVManager.HandleMessage(jobject);
    }
  }
}
