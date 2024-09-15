// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.Agents.ScheduledAgent
// Assembly: ImoSilverlightApp.Agents, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E5312DD8-91BD-43C5-A29E-03979D5872E9
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.Agents.dll

using Microsoft.Phone.Networking.Voip;
using Microsoft.Phone.Scheduler;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.IsolatedStorage;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Phone.Networking.Voip;
using Windows.Web;
using Windows.Web.Http;

namespace ImoSilverlightApp.Agents
{
  public class ScheduledAgent : ScheduledTaskAgent
  {
    private static readonly VoipCallCoordinator callCoordinator;
    private static readonly string installFolder;
    private static readonly Uri brandingImageUri;
    private static readonly Uri ringtoneUri;
    private static readonly EventWaitHandle waitHandle;
    private static readonly EventWaitHandle notInCallWaitHandle;
    private string streamsInfo;
    private string convId;
    private static VoipPhoneCall incomingCall;
    private static bool isVideoCall;
    private static bool isGroupCall;
    private static readonly string IMO_HOST = "imo.im";
    private static readonly string RestRpcPostUrl = "https://" + ScheduledAgent.IMO_HOST + "/imo";
    private const string keepAliveAgentId = "KeepAliveAgent";
    private const string incomingCallAgentId = "IncomingCallAgent";
    private bool isIncomingCallAgent;

    static ScheduledAgent()
    {
      Deployment.Current.Dispatcher.BeginInvoke((Action) (() => Application.Current.UnhandledException += new EventHandler<ApplicationUnhandledExceptionEventArgs>(ScheduledAgent.OnUnhandledException)));
      ScheduledAgent.callCoordinator = VoipCallCoordinator.GetDefault();
      ScheduledAgent.installFolder = Package.Current.InstalledLocation.Path + "\\";
      ScheduledAgent.brandingImageUri = new Uri(ScheduledAgent.installFolder + "\\Assets\\BadgeLogo.png");
      ScheduledAgent.ringtoneUri = new Uri(ScheduledAgent.installFolder + "\\Assets\\ringing.wma");
      ScheduledAgent.waitHandle = new EventWaitHandle(false, EventResetMode.AutoReset, "ImoSilverlightAppVoipAgent");
      ScheduledAgent.notInCallWaitHandle = new EventWaitHandle(true, EventResetMode.ManualReset, "ImoSilverlightAppNotInCallHandle");
    }

    private static void OnUnhandledException(
      object sender,
      ApplicationUnhandledExceptionEventArgs e)
    {
      if (!Debugger.IsAttached)
        return;
      Debugger.Break();
    }

    protected override async void OnInvoke(ScheduledTask task)
    {
      switch (task)
      {
        case VoipHttpIncomingCallTask incomingCallTask:
          this.isIncomingCallAgent = true;
          if (!ScheduledAgent.notInCallWaitHandle.WaitOne(0) || ScheduledAgent.incomingCall != null)
          {
            this.Complete();
            break;
          }
          string streamsInfo = (string) null;
          string convId = (string) null;
          string alias;
          string callType;
          try
          {
            string[] strArray = Encoding.UTF8.GetString(incomingCallTask.MessageBody, 0, incomingCallTask.MessageBody.Length).Split('\n');
            alias = strArray[0];
            if (strArray[1].StartsWith("group"))
            {
              ScheduledAgent.isVideoCall = true;
              ScheduledAgent.isGroupCall = true;
              callType = "Group ";
            }
            else if (strArray[1].StartsWith("video"))
            {
              ScheduledAgent.isVideoCall = true;
              ScheduledAgent.isGroupCall = false;
              streamsInfo = strArray[2];
              callType = "Video ";
            }
            else if (strArray[1].StartsWith("audio"))
            {
              ScheduledAgent.isVideoCall = false;
              ScheduledAgent.isGroupCall = false;
              streamsInfo = strArray[2];
              callType = "";
            }
            else
            {
              this.Complete();
              break;
            }
            int num = strArray[1].IndexOf(';');
            if (num > 0)
            {
              if (num + 1 < strArray[1].Length)
                convId = strArray[1].Substring(num + 1);
            }
          }
          catch (Exception ex)
          {
            this.Complete();
            break;
          }
          bool incomingCallProcessingStarted = true;
          bool flag = !ScheduledAgent.isGroupCall;
          if (flag)
            flag = await this.ShouldStopRinging(convId, new int?(1500));
          if (flag)
          {
            this.Complete();
            break;
          }
          try
          {
            TimeSpan timeSpan = TimeSpan.FromSeconds(60.0);
            ScheduledAgent.callCoordinator.RequestNewIncomingCall("/MainPage.xaml?incoming=true" + (ScheduledAgent.isGroupCall ? "&gid=" + convId : ""), alias, "Incoming " + callType + "Call", (Uri) null, "IMO", ScheduledAgent.brandingImageUri, "", ScheduledAgent.ringtoneUri, (VoipCallMedia) 1, timeSpan, ref ScheduledAgent.incomingCall);
            this.streamsInfo = streamsInfo;
            this.convId = convId;
            VoipPhoneCall incomingCall1 = ScheduledAgent.incomingCall;
            // ISSUE: method pointer
            WindowsRuntimeMarshal.AddEventHandler<TypedEventHandler<VoipPhoneCall, CallAnswerEventArgs>>(new Func<TypedEventHandler<VoipPhoneCall, CallAnswerEventArgs>, EventRegistrationToken>(incomingCall1.add_AnswerRequested), new Action<EventRegistrationToken>(incomingCall1.remove_AnswerRequested), new TypedEventHandler<VoipPhoneCall, CallAnswerEventArgs>((object) this, __methodptr(IncomingCall_AnswerRequested)));
            VoipPhoneCall incomingCall2 = ScheduledAgent.incomingCall;
            // ISSUE: method pointer
            WindowsRuntimeMarshal.AddEventHandler<TypedEventHandler<VoipPhoneCall, CallRejectEventArgs>>(new Func<TypedEventHandler<VoipPhoneCall, CallRejectEventArgs>, EventRegistrationToken>(incomingCall2.add_RejectRequested), new Action<EventRegistrationToken>(incomingCall2.remove_RejectRequested), new TypedEventHandler<VoipPhoneCall, CallRejectEventArgs>((object) this, __methodptr(IncomingCall_RejectRequested)));
            VoipPhoneCall incomingCall3 = ScheduledAgent.incomingCall;
            // ISSUE: method pointer
            WindowsRuntimeMarshal.AddEventHandler<TypedEventHandler<VoipPhoneCall, CallStateChangeEventArgs>>(new Func<TypedEventHandler<VoipPhoneCall, CallStateChangeEventArgs>, EventRegistrationToken>(incomingCall3.add_EndRequested), new Action<EventRegistrationToken>(incomingCall3.remove_EndRequested), new TypedEventHandler<VoipPhoneCall, CallStateChangeEventArgs>((object) this, __methodptr(IncomingCall_EndRequested)));
          }
          catch (Exception ex)
          {
            incomingCallProcessingStarted = false;
          }
          if (!incomingCallProcessingStarted)
          {
            this.Complete();
            break;
          }
          while (!ScheduledAgent.isGroupCall && ScheduledAgent.incomingCall != null)
          {
            if (await this.ShouldStopRinging(convId))
            {
              if (ScheduledAgent.incomingCall == null)
                return;
              try
              {
                ScheduledAgent.incomingCall.NotifyCallEnded();
              }
              catch (Exception ex)
              {
              }
              this.Complete();
              return;
            }
            await Task.Delay(1000);
          }
          alias = (string) null;
          callType = (string) null;
          streamsInfo = (string) null;
          convId = (string) null;
          break;
        case VoipKeepAliveTask _:
          this.isIncomingCallAgent = false;
          this.Complete();
          break;
        default:
          throw new InvalidOperationException(string.Format("Unknown scheduled task type {0}", (object) task.GetType()));
      }
    }

    private void IncomingCall_EndRequested(VoipPhoneCall sender, CallStateChangeEventArgs args)
    {
      this.Complete();
    }

    private void IncomingCall_RejectRequested(VoipPhoneCall sender, CallRejectEventArgs args)
    {
      this.Complete();
    }

    private async void IncomingCall_AnswerRequested(VoipPhoneCall sender, CallAnswerEventArgs args)
    {
      ScheduledAgent.incomingCall = (VoipPhoneCall) null;
      try
      {
        if (!ScheduledAgent.isGroupCall)
        {
          if (this.streamsInfo == null)
          {
            this.Complete();
            return;
          }
          byte[] bytes = Encoding.UTF8.GetBytes(this.streamsInfo);
          using (IsolatedStorageFileStream file = IsolatedStorageFile.GetUserStoreForApplication().CreateFile("streams_info"))
            file.Write(bytes, 0, bytes.Length);
        }
        else
        {
          if (this.convId == null)
          {
            this.Complete();
            return;
          }
          byte[] bytes = Encoding.UTF8.GetBytes("group_call:" + this.convId);
          using (IsolatedStorageFileStream file = IsolatedStorageFile.GetUserStoreForApplication().CreateFile("streams_info"))
            file.Write(bytes, 0, bytes.Length);
        }
      }
      catch (Exception ex)
      {
        sender.NotifyCallEnded();
        this.Complete();
        return;
      }
      ScheduledAgent.waitHandle.Set();
      sender.NotifyCallActive();
      if (ScheduledAgent.isVideoCall)
      {
        await Task.Delay(2000);
      }
      else
      {
        await Task.Delay(10000);
        ScheduledAgent.notInCallWaitHandle.WaitOne();
      }
      sender.NotifyCallEnded();
      this.Complete();
    }

    protected override void OnCancel() => this.Complete();

    private void Complete()
    {
      ScheduledAgent.incomingCall = (VoipPhoneCall) null;
      this.NotifyComplete();
    }

    private async Task<bool> ShouldStopRinging(string convId, int? timeout = null)
    {
      if (convId == null)
        return false;
      IHttpContent content = (IHttpContent) new HttpFormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>) new KeyValuePair<string, string>[2]
      {
        new KeyValuePair<string, string>("method", "rest_rpc"),
        new KeyValuePair<string, string>("data", this.GetRestMessage(convId))
      });
      Task<string> task = Task.Run<string>((Func<Task<string>>) (async () =>
      {
        HttpClient httpClient = new HttpClient();
        HttpResponseMessage response = (HttpResponseMessage) null;
        try
        {
          response = await httpClient.PostAsync(new Uri(ScheduledAgent.RestRpcPostUrl), content);
        }
        catch (Exception ex)
        {
          WebError.GetStatus(ex.HResult);
          return "";
        }
        if (response != null && !response.IsSuccessStatusCode)
          return "";
        try
        {
          return await response.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
          return "";
        }
      }));
      if (!timeout.HasValue)
      {
        string str = await task;
        return task.Result.Contains("{\"edata\":false}");
      }
      object obj = (object) task;
      Task task1 = await Task.WhenAny((Task) task, Task.Delay(timeout.Value));
      if (obj != task1)
        return false;
      obj = (object) null;
      return task.Result.Contains("{\"edata\":false}");
    }

    private string GetRestMessage(string convId)
    {
      return "{\n  \"ack\": 0,\n  \"ssid\": null,\n  \"messages\": [\n    {\n      \"seq\": 0,\n      \"from\": {\n        \"system\": \"client\",\n        \"ssid\": null,\n        \"udid\": null,\n        \"uid\": \"Not registered account\"\n      },\n      \"to\": {\n        \"system\": \"av\"\n      },\n      \"data\": {\n        \"method\": \"is_call_ringing\",\n        \"data\": {\n          \"conv_id\": \"" + convId + "\"\n        }\n      }\n    }\n  ]\n}";
    }
  }
}
