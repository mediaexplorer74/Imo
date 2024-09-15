// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.AV.MacawHandler
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Helpers;
using MacawRT;
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Windows.Foundation;
using Windows.Phone.Media.Capture;
using Windows.Storage;


namespace ImoSilverlightApp.AV
{
  public class MacawHandler
  {
    private static readonly Logger log = LogManager.GetLogger(typeof (MacawHandler).Name);
    private RingBuffer<ulong> messageQueue = new RingBuffer<ulong>(100);
    private CallController callController;
    private Task mainLoopWorker;
    private CameraGrabber videoGrabber;
    private DateTime start;
    private int macawThreadId = -1;
    private TimeSpan minTimeBetweenFrames = TimeSpan.FromMilliseconds(50.0);
    private DateTime lastFrame;
    private int selfPreviewFramesScheduled;
    private bool isStarted;
    private int context;
    private HashSet<string> reportedExceptions = new HashSet<string>();
    public EventHandler CameraChangedHandler;
    private DateTime lastStatReport = DateTime.MinValue;

    public MacawHandler(CallController callController)
    {
      this.callController = callController;
      this.selfPreviewFramesScheduled = 0;
      if (!(callController.ChatType == "video_chat"))
        return;
      this.videoGrabber = CameraGrabber.CreateInstance();
      if (this.videoGrabber != null)
      {
        this.videoGrabber.SampleCaptured += new EventHandler<SampleGrabberEventArgs>(this.VideoGrabber_FrameCaptured);
        this.callController.CameraFacing = this.videoGrabber.CameraFacing;
      }
      else
        this.callController.CameraInitFailed = true;
      this.OnCameraChanged();
    }

    private void OnCameraChanged()
    {
      EventHandler cameraChangedHandler = this.CameraChangedHandler;
      if (cameraChangedHandler == null)
        return;
      cameraChangedHandler((object) this, new EventArgs());
    }

    public bool SelfVideoStarted => this.videoGrabber != null;

    public Size SelfVideoSize
    {
      get
      {
        return this.videoGrabber == null ? new Size() : new Size((double) this.videoGrabber.VideoWidth, (double) this.videoGrabber.VideoHeight);
      }
    }

    public async Task ChangeCamera()
    {
      if (this.videoGrabber != null)
      {
        this.videoGrabber.SampleCaptured -= new EventHandler<SampleGrabberEventArgs>(this.VideoGrabber_FrameCaptured);
        this.videoGrabber.Stop();
      }
      this.videoGrabber = CameraGrabber.CreateInstance(this.callController.CameraFacing == 1 ? (CameraSensorLocation) 0 : (CameraSensorLocation) 1);
      if (this.videoGrabber != null)
      {
        try
        {
          this.callController.CameraFacing = this.videoGrabber.CameraFacing;
          this.videoGrabber.SampleCaptured += new EventHandler<SampleGrabberEventArgs>(this.VideoGrabber_FrameCaptured);
          await this.videoGrabber.Start();
          this.callController.CameraInitFailed = false;
        }
        catch (Exception ex)
        {
          if (this.videoGrabber != null)
            this.videoGrabber.SampleCaptured -= new EventHandler<SampleGrabberEventArgs>(this.VideoGrabber_FrameCaptured);
          this.callController.CameraInitFailed = true;
          MacawHandler.log.Error(ex, "Camera initialization failed on change", 134, nameof (ChangeCamera));
        }
      }
      this.OnCameraChanged();
    }

    public void OnCallInitiated()
    {
      Macaw.init(this.GetIsVideoCall());
      Macaw.PhoneRotation = this.callController.PhoneRotation;
      this.mainLoopWorker = Task.Run((Action) (() => this.MainLoopWorker_DoWork()));
      this.start = DateTime.Now;
      if (this.videoGrabber != null && !this.videoGrabber.IsCapturing)
        this.videoGrabber.Start();
      this.isStarted = true;
    }

    public void StartAudio()
    {
      Macaw.StartAudio();
      this.messageQueue.Write(1UL);
    }

    public void OnSelfCallAccepted()
    {
      this.messageQueue.Write(7UL);
      this.StartAudio();
    }

    public void OnBuddyAcceptedCall() => this.StartAudio();

    public void Stop()
    {
      if (!this.isStarted)
        return;
      this.messageQueue.Write(new ulong[1]{ 10UL }, 1);
      if (this.videoGrabber != null)
      {
        this.videoGrabber.SampleCaptured -= new EventHandler<SampleGrabberEventArgs>(this.VideoGrabber_FrameCaptured);
        this.videoGrabber.Stop();
        this.videoGrabber = (CameraGrabber) null;
      }
      Macaw.StopAudio();
    }

    public void OnSelfConnect()
    {
    }

    public void OnSelfDisconnect()
    {
    }

    internal void OnReleaseStream(int streamId)
    {
      this.messageQueue.Write((ulong) streamId << 32 | (ulong) sbyte.MaxValue);
    }

    public void OnBuddyConnect()
    {
      Utils.BeginInvokeOnUI((Action) (() =>
      {
        if (this.IsInitiator())
        {
          this.callController.SendMacawAnswerCall();
          this.callController.BuddyAcceptedCall();
        }
        else
          this.callController.SelfAcceptedElsewhere();
      }));
    }

    public void OnBuddyDisconnect()
    {
      Utils.BeginInvokeOnUI((Action) (() => this.callController.BuddyDisconnect()));
    }

    public void OnNativeExit()
    {
    }

    public bool GetIsVideoCall() => this.callController.IsVideoCall;

    public void ReportStats(string stats)
    {
      if ((DateTime.Now - this.lastStatReport).TotalSeconds < 2.0)
        return;
      this.lastStatReport = DateTime.Now;
    }

    public async void SetFrame(byte[] data, int width, int height, int angle, int slotIndex)
    {
      if (this.callController == null)
        return;
      if (this.callController.IsGroupCall)
      {
        GroupCallController groupController = this.callController as GroupCallController;
        await Utils.InvokeOnUI((Action) (() =>
        {
          BitmapSource bitmap = width == 0 || height == 0 || data == null ? (BitmapSource) null : MacawHandler.GetBitmap(data, width, height);
          int num = (720 - angle - this.callController.UiRotation) % 360;
          switch (slotIndex)
          {
            case 0:
              groupController.CameraAngle1 = num;
              groupController.CurrentFrameSlot1 = (ImageSource) bitmap;
              break;
            case 1:
              groupController.CameraAngle2 = num;
              groupController.CurrentFrameSlot2 = (ImageSource) bitmap;
              break;
            case 2:
              groupController.CameraAngle3 = num;
              groupController.CurrentFrameSlot3 = (ImageSource) bitmap;
              break;
            case 3:
              groupController.CameraAngle4 = num;
              groupController.CurrentFrameSlot4 = (ImageSource) bitmap;
              break;
          }
        }));
      }
      else
        await Utils.InvokeOnUI((Action) (() =>
        {
          this.callController.BuddyCameraAngle = (720 - angle - this.callController.UiRotation) % 360;
          this.callController.CurrentFrame = (ImageSource) MacawHandler.GetBitmap(data, width, height);
        }));
    }

    public static BitmapSource GetBitmap(byte[] pixels, int width, int height)
    {
      WriteableBitmap bitmap = new WriteableBitmap(width, height);
      for (int index = 0; index < pixels.Length / 3; ++index)
      {
        bitmap.Pixels[index] = 0;
        bitmap.Pixels[index] |= (int) pixels[3 * index];
        bitmap.Pixels[index] |= (int) pixels[3 * index + 1] << 8;
        bitmap.Pixels[index] |= (int) pixels[3 * index + 2] << 16;
        bitmap.Pixels[index] |= -16777216;
      }
      return (BitmapSource) bitmap;
    }

    public string GetConvId() => this.callController.ConvId;

    public string GetLogPath() => ApplicationData.Current.LocalFolder.Path;

    public bool IsRefl() => false;

    public bool IsInitiator() => this.callController.IsInitiator;

    public bool IsABTestEnabled(int abTest)
    {
      if (this.callController.IsGroupCall)
        return abTest == 1 || abTest == 253 || abTest == 252;
      switch (abTest)
      {
        case 1:
          return Utils.GetNumberOfCores() > 1;
        case 27:
        case 35:
        case 40:
        case 61:
        case 63:
        case 69:
        case 70:
        case 71:
        case 72:
        case 78:
          return this.callController.IsAVTestOn(abTest);
        case 208:
          return this.callController.IsAVTestOn(12);
        case 219:
          return true;
        case 250:
          return this.callController.IsAVTestOn(46);
        case 251:
          return this.callController.IsAVTestOn(47);
        case 252:
          return this.callController.IsAVTestOn(48);
        case 253:
          return this.callController.IsAVTestOn(49);
        case 254:
          return this.callController.IsAVTestOn(53);
        case (int) byte.MaxValue:
          return this.callController.IsAVTestOn(55);
        case 256:
          return this.callController.IsAVTestOn(56);
        case 257:
          return this.callController.IsAVTestOn(57);
        default:
          return false;
      }
    }

    public byte[] GetSharedKey() => this.callController.SharedKey ?? new byte[0];

    public byte[] GetServerCbcKey() => this.callController.ServerCbcKey ?? new byte[0];

    public byte[] GetPeerCbcKey() => this.callController.PeerCbcKey ?? new byte[0];

    public byte[] GetServerKey() => this.callController.ServerKey ?? new byte[0];

    public byte[] GetConnServerTickets(int which)
    {
      if (which < 0 || which >= this.callController.Pipes.Count)
        return (byte[]) null;
      JArray jarray;
      for (jarray = (JArray) null; which >= 0 && jarray == null; --which)
        jarray = this.callController.Pipes[which].Value<JArray>((object) "tickets");
      int count = jarray != null ? jarray.Count : 0;
      int length = 0;
      int[] numArray1 = new int[count];
      for (int index = 0; index < count; ++index)
      {
        byte[] numArray2 = Convert.FromBase64String(jarray[index].Value<string>());
        numArray1[index] = numArray2.Length;
        length += numArray1[index];
      }
      byte[] dst = new byte[length];
      int dstOffset = 0;
      for (int index = 0; index < count; ++index)
      {
        Buffer.BlockCopy((Array) Convert.FromBase64String(jarray[index].Value<string>()), 0, (Array) dst, dstOffset, numArray1[index]);
        dstOffset += numArray1[index];
      }
      return dst;
    }

    public int[] GetConnServerTicketsLengths(int which)
    {
      if (which < 0 || which >= this.callController.Pipes.Count)
        return (int[]) null;
      JArray jarray;
      for (jarray = (JArray) null; which >= 0 && jarray == null; --which)
        jarray = this.callController.Pipes[which].Value<JArray>((object) "tickets");
      int count = jarray != null ? jarray.Count : 0;
      int[] serverTicketsLengths = new int[count];
      for (int index = 0; index < count; ++index)
        serverTicketsLengths[index] = Convert.FromBase64String(jarray[index].Value<string>()).Length;
      return serverTicketsLengths;
    }

    public byte[] GetInitiatorProtocolMask()
    {
      return this.callController.InitiatorProtocolMask ?? new byte[0];
    }

    public byte[] GetReceiverProtocolMask()
    {
      return this.callController.ReceiverProtocolMask ?? new byte[0];
    }

    public string GetConnectionType() => (Utils.GetNetworkTypeAndSubtype() ?? "").Trim().ToLower();

    public string GetIPv6Pipe() => "";

    public string GetLocalIPv6Address() => "";

    public int GetMaxVideoBitrateKbps() => this.callController.MaxVideoBitrateKbps;

    public int GetMaxVideoSlots() => 5;

    public double[] GetBitrateParams() => this.callController.BitrateParams ?? new double[0];

    public int[] GetMaxGroupVideoBitrates()
    {
      return this.callController.MaxGroupVideoBitrate ?? new int[0];
    }

    public bool IsErrorCorrectionAllowed() => this.callController.ErrorCorrectionAllowed;

    public double[] GetErrorCorrectionParams()
    {
      return this.callController.ErrorCorrectionParams ?? new double[0];
    }

    public void LogMonitor(string json_str)
    {
      this.callController.LogDictionary(JObject.Parse(json_str).ToObject<Dictionary<string, object>>());
      this.callController.OnNativeExit();
    }

    public ulong PollUI(bool blocking)
    {
      if (blocking)
      {
        ulong output;
        while (this.messageQueue.Read(out output) == 0)
          Task.Delay(TimeSpan.FromMilliseconds(10.0)).Wait();
        return output;
      }
      ulong output1;
      return this.messageQueue.Read(out output1) != 0 ? output1 : 0UL;
    }

    public void ReportNativeException(string trace) => string.IsNullOrEmpty(trace);

    public string GetConnServerName(int which)
    {
      return which >= 0 && which < this.callController.Pipes.Count ? this.callController.Pipes[which].Value<string>((object) "ip") : "";
    }

    public int GetConnServerPort(int which)
    {
      return which >= 0 && which < this.callController.Pipes.Count ? this.callController.Pipes[which].Value<int>((object) "port") : -1;
    }

    public double[] GetConnNetParams(int which)
    {
      if (which < 0 || which >= this.callController.Pipes.Count)
        return (double[]) null;
      return this.callController.Pipes[which].Value<JArray>((object) "net")?.ToObject<double[]>();
    }

    public string GetConnStringParams(int which)
    {
      if (which < 0 || which >= this.callController.Pipes.Count)
        return (string) null;
      JArray jarray = this.callController.Pipes[which].Value<JArray>((object) "s");
      if (jarray == null)
        return "";
      int count = jarray.Count;
      string connStringParams = "";
      for (int index = 0; index < count; ++index)
      {
        string str = jarray[index].Value<string>();
        connStringParams += str;
      }
      return connStringParams;
    }

    public int[] GetConnStringParamsLengths(int which)
    {
      if (which < 0 || which >= this.callController.Pipes.Count)
        return (int[]) null;
      JArray jarray = this.callController.Pipes[which].Value<JArray>((object) "s");
      int count = jarray != null ? jarray.Count : 0;
      int[] stringParamsLengths = new int[count];
      for (int index = 0; index < count; ++index)
        stringParamsLengths[index] = jarray[index].Value<string>().Length;
      return stringParamsLengths;
    }

    public int GetConnSourcePort(int which)
    {
      return which >= 0 && which < this.callController.Pipes.Count ? this.callController.Pipes[which].Value<int>((object) "src_port") : -1;
    }

    public int GetNumConnections() => this.callController.Pipes.Count;

    public ushort GetStreamId() => this.callController.StreamId;

    public double[] GetCallParams() => this.callController.CallParams;

    public double[] GetQualityConfigParams(int which)
    {
      return which < 0 || which >= this.callController.QualityConfigParams.Length ? (double[]) null : this.callController.QualityConfigParams[which];
    }

    public bool IsGroupCall() => this.callController.IsGroupCall;

    public void SetContext(int ctx) => this.context = ctx;

    private void VideoGrabber_FrameCaptured(object sender, SampleGrabberEventArgs e)
    {
      try
      {
        if (this.callController.CallState != AVState.TALKING)
          return;
        DateTime now = DateTime.Now;
        DateTime lastFrame = this.lastFrame;
        if ((now - this.lastFrame).CompareTo(this.minTimeBetweenFrames) <= 0)
          return;
        this.lastFrame = now;
        int angle = (this.callController.PhoneRotation + this.callController.CameraRotation) % 360;
        if (this.callController.CameraFacing == null)
          angle = (-this.callController.PhoneRotation - this.callController.CameraRotation + 720) % 360;
        Macaw.SendImageNV12(this.context, this.videoGrabber.VideoWidth, this.videoGrabber.VideoHeight, e.Buffer, (int) (DateTime.Now - this.start).TotalMilliseconds, angle);
      }
      catch (Exception ex)
      {
        if (this.reportedExceptions.Contains(ex.GetType().ToString() + ex.Message))
          return;
        this.reportedExceptions.Add(ex.GetType().ToString() + ex.Message);
        MacawHandler.log.Error(ex, "Video grabber sample captured callback error", 849, nameof (VideoGrabber_FrameCaptured));
      }
    }

    private int GetThreadId() => this.macawThreadId;

    private void MainLoopWorker_DoWork()
    {
      this.macawThreadId = this.GetThreadId();
      Macaw.SetOnSelfConnect(new VoidCallback(this.OnSelfConnect));
      Macaw.SetOnSelfDisconnect(new VoidCallback(this.OnSelfDisconnect));
      Macaw.SetOnBuddyConnect(new VoidCallback(this.OnBuddyConnect));
      Macaw.SetOnBuddyDisconnect(new VoidCallback(this.OnBuddyDisconnect));
      Macaw.SetOnExit(new VoidCallback(this.OnNativeExit));
      Macaw.SetGetIsVideoCall(new BoolCallback(this.GetIsVideoCall));
      Macaw.SetReportStats(new VoidStringCallback(this.ReportStats));
      Macaw.SetSetFrame(new SetFrameCallback(this.SetFrame));
      Macaw.SetGetConvId(new StringCallback(this.GetConvId));
      Macaw.SetGetLogPath(new StringCallback(this.GetLogPath));
      Macaw.SetIsRefl(new BoolCallback(this.IsRefl));
      Macaw.SetIsInitiator(new BoolCallback(this.IsInitiator));
      Macaw.SetIsABTestEnabled(new BoolIntCallback(this.IsABTestEnabled));
      Macaw.SetGetSharedKey(new ByteArrayCallback(this.GetSharedKey));
      Macaw.SetGetServerCbcKey(new ByteArrayCallback(this.GetServerCbcKey));
      Macaw.SetGetPeerCbcKey(new ByteArrayCallback(this.GetPeerCbcKey));
      Macaw.SetGetServerKey(new ByteArrayCallback(this.GetServerKey));
      Macaw.SetGetConnServerTickets(new IndexByteArrayCallback(this.GetConnServerTickets));
      Macaw.SetGetConnServerTicketsLengths(new IndexIntArrayCallback(this.GetConnServerTicketsLengths));
      Macaw.SetGetInitiatorProtocolMask(new ByteArrayCallback(this.GetInitiatorProtocolMask));
      Macaw.SetGetReceiverProtocolMask(new ByteArrayCallback(this.GetReceiverProtocolMask));
      Macaw.SetGetConnectionType(new StringCallback(this.GetConnectionType));
      Macaw.SetGetIPv6Pipe(new StringCallback(this.GetIPv6Pipe));
      Macaw.SetGetLocalIPv6Address(new StringCallback(this.GetLocalIPv6Address));
      Macaw.SetGetMaxVideoBitrateKbps(new IntCallback(this.GetMaxVideoBitrateKbps));
      Macaw.SetGetMaxVideoSlots(new IntCallback(this.GetMaxVideoSlots));
      Macaw.SetGetBitrateParams(new DoubleArrayCallback(this.GetBitrateParams));
      Macaw.SetIsErrorCorrectionAllowed(new BoolCallback(this.IsErrorCorrectionAllowed));
      Macaw.SetGetErrorCorrectionParams(new DoubleArrayCallback(this.GetErrorCorrectionParams));
      Macaw.SetLogMonitor(new VoidStringCallback(this.LogMonitor));
      Macaw.SetPollUI(new UlongBoolCallback(this.PollUI));
      Macaw.SetCalculateBufferDelay(new IntCallback(this.CalculateBufferDelay));
      Macaw.SetSetBufferDelay(new VoidIntCallback(this.SetBufferDelay));
      Macaw.SetReportException(new VoidStringCallback(this.ReportNativeException));
      Macaw.SetIsGroupCall(new BoolCallback(this.IsGroupCall));
      Macaw.SetGetQualityConfigParams(new IndexDoubleArrayCallback(this.GetQualityConfigParams));
      Macaw.SetGetCallParams(new DoubleArrayCallback(this.GetCallParams));
      Macaw.SetGetStreamId(new UshortCallback(this.GetStreamId));
      Macaw.SetGetNumConnections(new IntCallback(this.GetNumConnections));
      Macaw.SetGetConnServerName(new StringIntCallback(this.GetConnServerName));
      Macaw.SetGetConnSourcePort(new IntIntCallback(this.GetConnSourcePort));
      Macaw.SetGetConnServerPort(new IntIntCallback(this.GetConnServerPort));
      Macaw.SetGetConnNetParams(new IndexDoubleArrayCallback(this.GetConnNetParams));
      Macaw.SetGetConnStringParams(new StringIntCallback(this.GetConnStringParams));
      Macaw.SetGetConnStringParamsLengths(new IndexIntArrayCallback(this.GetConnStringParamsLengths));
      Macaw.SetSetContext(new VoidIntCallback(this.SetContext));
      Macaw.runThread();
    }

    public int CalculateBufferDelay() => 0;

    public void SetBufferDelay(int ms)
    {
    }
  }
}
