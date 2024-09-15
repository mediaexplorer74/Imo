// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.Storage.Models.Conversation
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Collections;
using ImoSilverlightApp.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;


namespace ImoSilverlightApp.Storage.Models
{
  public class Conversation : ModelBase
  {
    private static readonly Logger log = LogManager.GetLogger(typeof (Conversation).Name);
    private bool isSyncingRecent;
    private bool isLoadingHistory;
    private bool isGettingRecent;
    private int storageMessagesCount;
    private const int NUM_LOAD_MORE_MESSAGES = 30;
    private const int REDUCED_MESSAGES_COUNT = 30;
    private DateTime lastOpenChat = DateTime.MinValue;
    private DateTime lastUserActivity = DateTime.MinValue;
    private string buid;
    [JsonProperty(PropertyName = "alias")]
    private string alias;
    [JsonProperty(PropertyName = "last_activity_timestamp")]
    private long lastActivityTimestamp = -1;
    [JsonProperty(PropertyName = "last_message_timestamp")]
    private long lastMessageTimestamp = -1;
    private SortedObservableCollection<Message> messages;
    private long lastReadIndex = -1;
    private long lastRecvIndex = -1;
    private bool isOpen;
    private bool loadedAllHistory;
    private bool showInChatsList;
    private int unreadMessagesCount;
    private Contact contact;
    private int IN_CHAT_TIMEOUT = 60000;
    private int TYPER_TIMEOUT = 10000;
    private IList<Contact> seenAllContacts;
    private IDictionary<Contact, long> inChatContacts;
    private IDictionary<Contact, Conversation.TyperData> typers;

    public event EventHandler<EventArg<Message>> MessageAdded;

    public Conversation(string buid)
    {
      this.buid = buid;
      this.contact = IMO.ContactsManager.GetOrCreateContact(buid);
      this.inChatContacts = (IDictionary<Contact, long>) new Dictionary<Contact, long>();
      this.seenAllContacts = (IList<Contact>) new List<Contact>();
      this.typers = (IDictionary<Contact, Conversation.TyperData>) new Dictionary<Contact, Conversation.TyperData>();
      this.messages = new SortedObservableCollection<Message>((Comparer<Message>) new Conversation.MessageComparer());
      this.messages.CollectionChanged += new NotifyCollectionChangedEventHandler(this.Messages_CollectionChanged);
      this.InitMessagesFromStorage();
    }

    private void Messages_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      if (e.Action == NotifyCollectionChangedAction.Add)
      {
        if (!(e.NewItems[0] as Message).IsFromStorage())
          return;
        ++this.storageMessagesCount;
      }
      else if (e.Action == NotifyCollectionChangedAction.Remove)
      {
        if (!(e.OldItems[0] as Message).IsFromStorage())
          return;
        --this.storageMessagesCount;
      }
      else
      {
        if (e.Action != NotifyCollectionChangedAction.Reset)
          return;
        this.storageMessagesCount = this.messages.Where<Message>((Func<Message, bool>) (x => x.IsFromStorage())).Count<Message>();
      }
    }

    private void InitMessagesFromStorage()
    {
      foreach (JToken jtoken in IMO.ApplicationStorage.GetConversationHistory(this.buid))
      {
        MessageType messageType = (MessageType) Enum.Parse(typeof (MessageType), jtoken.Value<string>((object) "message_type"), true);
        this.messages.Add(Message.CreateFromJObject(jtoken.ToObject<JObject>(), MessageOrigin.STORAGE, messageType));
      }
    }

    internal void UpdateNewDayMessagesTimestampUI()
    {
      foreach (Message message in this.messages)
        message.UpdateTimestampUI();
    }

    internal void ClearUnsentMessages()
    {
      foreach (Message message in this.messages)
      {
        if (message.IsPendingSend)
          this.messages.Remove(message);
      }
    }

    internal void InvalidatePendingRequests()
    {
      this.isSyncingRecent = false;
      this.isLoadingHistory = false;
      this.IsGettingRecent = false;
    }

    internal void ClearMessages()
    {
      this.messages.Clear();
      this.lastReadIndex = -1L;
      this.lastRecvIndex = -1L;
      this.UnreadMessagesCount = 0;
      this.loadedAllHistory = false;
      foreach (Contact contact in this.seenAllContacts.ToList<Contact>())
        this.RemoveSeenAllContact(contact);
      foreach (Contact contact in this.typers.Keys.ToList<Contact>())
        this.RemoveTyper(contact);
      foreach (Contact contact in this.inChatContacts.Keys.ToList<Contact>())
        this.RemoveInChatContact(contact);
    }

    public void ResetUnreadMessages()
    {
      int num = 0;
      foreach (Message message in this.messages.Reverse())
      {
        if (this.UnreadMessagesCount - num != 0)
        {
          if (message.IsUnread)
          {
            message.IsUnread = false;
            ++num;
          }
        }
        else
          break;
      }
      this.UnreadMessagesCount -= num;
    }

    [JsonProperty(PropertyName = "buid")]
    public string Buid => this.buid;

    public bool IsGroup => this.Buid != null && this.Buid.EndsWith(";");

    internal void OpenChat()
    {
      this.IsOpen = true;
      IMO.IM.OpenChat(this.buid);
      this.lastOpenChat = DateTime.Now;
      this.contact.SyncGroupMembers();
    }

    internal void CloseChat()
    {
      this.IsOpen = false;
      IMO.IM.CloseChat(this.buid);
    }

    public Contact Contact => this.contact;

    public bool IsOpen
    {
      get => this.isOpen;
      private set
      {
        if (this.isOpen == value)
          return;
        this.isOpen = value;
        this.OnPropertyChanged(nameof (IsOpen));
      }
    }

    public bool ShowInChatsList
    {
      get => this.showInChatsList;
      set
      {
        if (this.showInChatsList == value)
          return;
        this.showInChatsList = value;
        this.OnPropertyChanged(nameof (ShowInChatsList));
      }
    }

    internal bool IsUserWatchingIt()
    {
      return this.IsOpen && (DateTime.Now - this.LastUserActivity).TotalSeconds <= 30.0;
    }

    public SortedObservableCollection<Message> Messages => this.messages;

    public long LastRecvIndex
    {
      get => this.lastRecvIndex;
      set
      {
        if (this.lastRecvIndex == value)
          return;
        this.lastRecvIndex = value;
        this.OnPropertyChanged(nameof (LastRecvIndex));
      }
    }

    public string Alias
    {
      get => this.alias;
      set
      {
        if (!(this.alias != value))
          return;
        this.alias = value;
        this.OnPropertyChanged(nameof (Alias));
      }
    }

    public long LastActivityTimestamp
    {
      get => this.lastActivityTimestamp;
      set
      {
        if (this.lastActivityTimestamp == value)
          return;
        this.lastActivityTimestamp = value;
        this.OnPropertyChanged(nameof (LastActivityTimestamp));
      }
    }

    public long LastMessageTimestamp
    {
      get => this.lastMessageTimestamp;
      set
      {
        if (this.lastMessageTimestamp == value)
          return;
        this.lastMessageTimestamp = value;
        this.OnPropertyChanged(nameof (LastMessageTimestamp));
      }
    }

    private void OnMessageAdded(Message message)
    {
      EventHandler<EventArg<Message>> messageAdded = this.MessageAdded;
      if (messageAdded == null)
        return;
      messageAdded((object) this, new EventArg<Message>(message));
    }

    public DateTime LastUserActivity => this.lastUserActivity;

    public void UpdateLastUserActivity()
    {
      this.lastUserActivity = DateTime.Now;
      IMO.Session.UpdateLastUserActivity();
      if ((this.lastUserActivity - this.lastOpenChat).TotalSeconds < 30.0)
        return;
      this.OpenChat();
    }

    public int UnreadMessagesCount
    {
      get => this.unreadMessagesCount;
      set
      {
        if (this.unreadMessagesCount == value)
          return;
        this.unreadMessagesCount = value;
        this.OnPropertyChanged(nameof (UnreadMessagesCount));
        this.OnPropertyChanged("HasUnreadMessages");
      }
    }

    public bool HasUnreadMessages => this.unreadMessagesCount > 0;

    internal IList<Message> GetMessages() => (IList<Message>) this.messages.ToList<Message>();

    public bool IsLastMessageSeen
    {
      get
      {
        Message lastMessage = this.GetLastMessage();
        return lastMessage != null && lastMessage.Seen;
      }
    }

    public bool LoadedAllHistory
    {
      get => this.loadedAllHistory;
      internal set
      {
        if (this.loadedAllHistory == value)
          return;
        this.loadedAllHistory = value;
        this.OnPropertyChanged(nameof (LoadedAllHistory));
      }
    }

    public Message GetLastMessage() => this.messages.Last;

    private long GetLastMessageTimestampNano()
    {
      foreach (Message message in this.messages.Reverse())
      {
        if (message.TimestampNano > 0L)
          return message.TimestampNano;
      }
      return -1;
    }

    private long GetLastMessageSenderTimestampNano()
    {
      foreach (Message message in this.messages.Reverse())
      {
        if (!message.IsPendingSend)
          return message.SenderTimestampNano;
      }
      return -1;
    }

    internal void MarkSeenMessages(long timestamp_nano)
    {
      foreach (Message message in this.messages.Reverse())
      {
        if (message.Seen)
          break;
        if (message.TimestampNano <= timestamp_nano)
          message.Seen = true;
      }
    }

    internal void RemovePhotoMessage(string objectId)
    {
      foreach (Message message in this.messages)
      {
        if (message is PhotoMessage && (message as PhotoMessage).PhotoID == objectId)
        {
          this.messages.Remove(message);
          break;
        }
      }
    }

    private void InternalAddMessage(Message message, bool dontUpdateStorage = false)
    {
      if (this.messages.Contains(message))
      {
        Message message1 = this.messages.Get(message);
        if (message1.IsFromStorage())
        {
          this.messages.Remove(message1);
        }
        else
        {
          if (!message.IsUnread || message1.IsUnread)
            return;
          message1.IsUnread = true;
          ++this.UnreadMessagesCount;
          return;
        }
      }
      if (!message.IsFromStorage() && !message.IsPendingSend)
        this.ClearStorageMessages();
      if (message.IsUnread)
        ++this.UnreadMessagesCount;
      if (this.Messages.Contains(message))
        return;
      this.MaybeActivateConversation(message);
      this.Messages.Add(message);
      if (this.lastMessageTimestamp == -1L || this.lastMessageTimestamp < message.Timestamp)
      {
        this.LastMessageTimestamp = message.Timestamp;
        this.SaveToStorage();
      }
      Message after = this.messages.GetAfter(message);
      Message before = this.messages.GetBefore(message);
      if (!message.IsPendingSend)
        this.TryMergeMessages(message, after);
      this.TryMergeMessages(before, message);
      switch (message.Origin)
      {
        case MessageOrigin.RECV:
        case MessageOrigin.REFLECT:
        case MessageOrigin.UNREAD:
        case MessageOrigin.SEND:
          this.ClearSeenAllContacts();
          break;
      }
      this.MarkSeenMessagesFromSeenSet(message);
      this.OnMessageAdded(message);
      if (!dontUpdateStorage)
        this.SaveMessagesToStorage(message);
      this.OnPropertyChanged("IsInChat");
      this.OnPropertyChanged("IsLastMessageSeen");
    }

    private void ClearStorageMessages()
    {
      if (this.storageMessagesCount <= 0)
        return;
      foreach (Message message in this.messages.ToList<Message>())
      {
        if (message.IsFromStorage())
          this.messages.Remove(message);
        if (this.storageMessagesCount == 0)
          break;
      }
    }

    private void SaveMessagesToStorage(Message message)
    {
      List<Message> list = this.messages.Reverse().Where<Message>((Func<Message, bool>) (x => !x.IsPendingSend)).Take<Message>(30).ToList<Message>();
      if (!list.Contains(message))
        return;
      IMO.ApplicationStorage.AddConversationHistory(this.buid, (IList<Message>) list);
    }

    private void MaybeActivateConversation(Message message)
    {
      switch (message.Origin)
      {
        case MessageOrigin.RECENT:
          this.ActivateConversation(message.Timestamp, false);
          break;
        case MessageOrigin.RECV:
        case MessageOrigin.REFLECT:
        case MessageOrigin.UNREAD:
          this.ActivateConversation(message.Timestamp, true);
          break;
        case MessageOrigin.SEND:
          this.ActivateConversation(Utils.GetTimestamp(), true);
          break;
      }
    }

    internal void ActivateConversation(long timestamp, bool showInChats)
    {
      if (timestamp <= this.LastActivityTimestamp)
        return;
      this.LastActivityTimestamp = timestamp;
      if (showInChats)
      {
        this.ShowInChatsList = true;
        if (!this.contact.IsBuddy)
          IMO.ApplicationStorage.AddContact(this.contact);
      }
      if (this.alias != this.contact.Alias)
        this.Alias = this.contact.Alias;
      this.SaveToStorage();
    }

    private void MarkSeenMessagesFromSeenSet(Message message)
    {
      IList<string> seenSet = message.SeenSet;
      if (seenSet == null)
        return;
      foreach (string buid in (IEnumerable<string>) seenSet)
      {
        if (buid != IMO.User.Uid)
          this.SetMemberSeenMessage(IMO.ContactsManager.GetOrCreateContact(buid), message.TimestampNano);
      }
    }

    private void TryMergeMessages(Message prevMsg, Message mergeMsg)
    {
      if (mergeMsg == null || prevMsg == null)
        return;
      if (mergeMsg.Author != prevMsg.Author || mergeMsg.MessageType != prevMsg.MessageType || !(prevMsg is TextMessage))
      {
        mergeMsg.MergeType = MessageMergeState.NONE;
      }
      else
      {
        if (!(prevMsg.Author == mergeMsg.Author) || prevMsg.MessageType != mergeMsg.MessageType)
          return;
        if (((mergeMsg.TimestampNano >= 0L ? Utils.TimestampToDateTime(mergeMsg.Timestamp) : DateTime.Now) - (prevMsg.TimestampNano >= 0L ? Utils.TimestampToDateTime(prevMsg.Timestamp) : DateTime.Now)).TotalMinutes > 1.0)
          mergeMsg.MergeType = MessageMergeState.NOHEAD;
        else
          mergeMsg.MergeType = MessageMergeState.MERGE;
      }
    }

    internal void MarkMessagesAsRead(long index = -1)
    {
      if (this.unreadMessagesCount == 0)
        return;
      int num = 0;
      foreach (Message message in this.messages.Reverse())
      {
        if (this.unreadMessagesCount - num != 0)
        {
          if (message.IsUnread && (index == -1L || message.Index <= index))
          {
            message.IsUnread = false;
            ++num;
          }
        }
        else
          break;
      }
      this.UnreadMessagesCount -= num;
      if (index == -1L)
      {
        IMO.IM.MarkMessagesAsRead(this.buid, this.LastRecvIndex);
        this.lastReadIndex = this.LastRecvIndex;
      }
      else
        this.lastReadIndex = Math.Max(this.lastReadIndex, index);
    }

    public bool HasMessages() => !this.Messages.IsEmpty;

    public Message SendMessage(string msg, JObject imdata, Action ackCallback = null)
    {
      Message message = Message.CreateFromJObject(new JObject()
      {
        {
          "buid",
          (JToken) this.buid
        },
        {
          nameof (msg),
          (JToken) msg
        },
        {
          nameof (imdata),
          (JToken) imdata
        }
      }, MessageOrigin.SEND, MessageType.Sent);
      message.SentTimestamp = Utils.GetTimestamp();
      this.InternalAddMessage(message);
      string unsentMsgID = IMO.ApplicationStorage.AddUnackedMessage(this.buid, msg, imdata);
      Action<JToken> successCallback = (Action<JToken>) (response =>
      {
        if (response == null)
          return;
        this.Messages.Remove(message);
        message.TimestampNano = response.Value<long>((object) "timestamp_nano");
        message.SenderTimestampNano = message.TimestampNano;
        this.InternalAddMessage(message);
      });
      Action onDispatcherAck = (Action) (() =>
      {
        message.Acked = true;
        IMO.ApplicationStorage.RemoveUnackedMessage(unsentMsgID);
        if (ackCallback == null)
          return;
        ackCallback();
      });
      IMO.IM.SendMessage(msg, this.buid, imdata, successCallback, onDispatcherAck);
      return message;
    }

    private void SendFileUploadedMessage(string fileUrl, string localPath, Action ackCallback)
    {
      FileMessage fileMessage = this.SendMessage("I sent you a file through imo: " + fileUrl, new JObject()
      {
        {
          "type",
          (JToken) "file_uploaded"
        },
        {
          "url",
          (JToken) fileUrl
        }
      }, ackCallback) as FileMessage;
      fileMessage.LocalPath = localPath;
      fileMessage.IsDownloaded = true;
    }

    public Action SendFile(
      string filePath,
      Action<string> callback = null,
      Action<double> progressCallback = null)
    {
      IMO.MonitorLog.Log("send_files", "send_start");
      return IMO.FileUploader.UploadFile(filePath, (Action<string, JObject>) ((error, result) =>
      {
        if (error != null)
        {
          IMO.MonitorLog.Log("send_files", "send_error");
          if (callback == null)
            return;
          callback(error);
        }
        else if (result == null)
          callback((string) null);
        else
          this.SendFileUploadedMessage(result.Value<string>((object) "file_url"), filePath, (Action) (() =>
          {
            IMO.MonitorLog.Log("send_files", "send_success");
            if (callback == null)
              return;
            callback((string) null);
          }));
      }), progressCallback);
    }

    public async Task<string> SendPhoto(
      string filePath,
      Action<string> successCallback = null,
      Action<double> progressCallback = null)
    {
      PhotoMessage message = PhotoMessage.MakePhotoMessage(this.buid, filePath);
      this.InternalAddMessage((Message) message);
      return await IMO.MediaUploader.UploadImage(filePath, (Action<string>) (photoId =>
      {
        if (!string.IsNullOrEmpty(photoId))
          IMO.Pixel.SharePhoto(this.buid, photoId, (Action<JToken>) (result =>
          {
            JObject jobject = (JObject) ((JArray) result)[0];
            this.messages.Remove((Message) message);
            message.TimestampNano = jobject.Value<long>((object) "timestamp_nano");
            message.Acked = true;
            message.SetPhotoId(jobject.Value<string>((object) "object_id"), jobject.Value<JObject>((object) "type_specific_params"));
            this.messages.Add((Message) message);
            IMO.PhotoStreamsManager.GetOrCreatePhotoStream(this.buid).InvalidateIsSynced();
          }));
        if (successCallback == null)
          return;
        successCallback(photoId);
      }), progressCallback);
    }

    public static async Task<string> SendPhotoToMembers(
      string filePath,
      IEnumerable<Conversation> conversations,
      Action<string> successCallback = null,
      Action<double> progressCallback = null)
    {
      Dictionary<string, PhotoMessage> sentMessages = new Dictionary<string, PhotoMessage>();
      foreach (Conversation conversation in conversations)
      {
        PhotoMessage photoMessage = PhotoMessage.MakePhotoMessage(conversation.Buid, filePath);
        conversation.InternalAddMessage((Message) photoMessage);
        sentMessages[conversation.Buid] = photoMessage;
      }
      return await IMO.MediaUploader.UploadImage(filePath, (Action<string>) (photoId =>
      {
        if (!string.IsNullOrEmpty(photoId))
        {
          foreach (Conversation conversation1 in conversations)
          {
            Conversation conversation = conversation1;
            IMO.Pixel.SharePhoto(conversation.Buid, photoId, (Action<JToken>) (result =>
            {
              PhotoMessage photoMessage = sentMessages[conversation.Buid];
              JObject jobject = (JObject) ((JArray) result)[0];
              conversation.messages.Remove((Message) photoMessage);
              photoMessage.TimestampNano = jobject.Value<long>((object) "timestamp_nano");
              photoMessage.Acked = true;
              photoMessage.SetPhotoId(jobject.Value<string>((object) "object_id"), jobject.Value<JObject>((object) "type_specific_params"));
              conversation.messages.Add((Message) photoMessage);
            }));
          }
        }
        if (successCallback == null)
          return;
        successCallback(photoId);
      }), progressCallback);
    }

    public async Task<string> SendVideo(string filePath, Action<string> successCallback = null)
    {
      VideoMessage message = VideoMessage.MakeVideoMessage(this.buid, filePath);
      this.InternalAddMessage((Message) message);
      return await IMO.MediaUploader.UploadVideo(filePath, (Action<string>) (videoId =>
      {
        message.ProgressPercent = -1;
        this.SendUploadedVideo(videoId, message);
        if (successCallback == null)
          return;
        successCallback(videoId);
      }), (Action<double>) (progress => message.ProgressPercent = (int) (progress * 100.0)));
    }

    internal void SendUploadedVideo(string videoId, VideoMessage previewMessage)
    {
      if (string.IsNullOrEmpty(videoId))
        return;
      IMO.Pixel.ShareVideo(this.buid, videoId, (Action<JToken>) (result =>
      {
        JObject jobject = (JObject) ((JArray) result)[0];
        this.messages.Remove((Message) previewMessage);
        previewMessage.TimestampNano = jobject.Value<long>((object) "timestamp_nano");
        previewMessage.Acked = true;
        previewMessage.SetVideoId(jobject.Value<string>((object) "object_id"), jobject.Value<JObject>((object) "type_specific_params"));
        this.messages.Add((Message) previewMessage);
        IMO.PhotoStreamsManager.GetOrCreatePhotoStream(this.buid).InvalidateIsSynced();
      }));
    }

    internal void SendSticker(Sticker sticker)
    {
      IMO.MonitorLog.Log("send_sticker", "send_sticker_start");
      this.SendMessage("", sticker.GetIMData());
    }

    internal VideoMessage AddSendVideoMessage(string filePath)
    {
      VideoMessage videoMessage = VideoMessage.MakeVideoMessage(this.buid, filePath);
      this.InternalAddMessage((Message) videoMessage);
      return videoMessage;
    }

    private void UpdateContactFromMesssage(Message message)
    {
      Contact contact1 = IMO.ContactsManager.GetOrCreateContact(message.Buid);
      switch (message.Origin)
      {
        case MessageOrigin.RECENT:
        case MessageOrigin.HISTORY:
          if (message.IsMyMessage)
            break;
          if (contact1.Alias == null)
          {
            contact1.Alias = message.AuthorAlias;
            contact1.UpdateStorageIfNeeded();
          }
          if (this.Alias == null)
          {
            this.Alias = message.Alias;
            this.SaveToStorage();
          }
          if (!message.IsGroup)
            break;
          Contact contact2 = IMO.ContactsManager.GetOrCreateContact(message.ConvBuid);
          if (contact2.Alias == null)
          {
            contact2.Alias = message.Alias;
            contact2.UpdateStorageIfNeeded();
          }
          if (contact2.Icon != null)
            break;
          contact2.Icon = message.Icon;
          contact2.UpdateStorageIfNeeded();
          break;
        case MessageOrigin.RECV:
        case MessageOrigin.UNREAD:
          if (contact1.Alias != message.AuthorAlias)
          {
            contact1.Alias = message.AuthorAlias;
            contact1.UpdateStorageIfNeeded();
          }
          if (this.Alias != message.Alias)
          {
            this.Alias = message.Alias;
            this.SaveToStorage();
          }
          if (contact1.Icon != message.AuthorIcon && message.AuthorIcon != null)
          {
            contact1.Icon = message.AuthorIcon;
            contact1.UpdateStorageIfNeeded();
          }
          if (!message.IsGroup)
            break;
          Contact contact3 = IMO.ContactsManager.GetOrCreateContact(message.ConvBuid);
          if (contact3.Alias != message.Alias)
          {
            contact3.Alias = message.Alias;
            contact3.UpdateStorageIfNeeded();
          }
          if (!(contact3.Icon != message.Icon))
            break;
          contact3.Icon = message.Icon;
          contact3.UpdateStorageIfNeeded();
          break;
      }
    }

    public void AddIncommingMessage(Message message, bool dontUpdateStorage = false)
    {
      if (message.Index > this.LastRecvIndex)
        this.LastRecvIndex = message.Index;
      this.InternalAddMessage(message, dontUpdateStorage);
      this.UpdateContactFromMesssage(message);
    }

    internal void HideFromChatList(bool force = false)
    {
      if (!(this.ShowInChatsList | force))
        return;
      IMO.ApplicationStorage.RemoveConversation(this.buid);
      if (!this.contact.IsBuddy)
        IMO.ApplicationStorage.RemoveContact(this.buid);
      this.ShowInChatsList = false;
    }

    public void SyncRecent()
    {
      if (this.isSyncingRecent)
        return;
      if (this.messages.Reverse().FirstOrDefault<Message>((Func<Message, bool>) (x => !x.IsFromStorage() && !x.IsPendingSend)) == null)
      {
        this.LoadHistory();
      }
      else
      {
        long startTime = this.GetLastMessageTimestampNano() + 1L;
        this.isSyncingRecent = true;
        IMO.IM.GetConversation(this.buid, startTime, -1L, -1, true, (Action<JToken>) (response =>
        {
          this.isSyncingRecent = false;
          JArray jarray = response as JArray;
          if (response == null)
          {
            Conversation.log.Error("Sync recent arrayResponse is null, response: " + response.ToString(), 1063, nameof (SyncRecent));
          }
          else
          {
            for (int index = 0; index < jarray.Count; ++index)
            {
              JObject edata = (JObject) jarray[index];
              Message fromJobject = Message.CreateFromJObject(edata, MessageOrigin.RECENT, edata.Value<bool>((object) "is_sent") ? MessageType.Sent : MessageType.Received);
              fromJobject.Acked = true;
              this.AddIncommingMessage(fromJobject);
            }
          }
        }));
      }
    }

    public void GetRecentMessages()
    {
      if (this.IsGettingRecent || this.LoadedAllHistory)
        return;
      this.IsGettingRecent = true;
      IMO.IM.GetRecentMessages(this.buid, (Action<JToken>) (response =>
      {
        this.IsGettingRecent = false;
        if (!(response is JArray jarray2))
          return;
        for (int index = 0; index < jarray2.Count; ++index)
        {
          JObject edata = (JObject) jarray2[index];
          Message fromJobject = Message.CreateFromJObject(edata, MessageOrigin.HISTORY, edata.Value<bool>((object) "is_sent") ? MessageType.Sent : MessageType.Received);
          fromJobject.Acked = true;
          this.AddIncommingMessage(fromJobject);
        }
        if (jarray2.Count != 0)
          return;
        this.LoadedAllHistory = true;
      }));
    }

    public bool IsGettingRecent
    {
      get => this.isGettingRecent;
      set
      {
        if (this.isGettingRecent == value)
          return;
        this.isGettingRecent = value;
        this.OnPropertyChanged(nameof (IsGettingRecent));
      }
    }

    public void LoadHistory()
    {
      if (!this.HasNonStorageMessages())
        this.GetRecentMessages();
      else
        this.InternalLoadHistory();
    }

    public bool HasNonStorageMessages()
    {
      return this.messages.FirstOrDefault<Message>((Func<Message, bool>) (x => !x.IsFromStorage())) != null;
    }

    public void ReduceMessages()
    {
      Message message1 = (Message) null;
      int num1 = 0;
      int num2 = 0;
      foreach (Message message2 in this.messages.Reverse())
      {
        ++num1;
        if (message2.IsUnread)
          ++num2;
        if (num1 >= 30 && !message2.IsPendingSend && (message2.MergeType != MessageMergeState.MERGE || num1 >= 60))
        {
          message1 = message2;
          break;
        }
      }
      if (message1 == null)
        return;
      this.messages.RemoveAllBefore(message1);
      if (this.UnreadMessagesCount > this.messages.Count)
        this.UnreadMessagesCount = this.messages.Count;
      this.LoadedAllHistory = false;
    }

    private void InternalLoadHistory()
    {
      if (this.LoadedAllHistory || this.isLoadingHistory)
        return;
      int startTime = 0;
      long endTime = this.messages.First.TimestampNano - 1L;
      this.isLoadingHistory = true;
      IMO.IM.GetConversation(this.buid, (long) startTime, endTime, 30, true, (Action<JToken>) (response =>
      {
        this.isLoadingHistory = false;
        JArray jarray = response as JArray;
        for (int index = 0; index < jarray.Count; ++index)
        {
          JObject edata = (JObject) jarray[index];
          Message fromJobject = Message.CreateFromJObject(edata, MessageOrigin.HISTORY, edata.Value<bool>((object) "is_sent") ? MessageType.Sent : MessageType.Received);
          fromJobject.Acked = true;
          this.AddIncommingMessage(fromJobject);
        }
        if (jarray.Count >= 30)
          return;
        this.LoadedAllHistory = true;
      }));
    }

    internal void DeleteMessage(Message message)
    {
      if (!this.messages.Contains(message))
        return;
      this.messages.Remove(message);
    }

    private void SaveToStorage()
    {
      if (!this.ShowInChatsList)
        return;
      IMO.ApplicationStorage.AddConversation(this);
    }

    public IList<Contact> GetInChatContacts()
    {
      return (IList<Contact>) this.inChatContacts.Keys.ToList<Contact>();
    }

    public IList<Contact> GetSeenAllContacts()
    {
      return (IList<Contact>) this.seenAllContacts.ToList<Contact>();
    }

    public IDictionary<Contact, string> GetTypers()
    {
      return (IDictionary<Contact, string>) this.typers.ToDictionary<KeyValuePair<Contact, Conversation.TyperData>, Contact, string>((Func<KeyValuePair<Contact, Conversation.TyperData>, Contact>) (x => x.Key), (Func<KeyValuePair<Contact, Conversation.TyperData>, string>) (x => x.Value.Message));
    }

    public event EventHandler<EventArg<Contact>> InChatContactAdded;

    public event EventHandler<EventArg<Contact>> InChatContactRemoved;

    private void OnInChatContactAdded(Contact contact)
    {
      EventHandler<EventArg<Contact>> chatContactAdded = this.InChatContactAdded;
      if (chatContactAdded == null)
        return;
      chatContactAdded((object) this, new EventArg<Contact>(contact));
    }

    private void OnInChatContactRemoved(Contact contact)
    {
      EventHandler<EventArg<Contact>> chatContactRemoved = this.InChatContactRemoved;
      if (chatContactRemoved == null)
        return;
      chatContactRemoved((object) this, new EventArg<Contact>(contact));
    }

    public event EventHandler<EventArg<Contact>> SeenAllContactAdded;

    public event EventHandler<EventArg<Contact>> SeenAllContactRemoved;

    private void OnSeenAllContactAdded(Contact contact)
    {
      EventHandler<EventArg<Contact>> seenAllContactAdded = this.SeenAllContactAdded;
      if (seenAllContactAdded == null)
        return;
      seenAllContactAdded((object) this, new EventArg<Contact>(contact));
    }

    private void OnSeenAllContactRemoved(Contact contact)
    {
      EventHandler<EventArg<Contact>> allContactRemoved = this.SeenAllContactRemoved;
      if (allContactRemoved == null)
        return;
      allContactRemoved((object) this, new EventArg<Contact>(contact));
    }

    public event EventHandler<EventArg<KeyValuePair<Contact, string>>> TyperAdded;

    public event EventHandler<EventArg<Contact>> TyperRemoved;

    private void OnTyperAdded(Contact contact, string text)
    {
      EventHandler<EventArg<KeyValuePair<Contact, string>>> typerAdded = this.TyperAdded;
      if (typerAdded == null)
        return;
      typerAdded((object) this, new EventArg<KeyValuePair<Contact, string>>(new KeyValuePair<Contact, string>(contact, text)));
    }

    private void OnTyperRemoved(Contact contact)
    {
      EventHandler<EventArg<Contact>> typerRemoved = this.TyperRemoved;
      if (typerRemoved == null)
        return;
      typerRemoved((object) this, new EventArg<Contact>(contact));
    }

    public void AddInChatContact(Contact contact)
    {
      long num1 = 0;
      this.inChatContacts.TryGetValue(contact, out num1);
      this.inChatContacts[contact] = Utils.GetTimestamp();
      Utils.DelayExecute(this.IN_CHAT_TIMEOUT, (Action) (() =>
      {
        long num2 = 0;
        this.inChatContacts.TryGetValue(contact, out num2);
        if (num2 == 0L || Utils.GetTimestamp() - num2 < (long) (this.IN_CHAT_TIMEOUT - 100))
          return;
        this.RemoveInChatContact(contact);
      }));
      if (num1 != 0L)
        return;
      this.OnInChatContactAdded(contact);
    }

    public void RemoveInChatContact(Contact contact)
    {
      this.inChatContacts.Remove(contact);
      this.OnInChatContactRemoved(contact);
    }

    public void AddSeenAllContact(Contact contact)
    {
      if (this.seenAllContacts.Contains(contact))
        return;
      this.seenAllContacts.Add(contact);
      this.OnSeenAllContactAdded(contact);
    }

    public void RemoveSeenAllContact(Contact contact)
    {
      if (!this.seenAllContacts.Contains(contact))
        return;
      this.seenAllContacts.Remove(contact);
      this.OnSeenAllContactRemoved(contact);
    }

    internal void SetMemberSeenMessage(Contact contact, long timestamp_nano)
    {
      if (timestamp_nano < this.GetLastMessageSenderTimestampNano())
        return;
      this.AddSeenAllContact(contact);
    }

    internal void ClearSeenAllContacts()
    {
      foreach (Contact seenAllContact in (IEnumerable<Contact>) this.GetSeenAllContacts())
        this.RemoveSeenAllContact(seenAllContact);
    }

    public void RemoveTyper(Contact contact)
    {
      if (!this.typers.ContainsKey(contact))
        return;
      this.typers.Remove(contact);
      this.OnTyperRemoved(contact);
    }

    private long GetTyperTimestamp(Contact contact)
    {
      return this.typers.ContainsKey(contact) ? this.typers[contact].Timestamp : 0L;
    }

    public void AddTyper(Contact contact, string text)
    {
      if (text == null || text == string.Empty)
      {
        this.RemoveTyper(contact);
      }
      else
      {
        this.typers[contact] = new Conversation.TyperData()
        {
          Message = text,
          Timestamp = Utils.GetTimestamp()
        };
        Utils.DelayExecute(this.TYPER_TIMEOUT, (Action) (() =>
        {
          long typerTimestamp = this.GetTyperTimestamp(contact);
          if (typerTimestamp == 0L || Utils.GetTimestamp() - typerTimestamp < (long) (this.TYPER_TIMEOUT - 100))
            return;
          this.RemoveTyper(contact);
        }));
        this.typers[contact].Message = text;
        this.OnTyperAdded(contact, text);
      }
    }

    public void SharePhotoWith(string buid, string objectId)
    {
      PhotoMessage message = PhotoMessage.MakePhotoMessage(this.contact.Buid, objectId, 320, 240);
      this.InternalAddMessage((Message) message);
      IMO.Pixel.SharePhoto(this.contact.Buid, objectId, (Action<JToken>) (result =>
      {
        JObject jobject = (JObject) ((JArray) result)[0];
        this.messages.Remove((Message) message);
        message.TimestampNano = jobject.Value<long>((object) "timestamp_nano");
        message.Acked = true;
        message.SetPhotoId(jobject.Value<string>((object) "object_id"), jobject.Value<JObject>((object) "type_specific_params"));
        this.messages.Add((Message) message);
      }));
    }

    public void ShareVideoWith(string buid, string objectId)
    {
      VideoMessage message = VideoMessage.MakeVideoMessage(this.contact.Buid);
      this.InternalAddMessage((Message) message);
      IMO.Pixel.ShareVideo(this.contact.Buid, objectId, (Action<JToken>) (result =>
      {
        JObject jobject = (JObject) ((JArray) result)[0];
        this.messages.Remove((Message) message);
        message.TimestampNano = jobject.Value<long>((object) "timestamp_nano");
        message.Acked = true;
        message.SetVideoId(jobject.Value<string>((object) "object_id"), jobject.Value<JObject>((object) "type_specific_params"));
        this.messages.Add((Message) message);
      }));
    }

    private class MessageComparer : Comparer<Message>
    {
      public override int Compare(Message x, Message y)
      {
        if (x.TimestampNano == -1L && y.TimestampNano == -1L)
          return x.SentTimestamp.CompareTo(y.SentTimestamp);
        if (x.TimestampNano == -1L)
          return 1;
        return y.TimestampNano == -1L ? -1 : x.TimestampNano.CompareTo(y.TimestampNano);
      }
    }

    private class TyperData
    {
      public string Message;
      public long Timestamp;
    }
  }
}
