// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.UI.Views.ContactsListViewModel
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Collections;
using ImoSilverlightApp.Storage.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;


namespace ImoSilverlightApp.UI.Views
{
  internal class ContactsListViewModel : ViewModelBase
  {
    private static readonly Logger log = LogManager.GetLogger(typeof (ContactsListViewModel).Name);
    private SortedObservableCollection<ContactsListItemBase> contactItems;

    public ContactsListViewModel(FrameworkElement el)
      : base(el)
    {
      this.contactItems = new SortedObservableCollection<ContactsListItemBase>((Comparer<ContactsListItemBase>) new ContactsListViewModel.ContactsListItemComparer());
      this.InternalReset();
    }

    private void InternalReset()
    {
      IEnumerable<Contact> source = IMO.ContactsManager.GetAllBuddiesList().Where<Contact>((Func<Contact, bool>) (x => !x.HideInContactsList));
      this.contactItems.SetItems(source.Select<Contact, ContactsListContactItem>((Func<Contact, ContactsListContactItem>) (x => !x.IsGroup ? new ContactsListContactItem(x) : (ContactsListContactItem) new ContactsListGroupContactItem(x))).Cast<ContactsListItemBase>());
      this.contactItems.Add((ContactsListItemBase) new ContactsListInviteItem());
      this.contactItems.Add((ContactsListItemBase) new ContactsListCreateGroupItem());
      foreach (ModelBase modelBase in source)
        modelBase.PropertyChanged += new PropertyChangedEventHandler(this.Contact_PropertyChanged);
    }

    private void InternalReload()
    {
      IEnumerable<ContactsListContactItem> contactsListContactItems = IMO.ContactsManager.GetAllBuddiesList().Where<Contact>((Func<Contact, bool>) (x => !x.HideInContactsList)).Select<Contact, ContactsListContactItem>((Func<Contact, ContactsListContactItem>) (x => !x.IsGroup ? new ContactsListContactItem(x) : (ContactsListContactItem) new ContactsListGroupContactItem(x)));
      Dictionary<string, ContactsListContactItem> dictionary1 = new Dictionary<string, ContactsListContactItem>();
      Dictionary<string, ContactsListContactItem> dictionary2 = new Dictionary<string, ContactsListContactItem>();
      foreach (ContactsListItemBase contactItem in this.contactItems)
      {
        if (contactItem is ContactsListContactItem)
          dictionary1.Add(((ContactsListContactItem) contactItem).Contact.Buid, (ContactsListContactItem) contactItem);
      }
      foreach (ContactsListContactItem contactsListContactItem in contactsListContactItems)
        dictionary2.Add(contactsListContactItem.Contact.Buid, contactsListContactItem);
      foreach (KeyValuePair<string, ContactsListContactItem> keyValuePair in dictionary1)
      {
        if (!dictionary2.ContainsKey(keyValuePair.Key))
          this.RemoveBuddy(keyValuePair.Value.Contact);
      }
      foreach (KeyValuePair<string, ContactsListContactItem> keyValuePair in dictionary2)
      {
        if (!dictionary1.ContainsKey(keyValuePair.Key))
          this.AddBuddy(keyValuePair.Value.Contact);
      }
    }

    protected override void OnLoaded(object sender, RoutedEventArgs e)
    {
      this.InternalReload();
      IMO.ContactsManager.BuddyAdded += new EventHandler<EventArg<Contact>>(this.BuddyAddedHandler);
      IMO.ContactsManager.BuddyRemoved += new EventHandler<EventArg<Contact>>(this.BuddyRemovedHandler);
      IMO.ContactsManager.ContactsCleared += new EventHandler<EventArgs>(this.ContactsManager_ContactsCleared);
      IMO.ContactsManager.ContactsReset += new EventHandler<EventArgs>(this.ContactsManager_ContactsReset);
    }

    protected override void OnUnloaded(object sender, RoutedEventArgs e)
    {
      IMO.ContactsManager.BuddyAdded -= new EventHandler<EventArg<Contact>>(this.BuddyAddedHandler);
      IMO.ContactsManager.BuddyRemoved -= new EventHandler<EventArg<Contact>>(this.BuddyRemovedHandler);
      IMO.ContactsManager.ContactsCleared -= new EventHandler<EventArgs>(this.ContactsManager_ContactsCleared);
      IMO.ContactsManager.ContactsReset -= new EventHandler<EventArgs>(this.ContactsManager_ContactsReset);
    }

    private void ContactsManager_ContactsReset(object sender, EventArgs e) => this.InternalReset();

    private void ContactsManager_ContactsCleared(object sender, EventArgs e)
    {
      this.ClearContacts();
    }

    private void ClearContacts()
    {
      foreach (ContactsListItemBase contactItem in this.contactItems)
      {
        if (contactItem is ContactsListContactItem)
          (contactItem as ContactsListContactItem).Contact.PropertyChanged -= new PropertyChangedEventHandler(this.Contact_PropertyChanged);
      }
      this.contactItems.Clear();
    }

    private void Contact_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      Contact contact = (Contact) sender;
      if (!(e.PropertyName == "HideInContactsList") || !contact.HideInContactsList)
        return;
      this.RemoveBuddy(contact);
    }

    private void BuddyAddedHandler(object sender, EventArg<Contact> e) => this.AddBuddy(e.Arg);

    private void BuddyRemovedHandler(object sender, EventArg<Contact> e) => this.RemoveBuddy(e.Arg);

    private void AddBuddy(Contact contact)
    {
      ContactsListContactItem contactsListContactItem = contact.IsGroup ? (ContactsListContactItem) new ContactsListGroupContactItem(contact) : new ContactsListContactItem(contact);
      if (this.contactItems.Contains((ContactsListItemBase) contactsListContactItem) || contact.HideInContactsList)
        return;
      this.contactItems.Add((ContactsListItemBase) contactsListContactItem);
      contact.PropertyChanged += new PropertyChangedEventHandler(this.Contact_PropertyChanged);
    }

    private void RemoveBuddy(Contact contact)
    {
      ContactsListItemBase contactsListItemBase = this.contactItems.Where<ContactsListItemBase>((Func<ContactsListItemBase, bool>) (x => x is ContactsListContactItem && ((ContactsListContactItem) x).Contact == contact)).FirstOrDefault<ContactsListItemBase>();
      if (contactsListItemBase == null || !this.contactItems.Contains(contactsListItemBase))
        return;
      this.contactItems.Remove(contactsListItemBase);
      contact.PropertyChanged -= new PropertyChangedEventHandler(this.Contact_PropertyChanged);
    }

    public ContactsListInviteItem InviteItem => this.contactItems.First as ContactsListInviteItem;

    public ContactsListCreateGroupItem CreateGroupItem
    {
      get
      {
        return this.contactItems.Skip<ContactsListItemBase>(1).First<ContactsListItemBase>() as ContactsListCreateGroupItem;
      }
    }

    public SortedObservableCollection<ContactsListItemBase> ContactItems => this.contactItems;

    private class ContactsListItemComparer : Comparer<ContactsListItemBase>
    {
      public override int Compare(ContactsListItemBase x, ContactsListItemBase y)
      {
        if (x.GetUIPriority() != y.GetUIPriority())
          return x.GetUIPriority().CompareTo(y.GetUIPriority());
        if (!(x is ContactsListContactItem) || !(y is ContactsListContactItem))
          return 0;
        Contact contact1 = (x as ContactsListContactItem).Contact;
        Contact contact2 = (y as ContactsListContactItem).Contact;
        if (contact1.IsFavorite && !contact2.IsFavorite)
          return -1;
        if (!contact1.IsFavorite && contact2.IsFavorite)
          return 1;
        if (contact1.IsGroup && !contact2.IsGroup)
          return -1;
        return !contact1.IsGroup && contact2.IsGroup ? 1 : contact1.Alias.CompareTo(contact2.Alias);
      }
    }
  }
}
