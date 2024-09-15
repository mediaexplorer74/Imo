// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.Storage.Models.Contact
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Converters;
using ImoSilverlightApp.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace ImoSilverlightApp.Storage.Models
{
  public class Contact : ModelBase, IComparable<Contact>
  {
    private string buid;
    private string icon;
    private Primitive primitive;
    private bool isFavorite;
    private bool isBlocked;
    private string alias;
    private bool isBuddy;
    private string phonebookAlias;
    [JsonProperty(PropertyName = "phone_number")]
    private string phoneNumber;
    [JsonProperty(PropertyName = "last_activity")]
    private long lastActivity;
    private bool isMuted;
    private IDictionary<string, Contact> groupMembers;
    private List<Contact> callMembers;
    private bool isSyncingGroupMembers;
    private bool hasSyncedGroupMembers;

    public Contact(string buid)
    {
      this.buid = buid;
      this.groupMembers = (IDictionary<string, Contact>) new Dictionary<string, Contact>();
    }

    public static Contact CreateFastFromJObject(JObject jObj)
    {
      Contact fastFromJobject = new Contact(jObj.Value<string>((object) "buid"));
      fastFromJobject.UpdateFastFromJObject(jObj);
      return fastFromJobject;
    }

    public void UpdateFastFromJObject(JObject jObj)
    {
      if (jObj["phone_number"] != null)
        this.PhoneNumber = jObj.Value<string>((object) "phone_number");
      if (jObj["last_activity"] != null)
        this.LastActivity = jObj.Value<long>((object) "last_activity");
      if (jObj["alias"] != null)
        this.Alias = jObj.Value<string>((object) "alias");
      if (string.IsNullOrEmpty(this.alias) && jObj["display"] != null)
        this.Alias = jObj.Value<string>((object) "display");
      if (jObj["icon"] != null)
        this.Icon = jObj.Value<string>((object) "icon");
      if (jObj["is_buddy"] != null)
        this.IsBuddy = jObj.Value<bool>((object) "is_buddy");
      if (jObj["blocked"] != null)
        this.IsBlocked = jObj.Value<bool>((object) "blocked");
      if (jObj["favorite"] != null)
        this.IsFavorite = jObj.Value<bool>((object) "favorite");
      if (jObj["primitive"] != null)
        this.Primitive = (Primitive) Enum.Parse(typeof (Primitive), jObj.Value<string>((object) "primitive"), true);
      if (jObj["is_muted"] == null)
        return;
      this.IsMuted = jObj.Value<bool>((object) "is_muted");
    }

    [JsonProperty(PropertyName = "buid")]
    public string Buid => this.buid;

    [JsonProperty(PropertyName = "alias")]
    public string Alias
    {
      get => this.phonebookAlias == null ? this.alias : this.phonebookAlias;
      set
      {
        if (!(this.alias != value))
          return;
        this.alias = value;
        this.OnPropertyChanged(nameof (Alias));
        this.OnPropertyChanged("AliasShort");
      }
    }

    public string AliasShort
    {
      get
      {
        if (this.alias == null)
          return string.Empty;
        int length = this.alias.IndexOf(" ");
        return length == -1 || length > 10 ? this.alias.Substring(0, Math.Min(this.alias.Length, 10)) : this.alias.Substring(0, length);
      }
    }

    [JsonProperty(PropertyName = "display")]
    public string Display
    {
      get => this.alias;
      set
      {
        if (!(this.alias != value))
          return;
        this.alias = value;
        this.OnPropertyChanged("Alias");
      }
    }

    [JsonProperty(PropertyName = "icon")]
    public string Icon
    {
      get => this.icon;
      set
      {
        if (!(this.icon != value))
          return;
        this.icon = value;
        this.OnPropertyChanged(nameof (Icon));
        this.OnPropertyChanged("IconUrl");
        this.OnPropertyChanged("PhotoUrl");
      }
    }

    public string Gid
    {
      get => !this.IsGroup ? (string) null : this.Buid.Substring(0, this.Buid.IndexOf(";"));
    }

    public bool NonBuddy
    {
      get => throw new Exception("Use IsBuddy");
      set
      {
      }
    }

    [JsonProperty(PropertyName = "is_buddy")]
    public bool IsBuddy
    {
      get => this.isBuddy;
      set
      {
        if (this.isBuddy == value)
          return;
        this.isBuddy = value;
        if (value)
          this.IsBlocked = false;
        this.OnPropertyChanged(nameof (IsBuddy));
        this.OnPropertyChanged("HideInContactsList");
      }
    }

    [JsonProperty(PropertyName = "blocked")]
    public bool IsBlocked
    {
      get => this.isBlocked;
      set
      {
        if (this.isBlocked == value)
          return;
        this.isBlocked = value;
        this.OnPropertyChanged(nameof (IsBlocked));
      }
    }

    public string PhoneNumber
    {
      get => this.phoneNumber;
      set
      {
        if (!(this.phoneNumber != value))
          return;
        this.phoneNumber = value;
        this.OnPropertyChanged(nameof (PhoneNumber));
      }
    }

    public string PhonebookAlias
    {
      get => this.phonebookAlias;
      set
      {
        if (!(this.phonebookAlias != value))
          return;
        this.phonebookAlias = value;
        this.OnPropertyChanged(nameof (PhonebookAlias));
        this.OnPropertyChanged("Alias");
      }
    }

    public long LastActivity
    {
      get => this.lastActivity;
      set
      {
        if (this.lastActivity == value)
          return;
        this.lastActivity = value;
        this.OnPropertyChanged(nameof (LastActivity));
        this.OnPropertyChanged("LastActivityFormatted");
      }
    }

    internal void UpdateLastActivityTimestampUI()
    {
      this.OnPropertyChanged("LastActivityFormatted");
    }

    public string LastActivityFormatted
    {
      get
      {
        return this.lastActivity == 0L ? (this.primitive != Primitive.Available ? "Away" : "Online") : (Utils.GetTimestamp() - this.lastActivity > 60000L ? "last seen " + Utils.ToFormattedTimestamp(this.lastActivity) : "Online");
      }
    }

    [JsonProperty(PropertyName = "favorite")]
    public bool IsFavorite
    {
      get => this.isFavorite;
      set
      {
        if (this.isFavorite == value)
          return;
        this.isFavorite = value;
        this.OnPropertyChanged(nameof (IsFavorite));
      }
    }

    [JsonProperty(PropertyName = "primitive")]
    [JsonConverter(typeof (LowercaseEnumConverter))]
    public Primitive Primitive
    {
      get => this.primitive;
      set
      {
        if (this.primitive == value)
          return;
        this.primitive = value;
        this.OnPropertyChanged(nameof (Primitive));
        this.OnPropertyChanged("HideInContactsList");
      }
    }

    [JsonProperty(PropertyName = "is_muted")]
    public bool IsMuted
    {
      get => this.IsGroup && this.isMuted;
      private set
      {
        if (this.isMuted == value)
          return;
        this.isMuted = value;
        this.OnPropertyChanged(nameof (IsMuted));
      }
    }

    public bool IsGroup => this.Buid != null && this.Buid.EndsWith(";");

    public string IconUrl
    {
      get
      {
        return this.Icon != null ? ImageUtils.GetPhotoUrlFromId(ImageUtils.GetObjectId(this.icon)) : (string) null;
      }
    }

    public string PhotoUrl
    {
      get
      {
        return this.Icon != null ? ImageUtils.GetPhotoUrlFromId(ImageUtils.GetObjectId(this.icon), PictureSize.Large) : (string) null;
      }
    }

    public bool HideInContactsList
    {
      get
      {
        if (!this.IsBuddy)
          return true;
        return !this.IsGroup && this.Primitive == Primitive.Offline;
      }
    }

    public List<Contact> GroupCallMembers
    {
      get => !this.IsGroup ? (List<Contact>) null : this.callMembers;
      set
      {
        this.callMembers = value;
        this.OnPropertyChanged(nameof (GroupCallMembers));
      }
    }

    public static bool IsGroupBuid(string buid) => buid != null && buid.EndsWith(";");

    public static bool IsGroupMemberBuid(string buid)
    {
      if (buid == null)
        return false;
      return buid.Split(';').Length == 3;
    }

    public static string GetUid(string buid)
    {
      if (Contact.IsGroupBuid(buid))
        return buid;
      if (Contact.IsGroupMemberBuid(buid))
        return buid.Split(';')[1];
      if (!buid.Contains<char>(';'))
        return buid;
      return buid.Split(';')[0];
    }

    public static string GetGid(string buid) => buid.Split(';')[0];

    internal void InvalidateSyncedGroupMembers()
    {
      this.hasSyncedGroupMembers = false;
      this.isSyncingGroupMembers = false;
    }

    internal void Mute() => this.InternalMute(true);

    internal void Unmute() => this.InternalMute(false);

    private void InternalMute(bool isMuted)
    {
      if (!this.IsGroup)
        throw new Exception("Only groups can be muted");
      IMO.IM.MuteGroup(Contact.GetGid(this.buid), isMuted, (Action) (() =>
      {
        this.IsMuted = isMuted;
        this.UpdateStorageIfNeeded();
      }));
    }

    internal void SyncGroupMembers(bool force = false)
    {
      if (!this.IsGroup || (this.hasSyncedGroupMembers || this.isSyncingGroupMembers) && !force)
        return;
      this.isSyncingGroupMembers = true;
      IMO.IM.GetGroupMembers(this.Gid, (Action<JToken>) (response =>
      {
        this.isSyncingGroupMembers = false;
        this.HasSyncedGroupMembers = true;
        Func<JToken, string> getBuidFromMemberUid = (Func<JToken, string>) (jObj =>
        {
          string str = jObj.Value<string>((object) "uid");
          return str.Substring(0, str.Length - ";imo".Length);
        });
        JArray source = JArray.Parse(response.ToString());
        foreach (Contact allGroupMembers in (IEnumerable<Contact>) this.GetAllGroupMembersList())
        {
          Contact member = allGroupMembers;
          if (source.Where<JToken>((Func<JToken, bool>) (x => getBuidFromMemberUid(x) == member.Buid)).Count<JToken>() == 0)
            this.InternalRemoveGroupMember(member);
        }
        foreach (JToken jtoken in source)
        {
          string str = getBuidFromMemberUid(jtoken);
          Contact contact = IMO.ContactsManager.GetOrCreateContact(str);
          contact.UpdateFromJObject((JToken) jtoken.ToObject<JObject>());
          if (!this.groupMembers.ContainsKey(str))
            this.InternalAddGroupMember(contact);
        }
        IMO.AVManager.GetGroupCallMembers(this.Buid, (Action<List<Contact>>) (t => this.GroupCallMembers = t));
      }));
    }

    public event EventHandler<EventArg<Contact>> GroupMemberAdded;

    public event EventHandler<EventArg<Contact>> GroupMemberRemoved;

    private void OnGroupMemberAdded(Contact contact)
    {
      EventHandler<EventArg<Contact>> groupMemberAdded = this.GroupMemberAdded;
      if (groupMemberAdded == null)
        return;
      groupMemberAdded((object) this, new EventArg<Contact>(contact));
    }

    private void OnGroupMemberRemoved(Contact contact)
    {
      EventHandler<EventArg<Contact>> groupMemberRemoved = this.GroupMemberRemoved;
      if (groupMemberRemoved == null)
        return;
      groupMemberRemoved((object) this, new EventArg<Contact>(contact));
    }

    public void AddGroupMember(Contact contact) => this.InternalAddGroupMember(contact);

    public void RemoveGroupMember(Contact contact) => this.InternalRemoveGroupMember(contact);

    private void InternalRemoveGroupMember(Contact member)
    {
      if (!this.groupMembers.ContainsKey(member.Buid))
        return;
      this.groupMembers.Remove(member.Buid);
      this.OnGroupMemberRemoved(member);
      this.OnPropertyChanged("GroupMembersCountString");
    }

    private void InternalAddGroupMember(Contact member)
    {
      this.groupMembers.Add(member.Buid, member);
      this.OnGroupMemberAdded(member);
      this.OnPropertyChanged("GroupMembersCountString");
    }

    public string GroupMembersCountString
    {
      get
      {
        return !this.HasSyncedGroupMembers ? string.Empty : string.Format("{0} members", (object) this.groupMembers.Count);
      }
    }

    public bool HasSyncedGroupMembers
    {
      get => this.hasSyncedGroupMembers;
      set
      {
        if (this.hasSyncedGroupMembers == value)
          return;
        this.hasSyncedGroupMembers = value;
        this.OnPropertyChanged(nameof (HasSyncedGroupMembers));
        this.OnPropertyChanged("GroupMembersCountString");
      }
    }

    public IList<Contact> GetAllGroupMembersList()
    {
      return (IList<Contact>) this.groupMembers.Values.ToList<Contact>();
    }

    internal void UpdateFromJObject(JToken jObj, bool notify = false)
    {
      JsonConvert.PopulateObject(jObj.ToString(), (object) this);
      this.UpdateStorageIfNeeded();
      if (!notify)
        return;
      this.OnPropertyChanged("");
    }

    public void UpdateStorageIfNeeded()
    {
      if (!this.isBuddy && !IMO.ConversationsManager.HasOpenConversationWith(this.buid))
        return;
      IMO.ApplicationStorage.AddContact(this);
    }

    internal async Task<bool> LeaveGroup(bool askForConfirmation = false)
    {
      if (!this.IsGroup)
        return false;
      if (askForConfirmation)
      {
        if (await ImoMessageBox.Show("Leave group " + this.Alias + "?", ImoMessageBoxButton.YesNo) != ImoMessageBoxResult.Yes)
          return false;
      }
      IMO.IM.LeaveGroup(Contact.GetGid(this.buid));
      return true;
    }

    public void Block()
    {
      IMO.IM.BlockContact(this.buid, (Action<JToken>) (result => this.IsBlocked = true));
    }

    internal void Unblock()
    {
      IMO.IM.UnblockContact(this.buid, (Action<JToken>) (result => this.IsBlocked = false));
    }

    public int CompareTo(Contact c)
    {
      if (this.IsFavorite != c.IsFavorite)
        return -this.IsFavorite.CompareTo(c.IsFavorite);
      if (this.IsGroup != c.IsGroup)
        return -this.IsGroup.CompareTo(c.IsGroup);
      if (this.Alias != null && c.Alias != null)
        return this.IsBuddy != c.IsBuddy ? -this.IsBuddy.CompareTo(c.IsBuddy) : this.Alias.CompareTo(c.Alias);
      if (this.Alias != null)
        return -1;
      return c.Alias != null ? 1 : this.buid.CompareTo(c.Buid);
    }
  }
}
