// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.UI.Views.GroupProfilePageViewModel
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Storage.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;


namespace ImoSilverlightApp.UI.Views
{
  internal class GroupProfilePageViewModel : ViewModelBase
  {
    private Contact contact;
    private ObservableCollection<Contact> groupMembers;

    public GroupProfilePageViewModel(string buid, FrameworkElement element)
      : base(element)
    {
      this.contact = IMO.ContactsManager.GetOrCreateContact(buid);
      this.GroupMembers = new ObservableCollection<Contact>((IEnumerable<Contact>) this.contact.GetAllGroupMembersList());
      this.contact.GroupMemberAdded += new EventHandler<EventArg<Contact>>(this.Contact_GroupMemberAdded);
      this.contact.GroupMemberRemoved += new EventHandler<EventArg<Contact>>(this.Contact_GroupMemberRemoved);
      this.contact.SyncGroupMembers();
    }

    public override void Dispose()
    {
      base.Dispose();
      this.contact.GroupMemberAdded -= new EventHandler<EventArg<Contact>>(this.Contact_GroupMemberAdded);
      this.contact.GroupMemberRemoved -= new EventHandler<EventArg<Contact>>(this.Contact_GroupMemberRemoved);
    }

    private void Contact_GroupMemberAdded(object sender, EventArg<Contact> e)
    {
      this.groupMembers.Add(e.Arg);
    }

    private void Contact_GroupMemberRemoved(object sender, EventArg<Contact> e)
    {
      this.groupMembers.Remove(e.Arg);
    }

    public Contact Contact
    {
      get => this.contact;
      set
      {
        this.contact = value;
        this.OnPropertyChanged(nameof (Contact));
      }
    }

    public ObservableCollection<Contact> GroupMembers
    {
      get => this.groupMembers;
      set
      {
        if (this.groupMembers == value)
          return;
        this.groupMembers = value;
        this.OnPropertyChanged(nameof (GroupMembers));
      }
    }
  }
}
