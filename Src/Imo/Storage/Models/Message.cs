// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.Storage.Models.Message
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Converters;
using ImoSilverlightApp.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;


namespace ImoSilverlightApp.Storage.Models
{
  public abstract class Message : ModelBase
  {
    public const long MESSAGE_FAILED_TIMESTAMP = -2;
    public const long SENDING_MESSAGE_TIMESTAMP = -1;
    [JsonProperty(PropertyName = "message_type")]
    [JsonConverter(typeof (LowercaseEnumConverter))]
    protected MessageType messageType;
    [JsonProperty(PropertyName = "author")]
    private string author;
    [JsonProperty(PropertyName = "alias")]
    private string alias;
    [JsonProperty(PropertyName = "author_alias")]
    private string author_alias;
    [JsonProperty(PropertyName = "msg")]
    protected string msg;
    [JsonProperty(PropertyName = "index")]
    private long index = -1;
    [JsonProperty(PropertyName = "acked")]
    private bool acked;
    [JsonProperty(PropertyName = "delivered")]
    private bool delivered;
    [JsonProperty(PropertyName = "seen")]
    private bool seen;
    [JsonProperty(PropertyName = "icon")]
    private string icon;
    [JsonProperty(PropertyName = "author_icon")]
    private string author_icon;
    [JsonProperty(PropertyName = "imdata")]
    protected JObject imdata;
    [JsonProperty(PropertyName = "from_nonbuddy")]
    private bool fromNonBuddy;
    [JsonProperty(PropertyName = "seen_set")]
    private IList<string> seenSet;
    [JsonProperty(PropertyName = "buid")]
    private string buid;
    [JsonProperty(PropertyName = "is_unread")]
    private bool isUnread;
    private JObject edata;
    private MessageMergeState mergeType;
    private MessageOrigin origin;
    [JsonProperty(PropertyName = "timestamp_nano")]
    private long timestamp_nano = -1;
    [JsonProperty(PropertyName = "sender_timestamp_nano")]
    private long? sender_timestamp_nano;

    public MessageMergeState MergeType
    {
      get => this.mergeType;
      set
      {
        if (this.mergeType == value)
          return;
        this.mergeType = value;
        this.OnPropertyChanged(nameof (MergeType));
      }
    }

    public MessageState MessageState
    {
      get
      {
        if (this.seen)
          return MessageState.SEEN;
        if (this.delivered)
          return MessageState.DELIVERED;
        return this.acked ? MessageState.ACKED : MessageState.SENDING;
      }
    }

    public MessageOrigin Origin => this.origin;

    public string Buid
    {
      get => this.author != null ? this.author.Substring(0, this.author.IndexOf(";")) : this.buid;
    }

    public string ConvBuid => this.buid;

    public long TimestampNano
    {
      get => this.timestamp_nano;
      set
      {
        if (this.timestamp_nano == value)
          return;
        this.timestamp_nano = value;
        this.OnPropertyChanged(nameof (TimestampNano));
        this.OnPropertyChanged("FormattedTimestamp");
        this.OnPropertyChanged("FullFormattedTimestamp");
      }
    }

    public long SenderTimestampNano
    {
      get => this.sender_timestamp_nano ?? -1L;
      set
      {
        long? senderTimestampNano = this.sender_timestamp_nano;
        long num = value;
        if ((senderTimestampNano.GetValueOrDefault() == num ? (!senderTimestampNano.HasValue ? 1 : 0) : 1) == 0)
          return;
        this.sender_timestamp_nano = new long?(value);
        this.OnPropertyChanged(nameof (SenderTimestampNano));
      }
    }

    public long SentTimestamp { get; set; }

    public long Timestamp => this.timestamp_nano == -1L ? -1L : this.timestamp_nano / 1000000L;

    public string FormattedTimestamp
    {
      get
      {
        if (this.timestamp_nano == -1L)
          return "sending";
        return this.timestamp_nano == -2L ? "failed" : Utils.ToFormattedTimestamp(this.Timestamp);
      }
    }

    internal void UpdateTimestampUI() => this.OnPropertyChanged("FormattedTimestamp");

    public string FullFormattedTimestamp
    {
      get
      {
        if (this.timestamp_nano == -1L)
          return (string) null;
        return this.timestamp_nano == -2L ? (string) null : Utils.ToFullFormattedTimestamp(this.Timestamp);
      }
    }

    public bool IsFailed => this.timestamp_nano == -2L;

    public bool IsSending => this.timestamp_nano == -1L;

    public bool IsBuddy
    {
      get => !this.fromNonBuddy;
      set => this.fromNonBuddy = !value;
    }

    public string IconPath
    {
      get
      {
        return this.author_icon != null ? Utils.GetIconPath(this.author_icon) : Utils.GetIconPath(this.icon);
      }
    }

    public MessageType MessageType => this.messageType;

    public string Author => this.author;

    public string Alias => this.alias;

    public string AuthorAlias => this.author_alias;

    public string Msg => this.msg;

    public long Index => this.index;

    public bool Acked
    {
      get => this.acked;
      set
      {
        if (this.acked == value)
          return;
        this.acked = value;
        this.OnPropertyChanged(nameof (Acked));
        this.OnPropertyChanged("MessageState");
      }
    }

    public bool Delivered
    {
      get => this.delivered;
      set
      {
        if (this.delivered == value)
          return;
        this.delivered = value;
        this.OnPropertyChanged(nameof (Delivered));
        this.OnPropertyChanged("MessageState");
      }
    }

    public bool Seen
    {
      get => this.seen;
      set
      {
        if (this.seen == value)
          return;
        this.seen = value;
        this.OnPropertyChanged(nameof (Seen));
        this.OnPropertyChanged("MessageState");
      }
    }

    public bool IsSystem => this.messageType == MessageType.System;

    public bool IsMyMessage => this.messageType == MessageType.Sent;

    public IList<string> SeenSet => this.seenSet;

    public bool IsUnread
    {
      get => this.isUnread;
      set
      {
        if (this.isUnread == value)
          return;
        this.isUnread = value;
        this.OnPropertyChanged(nameof (IsUnread));
      }
    }

    public string Icon => this.icon;

    public string AuthorIcon => this.author_icon;

    public bool FromNonBuddy => this.fromNonBuddy;

    public bool IsPendingSend => this.timestamp_nano == -1L || this.timestamp_nano == -2L;

    public bool IsGroup => Contact.IsGroupBuid(this.ConvBuid);

    public JObject Edata => this.edata;

    public Message(MessageOrigin origin)
    {
      this.origin = origin;
      this.isUnread = origin == MessageOrigin.RECV || origin == MessageOrigin.UNREAD;
    }

    protected abstract void Init();

    public void PopulateFromEdata(JObject edata)
    {
      this.edata = edata;
      JsonConvert.PopulateObject(edata.ToString(), (object) this);
      if (!this.IsGroup)
      {
        if (this.author_alias == null)
          this.author_alias = this.messageType != MessageType.Sent ? this.alias : IMO.User.Alias;
        if (this.author_icon == null)
          this.author_icon = this.icon;
      }
      this.Init();
    }

    public override bool Equals(object obj)
    {
      if (this == obj)
        return true;
      return obj is Message message && !this.IsPendingSend && !message.IsPendingSend && this.buid.Equals(message.buid) && this.timestamp_nano == message.timestamp_nano;
    }

    public override int GetHashCode()
    {
      return this.IsPendingSend ? base.GetHashCode() : (1073741827 * 16777619 ^ (this.buid == null ? 0 : this.buid.GetHashCode())) * 16777619 ^ this.timestamp_nano.GetHashCode();
    }

    public static Message CreateFromJObject(
      JObject edata,
      MessageOrigin origin,
      MessageType messageType)
    {
      JObject jobject = edata.Value<JObject>((object) "imdata");
      string str = jobject == null ? (string) null : jobject.Value<string>((object) "type");
      if (edata.Value<string>((object) "message_type") == null)
        edata.Add("message_type", (JToken) messageType.ToString().ToLower());
      if ("photo_deleted".Equals(str))
      {
        TextMessage fromJobject = new TextMessage(origin);
        fromJobject.PopulateFromEdata(edata);
        return (Message) fromJobject;
      }
      if ("photo_uploaded".Equals(str))
      {
        PhotoMessage fromJobject1 = new PhotoMessage(origin);
        fromJobject1.PopulateFromEdata(edata);
        if (origin != MessageOrigin.STORAGE && !fromJobject1.IsPendingSend)
          IMO.PhotoStreamsManager.GetOrCreatePhotoStream(fromJobject1.Buid).InvalidateIsSynced();
        if (fromJobject1.PhotoID != null)
          return (Message) fromJobject1;
        SystemMessage fromJobject2 = new SystemMessage(origin);
        fromJobject2.IsDeletedPhoto = true;
        fromJobject2.PopulateFromEdata(edata);
        return (Message) fromJobject2;
      }
      if ("video_uploaded".Equals(str))
      {
        VideoMessage fromJobject = new VideoMessage(origin);
        fromJobject.PopulateFromEdata(edata);
        if (origin != MessageOrigin.STORAGE && !fromJobject.IsPendingSend)
          IMO.PhotoStreamsManager.GetOrCreatePhotoStream(fromJobject.Buid).InvalidateIsSynced();
        return (Message) fromJobject;
      }
      if ("audio_uploaded".Equals(str))
      {
        AudioMessage fromJobject = new AudioMessage(origin);
        fromJobject.PopulateFromEdata(edata);
        return (Message) fromJobject;
      }
      if ("file_uploaded".Equals(str))
      {
        TextMessage fromJobject = new TextMessage(origin);
        fromJobject.PopulateFromEdata(edata);
        return (Message) fromJobject;
      }
      if ("missed_call".Equals(str))
      {
        SystemMessage fromJobject = new SystemMessage(origin);
        fromJobject.IsMissedCall = true;
        fromJobject.PopulateFromEdata(edata);
        return (Message) fromJobject;
      }
      if ("sticker".Equals(str))
      {
        StickerMessage fromJobject = new StickerMessage(origin);
        fromJobject.PopulateFromEdata(edata);
        return (Message) fromJobject;
      }
      if ("joined_group".Equals(str))
      {
        SystemMessage fromJobject = new SystemMessage(origin);
        fromJobject.IsJoinedGroup = true;
        fromJobject.PopulateFromEdata(edata);
        return (Message) fromJobject;
      }
      if ("left_group".Equals(str))
      {
        SystemMessage fromJobject = new SystemMessage(origin);
        fromJobject.IsLeftGroup = true;
        fromJobject.PopulateFromEdata(edata);
        return (Message) fromJobject;
      }
      if ("just_joined".Equals(str))
      {
        SystemMessage fromJobject = new SystemMessage(origin);
        fromJobject.IsJustJoined = true;
        fromJobject.PopulateFromEdata(edata);
        return (Message) fromJobject;
      }
      if ("kick_member".Equals(str))
      {
        SystemMessage fromJobject = new SystemMessage(origin);
        fromJobject.IsKickedFromGroup = true;
        fromJobject.PopulateFromEdata(edata);
        return (Message) fromJobject;
      }
      TextMessage fromJobject3 = new TextMessage(origin);
      fromJobject3.PopulateFromEdata(edata);
      return (Message) fromJobject3;
    }

    internal bool IsFromStorage() => this.origin == MessageOrigin.STORAGE;
  }
}
