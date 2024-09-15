// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.Managers.IM
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Storage.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;


namespace ImoSilverlightApp.Managers
{
  internal class IM : BaseManager
  {
    public void GetGroupMembers(string gid, Action<JToken> callback)
    {
      BaseManager.Send("im", "get_group_members", new Dictionary<string, object>()
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
          nameof (gid),
          (object) gid
        }
      }, callback);
    }

    public void GetRecentMessags(string buid, Action<JToken> callback)
    {
      Dictionary<string, object> data = new Dictionary<string, object>();
      MessageFactory.AddSsidUidProto(data);
      data.Add(nameof (buid), (object) buid);
      data.Add("version", (object) 2);
      BaseManager.Send("convhistory", "get_recent_messages", data, callback);
    }

    public void GetConversation(
      string buid,
      long startTime,
      long endTime,
      int limit,
      bool fromEnd,
      Action<JToken> callback)
    {
      Dictionary<string, object> data = new Dictionary<string, object>();
      MessageFactory.AddSsidUidProto(data);
      data.Add(nameof (buid), (object) buid);
      data.Add("start_time", (object) startTime);
      if (endTime != -1L)
        data.Add("end_time", (object) endTime);
      if (limit != -1)
        data.Add(nameof (limit), (object) limit);
      data.Add("from_end", (object) fromEnd);
      data.Add("version", (object) 2);
      BaseManager.Send("convhistory", "get_conversation", data, callback);
    }

    public void SendMessage(
      string msg,
      string buid,
      JObject imdata,
      Action<JToken> successCallback,
      Action onDispatcherAck)
    {
      Dictionary<string, object> data = new Dictionary<string, object>();
      MessageFactory.AddSsidUidProto(data);
      data.Add(nameof (buid), (object) buid);
      data.Add(nameof (msg), (object) msg);
      data.Add(nameof (imdata), (object) imdata);
      BaseManager.Send("im", "send_im", data, successCallback, onDispatcherAck);
    }

    public void ClearHistory(Action<JToken> callback)
    {
      BaseManager.Send("convhistory", "clearHistory", new Dictionary<string, object>()
      {
        {
          "accounts",
          (object) JArray.FromObject((object) new JObject[1]
          {
            new JObject()
            {
              {
                "uid",
                (JToken) IMO.User.Uid
              },
              {
                "proto",
                (JToken) "imo"
              }
            }
          })
        }
      }, callback);
    }

    public void MarkMessagesAsRead(string buid, long lastIndex)
    {
      Dictionary<string, object> data = new Dictionary<string, object>();
      MessageFactory.AddSsidUidProto(data);
      data.Add(nameof (buid), (object) buid);
      data.Add("last_index", (object) lastIndex);
      BaseManager.Send("im", "mark_msgs_as_read", data);
    }

    public void SendTypingState(string typingState, string buid, string message)
    {
      Dictionary<string, object> data = new Dictionary<string, object>();
      MessageFactory.AddSsidUidProto(data);
      data.Add(nameof (buid), (object) buid);
      data.Add("typing_state", (object) typingState);
      if (message != null)
        data.Add(nameof (message), (object) message);
      BaseManager.Send("im", "im_typing", data);
    }

    public void OpenChat(string buid)
    {
      Dictionary<string, object> data = new Dictionary<string, object>();
      MessageFactory.AddSsidUidProto(data);
      data.Add(nameof (buid), (object) buid);
      BaseManager.Send("im", "open_chat", data);
    }

    public void CloseChat(string buid)
    {
      Dictionary<string, object> data = new Dictionary<string, object>();
      MessageFactory.AddSsidUidProto(data);
      data.Add(nameof (buid), (object) buid);
      BaseManager.Send("im", "close_chat", data);
    }

    public void BlockContact(string buid, Action<JToken> callback)
    {
      Dictionary<string, object> data = new Dictionary<string, object>();
      MessageFactory.AddSsidUidProto(data);
      data.Add(nameof (buid), (object) buid);
      BaseManager.Send("im", "block_buddy", data, callback);
    }

    public void UnblockContact(string buid, Action<JToken> callback)
    {
      Dictionary<string, object> data = new Dictionary<string, object>();
      MessageFactory.AddSsidUidProto(data);
      data.Add(nameof (buid), (object) buid);
      BaseManager.Send("im", "unblock_buddy", data, callback);
    }

    public void GetBlockedContacts(Action<JToken> callback)
    {
      Dictionary<string, object> data = new Dictionary<string, object>();
      MessageFactory.AddSsidUidProto(data);
      BaseManager.Send("pin", "get_blocked_buddies", data, callback);
    }

    internal void LeaveGroup(string gid)
    {
      Dictionary<string, object> data = new Dictionary<string, object>();
      MessageFactory.AddSsidUidProto(data);
      data.Add(nameof (gid), (object) gid);
      BaseManager.Send("im", "leave_group", data);
    }

    public void MuteGroup(string gid, bool mute, Action ackCallback)
    {
      BaseManager.Send("imogroups", "mute_group", new Dictionary<string, object>()
      {
        {
          "uid",
          (object) IMO.User.Uid
        },
        {
          nameof (gid),
          (object) gid
        },
        {
          nameof (mute),
          (object) mute
        }
      }, onDispatcherAckCallback: ackCallback);
    }

    public void GetRecentMessages(string buid, Action<JToken> callback)
    {
      Dictionary<string, object> data = new Dictionary<string, object>();
      MessageFactory.AddSsidUidProto(data);
      data.Add(nameof (buid), (object) buid);
      data.Add("version", (object) 2);
      BaseManager.Send("convhistory", "get_recent_messages", data, callback);
    }

    public void DeleteChatHistory(
      string buid,
      long startTime,
      long endTime,
      Action<JToken> callback)
    {
      Dictionary<string, object> data = new Dictionary<string, object>();
      MessageFactory.AddSsidUidProto(data);
      data.Add(nameof (buid), (object) buid);
      data.Add("start_time", (object) startTime);
      data.Add("end_time", (object) endTime);
      BaseManager.Send("convhistory", "delete_messages", data, callback);
    }

    public void DeleteChatHistory(string buid, Action<JToken> callback)
    {
      this.DeleteChatHistory(buid, -1L, -1L, callback);
    }

    public void DeleteMessage(Message message)
    {
      if (message == null || message.Timestamp <= -1L)
        return;
      this.DeleteChatHistory(message.ConvBuid, message.TimestampNano, message.TimestampNano, (Action<JToken>) (result =>
      {
        Conversation conversation = IMO.ConversationsManager.GetOrCreateConversation(message.ConvBuid);
        switch (message)
        {
          case PhotoMessage _:
            IMO.Pixel.DeletePhoto(message.ConvBuid, (message as PhotoMessage).PhotoID, (Action<JToken>) (_ =>
            {
              IMO.PhotoStreamsManager.GetOrCreatePhotoStream(message.ConvBuid).InvalidateIsSynced();
              conversation.DeleteMessage(message);
            }));
            break;
          case VideoMessage _:
            IMO.Pixel.DeletePhoto(message.ConvBuid, (message as VideoMessage).VideoID, (Action<JToken>) (_ =>
            {
              IMO.PhotoStreamsManager.GetOrCreatePhotoStream(message.ConvBuid).InvalidateIsSynced();
              conversation.DeleteMessage(message);
            }));
            break;
          case AudioMessage _:
            IMO.Pixel.DeletePhoto(message.ConvBuid, (message as AudioMessage).AudioID, (Action<JToken>) (_ => conversation.DeleteMessage(message)));
            break;
        }
        conversation.DeleteMessage(message);
      }));
    }

    public void DeleteMessage(string buid, long timestamp, Action<JToken> callback = null)
    {
      this.DeleteChatHistory(buid, timestamp, timestamp, callback);
    }

    public void GetAccountData(Action<JToken> callback)
    {
      BaseManager.Send("take_out", "get_data", new Dictionary<string, object>()
      {
        {
          "ssid",
          (object) IMO.Dispatcher.GetSSID()
        },
        {
          "uid",
          (object) IMO.User.Uid
        }
      }, callback);
    }
  }
}
