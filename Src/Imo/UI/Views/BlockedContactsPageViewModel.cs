// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.UI.Views.BlockedContactsPageViewModel
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Collections;
using ImoSilverlightApp.Storage.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Windows;


namespace ImoSilverlightApp.UI.Views
{
  internal class BlockedContactsPageViewModel : ViewModelBase
  {
    private SortedObservableCollection<Contact> blockedContacts;
    private bool showBusyIndicator;
    private bool hasNoBlockedContacts;

    public BlockedContactsPageViewModel(FrameworkElement element)
      : base(element)
    {
      this.blockedContacts = new SortedObservableCollection<Contact>((Comparer<Contact>) new BlockedContactsPageViewModel.ContactComparer());
    }

    protected override void OnLoaded(object sender, RoutedEventArgs e)
    {
      this.blockedContacts.Clear();
      this.ShowBusyIndicator = true;
      IMO.IM.GetBlockedContacts((Action<JToken>) (result =>
      {
        this.ShowBusyIndicator = false;
        foreach (JToken jtoken in (IEnumerable<JToken>) result)
        {
          JObject jObj = jtoken.ToObject<JObject>();
          Contact contact = IMO.ContactsManager.GetOrCreateContact(jObj.Value<string>((object) "buid"));
          contact.UpdateFromJObject((JToken) jObj);
          contact.IsBlocked = true;
          this.blockedContacts.Add(contact);
        }
        if (!this.blockedContacts.IsEmpty)
          return;
        this.HasNoBlockedContacts = true;
      }));
    }

    public SortedObservableCollection<Contact> BlockedContacts => this.blockedContacts;

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

    public bool HasNoBlockedContacts
    {
      get => this.hasNoBlockedContacts;
      set
      {
        if (this.hasNoBlockedContacts == value)
          return;
        this.hasNoBlockedContacts = value;
        this.OnPropertyChanged(nameof (HasNoBlockedContacts));
      }
    }

    private class ContactComparer : Comparer<Contact>
    {
      public override int Compare(Contact x, Contact y) => x.Alias.CompareTo(y.Alias);
    }
  }
}
