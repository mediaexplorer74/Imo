// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.Connection.IncommingMessagesLooper
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Helpers;
using NLog;
using System;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;


namespace ImoSilverlightApp.Connection
{
  internal class IncommingMessagesLooper
  {
    private static readonly Logger log = LogManager.GetLogger(typeof (IncommingMessagesLooper).Name);
    private bool gotNameChannel;
    private StreamSocket networkStream;
    private bool isRunning;
    private bool stopRequested;

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

    public async void Start(StreamSocket networkStream)
    {
      try
      {
        this.isRunning = true;
        this.networkStream = networkStream;
        this.gotNameChannel = false;
        DataReader reader = new DataReader(networkStream.InputStream);
        while (!this.stopRequested)
        {
          string message = await this.ReadMessage(reader);
          await IMO.Network.HandleMessageReceived(message);
        }
        reader = (DataReader) null;
      }
      catch (Exception ex)
      {
        IncommingMessagesLooper.log.Info("Stopping read incomming message loop: " + ex.Message);
      }
      finally
      {
        this.isRunning = false;
        await IMO.Network.HandleIncomingLoopEnded();
      }
    }

    private async Task<string> ReadMessage(DataReader dataReader)
    {
      byte[] key = this.gotNameChannel ? Sym.GetSecretKey() : Sym.GetNotSoSecretKey();
      this.gotNameChannel = true;
      if (await (IAsyncOperation<uint>) dataReader.LoadAsync(16U) != 16U)
        throw new Exception("SSL read stream ended.");
      byte[] iv = new byte[12];
      byte[] first = new byte[16];
      dataReader.ReadBytes(first);
      int remainingLengthToRead = Sym.FirstDecrypt(first, key, iv, !this.gotNameChannel);
      byte[] data = (long) await (IAsyncOperation<uint>) dataReader.LoadAsync((uint) remainingLengthToRead) == (long) remainingLengthToRead ? new byte[remainingLengthToRead] : throw new Exception("SSL read stream ended.");
      dataReader.ReadBytes(data);
      return ZLibHelper.Decompress(Sym.Decrypt(data, key, iv));
    }
  }
}
