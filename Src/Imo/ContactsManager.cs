// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.ContactsManager
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.AV;
using ImoSilverlightApp.Helpers;
using ImoSilverlightApp.Storage.Models;
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace ImoSilverlightApp
{
  internal class ContactsManager : BaseManager
  {
    private static readonly Logger log = LogManager.GetLogger(typeof (ContactsManager).Name);
    private bool isSyncingContacts;
    public const string PIN = "pin";
    public const string IM = "im";
    public const string PREFERENCE = "preference";
    public const string IMOGROUPS = "imogroups";
    public const string SYNC_GROUP_MEMBERS = "sync_group_members";
    public const string CHANGE_BUDDY_ALIAS = "change_buddy_alias";
    public const string ADD_CONTACTS = "add_contacts";
    public const string INVITE_BATCH_TO_GROUP = "invite_batch_to_group";
    public const string CREATE_SHARED_GROUP = "create_shared_group";
    public const string GET_ONLINE_BUDDIES = "get_online_buddies";
    public const string SYNC_BUDDY_LIST = "sync_buddy_list";
    public const string INVITATION_RESPONSE = "invitation_response";
    public const string FAVORITE_BUDDY = "favorite_buddy";
    public const string UNFAVORITE_BUDDY = "unfavorite_buddy";
    public const string MUTE_GROUP = "mute_group";
    public const string KICK_MEMBER = "kick_member";
    private IDictionary<string, Contact> contacts;

    public event EventHandler<EventArg<Contact>> BuddyAdded;

    public event EventHandler<EventArg<Contact>> BuddyRemoved;

    public event EventHandler<EventArgs> ContactsCleared;

    public event EventHandler<EventArgs> ContactsReset;

    public string ContactsHash
    {
      get => IMO.User.ContactsHash;
      set => IMO.User.ContactsHash = value;
    }

    public ContactsManager()
    {
      this.contacts = (IDictionary<string, Contact>) new Dictionary<string, Contact>();
      IMO.AccountManager.SignedOn += new EventHandler<EventArg<SignOnData>>(this.HandleSignedOn);
      IMO.AccountManager.SignedOff += new EventHandler(this.HandleSignedOff);
      IMO.Dispatcher.Resetted += new EventHandler(this.Dispatcher_Resetted);
      if (!IMO.ApplicationSettings.IsCookieSignedOn)
        return;
      this.InitContactsFromStorage();
    }

    private void Dispatcher_Resetted(object sender, EventArgs e)
    {
      this.isSyncingContacts = false;
      foreach (Contact contact in (IEnumerable<Contact>) this.contacts.Values)
        contact.InvalidateSyncedGroupMembers();
    }

    private void InitContactsFromStorage()
    {
      foreach (KeyValuePair<string, JToken> contact in IMO.ApplicationStorage.GetContacts())
      {
        Contact fastFromJobject = Contact.CreateFastFromJObject(JObject.Parse(contact.Value.ToString()));
        if (this.contacts.ContainsKey(fastFromJobject.Buid))
          this.contacts[fastFromJobject.Buid].UpdateFromJObject(contact.Value, true);
        else
          this.contacts.Add(fastFromJobject.Buid, fastFromJobject);
      }
      this.OnContactsReset();
    }

    internal IList<Contact> GetAllContactsList()
    {
      return (IList<Contact>) this.contacts.Values.ToList<Contact>();
    }

    internal IList<Contact> GetAllBuddiesList()
    {
      return (IList<Contact>) this.contacts.Values.Where<Contact>((Func<Contact, bool>) (x => x.IsBuddy)).ToList<Contact>();
    }

    internal IList<Contact> GetAllGroupsList()
    {
      return (IList<Contact>) this.contacts.Values.Where<Contact>((Func<Contact, bool>) (x => x.IsBuddy && x.IsGroup)).ToList<Contact>();
    }

    protected void OnBuddyAdded(Contact contact)
    {
      EventHandler<EventArg<Contact>> buddyAdded = this.BuddyAdded;
      if (buddyAdded == null)
        return;
      buddyAdded((object) this, new EventArg<Contact>(contact));
    }

    private void OnBuddyRemoved(Contact contact)
    {
      EventHandler<EventArg<Contact>> buddyRemoved = this.BuddyRemoved;
      if (buddyRemoved == null)
        return;
      buddyRemoved((object) this, new EventArg<Contact>(contact));
    }

    private void OnContactsCleared()
    {
      EventHandler<EventArgs> contactsCleared = this.ContactsCleared;
      if (contactsCleared == null)
        return;
      contactsCleared((object) this, EventArgs.Empty);
    }

    private void OnContactsReset()
    {
      EventHandler<EventArgs> contactsReset = this.ContactsReset;
      if (contactsReset == null)
        return;
      contactsReset((object) this, EventArgs.Empty);
    }

    private void HandleSignedOn(object sender, EventArg<SignOnData> e) => this.SyncContactsList();

    private void HandleSignedOff(object sender, EventArgs e)
    {
      this.contacts.Clear();
      this.OnContactsCleared();
    }

    public void HandleMessage(JObject message)
    {
      string str = message.Value<string>((object) "name");
      if ("buddy_icon".Equals(str))
        this.HandleContactIcons(message);
      else if ("buddy_added".Equals(str))
        this.HandleBuddyAddedOrUpdated(message);
      else if ("buddy_status".Equals(str))
        this.HandleBuddyAddedOrUpdated(message);
      else if ("buddy_removed".Equals(str))
        this.HandleBuddyRemoved(message);
      else
        ContactsManager.log.Warn("unknown name: " + str);
    }

    public void AddImoContact(string buid, string source, Action<JToken> callback = null)
    {
      this.AddImoContacts((IList<string>) new List<string>()
      {
        buid
      }, source, callback);
    }

    internal void UpdateNewDayActivityTimestampsUI()
    {
      foreach (Contact contact in (IEnumerable<Contact>) this.contacts.Values)
        contact.UpdateLastActivityTimestampUI();
    }

    public void AddImoContacts(IList<string> buids, string source, Action<JToken> callback)
    {
      Dictionary<string, object> baseRpcDictionary = this.GetBaseRpcDictionary();
      baseRpcDictionary.Add(nameof (source), (object) source);
      baseRpcDictionary.Add(nameof (buids), (object) buids);
      BaseManager.Send("pin", "add_contacts", baseRpcDictionary, callback);
    }

    public void CreateGroup(
      string group_name,
      Action<string> successCallback,
      JObject groupProperties = null)
    {
      Dictionary<string, object> baseRpcDictionary = this.GetBaseRpcDictionary();
      baseRpcDictionary.Add("name", (object) group_name);
      baseRpcDictionary.Add("is_native", (object) false);
      if (groupProperties != null)
        baseRpcDictionary.Add("properties", (object) groupProperties);
      BaseManager.Send("im", "create_shared_group", baseRpcDictionary, (Action<JToken>) (t =>
      {
        string str = t.Value<string>();
        if (successCallback == null)
          return;
        successCallback(str);
      }));
    }

    public void SendInviteBuddiesToGroup(
      string gid,
      IList<Contact> invitedBuddies,
      Action<JToken> onMembersInvited = null)
    {
      if (invitedBuddies == null || invitedBuddies.Count <= 0)
        return;
      Dictionary<string, object> baseRpcDictionary = this.GetBaseRpcDictionary();
      baseRpcDictionary.Add(nameof (gid), (object) gid);
      baseRpcDictionary.Add("members", (object) this.GetGroupMembersInviteData(invitedBuddies));
      BaseManager.Send("im", "invite_batch_to_group", baseRpcDictionary, (Action<JToken>) (t =>
      {
        if (onMembersInvited == null)
          return;
        onMembersInvited(t);
      }));
    }

    protected JArray GetGroupMembersInviteData(IList<Contact> members)
    {
      JArray membersInviteData = new JArray();
      foreach (Contact member in (IEnumerable<Contact>) members)
        membersInviteData.Add((JToken) new JObject()
        {
          {
            "iuid",
            (JToken) IMO.User.Uid
          },
          {
            "iproto",
            (JToken) "imo"
          },
          {
            "ibuid",
            (JToken) member.Buid
          },
          {
            "last_name",
            (JToken) ""
          },
          {
            "first_name",
            (JToken) ""
          }
        });
      return membersInviteData;
    }

    public async Task<bool> DeleteContact(Contact contact, bool promptForConfirmation = false)
    {
      if (promptForConfirmation)
      {
        if (await ImoMessageBox.Show("Remove " + contact.Alias + " from Contacts?", ImoMessageBoxButton.YesNo) != ImoMessageBoxResult.Yes)
          return false;
      }
      this.SendContactAction("del_buddy", contact.Buid);
      return true;
    }

    public void UnblockContact(string buid) => this.SendContactAction("unblock_buddy", buid);

    public void SyncContactsList()
    {
      if (this.isSyncingContacts)
        return;
      string account_uid = IMO.User.Uid;
      if (account_uid == null)
        return;
      Dictionary<string, object> baseRpcDictionary = this.GetBaseRpcDictionary();
      baseRpcDictionary.Add("blist_hash", (object) this.ContactsHash);
      this.isSyncingContacts = true;
      Action<JToken> successCallback = (Action<JToken>) (response =>
      {
        this.isSyncingContacts = false;
        if (response != null)
        {
          string str = response.Value<string>((object) "members_hash");
          if (str != null)
          {
            this.UpdateFromMembersResponse(response.Value<JArray>((object) "members"));
            this.ContactsHash = str;
            IMO.AVManager.AnybodyThere();
          }
          else if (this.ContactsHash == null)
          {
            this.SyncContactsList();
          }
          else
          {
            JArray jarray = response.Value<JArray>((object) "primitives");
            List<Contact> list = this.contacts.Values.Where<Contact>((Func<Contact, bool>) (x => x.IsBuddy)).OrderBy<Contact, string>((Func<Contact, string>) (x => x.Buid)).ToList<Contact>();
            if (list.Count != jarray.Count)
            {
              ContactsManager.log.Error(string.Format("Primitives count different after sync. Weird. Got: {0}, Expected: {1}. Resyncing contacts.", (object) jarray.Count, (object) list.Count), 348, nameof (SyncContactsList));
              this.ContactsHash = (string) null;
              this.SyncContactsList();
            }
            else
            {
              for (int index = 0; index < jarray.Count; ++index)
                list[index].Primitive = (Primitive) Enum.Parse(typeof (Primitive), jarray[index].ToString(), true);
              IMO.AVManager.AnybodyThere();
            }
          }
        }
        else
          ContactsManager.log.Error(string.Format("SyncBuddyList for account {0} response is null!", (object) account_uid), 367, nameof (SyncContactsList));
      });
      BaseManager.Send("im", "sync_buddy_list", baseRpcDictionary, successCallback);
    }

    private void UpdateFromMembersResponse(JArray members)
    {
      ContactsManager.log.Debug("updating members from response");
      foreach (Contact allBuddies in (IEnumerable<Contact>) this.GetAllBuddiesList())
      {
        Contact contact = allBuddies;
        if (members.Where<JToken>((Func<JToken, bool>) (x => x.Value<string>((object) "buid") == contact.Buid)).Count<JToken>() == 0)
          this.RemoveBuddy(contact.Buid);
      }
      members.ToList<JToken>().ForEach((Action<JToken>) (member =>
      {
        string key = member.Value<string>((object) "buid");
        if (this.contacts.ContainsKey(key))
        {
          Contact contact = this.contacts[key];
          contact.UpdateFromJObject((JToken) member.ToObject<JObject>());
          if (contact.IsBuddy)
            return;
          this.InternalMakeBuddy(contact);
        }
        else
        {
          Contact fastFromJobject = Contact.CreateFastFromJObject(member.ToObject<JObject>());
          fastFromJobject.IsBuddy = true;
          this.InternalAddContact(fastFromJobject);
        }
      }));
    }

    private void SendContactAction(string method, string buid)
    {
      Dictionary<string, object> baseRpcDictionary = this.GetBaseRpcDictionary();
      baseRpcDictionary.Add(nameof (buid), (object) buid);
      BaseManager.Send("im", method, baseRpcDictionary);
    }

    public void GetBlockedBuddies(Action<JToken> callback)
    {
      BaseManager.Send("pin", "get_blocked_buddies", this.GetBaseRpcDictionary(), callback);
    }

    private void HandleBuddyRemoved(JObject message)
    {
      string buid = message.Value<JObject>((object) "edata").Value<string>((object) "buid");
      if (Contact.IsGroupMemberBuid(buid))
      {
        Contact contact = IMO.ContactsManager.GetOrCreateContact(Contact.GetGid(buid) + ";");
        IMO.ContactsManager.GetOrCreateContact(Contact.GetUid(buid));
        int num = contact.HasSyncedGroupMembers ? 1 : 0;
      }
      else
        this.RemoveBuddy(buid);
    }

    public Contact GetOrCreateContact(string buid)
    {
      Contact contact = (Contact) null;
      this.contacts.TryGetValue(buid, out contact);
      if (contact == null)
      {
        contact = new Contact(buid);
        contact.IsBuddy = false;
        this.InternalAddContact(contact);
      }
      return contact;
    }

    private void HandleBuddyAddedOrUpdated(JObject message)
    {
      foreach (JObject jObj in message.Value<JArray>((object) "edata"))
      {
        string key = jObj.Value<string>((object) "buid");
        if (this.contacts.ContainsKey(key))
        {
          Contact contact = this.contacts[key];
          contact.UpdateFromJObject((JToken) jObj);
          if (!contact.IsBuddy)
            this.InternalMakeBuddy(contact);
        }
        else
        {
          Contact contact = jObj.ToObject<Contact>();
          contact.IsBuddy = true;
          this.InternalAddContact(contact);
        }
      }
    }

    private void HandleContactIcons(JObject message) => this.HandleBuddyAddedOrUpdated(message);

    public void AddContactToFavorites(Contact contact) => this.SendFavorite(contact, true);

    public void RemoveContactFromFavorites(Contact contact) => this.SendFavorite(contact, false);

    private void SendFavorite(Contact contact, bool favorite)
    {
      Dictionary<string, object> baseRpcDictionary = this.GetBaseRpcDictionary();
      baseRpcDictionary.Add("buid", (object) contact.Buid);
      BaseManager.Send("preference", favorite ? "favorite_buddy" : "unfavorite_buddy", baseRpcDictionary);
      contact.IsFavorite = favorite;
    }

    private void RemoveBuddy(string buid)
    {
      if (!this.contacts.ContainsKey(buid))
        return;
      Contact contact = this.contacts[buid];
      if (!IMO.ConversationsManager.HasOpenConversationWith(buid))
        IMO.ApplicationStorage.RemoveContact(contact.Buid);
      this.ContactsHash = (string) null;
      if (contact.IsBuddy)
      {
        contact.IsBuddy = false;
        if (contact.IsGroup)
          IMO.ConversationsManager.GetOrCreateConversation(contact.Buid).ShowInChatsList = false;
        this.OnBuddyRemoved(contact);
      }
      if (!contact.IsGroup)
        return;
      Conversation openConversation = IMO.ConversationsManager.CurrentOpenConversation;
      if (openConversation != null && openConversation.Contact == contact)
        IMO.NavigationManager.NavigateToHome();
      else
        IMO.NavigationManager.NavigatePastOpenChat(buid);
      CallController callController = IMO.AVManager.CallController;
      if (callController != null && callController is GroupCallController && callController.Buid == buid)
        IMO.AVManager.SelfEndCall();
      IMO.AVManager.OnGroupCallsListChanged();
    }

    public void KickGroupMember(string buid, string gid)
    {
      Dictionary<string, object> baseRpcDictionary = this.GetBaseRpcDictionary();
      baseRpcDictionary.Add(nameof (buid), (object) string.Format("{0};{1};{2}", (object) gid, (object) buid, (object) "imo"));
      BaseManager.Send("im", "kick_member", baseRpcDictionary, (Action<JToken>) (t => { }));
    }

    private void InternalAddContact(Contact contact)
    {
      this.contacts.Add(contact.Buid, contact);
      if (!contact.IsBuddy)
        return;
      this.InternalMakeBuddy(contact);
    }

    private void InternalMakeBuddy(Contact contact)
    {
      contact.IsBuddy = true;
      IMO.ApplicationStorage.AddContact(contact);
      this.ContactsHash = (string) null;
      this.OnBuddyAdded(contact);
    }

    private Dictionary<string, object> GetBaseRpcDictionary()
    {
      return new Dictionary<string, object>()
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
          "proto",
          (object) "imo"
        }
      };
    }
  }
}
