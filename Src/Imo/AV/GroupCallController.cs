// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.AV.GroupCallController
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Threading;


namespace ImoSilverlightApp.AV
{
  internal class GroupCallController : CallController
  {
    private ImageSource currentFrameSlot1;
    private ImageSource currentFrameSlot2;
    private ImageSource currentFrameSlot3;
    private ImageSource currentFrameSlot4;
    private int cameraAngle1;
    private int cameraAngle2;
    private int cameraAngle3;
    private int cameraAngle4;
    private DispatcherTimer autoRejectTimer = new DispatcherTimer();

    public ImageSource CurrentFrameSlot1
    {
      get => this.currentFrameSlot1;
      set
      {
        this.currentFrameSlot1 = value;
        this.OnPropertyChanged(nameof (CurrentFrameSlot1));
      }
    }

    public ImageSource CurrentFrameSlot2
    {
      get => this.currentFrameSlot2;
      set
      {
        this.currentFrameSlot2 = value;
        this.OnPropertyChanged(nameof (CurrentFrameSlot2));
      }
    }

    public ImageSource CurrentFrameSlot3
    {
      get => this.currentFrameSlot3;
      set
      {
        this.currentFrameSlot3 = value;
        this.OnPropertyChanged(nameof (CurrentFrameSlot3));
      }
    }

    public ImageSource CurrentFrameSlot4
    {
      get => this.currentFrameSlot4;
      set
      {
        this.currentFrameSlot4 = value;
        this.OnPropertyChanged(nameof (CurrentFrameSlot4));
      }
    }

    public int CameraAngle1
    {
      get => this.cameraAngle1;
      set
      {
        this.cameraAngle1 = value;
        this.OnPropertyChanged(nameof (CameraAngle1));
      }
    }

    public int CameraAngle2
    {
      get => this.cameraAngle2;
      set
      {
        this.cameraAngle2 = value;
        this.OnPropertyChanged(nameof (CameraAngle2));
      }
    }

    public int CameraAngle3
    {
      get => this.cameraAngle3;
      set
      {
        this.cameraAngle3 = value;
        this.OnPropertyChanged(nameof (CameraAngle3));
      }
    }

    public int CameraAngle4
    {
      get => this.cameraAngle4;
      set
      {
        this.cameraAngle4 = value;
        this.OnPropertyChanged(nameof (CameraAngle4));
      }
    }

    public GroupCallController(string buid, string srsOfClick, AVState callState = AVState.CALLING)
      : base(buid, "video_chat", srsOfClick, (byte[]) null)
    {
      this.ChatType = "video_chat";
      this.maxVideoBitrateKbps = 64;
      this.autoRejectTimer.Interval = TimeSpan.FromSeconds(60.0);
      this.autoRejectTimer.Tick += (EventHandler) ((s, e) => this.RejectCall());
      this.CallHandler = new MacawHandler((CallController) this);
      this.CallState = callState;
    }

    protected override void OnPropertyChanged(string name)
    {
      base.OnPropertyChanged(name);
      if (!(name == "CallState"))
        return;
      this.SetRingTone(this.CallState == AVState.RECEIVING);
      this.CallStatus = this.CallState == AVState.RECEIVING ? "is calling you" : "";
      if (this.CallState == AVState.RECEIVING)
        this.autoRejectTimer.Start();
      else
        this.autoRejectTimer.Stop();
    }

    public override bool IsGroupCall => true;

    internal void OnReleaseStream(int streamId) => this.CallHandler.OnReleaseStream(streamId);

    public void SetupGroupCallWithConvId(
      string convId,
      JArray pipes,
      byte[] sharedKey,
      byte[] cbcKey,
      ushort streamId,
      int videoBitrate,
      int[] maxGroupVideoBitrate)
    {
      this.Pipes = pipes;
      this.ConvId = convId;
      this.sharedKey = sharedKey;
      this.serverKey = sharedKey;
      this.serverCbcKey = cbcKey;
      this.StreamId = streamId;
      this.maxVideoBitrateKbps = videoBitrate;
      this.maxGroupVideoBitrate = maxGroupVideoBitrate;
      this.qualityConfigParams = new double[1][]
      {
        new double[3]
        {
          (double) this.maxVideoBitrateKbps,
          (double) this.maxVideoBitrateKbps,
          160.0
        }
      };
      this.BeginLog();
      this.ClientType = "macaw";
      this.CallState = AVState.TALKING;
      this.CallHandler.OnCallInitiated();
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
    }

    public override void EndAll(bool sendLog)
    {
      this.EndLog();
      if (!sendLog)
        this.monitorLog = (Dictionary<string, object>) null;
      IMO.AVManager.LeaveGroupCall(this.Buid);
      IMO.AVManager.RemoveCallController();
    }

    protected override void LogMonitorLog()
    {
      if (this.CallState != AVState.TALKING)
        return;
      IMO.MonitorLog.Log("group_talk_time_windows", new JObject()
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

    public override void SetCallState(AVState newState, string type)
    {
      if (newState != AVState.NONE)
        return;
      this.SetRingTone(false);
    }

    public override void SendTerminateCallForConvId(string conv, string reason)
    {
      string.IsNullOrEmpty(conv);
    }

    public override void SelfAcceptedElsewhere() => IMO.AVManager.RemoveCallController();

    public override void AcceptCall()
    {
      IMO.AVManager.InitiateGroupCall(this.Buid, "accept_incoming_button");
      this.CallState = AVState.TALKING;
    }

    public override void RejectCall() => IMO.AVManager.RemoveCallController();

    public override void BuddyAcceptedCall() => this.CallHandler.OnBuddyAcceptedCall();

    public override void SendMacawAnswerCall()
    {
    }

    public override void BuddyDisconnect()
    {
    }

    public override void BuddyBusy()
    {
    }

    public override void Stop()
    {
      this.autoRejectTimer.Stop();
      base.Stop();
    }
  }
}
