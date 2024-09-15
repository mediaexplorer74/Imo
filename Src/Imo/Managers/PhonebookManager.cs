// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.Managers.PhonebookManager
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Helpers;
using ImoSilverlightApp.Storage.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Chat;
using Windows.ApplicationModel.Contacts;


namespace ImoSilverlightApp.Managers
{
  internal class PhonebookManager
  {
    private static readonly Logger log = LogManager.GetLogger(typeof (PhonebookManager).Name);
    private bool isInitializedFromStorage;
    private bool hasUploadedPhoneBook;
    private bool isUploadingPhoneBook;
    private IDictionary<string, UploadedPhoneContact> uploadedPhonebook;

    public PhonebookManager()
    {
      IMO.Dispatcher.Resetted += new EventHandler(this.Dispatcher_Resetted);
    }

    private void Dispatcher_Resetted(object sender, EventArgs e)
    {
      this.isUploadingPhoneBook = false;
    }

    private async Task<Dictionary<string, string>> GetPhoneContacts()
    {
      Dictionary<string, string> phoneContacts = new Dictionary<string, string>();
      ContactStore contactStore = await ContactManager.RequestStoreAsync();
      if (contactStore == null)
        return (Dictionary<string, string>) null;
      IReadOnlyList<Contact> contactsAsync = await contactStore.FindContactsAsync();
      if (contactsAsync == null)
        return (Dictionary<string, string>) null;
      foreach (Contact contact in contactsAsync.ToList<Contact>())
      {
        foreach (ContactPhone phone in (IEnumerable<ContactPhone>) contact.Phones)
          phoneContacts[phone.Number] = contact.DisplayName;
      }
      return phoneContacts;
    }

    public async Task<Dictionary<string, string>> GetPhoneContactsSafe()
    {
      try
      {
        return await this.GetPhoneContacts() ?? new Dictionary<string, string>();
      }
      catch (Exception ex)
      {
        IMO.MonitorLog.Log("phonebook", "phonebook_error");
        return new Dictionary<string, string>();
      }
    }

    public async void UploadPhonebook(bool force = false)
    {
      if (!force && this.hasUploadedPhoneBook || this.isUploadingPhoneBook)
        return;
      this.isUploadingPhoneBook = true;
      if (!this.isInitializedFromStorage)
      {
        this.uploadedPhonebook = this.GetUploadedPhonebookFromStorage();
        this.isInitializedFromStorage = true;
      }
      Dictionary<string, string> phoneContactsSafe = await this.GetPhoneContactsSafe();
      if (phoneContactsSafe.Count == 0)
      {
        IMO.MonitorLog.Log("phonebook", "empty_phonebook");
      }
      else
      {
        List<JObject> notUploadedPhoneContacts = new List<JObject>();
        foreach (KeyValuePair<string, string> keyValuePair in phoneContactsSafe)
        {
          if (!this.uploadedPhonebook.ContainsKey(keyValuePair.Key))
            notUploadedPhoneContacts.Add(new JObject()
            {
              {
                "contact",
                (JToken) keyValuePair.Key
              },
              {
                "contact_type",
                (JToken) "phone"
              },
              {
                "rank",
                (JToken) 0
              },
              {
                "is_favorite",
                (JToken) false
              },
              {
                "name",
                (JToken) keyValuePair.Value
              }
            });
        }
        if (notUploadedPhoneContacts.Count > 0)
          IMO.ImoAccount.AddPhonebook((IList<JObject>) notUploadedPhoneContacts, (Action<JToken>) (result =>
          {
            this.isUploadingPhoneBook = false;
            if (!result.HasValues)
              return;
            this.hasUploadedPhoneBook = true;
            this.LogUploadedPhonebook(notUploadedPhoneContacts.Count);
            JArray source = result.Value<JArray>((object) "existing_accounts");
            foreach (JObject jobject in notUploadedPhoneContacts)
            {
              string phoneNumber = jobject.Value<string>((object) "contact");
              UploadedPhoneContact uploadedPhoneContact = new UploadedPhoneContact(jobject.Value<string>((object) "contact"), jobject.Value<string>((object) "name"), source.Where<JToken>((Func<JToken, bool>) (x => x.Value<string>((object) "phone") == phoneNumber)).Select<JToken, string>((Func<JToken, string>) (x => x.Value<string>((object) "uid"))).FirstOrDefault<string>());
              IMO.ApplicationStorage.AddUploadedPhonebookContact(phoneNumber, JsonConvert.SerializeObject((object) uploadedPhoneContact));
              this.uploadedPhonebook[uploadedPhoneContact.PhoneNumber] = uploadedPhoneContact;
            }
            foreach (JToken jtoken in source)
            {
              string buid = jtoken.Value<string>((object) "uid");
              string key = jtoken.Value<string>((object) "phone");
              Contact contact = IMO.ContactsManager.GetOrCreateContact(buid);
              contact.PhoneNumber = key;
              contact.PhonebookAlias = this.uploadedPhonebook[key].DisplayName;
              contact.UpdateStorageIfNeeded();
            }
          }));
        else
          this.isUploadingPhoneBook = false;
      }
    }

    public async Task<bool> AddContact(string phone)
    {
      TaskCompletionSource<bool> taskSource = new TaskCompletionSource<bool>();
      IMO.ImoAccount.AddContact(phone, (Action<JToken>) (result =>
      {
        if (result.HasValues)
        {
          JArray jarray = result.Value<JArray>((object) "existing_accounts");
          if (jarray.Count == 1)
          {
            string buid = jarray[0].Value<string>((object) "uid");
            IMO.ContactsManager.GetOrCreateContact(buid).PhoneNumber = jarray[0].Value<string>((object) nameof (phone));
            IMO.NavigationManager.NavigateToConversation(buid);
            taskSource.SetResult(true);
          }
          else
            taskSource.SetResult(false);
        }
        else
          Utils.ShowPopup("No user found with phone number: " + phone);
      }));
      return await taskSource.Task;
    }

    private void LogUploadedPhonebook(int count)
    {
      IMO.MonitorLog.Log("phonebook", new JObject()
      {
        {
          "uploaded_phonebook",
          (JToken) 1
        },
        {
          "uploaded_phonebook_count",
          (JToken) count
        }
      });
    }

    private IDictionary<string, UploadedPhoneContact> GetUploadedPhonebookFromStorage()
    {
      JObject uploadedPhonebook = IMO.ApplicationStorage.GetUploadedPhonebook();
      Dictionary<string, UploadedPhoneContact> phonebookFromStorage = new Dictionary<string, UploadedPhoneContact>();
      foreach (JProperty property in uploadedPhonebook.Properties())
        phonebookFromStorage[property.Name] = JsonConvert.DeserializeObject<UploadedPhoneContact>(property.Value.ToString());
      return (IDictionary<string, UploadedPhoneContact>) phonebookFromStorage;
    }

    public async Task SendSms(IList<string> phoneNumbers)
    {
      try
      {
        ChatMessage chatMessage = new ChatMessage();
        chatMessage.put_Body("Let's video chat and text on imo! Get the free app http://imo.im");
        foreach (string phoneNumber in (IEnumerable<string>) phoneNumbers)
          chatMessage.Recipients.Add(phoneNumber);
        await ChatMessageManager.ShowComposeSmsMessageAsync(chatMessage);
      }
      catch (Exception ex)
      {
        PhonebookManager.log.Error(ex, 236, nameof (SendSms));
        Utils.ShowPopup("Error!");
      }
    }
  }
}
