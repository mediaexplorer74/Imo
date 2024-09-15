// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.AV.CallController
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Helpers;
using ImoSilverlightApp.Storage.Models;
using MacawRT;
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using Windows.Devices.Sensors;
using Windows.Foundation;
using Windows.Graphics.Display;
using Windows.Phone.Devices.Notification;
using Windows.Phone.Media.Capture;
using Windows.Phone.Media.Devices;
using Windows.Phone.Networking.Voip;


namespace ImoSilverlightApp.AV
{
  public class CallController : BaseManager, INotifyPropertyChanged
  {
    private static readonly Logger log = LogManager.GetLogger(typeof (CallController).Name);
    private DispatcherTimer vibrationTimer;
    protected Dictionary<string, object> monitorLog = new Dictionary<string, object>();
    private string srcOfClick;
    private bool acceptedCall;
    private bool noMicDetected;
    private bool noSoundOutputDetected;
    private bool noCameraDetected;
    private bool cameraInitFailed;
    private bool micInitFailed;
    private VoipPhoneCall outgoingCall;
    private AVState callState;
    protected DateTime callStartTime;
    protected DispatcherTimer callTimeTimer;
    private DispatcherTimer autoCancelTimer;
    private MediaElement ringtoneElement;
    private CameraSensorLocation cameraFacing = (CameraSensorLocation) 1;
    private MacawHandler callHandler;
    protected byte[] sharedKey;
    protected byte[] serverKey;
    protected byte[] serverCbcKey;
    protected byte[] peerCbcKey;
    protected byte[][] serverTickets;
    protected byte[] abVector;
    protected int abFirst;
    protected int maxVideoBitrateKbps;
    protected double[] bitrateParams;
    protected int[] maxGroupVideoBitrate;
    protected bool errorCorrectionAllowed;
    protected double[] errorCorrectionParams;
    protected double[] callParams;
    protected byte[] initiatorProtocolMask;
    protected byte[] receiverProtocolMask;
    protected double[][] qualityConfigParams;
    private const int AUTO_CANCEL_MS = 70000;
    private ImageSource currentFrame;
    private int buddyCameraAngle;
    private string callStatus;
    private int selfOrientation;
    private Accelerometer accelerometer;
    private DisplayOrientations displayOrientation;
    public readonly int CameraRotation = 90;
    private DispatcherTimer hideFooterTimer;

    public AVState CallState
    {
      get => this.callState;
      set
      {
        if (this.callState == value)
          return;
        this.callState = value;
        int callState = (int) this.callState;
        switch (this.CallState)
        {
          case AVState.NONE:
            if (this.outgoingCall != null)
            {
              this.outgoingCall.NotifyCallEnded();
              this.outgoingCall = (VoipPhoneCall) null;
              break;
            }
            break;
          case AVState.CALLING:
            if (!this.IsVideoCall)
            {
              string alias = IMO.ContactsManager.GetOrCreateContact(this.Buid).Alias;
              VoipCallCoordinator.GetDefault().RequestNewOutgoingCall("", alias, "imo", (VoipCallMedia) 1, ref this.outgoingCall);
              VoipPhoneCall outgoingCall = this.outgoingCall;
              if (outgoingCall != null)
              {
                outgoingCall.NotifyCallActive();
                break;
              }
              break;
            }
            break;
          case AVState.TALKING:
            this.CurrentFrame = (ImageSource) null;
            this.FlipXRatioBig = 1;
            this.OnPropertyChanged("FlipXRatioBig");
            this.OnPropertyChanged("IsTalking");
            this.OnPropertyChanged("ShowSpeakerButton");
            if (this.IsVideoCall)
            {
              this.hideFooterTimer = new DispatcherTimer();
              this.hideFooterTimer.Interval = TimeSpan.FromMilliseconds(4500.0);
              this.hideFooterTimer.Tick += new EventHandler(this.HideFooter);
              this.hideFooterTimer.Start();
              break;
            }
            break;
        }
        this.OnPropertyChanged(nameof (CallState));
        this.OnPropertyChanged("ShowAnswerButton");
        this.OnPropertyChanged("IsTalking");
        this.OnPropertyChanged("ShowSelfPreview");
        this.OnPropertyChanged("ShowCallHeaderInfo");
      }
    }

    public bool IsInitiator { get; set; }

    public int ReqId { get; set; }

    public virtual bool IsGroupCall => false;

    public TimeSpan CallDuration
    {
      get => this.callState != AVState.TALKING ? new TimeSpan() : DateTime.Now - this.callStartTime;
    }

    public string ChatType { get; protected set; }

    public string ClientType { get; protected set; }

    public string ConvId { get; protected set; }

    public ushort StreamId { get; protected set; }

    public JArray Pipes { get; protected set; }

    public Image SelfPreviewImage { get; set; }

    public MediaElement RingtoneElement
    {
      get => this.ringtoneElement;
      set
      {
        if (this.ringtoneElement == value)
          return;
        this.ringtoneElement = value;
        this.SetRingTone(this.CallState == AVState.RECEIVING);
      }
    }

    public MediaElement CallOutElement { get; set; }

    public CameraSensorLocation CameraFacing
    {
      get => this.cameraFacing;
      set
      {
        if (this.cameraFacing == value)
          return;
        this.cameraFacing = value;
        this.FlipXRatio = this.cameraFacing == 1 ? -1 : 1;
        this.OnPropertyChanged("FlipXRatio");
        if (this.IsTalking)
          return;
        this.FlipXRatioBig = this.FlipXRatio;
        this.OnPropertyChanged("FlipXRatioBig");
      }
    }

    public MacawHandler CallHandler
    {
      get => this.callHandler;
      protected set
      {
        if (this.callHandler == value)
          return;
        if (this.callHandler != null)
          this.callHandler.CameraChangedHandler -= new EventHandler(this.OnCameraChanged);
        this.callHandler = value;
        this.OnPropertyChanged(nameof (CallHandler));
        this.OnPropertyChanged("ShowSelfPreview");
        if (this.callHandler == null)
          return;
        this.callHandler.CameraChangedHandler += new EventHandler(this.OnCameraChanged);
      }
    }

    private void OnCameraChanged(object sender, EventArgs e)
    {
      this.OnPropertyChanged("FlipXRatio");
      this.OnPropertyChanged("MeCameraAngle");
    }

    public bool IsSpeakerOn { get; set; }

    public void ToggleSpeaker()
    {
      if (!this.IsTalking)
        return;
      this.IsSpeakerOn = !this.IsSpeakerOn;
      AudioRoutingManager.GetDefault().SetAudioEndpoint(this.IsSpeakerOn ? (AudioRoutingEndpoint) 2 : (AudioRoutingEndpoint) 0);
      this.OnPropertyChanged("IsSpeakerOn");
    }

    public string Buid { get; set; }

    public byte[] SharedKey => this.sharedKey;

    public byte[] ServerKey => this.serverKey;

    public byte[] ServerCbcKey => this.serverCbcKey;

    public byte[] PeerCbcKey => this.peerCbcKey;

    public int AbFirst => this.abFirst;

    public int MaxVideoBitrateKbps => this.maxVideoBitrateKbps;

    public double[] BitrateParams => this.bitrateParams;

    public int[] MaxGroupVideoBitrate => this.maxGroupVideoBitrate;

    public bool ErrorCorrectionAllowed => this.errorCorrectionAllowed;

    public double[] ErrorCorrectionParams => this.errorCorrectionParams;

    public double[] CallParams => this.callParams;

    public byte[] InitiatorProtocolMask => this.initiatorProtocolMask;

    public byte[] ReceiverProtocolMask => this.receiverProtocolMask;

    public double[][] QualityConfigParams => this.qualityConfigParams;

    public CallController(string buid, string chatType, string srsOfClick, byte[] sharedKey)
    {
      this.Buid = buid;
      this.ChatType = chatType;
      this.IsSpeakerOn = this.IsVideoCall;
      this.srcOfClick = srsOfClick;
      this.maxVideoBitrateKbps = -1;
      this.sharedKey = sharedKey;
      if (!(chatType == "video_chat"))
        return;
      this.accelerometer = Accelerometer.GetDefault();
      if (this.accelerometer == null)
        return;
      this.accelerometer.put_ReportInterval(Math.Max(this.accelerometer.MinimumReportInterval, 200U));
      Accelerometer accelerometer = this.accelerometer;
      // ISSUE: method pointer
      WindowsRuntimeMarshal.AddEventHandler<TypedEventHandler<Accelerometer, AccelerometerReadingChangedEventArgs>>(new Func<TypedEventHandler<Accelerometer, AccelerometerReadingChangedEventArgs>, EventRegistrationToken>(accelerometer.add_ReadingChanged), new Action<EventRegistrationToken>(accelerometer.remove_ReadingChanged), new TypedEventHandler<Accelerometer, AccelerometerReadingChangedEventArgs>((object) this, __methodptr(Accelerometer_ReadingChanged)));
    }

    private void Accelerometer_ReadingChanged(
      Accelerometer sender,
      AccelerometerReadingChangedEventArgs args)
    {
      double accelerationX = args.Reading.AccelerationX;
      double accelerationY = args.Reading.AccelerationY;
      double accelerationZ = args.Reading.AccelerationZ;
      if ((accelerationX * accelerationX + accelerationY * accelerationY) * 4.0 < accelerationZ * accelerationZ)
        return;
      float num1 = 57.29578f;
      int num2 = ((int) Math.Round(Math.Atan2(accelerationX, -accelerationY) * (double) num1) % 360 + 360) % 360;
      int num3 = num2 % 90;
      int num4;
      if (num3 < 15)
        num4 = num2 - num3;
      else if (num3 > 75)
      {
        num4 = (num2 - num3 + 90) % 360;
      }
      else
      {
        num4 = num2 - num3;
        if (this.selfOrientation == num4 || this.selfOrientation == (num4 + 90) % 360)
          return;
        if (num3 > 45)
          num4 = (num4 + 90) % 360;
      }
      if (this.selfOrientation == num4)
        return;
      this.selfOrientation = num4;
      this.UpdateUiRotation();
    }

    public void SetupCallWithConvId(
      string convId,
      JArray pipes,
      byte[] sharedKey,
      byte[] serverKey,
      byte[] serverCbcKey,
      byte[] peerCbcKey,
      byte[] initiatorProtocolMask,
      byte[] receiverProtocolMask,
      byte[] abVector,
      int abFirst,
      int maxVideoBitrateKbps,
      double[] bitrateParams,
      bool errorCorrectionAllowed,
      double[] errorCorrectionParams,
      double[] callParams,
      double[][] qualityConfigParams)
    {
      this.ConvId = convId;
      this.Pipes = pipes;
      this.acceptedCall = false;
      this.sharedKey = sharedKey;
      this.serverKey = serverKey;
      this.serverCbcKey = serverCbcKey;
      this.peerCbcKey = peerCbcKey;
      this.abVector = abVector;
      this.abFirst = abFirst;
      this.maxVideoBitrateKbps = maxVideoBitrateKbps;
      this.bitrateParams = bitrateParams;
      this.errorCorrectionAllowed = errorCorrectionAllowed;
      this.errorCorrectionParams = errorCorrectionParams;
      this.initiatorProtocolMask = initiatorProtocolMask;
      this.receiverProtocolMask = receiverProtocolMask;
      this.callParams = callParams;
      this.StreamId = (ushort) 0;
      this.qualityConfigParams = qualityConfigParams;
      this.BeginLog();
    }

    public virtual void SetCallState(AVState newState, string type)
    {
      this.SetRingTone(newState == AVState.RECEIVING);
      this.SetCallOutSound(newState == AVState.CALLING);
      if (this.autoCancelTimer != null)
      {
        this.autoCancelTimer.Stop();
        this.autoCancelTimer.Tick -= new EventHandler(this.AutoCancel);
        this.autoCancelTimer = (DispatcherTimer) null;
      }
      if (newState == AVState.CALLING)
      {
        this.autoCancelTimer = new DispatcherTimer();
        this.autoCancelTimer.Tick += new EventHandler(this.AutoCancel);
      }
      switch (newState - 1)
      {
        case AVState.NONE:
          this.CallStatus = "Connecting...";
          break;
        case AVState.WAITING:
          this.CallStatus = "Calling...";
          this.autoCancelTimer.Interval = TimeSpan.FromMilliseconds(70000.0);
          this.autoCancelTimer.Start();
          break;
        case AVState.CALLING:
          this.CallStatus = "is calling you...";
          break;
        case AVState.RECEIVING:
          this.callStartTime = DateTime.Now;
          this.callTimeTimer = new DispatcherTimer();
          this.callTimeTimer.Interval = TimeSpan.FromMilliseconds(100.0);
          this.callTimeTimer.Tick += (EventHandler) ((s, e) =>
          {
            TimeSpan callDuration = this.CallDuration;
            // ISSUE: variable of a boxed type
            __Boxed<int> totalHours = (ValueType) (int) callDuration.TotalHours;
            callDuration = this.CallDuration;
            // ISSUE: variable of a boxed type
            __Boxed<int> minutes = (ValueType) callDuration.Minutes;
            callDuration = this.CallDuration;
            // ISSUE: variable of a boxed type
            __Boxed<int> seconds = (ValueType) callDuration.Seconds;
            this.CallStatus = "In call " + string.Format("{0:0}:{1:00}:{2:00}", (object) totalHours, (object) minutes, (object) seconds);
          });
          this.callTimeTimer.Start();
          break;
      }
      if (newState == AVState.NONE || this.CallHandler != null && type == this.ClientType)
      {
        if (newState == AVState.NONE)
          this.LogMonitorLog();
        CallController.log.Debug(string.Format("No need to set handler for type: {0} state: {1}", (object) type, (object) newState.ToString()));
        bool flag = false;
        if (this.CallState == AVState.WAITING && newState != AVState.NONE)
          flag = true;
        this.CallState = newState;
        if (!flag)
          return;
        this.CallHandler.OnCallInitiated();
      }
      else
      {
        if (this.CallHandler != null)
          this.CallHandler.Stop();
        CallController.log.Debug(string.Format("Setting handler for type: {0} state: {1}", (object) type, (object) newState.ToString()));
        this.ClientType = "macaw";
        this.CallHandler = new MacawHandler(this);
        this.Log("client_type", (object) this.ClientType);
        this.CallState = newState;
        if (newState == AVState.WAITING)
          return;
        this.CallHandler.OnCallInitiated();
      }
    }

    private void AutoCancel(object sender, EventArgs e)
    {
      this.autoCancelTimer.Stop();
      this.autoCancelTimer.Tick -= new EventHandler(this.AutoCancel);
      this.autoCancelTimer = (DispatcherTimer) null;
      string reason;
      if (this.callState == AVState.CALLING)
      {
        reason = "call_timeout";
        this.SendTerminateCallForConvId(this.ConvId, reason);
      }
      else
        reason = "invalid_autoreject";
      CallController.log.Info("Autorejecting call");
      this.Log("end_reason", (object) reason);
      this.EndAll(true);
    }

    protected virtual void LogMonitorLog()
    {
      if (this.callState != AVState.TALKING)
        return;
      IMO.MonitorLog.Log("talk_time_windows", new JObject()
      {
        {
          "is_initiator",
          (JToken) this.IsInitiator
        },
        {
          "call_type",
          (JToken) this.ChatType
        },
        {
          "talk_time_ms",
          (JToken) this.CallDuration.TotalMilliseconds
        },
        {
          "is_buddy",
          (JToken) IMO.ContactsManager.GetOrCreateContact(this.Buid).IsBuddy
        }
      }, addNamespacePrefix: false);
    }

    protected void SetRingTone(bool enabled)
    {
      if (enabled && IMO.ApplicationSettings.StreamsInfo == null)
      {
        if (this.RingtoneElement == null)
          return;
        this.ringtoneElement.AutoPlay = true;
        this.RingtoneElement.MediaEnded -= new RoutedEventHandler(this.MediaElement_Rewind);
        this.RingtoneElement.MediaEnded += new RoutedEventHandler(this.MediaElement_Rewind);
        this.RingtoneElement.Play();
        if (this.vibrationTimer != null)
          this.vibrationTimer.Stop();
        this.VibrateOnce((object) null, (object) null);
        this.vibrationTimer = new DispatcherTimer();
        this.vibrationTimer.Interval = TimeSpan.FromSeconds(2.0);
        this.vibrationTimer.Tick += new EventHandler(this.VibrateOnce);
        this.vibrationTimer.Start();
      }
      else
      {
        if (this.RingtoneElement != null)
        {
          this.RingtoneElement.MediaEnded -= new RoutedEventHandler(this.MediaElement_Rewind);
          this.RingtoneElement.Stop();
        }
        if (this.vibrationTimer == null)
          return;
        this.vibrationTimer.Stop();
        VibrationDevice.GetDefault().Cancel();
        this.vibrationTimer = (DispatcherTimer) null;
      }
    }

    private void SetCallOutSound(bool enabled)
    {
      if (this.CallOutElement == null)
        return;
      if (enabled)
      {
        this.CallOutElement.AutoPlay = true;
        this.CallOutElement.MediaEnded -= new RoutedEventHandler(this.MediaElement_Rewind);
        this.CallOutElement.MediaEnded += new RoutedEventHandler(this.MediaElement_Rewind);
        this.CallOutElement.Play();
      }
      else
      {
        this.CallOutElement.MediaEnded -= new RoutedEventHandler(this.MediaElement_Rewind);
        this.CallOutElement.Stop();
      }
    }

    private void MediaElement_Rewind(object sender, RoutedEventArgs e)
    {
      MediaElement mediaElement = (MediaElement) sender;
      mediaElement.Stop();
      mediaElement.Position = TimeSpan.FromSeconds(0.0);
      mediaElement.Play();
    }

    private void VibrateOnce(object sender, object e)
    {
      VibrationDevice.GetDefault().Vibrate(TimeSpan.FromSeconds(1.0));
    }

    public void CancelCall() => this.EndCallWithReason("self_cancel");

    public virtual void AcceptCall()
    {
      if (this.CallState != AVState.RECEIVING)
      {
        CallController.log.Debug("Bad state: acceptCall when in state " + this.CallState.ToString());
      }
      else
      {
        CallController.log.Debug("acceptCall");
        this.SetCallState(AVState.TALKING, this.ClientType);
        this.acceptedCall = true;
        this.CallHandler.OnSelfCallAccepted();
      }
    }

    public virtual void RejectCall() => this.EndCallWithReason("self_reject");

    public void EndCall()
    {
      if (this.CallState != AVState.TALKING)
        CallController.log.Debug(string.Format("Bad state: endCall when not in state {0}", (object) this.CallState.ToString()));
      else
        this.EndCallWithReason("self_end");
    }

    public void EndCallWithReason(string reason)
    {
      if (this.CallState == AVState.WAITING)
      {
        CallController.log.Debug("wait for streams_info to cancel!");
        IMO.AVManager.RemoveCallController();
      }
      else
      {
        CallController.log.Debug("End call: " + reason);
        this.Log("end_reason", (object) reason);
        this.SendTerminateCallWithReason(reason);
        this.EndAll(this.callState == AVState.TALKING);
      }
    }

    public void SendTerminateCall() => this.SendTerminateCallWithReason("none");

    public void SendTerminateCallWithReason(string reason)
    {
      this.SendTerminateCallForConvId(this.ConvId, reason);
    }

    public virtual void SendTerminateCallForConvId(string conv, string reason)
    {
      if (string.IsNullOrEmpty(conv))
        return;
      IMO.AVManager.TerminateCall(conv, reason);
    }

    public void EndForReestablishing()
    {
      CallController.log.Debug("willReestablish");
      this.Log("end_reason", (object) "reestablish");
      this.EndAll(true);
    }

    public void OnNativeExit() => this.SendLog(this.monitorLog, "macaw");

    public virtual void SelfAcceptedElsewhere()
    {
      if (this.acceptedCall)
        return;
      CallController.log.Debug("end call");
      this.EndAll(false);
    }

    public virtual void BuddyAcceptedCall()
    {
      if (this.CallState == AVState.RECEIVING || this.CallState == AVState.CALLING)
      {
        this.SetCallState(AVState.TALKING, this.ClientType);
        this.CallHandler.OnBuddyAcceptedCall();
      }
      else
        CallController.log.Debug("buddyAcceptedCall when not in a call!");
    }

    public virtual void BuddyDisconnect()
    {
      CallController.log.Debug("buddyDisconnect");
      if (this.CallState == AVState.CALLING || this.CallState == AVState.RECEIVING)
        this.Log("end_reason", (object) "buddy_cancel");
      else if (this.CallState == AVState.TALKING)
        this.Log("end_reason", (object) "buddy_end");
      this.EndAll(true);
    }

    public virtual void BuddyBusy()
    {
      CallController.log.Debug("buddyBusy");
      if (this.CallState != AVState.CALLING)
        return;
      this.Log("end_reason", (object) "buddy_busy");
      this.EndAll(true);
    }

    public void SendMessage(string message)
    {
      if (string.IsNullOrEmpty(this.ConvId))
        return;
      BaseManager.Send("av", "send_message", new Dictionary<string, object>()
      {
        {
          "conv_id",
          (object) this.ConvId
        },
        {
          "msg",
          (object) new Dictionary<string, object>()
          {
            {
              nameof (message),
              (object) message
            },
            {
              "client_type",
              (object) this.ClientType
            }
          }
        }
      });
    }

    public virtual void SendMacawAnswerCall()
    {
      if (string.IsNullOrEmpty(this.ConvId))
        return;
      BaseManager.Send("av", "macaw_notify_call_answered", new Dictionary<string, object>()
      {
        {
          "conv_id",
          (object) this.ConvId
        }
      });
    }

    public bool IsAVTestOn(int id)
    {
      id -= this.abFirst;
      if (id >= 0 && this.abVector != null)
      {
        int num1 = id / 8;
        int num2 = id % 8;
        if (num1 >= 0 && num1 < this.abVector.Length)
          return ((uint) this.abVector[this.abVector.Length - 1 - num1] & (uint) (1 << num2)) > 0U;
      }
      return false;
    }

    public virtual void EndAll(bool sendLog)
    {
      this.EndLog();
      if (!sendLog)
        this.monitorLog = (Dictionary<string, object>) null;
      IMO.AVManager.RemoveCallController();
    }

    public virtual void Stop()
    {
      if (this.callTimeTimer != null)
        this.callTimeTimer.Stop();
      this.RingtoneElement?.Stop();
      this.CallOutElement?.Stop();
      this.CallHandler?.Stop();
      if (!(this.ChatType == "video_chat") || this.accelerometer == null)
        return;
      // ISSUE: method pointer
      WindowsRuntimeMarshal.RemoveEventHandler<TypedEventHandler<Accelerometer, AccelerometerReadingChangedEventArgs>>(new Action<EventRegistrationToken>(this.accelerometer.remove_ReadingChanged), new TypedEventHandler<Accelerometer, AccelerometerReadingChangedEventArgs>((object) this, __methodptr(Accelerometer_ReadingChanged)));
    }

    public void EndLog()
    {
    }

    public void BeginLog()
    {
      this.monitorLog = new Dictionary<string, object>();
      this.Log("conv_id", (object) this.ConvId);
      this.Log("client_type", (object) this.ClientType);
      if (!string.IsNullOrEmpty(this.srcOfClick))
        this.Log("srcOfClick", (object) string.Format("{0} {1}", (object) this.srcOfClick, (object) this.ChatType));
      this.LogDeviceInfo();
    }

    public void LogDeviceInfo()
    {
      this.Log("connection", (object) Utils.GetNetworkTypeAndSubtype());
      this.Log("os", (object) "windows");
      this.Log("chat_type", (object) this.ChatType);
      this.Log("imo_version", (object) IMO.ApplicationProperties.Version.ToString());
      this.Log("app", (object) "imoWindowsPhone");
    }

    public void LogDictionary(Dictionary<string, object> dict)
    {
      Dictionary<string, object> monitorLog = this.monitorLog;
      if (monitorLog == null || dict == null)
        return;
      foreach (KeyValuePair<string, object> keyValuePair in dict)
        monitorLog[keyValuePair.Key] = keyValuePair.Value;
    }

    public void Log(string name, object obj)
    {
      Dictionary<string, object> monitorLog = this.monitorLog;
      if (monitorLog == null || name == null)
        return;
      monitorLog[name] = obj;
    }

    public void SendLog(Dictionary<string, object> dict, string ns)
    {
      if (dict == null)
        return;
      IMO.MonitorLog.Log(ns, JObject.FromObject((object) dict), addNamespacePrefix: false);
    }

    public int PhoneRotation => this.selfOrientation;

    private DisplayOrientations DisplayOrientation
    {
      set
      {
        if (this.displayOrientation == value)
          return;
        this.displayOrientation = value;
        this.UpdateUiRotation();
      }
    }

    public int MeCameraAngle => this.CameraFacing != 1 ? 270 : 90;

    public int UiRotation { get; set; }

    private void UpdateUiRotation()
    {
      int num = 0;
      switch ((int) this.displayOrientation)
      {
        case 0:
        case 2:
          num = 0;
          break;
        case 1:
          num = 90;
          break;
        case 4:
          num = 270;
          break;
        case 8:
          num = 180;
          break;
      }
      this.UiRotation = (this.PhoneRotation + num + 360) % 360;
      Macaw.PhoneRotation = this.PhoneRotation;
    }

    public int FlipXRatio { get; set; } = -1;

    public int FlipXRatioBig { get; set; } = -1;

    public bool ShowAnswerButton => this.CallState == AVState.RECEIVING;

    public bool IsTalking => this.callState == AVState.TALKING;

    public bool ShowFlipCameraButton => this.ChatType == "video_chat";

    public bool ShowSpeakerButton => !this.IsVideoCall && this.IsTalking;

    public Contact Contact => IMO.ContactsManager.GetOrCreateContact(this.Buid);

    public bool NoMicDetected
    {
      get => this.noMicDetected;
      set
      {
        if (this.noMicDetected == value)
          return;
        this.noMicDetected = value;
        this.OnPropertyChanged(nameof (NoMicDetected));
      }
    }

    public bool NoSoundOutputDetected
    {
      get => this.noSoundOutputDetected;
      set
      {
        if (this.noSoundOutputDetected == value)
          return;
        this.noSoundOutputDetected = value;
        this.OnPropertyChanged(nameof (NoSoundOutputDetected));
      }
    }

    public bool NoCameraDetected
    {
      get => this.noCameraDetected;
      set
      {
        if (this.noCameraDetected == value)
          return;
        this.noCameraDetected = value;
        this.OnPropertyChanged(nameof (NoCameraDetected));
      }
    }

    public bool CameraInitFailed
    {
      get => this.cameraInitFailed;
      set
      {
        if (this.cameraInitFailed == value)
          return;
        this.cameraInitFailed = value;
        this.OnPropertyChanged(nameof (CameraInitFailed));
      }
    }

    public bool MicInitFailed
    {
      get => this.micInitFailed;
      set
      {
        if (this.micInitFailed == value)
          return;
        this.micInitFailed = value;
        this.OnPropertyChanged(nameof (MicInitFailed));
      }
    }

    public string CallStatus
    {
      get => this.callStatus;
      set
      {
        if (!(this.callStatus != value))
          return;
        this.callStatus = value;
        this.OnPropertyChanged(nameof (CallStatus));
      }
    }

    public ImageSource CurrentFrame
    {
      get => this.currentFrame;
      set
      {
        this.currentFrame = value;
        this.OnPropertyChanged(nameof (CurrentFrame));
      }
    }

    public bool IsVideoCall => this.ChatType == "video_chat";

    public int BuddyCameraAngle
    {
      get => this.buddyCameraAngle;
      set
      {
        if (this.buddyCameraAngle == value)
          return;
        this.buddyCameraAngle = value;
        this.OnPropertyChanged(nameof (BuddyCameraAngle));
      }
    }

    public bool ShowSelfPreview
    {
      get
      {
        if (this.IsVideoCall && !this.IsGroupCall)
        {
          MacawHandler callHandler = this.CallHandler;
          if ((callHandler != null ? (callHandler.SelfVideoStarted ? 1 : 0) : 0) != 0)
            return this.CallState == AVState.TALKING;
        }
        return false;
      }
    }

    public bool ShowCallHeaderInfo => !this.IsVideoCall || this.CallState != AVState.TALKING;

    public int ScreenWidth => Utils.GetScreenWidth();

    public int ScreenHeight => Utils.GetScreenHeight();

    public int GroupSlotWidth => Utils.GetGroupCallSlotWidth();

    public int GroupSlotHeight => Utils.GetGroupCallSlotHeight();

    public bool ShowFooter { get; set; } = true;

    public void HideFooter(object sender = null, EventArgs e = null)
    {
      this.hideFooterTimer.Stop();
      this.ShowFooter = false;
      this.OnPropertyChanged("ShowFooter");
    }

    public void DisplayFooter()
    {
      this.hideFooterTimer.Stop();
      this.ShowFooter = true;
      this.OnPropertyChanged("ShowFooter");
      this.hideFooterTimer.Start();
    }

    public void ToggleFooter()
    {
      if (this.ShowFooter)
        this.HideFooter();
      else
        this.DisplayFooter();
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string name)
    {
      PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
      if (propertyChanged == null)
        return;
      propertyChanged((object) this, new PropertyChangedEventArgs(name));
    }
  }
}
