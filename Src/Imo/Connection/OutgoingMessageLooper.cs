// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.Connection.OutgoingMessageLooper
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;


namespace ImoSilverlightApp.Connection
{
  internal class OutgoingMessageLooper
  {
    private static readonly Logger log = LogManager.GetLogger(typeof (OutgoingMessageLooper).Name);
    private StreamSocket networkStream;
    private bool hasSentHeaders;
    private bool stopRequested;
    private bool isRunning;
    private DataWriter writer;

    public async Task Stop()
    {
      if (this.networkStream != null)
        this.networkStream.Dispose();
      if (!this.isRunning)
        return;
      this.stopRequested = true;
      while (this.isRunning)
        await Task.Delay(10);
      this.stopRequested = false;
    }

    public void Start(StreamSocket networkStream)
    {
      this.networkStream = networkStream;
      if (this.isRunning)
        OutgoingMessageLooper.log.Warn("Started outgoing loop that is already running");
      this.hasSentHeaders = false;
      this.RunConnectionOutgoingMessagesLoop();
    }

    private async void RunConnectionOutgoingMessagesLoop()
    {
      try
      {
        this.isRunning = true;
        this.writer = new DataWriter(this.networkStream.OutputStream);
        int num = await this.SendMessage(this.writer, MessageFactory.CreateNameChannelMessage(), true) ? 1 : 0;
        while (!this.stopRequested)
        {
          bool messageRetriesExceeded = false;
          bool messageAckTimeoutExceeded = false;
          JObject nextServerMessage = Dispatcher.Instance.GetNextServerMessage(ref messageAckTimeoutExceeded, ref messageRetriesExceeded);
          if (messageRetriesExceeded)
          {
            IMO.Network.Reconnect(true);
            break;
          }
          if (messageAckTimeoutExceeded)
          {
            IMO.Network.Reconnect();
            break;
          }
          if (nextServerMessage != null)
          {
            if (!this.hasSentHeaders)
            {
              nextServerMessage.Add("headers", MessageFactory.CreateFirstMessageHeaders());
              this.hasSentHeaders = true;
            }
            if (!await this.SendMessage(this.writer, nextServerMessage, false))
            {
              OutgoingMessageLooper.log.Info("Stoping outgoing messages loop");
              break;
            }
          }
          await Task.Delay(20);
        }
      }
      catch (Exception ex)
      {
        OutgoingMessageLooper.log.Info("Stoping outgoing messages loop: " + ex.Message);
      }
      finally
      {
        this.isRunning = false;
        await IMO.Network.HandleOutgoingLoopEnded();
      }
    }

    public void FlushOutgoingMessages()
    {
      while (!this.stopRequested)
      {
        bool messageRetriesExceeded = false;
        bool messageAckTimeoutExceeded = false;
        JObject nextServerMessage = Dispatcher.Instance.GetNextServerMessage(ref messageAckTimeoutExceeded, ref messageRetriesExceeded);
        if (nextServerMessage != null && this.writer != null)
        {
          if (!this.hasSentHeaders)
          {
            nextServerMessage.Add("headers", MessageFactory.CreateFirstMessageHeaders());
            this.hasSentHeaders = true;
          }
          if (!this.SendMessageBlocking(this.writer, nextServerMessage, false))
          {
            OutgoingMessageLooper.log.Info("Stoping outgoing messages loop");
            break;
          }
        }
        else
          break;
      }
      try
      {
        DataWriter writer = this.writer;
        if (writer == null)
          return;
        ((IAsyncOperation<uint>) writer.StoreAsync()).AsTask<uint>().Wait();
      }
      catch (Exception ex)
      {
      }
    }

    private async Task<bool> SendMessage(DataWriter writer, JObject jObj, bool isNameChannel)
    {
      byte[] numArray = Sym.Encrypt(ZLibHelper.Compress(Encoding.UTF8.GetBytes(jObj.ToString(Formatting.None))), isNameChannel ? Sym.GetNotSoSecretKey() : Sym.GetSecretKey(), isNameChannel);
      try
      {
        writer.WriteBytes(numArray);
        int num = (int) await (IAsyncOperation<uint>) writer.StoreAsync();
      }
      catch (ObjectDisposedException ex)
      {
        OutgoingMessageLooper.log.Info("TCP stream disposed");
        return false;
      }
      catch (Exception ex)
      {
        OutgoingMessageLooper.log.Error(ex, "TCP write exception", 180, nameof (SendMessage));
        return false;
      }
      IMO.Network.HandleMessageSent(jObj);
      return true;
    }

    private bool SendMessageBlocking(DataWriter writer, JObject jObj, bool isNameChannel)
    {
      byte[] numArray = Sym.Encrypt(ZLibHelper.Compress(Encoding.UTF8.GetBytes(jObj.ToString(Formatting.None))), isNameChannel ? Sym.GetNotSoSecretKey() : Sym.GetSecretKey(), isNameChannel);
      try
      {
        writer.WriteBytes(numArray);
      }
      catch (ObjectDisposedException ex)
      {
        OutgoingMessageLooper.log.Info("TCP stream disposed");
        return false;
      }
      catch (Exception ex)
      {
        OutgoingMessageLooper.log.Error(ex, "TCP write exception", 208, nameof (SendMessageBlocking));
        return false;
      }
      IMO.Network.HandleMessageSent(jObj);
      return true;
    }
  }
}
