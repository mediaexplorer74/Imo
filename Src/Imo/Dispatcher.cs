// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.Dispatcher
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Collections;
using ImoSilverlightApp.Helpers;
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Collections.Generic;


namespace ImoSilverlightApp
{
  public class Dispatcher
  {
    private static readonly Logger log = LogManager.GetLogger(typeof (Dispatcher).Name);
    private static Dispatcher instance;
    private string ssid;
    private Queue<OutgoingMessage> outgoingMessages;
    private PriorityQueue<JObject> incommingMessages;
    private Dictionary<string, Action<JToken>> callbackMap;
    private PriorityQueue<KeyValuePair<int, Action>> ackCallbackQueue;
    private DateTime lastSentMessageTime = DateTime.Now;
    private object syncRoot = new object();
    private int seq;
    private int processedServerSeq;
    private int currentServerAck;
    private int maxReceivedMessageSeq;
    private int lastSentAck;
    private const int KEEP_ALIVE_INTERVAL = 60000;

    public event EventHandler Resetted;

    private Dispatcher()
    {
      this.incommingMessages = new PriorityQueue<JObject>((Comparison<JObject>) ((a, b) => a.Value<int>((object) nameof (seq)) - b.Value<int>((object) nameof (seq))));
      this.outgoingMessages = new Queue<OutgoingMessage>();
      this.callbackMap = new Dictionary<string, Action<JToken>>();
      this.ackCallbackQueue = new PriorityQueue<KeyValuePair<int, Action>>((Comparison<KeyValuePair<int, Action>>) ((x, y) => x.Key - y.Key));
      this.InternalReset();
    }

    internal JObject GetNextOutgoingMessage()
    {
      return this.outgoingMessages.Count > 0 ? this.outgoingMessages.Peek().Message : (JObject) null;
    }

    internal JToken GetOutgoingQueueSize() => (JToken) this.outgoingMessages.Count;

    public string GetSSID()
    {
      if (this.ssid == null)
        this.GenerateNewSSID();
      return this.ssid;
    }

    private void GenerateNewSSID() => this.ssid = IMO.ImoDNSManager.GenerateSSID();

    public static Dispatcher Instance
    {
      get
      {
        if (Dispatcher.instance == null)
          Dispatcher.instance = new Dispatcher();
        return Dispatcher.instance;
      }
    }

    public string StoreCallback(Action<JToken> successCallback)
    {
      string requestId = this.GenerateRequestId();
      this.callbackMap.Add(requestId, successCallback);
      return requestId;
    }

    public void StoreAckCallback(Action ackCallback)
    {
      this.ackCallbackQueue.Enqueue(new KeyValuePair<int, Action>(this.seq, ackCallback));
    }

    private string GenerateRequestId() => Utils.GetRandomString(8);

    public void EnqueueOutgoingMessage(JObject jObject)
    {
      lock (this.syncRoot)
      {
        OutgoingMessage outgoingMessage = new OutgoingMessage(jObject, this.seq);
        ++this.seq;
        this.outgoingMessages.Enqueue(outgoingMessage);
      }
    }

    public void EnqueueOutgoingMessage(string message)
    {
      this.EnqueueOutgoingMessage(JObject.Parse(message));
    }

    private bool ProcessIncommingMessages()
    {
      bool flag = false;
      lock (this.syncRoot)
      {
        while (this.incommingMessages.Count > 0)
        {
          JObject mObj = this.incommingMessages.Peek();
          int num = mObj.Value<int>((object) "seq");
          if (mObj.Value<JObject>((object) "data").Value<string>((object) "type") == "reset")
          {
            this.incommingMessages.Dequeue();
            IMO.Network.Reconnect(true);
            break;
          }
          if (num <= this.processedServerSeq)
            this.incommingMessages.Dequeue();
          else if (num == this.processedServerSeq + 1)
          {
            ++this.processedServerSeq;
            this.incommingMessages.Dequeue();
            EventDispatcher.Instance.ProcessMessage(mObj);
            flag = true;
          }
          else
            break;
          string key = mObj.Value<JObject>((object) "data").Value<string>((object) "request_id");
          Action<JToken> action;
          if (key != null && this.callbackMap.TryGetValue(key, out action))
          {
            this.callbackMap.Remove(key);
            action(mObj.Value<JObject>((object) "data").Value<JToken>((object) "response"));
          }
        }
      }
      return flag;
    }

    public JObject GetNextServerMessage(
      ref bool messageAckTimeoutExceeded,
      ref bool messageRetriesExceeded)
    {
      lock (this.syncRoot)
      {
        IList<JObject> o = (IList<JObject>) new List<JObject>();
        foreach (OutgoingMessage outgoingMessage in this.outgoingMessages)
        {
          int routeNumber = IMO.Network.GetRouteNumber();
          if (outgoingMessage.ShouldTrySend(routeNumber))
          {
            outgoingMessage.WillTrySend(routeNumber);
            o.Add(outgoingMessage.Message);
            break;
          }
          if (outgoingMessage.ExceededSendRetriesLimit())
          {
            messageRetriesExceeded = true;
            return (JObject) null;
          }
          if (outgoingMessage.ExceededAckTimeout(routeNumber))
          {
            messageAckTimeoutExceeded = true;
            return (JObject) null;
          }
        }
        int currentAck = IMO.Dispatcher.GetCurrentAck();
        if (o.Count == 0 && this.lastSentAck >= currentAck)
        {
          if (this.ShouldSendKeepAlive())
          {
            this.lastSentMessageTime = DateTime.Now;
            IMO.Session.SendKeepALive();
          }
          return (JObject) null;
        }
        this.lastSentMessageTime = DateTime.Now;
        this.lastSentAck = currentAck;
        return new JObject()
        {
          {
            "method",
            (JToken) "forward_to_server"
          },
          {
            "data",
            (JToken) new JObject()
            {
              {
                "ssid",
                (JToken) IMO.Dispatcher.GetSSID()
              },
              {
                "ack",
                (JToken) currentAck
              },
              {
                "messages",
                (JToken) JArray.FromObject((object) o)
              }
            }
          }
        };
      }
    }

    private bool ShouldSendKeepAlive()
    {
      return (DateTime.Now - this.lastSentMessageTime).TotalMilliseconds > 60000.0;
    }

    public int GetCurrentAck()
    {
      lock (this.syncRoot)
        return this.maxReceivedMessageSeq + 1;
    }

    public void EnqueueIncommingMessage(JObject jObj) => this.incommingMessages.Enqueue(jObj);

    public void EnqueueIncommingMessage(string message)
    {
      this.EnqueueIncommingMessage(JObject.Parse(message));
    }

    public void ProcessServerMessage(JObject jObj)
    {
      string str = jObj.Value<string>((object) "method");
      JObject jobject = jObj.Value<JObject>((object) "data");
      JArray jarray = jobject.Value<JArray>((object) "messages");
      int ack = jobject.Value<int>((object) "ack");
      switch (str)
      {
        case "name_channel":
          return;
        case null:
          if (jarray == null || jarray.Count == 0)
          {
            this.HandleServerAck(ack);
            return;
          }
          break;
      }
      this.HandleServerAck(ack);
      foreach (JToken jtoken in jarray)
      {
        this.EnqueueIncommingMessage(jtoken.ToObject<JObject>());
        lock (this.syncRoot)
          this.maxReceivedMessageSeq = Math.Max(jtoken.Value<int>((object) "seq"), this.maxReceivedMessageSeq);
      }
      this.ProcessIncommingMessages();
    }

    private void HandleServerAck(int ack)
    {
      if (this.currentServerAck >= ack)
        return;
      this.currentServerAck = ack;
      lock (this.syncRoot)
      {
        while (this.outgoingMessages.Count > 0)
        {
          if (this.outgoingMessages.Peek().Seq < ack)
            this.outgoingMessages.Dequeue();
          else
            break;
        }
      }
      while (this.ackCallbackQueue.Count > 0 && this.ackCallbackQueue.Peek().Key < ack)
        this.ackCallbackQueue.Dequeue().Value();
    }

    public void Reset()
    {
      Dispatcher.log.Info("Resetting dispatcher");
      this.InternalReset();
      ++IMO.ApplicationProperties.DispatcherResets;
      IMO.Session.CookieLogin("reset");
      this.OnReset();
    }

    private void InternalReset()
    {
      lock (this.syncRoot)
      {
        this.seq = 0;
        this.processedServerSeq = -1;
        this.currentServerAck = -1;
        this.lastSentAck = 0;
        this.lastSentMessageTime = DateTime.Now;
        this.maxReceivedMessageSeq = -1;
        this.outgoingMessages.Clear();
        this.incommingMessages.Clear();
        this.ackCallbackQueue.Clear();
        this.callbackMap.Clear();
        this.GenerateNewSSID();
      }
    }

    private void OnReset()
    {
      if (this.Resetted == null)
        return;
      this.Resetted((object) this, EventArgs.Empty);
    }
  }
}
