// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.UI.Views.ContactSelectorViewModel
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Storage.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;


namespace ImoSilverlightApp.UI.Views
{
  internal class ContactSelectorViewModel : ViewModelBase
  {
    private List<SelectorItemBase> searchResults;
    private ObservableCollection<SelectorItemBase> selectedItems;
    private string searchText = "";
    private bool showNoResultsMessage;
    private bool hasGroups;
    private bool showBusyIndicator;
    private bool isPhonebookOnly;
    private IDictionary<string, string> phonebookContacts;

    public ContactSelectorViewModel(FrameworkElement element)
      : base(element)
    {
      this.searchResults = new List<SelectorItemBase>();
      this.selectedItems = new ObservableCollection<SelectorItemBase>();
      this.selectedItems.CollectionChanged += new NotifyCollectionChangedEventHandler(this.SelectedContacts_CollectionChanged);
    }

    protected override void OnLoaded(object sender, RoutedEventArgs e)
    {
      ContactSelectorView contactSelectorView = sender as ContactSelectorView;
      this.hasGroups = contactSelectorView.HasGroups;
      this.isPhonebookOnly = contactSelectorView.IsPhonebookOnly;
      this.Search(this.searchText);
    }

    private void SelectedContacts_CollectionChanged(
      object sender,
      NotifyCollectionChangedEventArgs e)
    {
      this.Search(this.SearchText);
    }

    public Contact ExistingGroup { get; set; }

    public string SearchText
    {
      get => this.searchText;
      set
      {
        if (!(this.searchText != value))
          return;
        this.searchText = value;
        this.Search(this.searchText);
        this.OnPropertyChanged(nameof (SearchText));
      }
    }

    public void Clear()
    {
      this.selectedItems.Clear();
      this.searchText = "";
      this.Search("");
    }

    public List<SelectorItemBase> SearchResults
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

    public bool ShowBusyIndicator
    {
      get => this.showBusyIndicator;
      set
      {
        if (this.showBusyIndicator == value)
          return;
        this.showBusyIndicator = value;
        this.OnPropertyChanged(nameof (ShowBusyIndicator));
      }
    }

    internal IList<Contact> GetSelectedContacts()
    {
      return (IList<Contact>) this.selectedItems.Where<SelectorItemBase>((Func<SelectorItemBase, bool>) (x => x is SelectorContactItem)).Select<SelectorItemBase, Contact>((Func<SelectorItemBase, Contact>) (x => (x as SelectorContactItem).Contact)).ToList<Contact>();
    }

    public ObservableCollection<SelectorItemBase> SelectedItems => this.selectedItems;

    public void SelectContact(SelectorItemBase selectorItem)
    {
      if (this.selectedItems.Contains(selectorItem))
        return;
      this.selectedItems.Add(selectorItem);
      this.SearchText = "";
    }

    public void DeselectItem(SelectorItemBase items)
    {
      if (!this.selectedItems.Contains(items))
        return;
      this.selectedItems.Remove(items);
    }

    private async void Search(string text)
    {
      if (IMO.ContactsManager == null)
        return;
      if (this.isPhonebookOnly)
        this.SearchResults = await this.SearchPhonebook(text);
      else
        this.SearchResults = this.SearchContacts(text);
      this.ShowNoResultsMessage = this.SearchResults.Count == 0;
    }

    private async Task<List<SelectorItemBase>> SearchPhonebook(string text)
    {
      if (this.phonebookContacts == null)
      {
        this.ShowBusyIndicator = true;
        ContactSelectorViewModel selectorViewModel = this;
        IDictionary<string, string> phonebookContacts = selectorViewModel.phonebookContacts;
        Dictionary<string, string> phoneContactsSafe = await IMO.PhonebookManager.GetPhoneContactsSafe();
        selectorViewModel.phonebookContacts = (IDictionary<string, string>) phoneContactsSafe;
        selectorViewModel = (ContactSelectorViewModel) null;
        this.ShowBusyIndicator = false;
      }
      Func<KeyValuePair<string, string>, bool> predicate = (Func<KeyValuePair<string, string>, bool>) (kvp => (string.IsNullOrEmpty(text) || kvp.Value.ToLower().Contains(text.ToLower())) && !this.selectedItems.Contains((SelectorItemBase) new SelectorPhonebookContactItem(kvp.Key, kvp.Value)));
      List<SelectorItemBase> selectorItemBaseList = new List<SelectorItemBase>();
      foreach (KeyValuePair<string, string> keyValuePair in this.phonebookContacts.Where<KeyValuePair<string, string>>(predicate).ToList<KeyValuePair<string, string>>())
      {
        if (keyValuePair.Value.ToLower().Contains(text.ToLower()) || string.IsNullOrEmpty(text))
          selectorItemBaseList.Add((SelectorItemBase) new SelectorPhonebookContactItem(keyValuePair.Key, keyValuePair.Value));
      }
      return selectorItemBaseList;
    }

    private List<SelectorItemBase> SearchContacts(string text)
    {
      Contact existingGroup = this.ExistingGroup;
      List<Contact> existingGroupGroupMembers = existingGroup != null ? existingGroup.GetAllGroupMembersList().ToList<Contact>() : (List<Contact>) null;
      Func<Contact, bool> predicate = (Func<Contact, bool>) (contact => !contact.HideInContactsList && (!contact.IsGroup || this.hasGroups) && IMO.User.Uid != contact.Buid && (text == "" || contact.Alias.ToLower().Contains(text.ToLower())) && (existingGroupGroupMembers == null || !existingGroupGroupMembers.Contains(contact)) && !this.selectedItems.Contains((SelectorItemBase) new SelectorContactItem(contact)));
      List<Contact> list = IMO.ContactsManager.GetAllBuddiesList().Where<Contact>(predicate).ToList<Contact>();
      list.Sort((Comparison<Contact>) ((x, y) =>
      {
        bool flag1 = IMO.ConversationsManager.HasOpenConversationWith(x.Buid);
        bool flag2 = IMO.ConversationsManager.HasOpenConversationWith(y.Buid);
        return flag1 != flag2 ? -flag1.CompareTo(flag2) : x.CompareTo(y);
      }));
      List<SelectorItemBase> selectorItemBaseList = new List<SelectorItemBase>();
      selectorItemBaseList.AddRange((IEnumerable<SelectorItemBase>) list.Select<Contact, SelectorContactItem>((Func<Contact, SelectorContactItem>) (x => new SelectorContactItem(x))));
      return selectorItemBaseList;
    }
  }
}
