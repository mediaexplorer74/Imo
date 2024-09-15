// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.UI.Views.SearchContactsPageViewModel
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Helpers;
using ImoSilverlightApp.Storage.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;


namespace ImoSilverlightApp.UI.Views
{
  internal class SearchContactsPageViewModel : ViewModelBase
  {
    private ObservableCollection<SearchListItemBase> searchResults;
    private IDictionary<string, string> phonebookContacts;
    private string searchText;
    private string lastSearchDirectoryText;
    private bool showNoResultsMessage;
    private int searchId;

    public SearchContactsPageViewModel(FrameworkElement el)
      : base(el)
    {
      this.searchResults = new ObservableCollection<SearchListItemBase>();
    }

    protected override void OnLoaded(object sender, RoutedEventArgs e)
    {
      this.phonebookContacts = (IDictionary<string, string>) null;
      IMO.ContactsManager.ContactsReset += new EventHandler<EventArgs>(this.ContactsManager_ContactsReset);
      IMO.QueryEngine.ReceivedResult += new EventHandler<EventArg<JObject>>(this.QueryEngine_ReceivedResult);
    }

    protected override void OnUnloaded(object sender, RoutedEventArgs e)
    {
      IMO.ContactsManager.ContactsReset -= new EventHandler<EventArgs>(this.ContactsManager_ContactsReset);
      IMO.QueryEngine.ReceivedResult -= new EventHandler<EventArg<JObject>>(this.QueryEngine_ReceivedResult);
    }

    private void ContactsManager_ContactsReset(object sender, EventArgs e) => this.SearchContacts();

    public ObservableCollection<SearchListItemBase> SearchResults
    {
      get => this.searchResults;
      set
      {
        if (this.searchResults == value)
          return;
        this.searchResults = value;
        this.OnPropertyChanged(nameof (SearchResults));
      }
    }

    private async void SearchContacts()
    {
      ++this.searchId;
      string text = this.searchText;
      if (text == null || text.Length == 0)
      {
        this.SearchResults.Clear();
        this.ShowNoResultsMessage = false;
      }
      else
      {
        Func<Contact, bool> predicate = (Func<Contact, bool>) (contact => !contact.HideInContactsList && contact.Alias.ToLower().Contains(text.ToLower()));
        List<Contact> list = IMO.ContactsManager.GetAllBuddiesList().Where<Contact>(predicate).ToList<Contact>();
        list.Sort((Comparison<Contact>) ((x, y) =>
        {
          bool flag1 = IMO.ConversationsManager.HasOpenConversationWith(x.Buid);
          bool flag2 = IMO.ConversationsManager.HasOpenConversationWith(y.Buid);
          return flag1 != flag2 ? -flag1.CompareTo(flag2) : x.CompareTo(y);
        }));
        this.SearchResults = new ObservableCollection<SearchListItemBase>(list.Select<Contact, SearchListItemBase>((Func<Contact, SearchListItemBase>) (x => (SearchListItemBase) new SearchListContact(x))));
        if (new Regex("^[-0-9() +]*$").Match(this.searchText).Success)
          this.SearchResults.Add((SearchListItemBase) AddToContactsItem.Instance);
        this.SearchResults.Add((SearchListItemBase) SearchDirectoryItem.Instance);
        if (this.phonebookContacts == null)
        {
          int currentSearchId = this.searchId;
          SearchContactsPageViewModel contactsPageViewModel = this;
          IDictionary<string, string> phonebookContacts = contactsPageViewModel.phonebookContacts;
          Dictionary<string, string> phoneContactsSafe = await IMO.PhonebookManager.GetPhoneContactsSafe();
          contactsPageViewModel.phonebookContacts = (IDictionary<string, string>) phoneContactsSafe;
          contactsPageViewModel = (SearchContactsPageViewModel) null;
          if (currentSearchId != this.searchId)
            return;
        }
        foreach (KeyValuePair<string, string> phonebookContact in (IEnumerable<KeyValuePair<string, string>>) this.phonebookContacts)
        {
          string key = phonebookContact.Key;
          string name = phonebookContact.Value;
          if (key.Contains(text) || name.ToLower().Contains(text.ToLower()))
            this.SearchResults.Add((SearchListItemBase) new SearchListPhonebookContact(name, key));
        }
        this.ShowNoResultsMessage = this.SearchResults.Count == 0;
      }
    }

    internal async void AddContact()
    {
      if (await IMO.PhonebookManager.AddContact(this.searchText))
        return;
      Utils.ShowPopup("No imo user found with this number!");
    }

    internal void SearchDirectory()
    {
      this.SearchResults.Remove((SearchListItemBase) SearchDirectoryItem.Instance);
      this.lastSearchDirectoryText = this.searchText;
      IMO.QueryEngine.Search(this.searchText);
    }

    private void QueryEngine_ReceivedResult(object sender, EventArg<JObject> e)
    {
      if (this.searchText != this.lastSearchDirectoryText)
        return;
      foreach (JToken jtoken in e.Arg.Value<JArray>((object) "results"))
      {
        string buid = jtoken.Value<string>((object) "uid");
        string str1 = jtoken.Value<string>((object) "profile_photo_id");
        string str2 = jtoken.Value<string>((object) "display_name");
        jtoken.Value<string>((object) "phone_cc");
        if (!jtoken.Value<bool>((object) "is_in_contacts"))
        {
          Contact contact = IMO.ContactsManager.GetOrCreateContact(buid);
          contact.Alias = str2;
          contact.Icon = str1;
          this.searchResults.Add((SearchListItemBase) new SearchListContact(contact));
        }
      }
    }

    public string SearchText
    {
      get => this.searchText;
      set
      {
        if (!(this.searchText != value))
          return;
        this.searchText = value;
        this.SearchContacts();
        this.OnPropertyChanged(nameof (SearchText));
      }
    }

    public bool ShowNoResultsMessage
    {
      get => this.showNoResultsMessage;
      set
      {
        if (this.showNoResultsMessage == value)
          return;
        this.showNoResultsMessage = value;
        this.OnPropertyChanged(nameof (ShowNoResultsMessage));
      }
    }
  }
}
