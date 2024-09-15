// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.Managers.ConversationsManager
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Connection;
using ImoSilverlightApp.Helpers;
using ImoSilverlightApp.Storage.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;


namespace ImoSilverlightApp.Managers
{
  public class ConversationsManager : INotifyPropertyChanged
  {
    private static readonly Logger log = LogManager.GetLogger(typeof (ConversationsManager).Name);
    private int CONVERSATION_TIMEOUT_DAYS = 7;
    private IDictionary<string, Conversation> conversations;
    private int unreadConversationsCount;
    private HashSet<Conversation> unreadConversations;
    private Conversation currentOpenConversation;

    public event EventHandler<EventArg<Conversation>> ConversationAdded;

    public event EventHandler<EventArg<Conversation>> ConversationRemoved;

    public event EventHandler<EventArg<Conversation>> ConversationActivated;

    public event EventHandler<EventArgs> ConversationsReset;

    public event EventHandler<EventArg<Message>> IMMessageReceived;

    public event PropertyChangedEventHandler PropertyChanged;

    public ConversationsManager()
    {
      this.conversations = (IDictionary<string, Conversation>) new Dictionary<string, Conversation>();
      this.unreadConversations = new HashSet<Conversation>();
      IMO.AccountManager.SignedOn += new EventHandler<EventArg<SignOnData>>(this.SignedOnHandler);
      IMO.AccountManager.SignedOff += new EventHandler(this.SignedOffHandler);
      IMO.Network.Connected += new EventHandler<EventArg<ConnectionData>>(this.Network_Connected);
      IMO.Dispatcher.Resetted += new EventHandler(this.Dispatcher_Resetted);
      if (!IMO.ApplicationSettings.IsCookieSignedOn)
        return;
      this.InitConversationsFromStorage();
      this.RemoveTimedOutConversations();
    }

    internal void UpdateNewDayMessagesTimestampUI()
    {
      foreach (Conversation conversation in (IEnumerable<Conversation>) this.conversations.Values)
        conversation.UpdateNewDayMessagesTimestampUI();
    }

    private void Network_Connected(object sender, EventArg<ConnectionData> e)
    {
      if (!e.Arg.ResetDispatcher || this.currentOpenConversation == null)
        return;
      this.currentOpenConversation.SyncRecent();
      this.currentOpenConversation.ResetUnreadMessages();
      this.currentOpenConversation.Contact.SyncGroupMembers(true);
    }

    internal void ClearConversationsMessages()
    {
      foreach (Conversation conversation in (IEnumerable<Conversation>) this.conversations.Values)
        conversation.ClearMessages();
    }

    public void ResendUnackedMessages()
    {
      IList<JObject> unackedMessages = IMO.ApplicationStorage.GetUnackedMessages();
      if (unackedMessages.Count == 0)
        return;
      ConversationsManager.log.Info("Resending " + (object) unackedMessages.Count + " unacked messages");
      IMO.ApplicationStorage.ClearPendingSendMessages();
      foreach (Conversation conversation in (IEnumerable<Conversation>) this.conversations.Values)
        conversation.ClearUnsentMessages();
      foreach (JObject jobject in (IEnumerable<JObject>) unackedMessages)
      {
        string buid = jobject.Value<string>((object) "buid");
        string msg = jobject.Value<string>((object) "msg");
        JObject imdata = jobject.Value<JObject>((object) "imdata");
        this.GetOrCreateConversation(buid).SendMessage(msg, imdata);
      }
    }

    private void Dispatcher_Resetted(object sender, EventArgs e)
    {
      foreach (Conversation conversation in (IEnumerable<Conversation>) this.conversations.Values)
        conversation.InvalidatePendingRequests();
    }

    private void SignedOnHandler(object sender, EventArg<SignOnData> e)
    {
      this.ResendUnackedMessages();
    }

    private void RemoveTimedOutConversations()
    {
      foreach (Conversation conversation in (IEnumerable<Conversation>) this.conversations.Values)
      {
        if (conversation.LastActivityTimestamp != -1L && TimeSpan.FromMilliseconds((double) (Utils.GetTimestamp() - conversation.LastActivityTimestamp)).TotalDays > (double) this.CONVERSATION_TIMEOUT_DAYS)
          conversation.HideFromChatList();
      }
    }

    private void SignedOffHandler(object sender, EventArgs e)
    {
      if (this.currentOpenConversation != null)
        this.currentOpenConversation.CloseChat();
      foreach (Conversation allConversations in (IEnumerable<Conversation>) this.GetAllConversationsList())
      {
        this.conversations.Remove(allConversations.Buid);
        this.UpdateUnreadConversations(allConversations);
        allConversations.PropertyChanged -= new PropertyChangedEventHandler(this.Conversation_PropertyChanged);
        allConversations.Contact.PropertyChanged -= new PropertyChangedEventHandler(this.Contact_PropertyChanged);
        this.OnConversationRemoved(allConversations);
      }
    }

    private void InitConversationsFromStorage()
    {
      foreach (KeyValuePair<string, JToken> conversation1 in IMO.ApplicationStorage.GetConversations())
      {
        Conversation conversation2 = this.GetOrCreateConversation(conversation1.Key);
        JsonConvert.PopulateObject(conversation1.Value.ToString(), (object) conversation2);
        conversation2.ShowInChatsList = true;
      }
      this.OnConversationsReset();
    }

    private void OnConversationAdded(Conversation conversation)
    {
      EventHandler<EventArg<Conversation>> conversationAdded = this.ConversationAdded;
      if (conversationAdded == null)
        return;
      conversationAdded((object) this, new EventArg<Conversation>(conversation));
    }

    private void OnConversationActivated(Conversation conversation)
    {
      EventHandler<EventArg<Conversation>> conversationActivated = this.ConversationActivated;
      if (conversationActivated == null)
        return;
      conversationActivated((object) this, new EventArg<Conversation>(conversation));
    }

    private void OnConversationsReset()
    {
      EventHandler<EventArgs> conversationsReset = this.ConversationsReset;
      if (conversationsReset == null)
        return;
      conversationsReset((object) this, EventArgs.Empty);
    }

    private void OnIMMessageReceived(Message message)
    {
      EventHandler<EventArg<Message>> imMessageReceived = this.IMMessageReceived;
      if (imMessageReceived == null)
        return;
      imMessageReceived((object) this, new EventArg<Message>(message));
    }

    private void OnConversationRemoved(Conversation conversation)
    {
      EventHandler<EventArg<Conversation>> conversationRemoved = this.ConversationRemoved;
      if (conversationRemoved == null)
        return;
      conversationRemoved((object) this, new EventArg<Conversation>(conversation));
    }

    internal void MarkAllAsRead()
    {
      foreach (Conversation conversation in (IEnumerable<Conversation>) this.conversations.Values)
        conversation.MarkMessagesAsRead();
    }

    internal void MarkCurrentOpenConversationAsRead()
    {
      if (this.currentOpenConversation == null)
        return;
      this.currentOpenConversation.MarkMessagesAsRead();
    }

    public void SendTypingState(string typingState, string buid, string message)
    {
      IMO.IM.SendTypingState(typingState, buid, message);
    }

    public void HandleMessage(JObject message)
    {
      string state = message.Value<string>((object) "name");
      switch (state)
      {
        case null:
          break;
        case "recv_im":
          this.HandleRecvIm(message);
          break;
        case "recv_unread_msgs":
          this.HandleRecvUnreadMsgs(message);
          break;
        case "marked_msgs_as_read":
          this.HandleMarkedMsgsAsRead(message);
          break;
        case "typing":
        case "not_typing":
        case "typed":
          this.HandleTypingState(message, state);
          break;
        case "reflect":
          this.HandleReflect(message);
          break;
        case "message_acked":
          this.HandleMessageDelivered(message);
          break;
        case "message_seen":
          this.HandleMessageSeen(message);
          break;
        case "group_message_seen":
          this.HndleGroupMessageSeen(message);
          break;
        case "chat_opened":
          this.HandleChatOpened(message);
          break;
        case "chat_closed":
          this.HandleChatClosed(message);
          break;
        case "group_chat_opened":
          this.HandleGroupChatOpened(message);
          break;
        case "group_chat_closed":
          this.HandleGroupChatClosed(message);
          break;
        case "last_activity":
          this.HandleLastActivity(message);
          break;
        default:
          ConversationsManager.log.Warn("unknown name: " + state);
          break;
      }
    }

    private void HandleLastActivity(JObject message)
    {
      JObject jobject = message.Value<JObject>((object) "edata");
      long num = jobject.Value<long>((object) "last_activity_timestamp_ms");
      Contact contact = IMO.ContactsManager.GetOrCreateContact(jobject.Value<string>((object) "buid"));
      contact.LastActivity = num;
      contact.UpdateStorageIfNeeded();
    }

    private void HndleGroupMessageSeen(JObject message)
    {
      JObject jobject = message.Value<JObject>((object) "edata");
      string buid1 = jobject.Value<string>((object) "gid") + ";";
      string buid2 = jobject.Value<string>((object) "buid");
      long timestamp_nano = jobject.Value<long>((object) "timestamp_nano");
      Contact contact = IMO.ContactsManager.GetOrCreateContact(buid2);
      this.GetOrCreateConversation(buid1).SetMemberSeenMessage(contact, timestamp_nano);
    }

    private void HandleGroupChatClosed(JObject message)
    {
      JObject jobject = message.Value<JObject>((object) "edata");
      string buid = jobject.Value<string>((object) "gid") + ";";
      Contact contact = IMO.ContactsManager.GetOrCreateContact(jobject.Value<string>((object) "buid"));
      this.GetOrCreateConversation(buid).RemoveInChatContact(contact);
    }

    private void HandleGroupChatOpened(JObject message)
    {
      JObject jobject = message.Value<JObject>((object) "edata");
      string buid = jobject.Value<string>((object) "gid") + ";";
      Contact contact = IMO.ContactsManager.GetOrCreateContact(jobject.Value<string>((object) "buid"));
      this.GetOrCreateConversation(buid).AddInChatContact(contact);
    }

    private void HandleRecvIm(JObject mObj)
    {
      JObject edata = mObj.Value<JObject>((object) "edata");
      Message fromJobject = Message.CreateFromJObject(edata, MessageOrigin.RECV, MessageType.Received);
      Conversation conversation = this.GetOrCreateConversation(fromJobject.ConvBuid);
      string buid = fromJobject.ConvBuid;
      if (Contact.IsGroupBuid(fromJobject.ConvBuid))
        buid = fromJobject.Author.Substring(0, fromJobject.Author.IndexOf(";"));
      Contact contact = IMO.ContactsManager.GetOrCreateContact(buid);
      if (!fromJobject.IsSystem)
        conversation.AddInChatContact(contact);
      conversation.RemoveTyper(contact);
      int num = edata.Value<bool>((object) "is_silent") ? 1 : 0;
      conversation.AddIncommingMessage(fromJobject);
      if (num == 0)
        this.OnIMMessageReceived(fromJobject);
      if (!(fromJobject is SystemMessage))
        return;
      SystemMessage systemMessage = (SystemMessage) fromJobject;
      if (!systemMessage.IsJoinedGroup && !systemMessage.IsKickedFromGroup && !systemMessage.IsLeftGroup)
        return;
      conversation.Contact.SyncGroupMembers(true);
    }

    protected void HandleMarkedMsgsAsRead(JObject message)
    {
      message.Value<string>((object) "uid");
      JObject jobject = message.Value<JObject>((object) "edata");
      JToken jtoken = jobject["last_index"];
      if (jtoken == null || jtoken.Type == JTokenType.Null)
        return;
      long index = jtoken.ToObject<long>();
      this.GetOrCreateConversation(jobject.Value<string>((object) "buid")).MarkMessagesAsRead(index);
    }

    protected void HandleTypingState(JObject jobj, string state)
    {
      jobj.Value<string>((object) "uid");
      JObject jobject = jobj.Value<JObject>((object) "edata");
      string buid1 = jobject.Value<string>((object) "buid");
      string text = jobject.Value<string>((object) "message");
      Conversation conversation = IMO.ConversationsManager.GetOrCreateConversation(buid1);
      string buid2 = buid1;
      if (Contact.IsGroupBuid(buid1))
      {
        string str = jobject.Value<string>((object) "author");
        buid2 = str.Substring(0, str.Length - ";imo".Length);
      }
      Contact contact = IMO.ContactsManager.GetOrCreateContact(buid2);
      conversation.AddTyper(contact, text);
      conversation.AddInChatContact(contact);
    }

    protected void HandleReflect(JObject message)
    {
      JObject edata = message.Value<JObject>((object) "edata");
      string str = edata.Value<string>((object) "r_name");
      if ("send_im".Equals(str.ToLower()))
      {
        Message fromJobject = Message.CreateFromJObject(edata, MessageOrigin.REFLECT, MessageType.Sent);
        fromJobject.Acked = true;
        fromJobject.SenderTimestampNano = fromJobject.TimestampNano;
        this.GetOrCreateConversation(fromJobject.ConvBuid).AddIncommingMessage(fromJobject);
      }
      else
      {
        if ("invitation_response".Equals(str.ToLower()) || "leave_group".Equals(str.ToLower()))
          return;
        ConversationsManager.log.Error("Unrecognized reflect name: " + str, 463, nameof (HandleReflect));
      }
    }

    protected void HandleMessageDelivered(JObject message)
    {
      message.Value<string>((object) "uid");
      JObject jobject = message.Value<JObject>((object) "edata");
      string buid = jobject.Value<string>((object) "buid");
      long num = jobject.Value<long>((object) "timestamp_nano");
      foreach (Message message1 in this.GetOrCreateConversation(buid).Messages.Reverse())
      {
        if (message1.Delivered)
          break;
        if (message1.TimestampNano <= num)
          message1.Delivered = true;
      }
    }

    protected void HandleMessageSeen(JObject message)
    {
      message.Value<string>((object) "uid");
      JObject jobject = message.Value<JObject>((object) "edata");
      string buid = jobject.Value<string>((object) "buid");
      long timestamp_nano = jobject.Value<long>((object) "timestamp_nano");
      Conversation conversation = this.GetOrCreateConversation(buid);
      conversation.MarkSeenMessages(timestamp_nano);
      conversation.AddInChatContact(IMO.ContactsManager.GetOrCreateContact(buid));
    }

    protected void HandleChatOpened(JObject message)
    {
      message.Value<string>((object) "uid");
      string buid = message.Value<JObject>((object) "edata").Value<string>((object) "buid");
      this.GetOrCreateConversation(buid).AddInChatContact(IMO.ContactsManager.GetOrCreateContact(buid));
    }

    protected void HandleChatClosed(JObject message)
    {
      message.Value<string>((object) "uid");
      string buid = message.Value<JObject>((object) "edata").Value<string>((object) "buid");
      this.GetOrCreateConversation(buid).RemoveInChatContact(IMO.ContactsManager.GetOrCreateContact(buid));
    }

    protected async void HandleRecvUnreadMsgs(JObject jObj)
    {
      await Task.Delay(1000);
      JObject jobject = jObj.Value<JObject>((object) "edata");
      List<Message> messageList = new List<Message>();
      long val1 = -1;
      bool flag1 = true;
      foreach (KeyValuePair<string, JToken> keyValuePair in jobject)
      {
        string key = keyValuePair.Key;
        JArray jarray = jobject.Value<JObject>((object) key).Value<JArray>((object) "msgs");
        int num = 0;
        foreach (JObject edata in jarray)
        {
          Message fromJobject = Message.CreateFromJObject(edata, MessageOrigin.UNREAD, MessageType.Received);
          fromJobject.Acked = true;
          bool flag2 = edata.Value<bool>((object) "is_silent");
          flag1 &= flag2;
          this.GetOrCreateConversation(fromJobject.ConvBuid).AddIncommingMessage(fromJobject, num != jarray.Count - 1);
          val1 = Math.Max(val1, fromJobject.TimestampNano);
          messageList.Add(fromJobject);
          ++num;
        }
      }
    }

    public IList<Conversation> GetAllConversationsList()
    {
      return (IList<Conversation>) this.conversations.Values.ToList<Conversation>();
    }

    public Conversation GetOrCreateConversation(string buid)
    {
      Conversation conversation;
      if (!this.conversations.TryGetValue(buid, out conversation))
      {
        conversation = new Conversation(buid);
        this.conversations.Add(buid, conversation);
        conversation.PropertyChanged += new PropertyChangedEventHandler(this.Conversation_PropertyChanged);
        conversation.Contact.PropertyChanged += new PropertyChangedEventHandler(this.Contact_PropertyChanged);
        this.OnConversationAdded(conversation);
      }
      return conversation;
    }

    public bool HasOpenConversationWith(string buid)
    {
      return this.GetOrCreateConversation(buid).ShowInChatsList;
    }

    private void Contact_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      Contact contact = (Contact) sender;
      if (!(e.PropertyName == "IsMuted"))
        return;
      this.UpdateUnreadConversations(this.GetOrCreateConversation(contact.Buid));
    }

    private void Conversation_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      Conversation conversation = (Conversation) sender;
      if (e.PropertyName == "UnreadMessagesCount" || e.PropertyName == "ShowInChatsList")
      {
        this.UpdateUnreadConversations(conversation);
        if (!(e.PropertyName == "ShowInChatsList") || !conversation.ShowInChatsList)
          return;
        this.OnConversationActivated(conversation);
      }
      else
      {
        if (!(e.PropertyName == "IsOpen"))
          return;
        if (!conversation.IsOpen)
        {
          if (this.currentOpenConversation == conversation)
            this.currentOpenConversation = (Conversation) null;
          else
            ConversationsManager.log.Error("Closed conversation that was not currently open", 619, nameof (Conversation_PropertyChanged));
        }
        else
        {
          if (this.currentOpenConversation != null && this.currentOpenConversation.IsOpen)
            ConversationsManager.log.Error("Opened a conversation without closing the previous one", 628, nameof (Conversation_PropertyChanged));
          this.currentOpenConversation = conversation;
          if (conversation.HasNonStorageMessages())
            return;
          conversation.GetRecentMessages();
        }
      }
    }

    private void UpdateUnreadConversations(Conversation conversation)
    {
      if (conversation.UnreadMessagesCount > 0 && !conversation.Contact.IsMuted && conversation.ShowInChatsList && this.conversations.ContainsKey(conversation.Buid))
      {
        if (this.unreadConversations.Contains(conversation))
          return;
        this.unreadConversations.Add(conversation);
        ++this.UnreadConversationsCount;
      }
      else
      {
        if (conversation.UnreadMessagesCount != 0 && !conversation.Contact.IsMuted && conversation.ShowInChatsList && this.conversations.ContainsKey(conversation.Buid) || !this.unreadConversations.Contains(conversation))
          return;
        this.unreadConversations.Remove(conversation);
        --this.UnreadConversationsCount;
      }
    }

    public int UnreadConversationsCount
    {
      get => this.unreadConversationsCount;
      set
      {
        if (this.unreadConversationsCount == value)
          return;
        this.unreadConversationsCount = value;
        this.OnPropertyChanged(nameof (UnreadConversationsCount));
        this.OnPropertyChanged("HasUnreadConversations");
      }
    }

    public bool HasUnreadConversations => this.unreadConversations.Count > 0;

    public Conversation CurrentOpenConversation => this.currentOpenConversation;

    protected void OnPropertyChanged(string name)
    {
      PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
      if (propertyChanged == null)
        return;
      propertyChanged((object) this, new PropertyChangedEventArgs(name));
    }
  }
}
