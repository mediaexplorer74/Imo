// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.AV.AVManager
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Helpers;
using ImoSilverlightApp.Storage.Models;
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.System.Display;


namespace ImoSilverlightApp.AV
{
  public class AVManager : BaseManager, INotifyPropertyChanged
  {
    private static readonly Logger log = LogManager.GetLogger(typeof (AVManager).Name);
    private const int AES_128_KEY_SIZE = 16;
    private const int AES_SALT_SIZE = 16;
    private int lastReqId;
    private DisplayRequest displayRequest;
    private EventWaitHandle notInCallEventHandle;
    private CallController callController;

    public AVManager()
    {
      this.notInCallEventHandle = new EventWaitHandle(true, EventResetMode.ManualReset, "ImoSilverlightAppNotInCallHandle");
    }

    private void InitializeVideoWindow()
    {
    }

    private void OnMainWindowActiveChanged(object sender, EventArgs e)
    {
    }

    private void RequestedNavigation()
    {
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string name)
    {
      PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
      if (propertyChanged == null)
        return;
      propertyChanged((object) this, new PropertyChangedEventArgs(name));
    }

    public event EventHandler GroupCallsListChanged;

    internal void OnGroupCallsListChanged()
    {
      EventHandler callsListChanged = this.GroupCallsListChanged;
      if (callsListChanged == null)
        return;
      callsListChanged((object) this, new EventArgs());
    }

    public bool IsCallOpened { get; set; }

    public bool IsInCall => this.CallController != null;

    public CallController CallController
    {
      get => this.callController;
      private set
      {
        if (this.callController == value)
          return;
        if (value == null)
        {
          this.displayRequest.RequestRelease();
          this.displayRequest = (DisplayRequest) null;
          this.notInCallEventHandle.Set();
        }
        IMO.ApplicationSettings.WasAppInCall = value != null;
        this.callController = value;
        this.OnPropertyChanged(nameof (CallController));
        if (value == null)
          return;
        this.displayRequest = new DisplayRequest();
        this.displayRequest.RequestActive();
        IMO.ConversationsManager.GetOrCreateConversation(this.callController.Buid).ActivateConversation(Utils.GetTimestamp(), true);
        this.notInCallEventHandle.Reset();
      }
    }

    public IEnumerable<Contact> CurrentGroupCalls
    {
      get
      {
        return IMO.ContactsManager.GetAllContactsList().Where<Contact>((Func<Contact, bool>) (x => x.IsBuddy && x.IsGroup && x.GroupCallMembers != null && x.GroupCallMembers.Count > 0));
      }
    }

    public void RemoveCallController()
    {
      if (this.CallController != null)
      {
        this.CallController.SetCallState(AVState.NONE, (string) null);
        this.CallController.Stop();
        if (this.IsCallOpened)
          IMO.NavigationManager.NavigateBackOrHome();
      }
      this.CallController = (CallController) null;
    }

    public void SelfAcceptCall()
    {
      if (this.CallController == null)
        return;
      this.CallController.AcceptCall();
    }

    public void SelfRejectCall() => this.CallController.RejectCall();

    public void SelfCancelCall() => this.CallController.CancelCall();

    public void EndCallWithReason(string reason) => this.CallController.EndCallWithReason(reason);

    private void InitiateChatWithBuid(string buid, string from, string chatType)
    {
      if (this.CallController != null)
        return;
      byte[] randomBytes = Utils.GetRandomBytes(32);
      string base64String = Convert.ToBase64String(randomBytes);
      this.CallController = new CallController(buid, chatType, string.Format("win_%{0}", (object) from), randomBytes);
      this.CallController.IsInitiator = true;
      this.CallController.ReqId = ++this.lastReqId;
      this.CallController.SetCallState(AVState.WAITING, "macaw");
      Dictionary<string, object> data = new Dictionary<string, object>()
      {
        {
          "uid",
          (object) IMO.User.Uid
        },
        {
          "proto",
          (object) "imo"
        },
        {
          nameof (buid),
          (object) buid
        },
        {
          "chat_type",
          (object) chatType
        },
        {
          "client_type",
          (object) "macaw"
        },
        {
          "req_id",
          (object) this.CallController.ReqId
        },
        {
          "shared_key",
          (object) base64String
        }
      };
      data["connection_type"] = (object) Utils.GetNetworkTypeAndSubtype();
      BaseManager.Send("av", "start_chat", data);
    }

    public void InitiateGroupCall(string buid, string from)
    {
      if (this.CallController != null && (this.CallController.Buid != buid || this.CallController.CallState != AVState.RECEIVING))
        return;
      IMO.MonitorLog.Log("group_call_srcOfClick", from);
      Contact.GetGid(buid);
      GroupCallController temp = this.CallController as GroupCallController;
      if (temp == null)
      {
        temp = new GroupCallController(buid, string.Format("win_%{0}", (object) from));
        this.CallController = (CallController) temp;
        this.CallController.IsInitiator = true;
        this.CallController.ReqId = ++this.lastReqId;
      }
      BaseManager.Send("groupav", "join_group", new Dictionary<string, object>()
      {
        {
          "ssid",
          (object) IMO.Dispatcher.GetSSID()
        },
        {
          "uid",
          (object) IMO.User.Uid
        },
        {
          "gid",
          (object) Contact.GetGid(buid)
        }
      }, (Action<JToken>) (t =>
      {
        if (this.CallController != temp)
          return;
        string reason = t.Value<string>((object) "result");
        if (reason == "join")
        {
          string str = t.Value<string>((object) "macaw_ip");
          int num = t.Value<int>((object) "macaw_port");
          string content = t.Value<string>((object) "cbc_handshake");
          byte[] cbcKey = Convert.FromBase64String(t.Value<string>((object) "cbc_key") ?? "");
          JArray pipes = new JArray()
          {
            (JToken) new JObject()
            {
              {
                "ip",
                (JToken) str
              },
              {
                "port",
                (JToken) num
              },
              {
                "tickets",
                (JToken) new JArray((object) content)
              }
            }
          };
          byte[] sharedKey = Convert.FromBase64String(t.Value<string>((object) "shared_key") ?? "");
          int[] maxGroupVideoBitrate = t.Value<JArray>((object) "max_group_video_bitrates")?.ToObject<int[]>();
          int videoBitrate = 64;
          if (t.Value<JToken>((object) "video_bitrate") != null)
            videoBitrate = t.Value<int>((object) "video_bitrate");
          ushort streamId = t.Value<ushort>((object) "stream_id");
          temp.SetupGroupCallWithConvId(Contact.GetGid(buid), pipes, sharedKey, cbcKey, streamId, videoBitrate, maxGroupVideoBitrate);
          this.SendJoinedGroupCallMessage(buid);
          if (!this.CallController.IsInitiator)
            this.CallController.CallHandler.StartAudio();
          IMO.NavigationManager.NavigateToAVCallPage();
        }
        else
          this.EndCallWithReason(reason);
      }));
    }

    private void SendJoinedGroupCallMessage(string buid)
    {
      IMO.ConversationsManager.GetOrCreateConversation(buid).SendMessage("joined group call", new JObject()
      {
        {
          "type",
          (JToken) "joined_group_call"
        }
      });
    }

    internal void StartAudioCall(string buid, string srcOfClick)
    {
      IMO.MonitorLog.Log("audio_call_srcOfClick", srcOfClick);
      IMO.AVManager.InitiateChatWithBuid(buid, srcOfClick, "audio_chat");
      IMO.NavigationManager.NavigateToAVCallPage();
    }

    internal void StartVideoCall(string buid, string srcOfClick)
    {
      IMO.MonitorLog.Log("video_call_srcOfClick", srcOfClick);
      IMO.AVManager.InitiateChatWithBuid(buid, srcOfClick, "video_chat");
      IMO.NavigationManager.NavigateToAVCallPage();
    }

    public void SelfEndCall() => this.CallController.EndCall();

    public void HandleMessage(JObject data)
    {
      string str1 = data.Value<string>((object) "name");
      AVManager.log.Debug("Got message: " + str1);
      switch (str1)
      {
        case "streams_info":
          if (!data.Value<JObject>((object) "edata").Value<bool>((object) "is_initiator") && !this.IsInCall)
            break;
          this.HandleStreamsInfo(data);
          break;
        case "leave_group_call":
          this.HandleGroupLeave(data);
          break;
        case "failed":
          this.HandleFailed(data);
          break;
        case "call_answered":
          Utils.Assert(this.CallController != null, "CallController is null in AVManager.HandleMessage for call_answered");
          if (!this.CallController.IsInitiator)
            break;
          string str2 = data.Value<string>((object) "client_type");
          if (str2 == this.CallController.ClientType)
          {
            this.CallController.BuddyAcceptedCall();
            break;
          }
          AVManager.log.Debug("ERROR Unknown clientType: " + str2);
          break;
        case "answered_call":
          if (this.CallController != null)
          {
            this.CallController.SelfAcceptedElsewhere();
            break;
          }
          AVManager.log.Error("CallController is null in AVManager.HandleMessage for answered_call", 384, nameof (HandleMessage));
          break;
        case "receive_av_message":
          string str3 = data.Value<string>((object) "conv_id");
          if (this.CallController != null && str3 == this.CallController.ConvId)
          {
            JObject jobject = data.Value<JObject>((object) "msg");
            string str4 = jobject?.Value<string>((object) "type");
            string str5 = jobject?.Value<string>((object) "reason");
            if (str4 == "terminate_call")
            {
              switch (str5)
              {
                case "busy":
                  this.CallController.BuddyBusy();
                  return;
                case "call_cancelled":
                  this.CallController.BuddyDisconnect();
                  return;
                default:
                  this.CallController.BuddyDisconnect();
                  AVManager.log.Debug("WARNING Unknown 'reason' in terminate_call msg: " + str5);
                  return;
              }
            }
            else
            {
              AVManager.log.Debug("ERROR Unhandled message " + str1);
              break;
            }
          }
          else
          {
            AVManager.log.Debug("WARNING nonmatching conv_id " + str3 + " - ignoring");
            break;
          }
        case "sync_group":
          this.HandleSyncGroup(data);
          break;
        case "call_acked":
          CallController callController = this.CallController;
          if (callController == null)
            break;
          callController.CallStatus = "Ringing...";
          break;
        default:
          AVManager.log.Debug("ERROR Unhandled message " + str1);
          break;
      }
    }

    public void AnybodyThere()
    {
      Dictionary<string, object> data = new Dictionary<string, object>();
      data.Add("ssid", (object) IMO.Dispatcher.GetSSID());
      data.Add("uid", (object) IMO.User.Uid);
      string[] gids = IMO.ContactsManager.GetAllGroupsList().Select<Contact, string>((Func<Contact, string>) (x => x.Buid.Substring(0, x.Buid.Length - 1))).ToArray<string>();
      data.Add("gids", (object) gids);
      BaseManager.Send("groupav", "anybody_there", data, (Action<JToken>) (response =>
      {
        if (!(response is JObject jobject2))
          return;
        foreach (string propertyName in gids)
        {
          Contact contact = IMO.ContactsManager.GetOrCreateContact(propertyName + ";");
          if (jobject2[propertyName] != null)
          {
            List<Contact> contactList = new List<Contact>();
            foreach (KeyValuePair<string, JToken> keyValuePair in jobject2[propertyName] as JObject)
              contactList.Add(IMO.ContactsManager.GetOrCreateContact(keyValuePair.Key));
            contact.GroupCallMembers = contactList;
          }
          else
            contact.GroupCallMembers = (List<Contact>) null;
        }
        this.OnGroupCallsListChanged();
      }));
    }

    private void HandleSyncGroup(JObject data)
    {
      JObject jobject1 = data.Value<JObject>((object) "edata");
      JObject jobject2 = jobject1.Value<JObject>((object) "members");
      string str = jobject1.Value<string>((object) "gid");
      jobject1.Value<string>((object) "uid");
      Contact contact = IMO.ContactsManager.GetOrCreateContact(str + ";");
      bool? nullable = jobject1.Value<bool?>((object) "ring");
      bool flag = true;
      if ((nullable.GetValueOrDefault() == flag ? (nullable.HasValue ? 1 : 0) : 0) != 0 && this.CallController == null)
      {
        int num = contact.IsMuted ? 1 : 0;
      }
      List<Contact> contactList = new List<Contact>();
      if (jobject2 != null)
      {
        foreach (KeyValuePair<string, JToken> keyValuePair in jobject2)
        {
          if (keyValuePair.Key != IMO.User.Uid)
            contactList.Add(IMO.ContactsManager.GetOrCreateContact(keyValuePair.Key));
        }
        contact.GroupCallMembers = contactList;
        this.OnGroupCallsListChanged();
      }
      if (!(this.CallController is GroupCallController callController) || callController.CallState != AVState.RECEIVING || !(callController.Buid == contact.Buid) || contactList.Count != 0)
        return;
      this.RemoveCallController();
    }

    private void HandleGroupLeave(JObject data)
    {
      int streamId = data.Value<JObject>((object) "edata").Value<int>((object) "stream_id");
      if (this.CallController == null || !this.CallController.IsGroupCall)
        return;
      ((GroupCallController) this.CallController).OnReleaseStream(streamId);
    }

    public void LeaveGroupCall(string buid)
    {
      BaseManager.Send("groupav", "leave_group", new JObject()
      {
        {
          "uid",
          (JToken) IMO.User.Uid
        },
        {
          "gid",
          (JToken) Contact.GetGid(buid)
        }
      });
    }

    public void HandleStreamsInfo(JObject message)
    {
      string str = Convert.ToString((object) message.Value<JObject>((object) "edata").Value<string>((object) "chat_type"));
      if (str == "audio_chat" || str == "video_chat")
        this.HandleStreamInfoAVChat(message);
      else
        AVManager.log.Debug("Unknown stream info type " + str);
    }

    public void HandleStreamInfoAVChat(JObject message)
    {
      JObject jobject1 = message.Value<JObject>((object) "edata");
      AVManager.log.Debug("edata: " + (object) jobject1);
      Utils.Assert(jobject1 != null, "streams_info edata is null in HandleStreamInfoAVChat");
      bool flag = jobject1.Value<bool>((object) "is_initiator");
      string buid = jobject1.Value<string>((object) "buid");
      JObject jobject2 = jobject1.Value<JObject>((object) "pipe");
      JArray pipes = jobject1.Value<JArray>((object) "pipes") ?? new JArray();
      if (pipes.Count == 0)
      {
        jobject2["port"] = jobject2["port_udp"];
        pipes.Add((JToken) jobject2);
      }
      string chatType = jobject1.Value<string>((object) "chat_type");
      string str1 = jobject2.Value<string>((object) "conv");
      string type1 = jobject1.Value<string>((object) "client_type");
      jobject1.Value<int>((object) "req_id");
      byte[] sharedKey = Convert.FromBase64String(jobject1.Value<string>((object) "shared_key") ?? "");
      byte[] serverKey = Convert.FromBase64String(jobject1.Value<string>((object) "server_key") ?? "");
      byte[] serverCbcKey = Convert.FromBase64String(jobject1.Value<string>((object) "cbc_key") ?? "");
      byte[] peerCbcKey = Convert.FromBase64String(jobject1.Value<string>((object) "peer_cbc_key") ?? "");
      byte[] abVector = Convert.FromBase64String(jobject1.Value<string>((object) "ab_str") ?? "");
      int abFirst = jobject1.Value<int>((object) "ab_first");
      int maxVideoBitrateKbps = jobject1.Value<int>((object) "max_video_bitrate");
      double[] bitrateParams = jobject1.Value<JArray>((object) "bit_params")?.ToObject<double[]>();
      bool errorCorrectionAllowed = jobject1.Value<bool>((object) "error_correction_allowed");
      double[] errorCorrectionParams = jobject1.Value<JArray>((object) "error_correction_params")?.ToObject<double[]>();
      double[] callParams = jobject1.Value<JArray>((object) "call_params")?.ToObject<double[]>();
      double[][] qualityConfigParams = jobject1.Value<JArray>((object) "quality_config")?.ToObject<double[][]>() ?? new double[0][];
      byte[] initiatorProtocolMask = (byte[]) null;
      byte[] receiverProtocolMask = (byte[]) null;
      if (!flag)
      {
        initiatorProtocolMask = Convert.FromBase64String(jobject1.Value<string>((object) "initiator_proto_mask") ?? "");
        receiverProtocolMask = Convert.FromBase64String(jobject1.Value<string>((object) "receiver_proto_mask") ?? "");
      }
      List<string> content = new List<string>();
      string str2 = jobject1.Value<string>((object) "cbc_ticket") ?? "";
      if (!string.IsNullOrEmpty(str2))
        content.Add(str2);
      try
      {
        string[] collection = jobject1.Value<JArray>((object) "cbc_ticket2")?.ToObject<string[]>();
        if (collection != null)
          content.AddRange((IEnumerable<string>) collection);
      }
      catch (Exception ex)
      {
        AVManager.log.Error("invalid cbc ticket list!" + (object) ex, 621, nameof (HandleStreamInfoAVChat));
      }
      if (content.Count > 0 && pipes[0].Value<JToken>((object) "tickets") == null)
        pipes[0][(object) "tickets"] = (JToken) new JArray((object) content);
      if (this.CallController != null)
      {
        if (this.CallController.Buid != buid)
        {
          AVManager.log.Debug("Received push call for wrong chat " + str1 + ", ignoring");
          this.CallController.SendTerminateCallForConvId(str1, "busy");
          return;
        }
        AVManager.log.Debug("Received incoming call " + this.CallController.ConvId + " while in " + str1);
        if (this.CallController.ConvId == str1)
          return;
        if (this.CallController.ChatType == "audio_chat" && chatType == "video_chat")
        {
          AVManager.log.Debug("Received a video call being in an audio call " + str1);
          this.CallController.SendTerminateCallForConvId(str1, "new_incoming_video_while_audio");
          this.CallController.EndCallWithReason("incoming_video_while_audio");
          return;
        }
        if (this.CallController.CallState == AVState.WAITING && !flag)
        {
          this.CallController.Log("new_conv_id", (object) str1);
          this.CallController.EndForReestablishing();
          AVManager.log.Debug("Will reestablish as " + str1);
        }
        else if (!flag || this.CallController.CallState == AVState.RECEIVING)
          return;
      }
      else if (flag)
      {
        AVManager.log.Debug("Unexpected outgoing chat " + str1 + ", ignoring");
        this.TerminateCall(str1, "call_cancelled");
        return;
      }
      if (flag)
      {
        AVManager.log.Debug("initiateCall");
        this.CallController.SetupCallWithConvId(str1, pipes, sharedKey, serverKey, serverCbcKey, peerCbcKey, initiatorProtocolMask, receiverProtocolMask, abVector, abFirst, maxVideoBitrateKbps, bitrateParams, errorCorrectionAllowed, errorCorrectionParams, callParams, qualityConfigParams);
        this.CallController.IsInitiator = true;
        Utils.Assert(type1 != null, "newClientType in AVManager.HandleStreamInfoAVChat is null", false);
        CallController callController = this.CallController;
        string type2;
        if (type1 == null)
          type2 = (string) null;
        else
          type2 = type1.Split(' ')[0];
        callController.SetCallState(AVState.CALLING, type2);
      }
      else
      {
        AVManager.log.Debug("receiveCall");
        this.CallController = new CallController(buid, chatType, (string) null, sharedKey);
        this.CallController.SetupCallWithConvId(str1, pipes, sharedKey, serverKey, serverCbcKey, peerCbcKey, initiatorProtocolMask, receiverProtocolMask, abVector, abFirst, maxVideoBitrateKbps, bitrateParams, errorCorrectionAllowed, errorCorrectionParams, callParams, qualityConfigParams);
        IMO.NavigationManager.NavigateToAVCallPage();
        this.CallController.SetCallState(AVState.RECEIVING, type1);
      }
    }

    public void GetGroupCallMembers(string buid, Action<List<Contact>> callback)
    {
      BaseManager.Send("groupav", "who_there", new Dictionary<string, object>()
      {
        {
          "ssid",
          (object) IMO.Dispatcher.GetSSID()
        },
        {
          "uid",
          (object) IMO.User.Uid
        },
        {
          "gid",
          (object) Contact.GetGid(buid)
        }
      }, (Action<JToken>) (response =>
      {
        if (!(response is JObject jobject2))
          return;
        List<Contact> contactList = new List<Contact>();
        foreach (KeyValuePair<string, JToken> keyValuePair in jobject2)
        {
          if (keyValuePair.Key != IMO.User.Uid)
            contactList.Add(IMO.ContactsManager.GetOrCreateContact(keyValuePair.Key));
        }
        callback(contactList);
        this.OnGroupCallsListChanged();
      }));
    }

    public void TerminateCall(string convId, string reason)
    {
      Dictionary<string, object> dictionary = new Dictionary<string, object>()
      {
        {
          "message",
          (object) new Dictionary<string, object>()
          {
            {
              "type",
              (object) "terminate_call"
            },
            {
              nameof (reason),
              (object) reason
            }
          }
        }
      };
      BaseManager.Send("av", "send_message", new Dictionary<string, object>()
      {
        {
          "conv_id",
          (object) convId
        },
        {
          "msg",
          (object) dictionary
        }
      });
    }

    public async Task HandleFailed(JObject message)
    {
      AVManager.log.Debug("handleFailed:" + (object) message);
      if (this.CallController == null)
        return;
      string str1 = (string) message["buid"];
      string str2 = (string) message["uid"];
      if (this.CallController.ReqId != (int) message["req_id"] || this.CallController.Buid != str1 || this.CallController.CallState != AVState.WAITING)
        return;
      string str3 = (string) message["reason"];
      string message1 = (string) null;
      switch (str3)
      {
        case "offline_imo":
          message1 = "Your buddy is not logged in to imo.";
          goto case "incompatible";
        case "nopoints":
        case "not_buddy":
          message1 = "Your friend must add you into his/her contacts before you can call.";
          goto case "incompatible";
        case "incompatible":
          ImoMessageBoxResult messageBoxResult = await ImoMessageBox.Show(message1);
          this.RemoveCallController();
          break;
        default:
          message1 = "Failed to start a call.";
          goto case "incompatible";
      }
    }
  }
}
