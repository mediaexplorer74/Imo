// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.Storage.ApplicationStorage
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Helpers;
using ImoSilverlightApp.Storage.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace ImoSilverlightApp.Storage
{
  internal class ApplicationStorage : PersistentStorage
  {
    private const string TABLE_SETTINGS = "Settings";
    private const string TABLE_CONTACTS = "Contacts";
    private const string TABLE_CONVERSATIONS = "Conversations";
    private const string TABLE_UPLOADED_PHONEBOOK = "UploadedPhonebook";
    private const string TABLE_USER = "User";
    private const string TABLE_BROADCASTS = "Broadcasts";
    private const string TABLE_STICKER_PACKS = "StickerPacks";
    private const string TABLE_UNACKED_MESSAGES = "UnackedMessages";
    private const string TABLE_CONVERSATION_HISTORY = "ConversationHistory";
    private static ApplicationStorage instance;

    private ApplicationStorage()
    {
      this.InitTree(Path.Combine(FSUtils.GetOrCreateCacheDir(), "data"));
    }

    public static ApplicationStorage Instance
    {
      get
      {
        if (ApplicationStorage.instance == null)
          ApplicationStorage.instance = new ApplicationStorage();
        return ApplicationStorage.instance;
      }
    }

    public JObject GetSettings() => this.InternalGetDictionary("Settings");

    public JObject GetUploadedPhonebook() => this.InternalGetDictionary("UploadedPhonebook");

    public JObject GetStickerPacks() => this.InternalGetDictionary("StickerPacks");

    public IList<JObject> GetUnackedMessages()
    {
      return (IList<JObject>) this.InternalGetDictionary("UnackedMessages").Select<KeyValuePair<string, JToken>, JObject>((Func<KeyValuePair<string, JToken>, JObject>) (x => JObject.Parse(x.Value.ToString()))).OrderBy<JObject, string>((Func<JObject, string>) (x => x.Value<string>((object) "timestamp"))).ToList<JObject>();
    }

    public void SetSettingsProperty(string key, string value)
    {
      this.InternalSetProperty("Settings", key, value);
    }

    public void SetUserProperty(string key, string value)
    {
      this.InternalSetProperty("User", key, value);
    }

    internal void AddUploadedPhonebookContact(string phoneNumber, string value)
    {
      this.InternalSetProperty("UploadedPhonebook", phoneNumber, value);
    }

    internal string AddUnackedMessage(string buid, string msg, JObject imdata)
    {
      long timestamp = Utils.GetTimestamp();
      JObject jobject = new JObject();
      jobject.Add(nameof (imdata), (JToken) imdata);
      jobject.Add(nameof (msg), (JToken) msg);
      jobject.Add("timestamp", (JToken) timestamp);
      jobject.Add(nameof (buid), (JToken) buid);
      string unsafeRandomString;
      do
      {
        unsafeRandomString = Utils.GetUnsafeRandomString(16);
      }
      while (this.InternalContainsKey("UnackedMessages", unsafeRandomString));
      this.InternalSetProperty("UnackedMessages", unsafeRandomString, jobject.ToString());
      return unsafeRandomString;
    }

    internal void ClearPendingSendMessages() => this.InternalClearTable("UnackedMessages");

    public void SetPhonebookProperty(string key, string value)
    {
      this.InternalSetProperty("UploadedPhonebook", key, value);
    }

    public JObject GetContacts() => this.InternalGetDictionary("Contacts");

    public JObject GetRecentConversations() => this.InternalGetDictionary("Conversations");

    public JObject GetConversations() => this.InternalGetDictionary("Conversations");

    public void AddConversation(Conversation conversation)
    {
      this.InternalSetProperty("Conversations", conversation.Buid, JsonConvert.SerializeObject((object) conversation));
    }

    public void AddConversationHistory(string buid, IList<Message> messages)
    {
      this.InternalSetProperty("ConversationHistory", buid, JsonConvert.SerializeObject((object) messages));
    }

    public JArray GetConversationHistory(string buid)
    {
      string json = this.InternalGetValue("ConversationHistory", buid);
      return string.IsNullOrEmpty(json) ? new JArray() : JArray.Parse(json);
    }

    public void RemoveConversation(string buid)
    {
      this.InternalRemoveProperty("Conversations", buid);
    }

    public JObject GetUser() => this.InternalGetDictionary("User");

    public void SetContactsList(IList<Contact> contacts)
    {
      this.InternalClearTable("Contacts");
      for (int index = 0; index < contacts.Count; ++index)
      {
        Contact contact = contacts[index];
        this.InternalSetProperty("Contacts", contact.Buid, JsonConvert.SerializeObject((object) contact));
      }
    }

    public void ClearConversations() => this.InternalClearTable("Conversations");

    public void ClearUploadedPhonebook() => this.InternalClearTable("UploadedPhonebook");

    public void ClearBroadcasts() => this.InternalClearTable("Broadcasts");

    public void AddContact(Contact contact)
    {
      this.InternalSetProperty("Contacts", contact.Buid, JsonConvert.SerializeObject((object) contact));
    }

    public void RemoveContact(string buid) => this.InternalRemoveProperty("Contacts", buid);

    internal void RemoveUnackedMessage(string id)
    {
      this.InternalRemoveProperty("UnackedMessages", id);
    }

    public void ClearUser() => this.InternalClearTable("User");

    public void AddStickerPack(StickerPack stickerPack)
    {
      this.InternalSetProperty("StickerPacks", stickerPack.PackId, JsonConvert.SerializeObject((object) stickerPack));
    }

    public void RemoveStickerPack(StickerPack stickerPack)
    {
      this.InternalRemoveProperty("StickerPacks", stickerPack.PackId);
    }
  }
}
