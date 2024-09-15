// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.Connection.Network
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Collections;
using ImoSilverlightApp.Helpers;
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Networking;
using Windows.Networking.Sockets;


namespace ImoSilverlightApp.Connection
{
  internal class Network
  {
    private static readonly Logger log = LogManager.GetLogger(typeof (Network).Name);
    private const int RECONNECT_INTERVAL = 5000;
    private const int SAVED_MESSAGES_COUNT = 100;
    private bool isConnected;
    private ConnectionData connectionData;
    private bool isDisconnecting;
    private bool hasReconnectScheduled;
    private bool scheduledReconnectResetDispatcher;
    private bool noReconnectOnDisconnected;
    private IncommingMessagesLooper incommingMessagesLooper;
    private OutgoingMessageLooper outgoingMessagesLooper;
    private StreamSocket client;
    private Queue<TimestampedObject<JObject>> receivedMessages;
    private Queue<TimestampedObject<JObject>> sentMessages;
    private int routeNumber;
    private bool isReconnecting;

    public Network()
    {
      this.incommingMessagesLooper = new IncommingMessagesLooper();
      this.outgoingMessagesLooper = new OutgoingMessageLooper();
      this.receivedMessages = new Queue<TimestampedObject<JObject>>();
      this.sentMessages = new Queue<TimestampedObject<JObject>>();
    }

    public IEnumerable<TimestampedObject<JObject>> GetReceivedMessages()
    {
      return (IEnumerable<TimestampedObject<JObject>>) this.receivedMessages.ToList<TimestampedObject<JObject>>();
    }

    public IEnumerable<TimestampedObject<JObject>> GetSentMessages()
    {
      return (IEnumerable<TimestampedObject<JObject>>) this.sentMessages.ToList<TimestampedObject<JObject>>();
    }

    public bool IsConnected => this.isConnected;

    public ConnectionData ConnectionData
    {
      get
      {
        if (this.connectionData == null)
          this.connectionData = ConnectionData.CreateFromApplicationSettings();
        return this.connectionData;
      }
    }

    internal void FlushOutgoingMessages() => this.outgoingMessagesLooper.FlushOutgoingMessages();

    public event EventHandler<EventArg<TimestampedObject<JObject>>> SentMessage;

    private void OnSentMessage(JObject jObj)
    {
      TimestampedObject<JObject> timestampedObject = new TimestampedObject<JObject>(jObj);
      this.sentMessages.Enqueue(timestampedObject);
      if (this.sentMessages.Count > 100)
        this.sentMessages.Dequeue();
      EventHandler<EventArg<TimestampedObject<JObject>>> sentMessage = this.SentMessage;
      if (sentMessage == null)
        return;
      sentMessage((object) this, new EventArg<TimestampedObject<JObject>>(timestampedObject));
    }

    public event EventHandler<EventArg<TimestampedObject<JObject>>> ReceivedMessage;

    private void OnReceivedMessage(JObject jObj)
    {
      TimestampedObject<JObject> timestampedObject = new TimestampedObject<JObject>(jObj);
      this.receivedMessages.Enqueue(timestampedObject);
      if (this.receivedMessages.Count > 100)
        this.receivedMessages.Dequeue();
      EventHandler<EventArg<TimestampedObject<JObject>>> receivedMessage = this.ReceivedMessage;
      if (receivedMessage == null)
        return;
      receivedMessage((object) this, new EventArg<TimestampedObject<JObject>>(timestampedObject));
    }

    public event EventHandler<EventArg<ConnectionData>> Connected;

    private void OnConnected(ConnectionData connectionData)
    {
      EventHandler<EventArg<ConnectionData>> connected = this.Connected;
      if (connected == null)
        return;
      connected((object) this, new EventArg<ConnectionData>(connectionData));
    }

    public event EventHandler<EventArgs> Disconnected;

    private void OnDisconnected()
    {
      EventHandler<EventArgs> disconnected = this.Disconnected;
      if (disconnected == null)
        return;
      disconnected((object) this, new EventArgs());
    }

    public event EventHandler<EventArgs> ConnectionFailed;

    private void OnConnectionFailed()
    {
      EventHandler<EventArgs> connectionFailed = this.ConnectionFailed;
      if (connectionFailed == null)
        return;
      connectionFailed((object) this, new EventArgs());
    }

    public async Task HandleMessageReceived(string message)
    {
      JObject jObj = JObject.Parse(message);
      this.OnReceivedMessage(jObj);
      Dispatcher.Instance.ProcessServerMessage(jObj);
    }

    public void HandleMessageSent(JObject jObj) => this.OnSentMessage(jObj);

    public int GetRouteNumber() => this.routeNumber;

    internal async Task HandleOutgoingLoopEnded()
    {
      Network.log.Info("Handle outgoing loop ended");
      await this.Disconnect(this.noReconnectOnDisconnected);
      if (this.noReconnectOnDisconnected)
        return;
      this.ScheduleReconnect();
    }

    internal async Task HandleIncomingLoopEnded()
    {
      Network.log.Info("Handle incoming loop ended");
      await this.Disconnect(this.noReconnectOnDisconnected);
      if (this.noReconnectOnDisconnected)
        return;
      this.ScheduleReconnect();
    }

    public void Reconnect(bool resetDispatcher = false, bool onlyIfDisconnected = false)
    {
      if (onlyIfDisconnected && this.isConnected)
        return;
      if (this.isReconnecting)
      {
        this.ScheduleReconnect(resetDispatcher);
      }
      else
      {
        Network.log.Info("Reconnecting");
        this.connectionData = ConnectionData.CreateFromApplicationSettings();
        this.connectionData.ResetDispatcher = resetDispatcher;
        this.InternalReconnect((object) this.connectionData);
      }
    }

    public async Task Disconnect(bool noReconnect = false)
    {
      this.noReconnectOnDisconnected = noReconnect;
      if (this.isDisconnecting)
        return;
      this.isDisconnecting = true;
      Network.log.Info("Disconnecting");
      await this.incommingMessagesLooper.Stop();
      await this.outgoingMessagesLooper.Stop();
      if (this.client != null)
        this.client.Dispose();
      this.isDisconnecting = false;
      Network.log.Info("Disconnected");
      this.isConnected = false;
      this.OnDisconnected();
    }

    private async void InternalReconnect(object parameter)
    {
      this.isReconnecting = true;
      await this.Disconnect();
      ConnectionData connectionData = (ConnectionData) parameter;
      if (connectionData.ResetDispatcher)
      {
        Network.log.Info("Resetting dispatcher");
        IMO.Dispatcher.Reset();
        this.scheduledReconnectResetDispatcher = false;
      }
      try
      {
        Network.log.Info("Creating new socket");
        this.client = new StreamSocket();
        this.client.Control.put_NoDelay(true);
        await this.client.ConnectAsync(new HostName(connectionData.Host), connectionData.Port.ToString());
      }
      catch (Exception ex)
      {
        Network.log.Warn(ex, "Failed to connect tcpClient. Will reconnect. " + connectionData.Host);
        this.OnConnectionFailed();
        this.isReconnecting = false;
        this.ScheduleReconnect(connectionData.ResetDispatcher);
        return;
      }
      Network.log.Debug("Created new network stream: " + (object) this.client.GetHashCode());
      ++this.routeNumber;
      Network.log.Info("Connected");
      this.isConnected = true;
      this.connectionData = connectionData;
      this.OnConnected(connectionData);
      this.isReconnecting = false;
      try
      {
        this.incommingMessagesLooper.Start(this.client);
        this.outgoingMessagesLooper.Start(this.client);
      }
      catch (Exception ex)
      {
        Network.log.Error(ex, "Error starting looper threads", 287, nameof (InternalReconnect));
        this.ScheduleReconnect(connectionData.ResetDispatcher);
      }
    }

    private void ScheduleReconnect(bool resetDispatcher = false)
    {
      if (resetDispatcher)
        this.scheduledReconnectResetDispatcher = true;
      if (this.hasReconnectScheduled)
        return;
      Network.log.Info("Scheduling reconnect");
      this.hasReconnectScheduled = true;
      Utils.DelayExecute(5000, (Action) (() =>
      {
        this.hasReconnectScheduled = false;
        this.Reconnect(this.scheduledReconnectResetDispatcher, !this.scheduledReconnectResetDispatcher);
      }));
    }
  }
}
