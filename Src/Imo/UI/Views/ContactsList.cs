// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.UI.Views.ContactsList
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using Imo.Phone.Controls;
using ImoSilverlightApp.Helpers;
using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace ImoSilverlightApp.UI.Views
{
  public class ContactsList : UserControl
  {
    private static int constructorCalledCount;
    private long initialLoadedTimestamp;
    private long loadedTimestamp;
    internal LongListSelector contactsList;
    private bool _contentLoaded;

    public ContactsList()
    {
      ++ContactsList.constructorCalledCount;
      this.InitializeComponent();
      this.DataContext = (object) (this.ViewModel = new ContactsListViewModel((FrameworkElement) this));
      this.Loaded += new RoutedEventHandler(this.LoadedHandler);
      this.Unloaded += new RoutedEventHandler(this.UnloadedHandler);
    }

    private ContactsListViewModel ViewModel { get; set; }

    private void LoadedHandler(object sender, RoutedEventArgs e)
    {
      if (ContactsList.constructorCalledCount == 1)
        this.initialLoadedTimestamp = Utils.GetTimestamp();
      else
        this.loadedTimestamp = Utils.GetTimestamp();
      this.ViewModel.ContactItems.CollectionChanged += new NotifyCollectionChangedEventHandler(this.Contacts_CollectionChanged);
    }

    private void UnloadedHandler(object sender, RoutedEventArgs e)
    {
      this.ViewModel.ContactItems.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.Contacts_CollectionChanged);
    }

    private void Contacts_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
    }

    private void Conversation_Tapped(object sender, GestureEventArgs e)
    {
      ContactsListContactItem viewModelOf = VisualUtils.GetViewModelOf<ContactsListContactItem>(sender);
      if (viewModelOf == null)
        return;
      IMO.NavigationManager.NavigateToConversation(viewModelOf.Contact.Buid, true);
      e.Handled = true;
    }

    private async void RemoveContact_Click(object sender, EventArgs e)
    {
      ContactsListContactItem viewModelOf = VisualUtils.GetViewModelOf<ContactsListContactItem>(sender);
      if (viewModelOf == null)
        return;
      int num = await IMO.ContactsManager.DeleteContact(viewModelOf.Contact, true) ? 1 : 0;
    }

    private void LeaveGroup_Click(object sender, EventArgs e)
    {
      VisualUtils.GetViewModelOf<ContactsListContactItem>(sender)?.Contact.LeaveGroup(true);
    }

    private void SendMessage_Click(object sender, EventArgs e)
    {
      ContactsListContactItem viewModelOf = VisualUtils.GetViewModelOf<ContactsListContactItem>(sender);
      if (viewModelOf == null)
        return;
      IMO.NavigationManager.NavigateToConversation(viewModelOf.Contact.Buid, true);
    }

    private void AudioCall_Click(object sender, EventArgs e)
    {
      ContactsListContactItem viewModelOf = VisualUtils.GetViewModelOf<ContactsListContactItem>(sender);
      if (viewModelOf == null)
        return;
      IMO.AVManager.StartAudioCall(viewModelOf.Contact.Buid, "contacts_list-" + sender.GetType().Name);
    }

    private void VideoCall_Click(object sender, GestureEventArgs e)
    {
      ContactsListContactItem viewModelOf = VisualUtils.GetViewModelOf<ContactsListContactItem>(sender);
      if (viewModelOf == null)
        return;
      IMO.AVManager.StartVideoCall(viewModelOf.Contact.Buid, "contacts_list-" + sender.GetType().Name);
    }

    private void AddToFavorites_Click(object sender, EventArgs e)
    {
      ContactsListContactItem viewModelOf = VisualUtils.GetViewModelOf<ContactsListContactItem>(sender);
      if (viewModelOf == null)
        return;
      IMO.ContactsManager.AddContactToFavorites(viewModelOf.Contact);
    }

    private void RemoveFromFavorites_Click(object sender, EventArgs e)
    {
      ContactsListContactItem viewModelOf = VisualUtils.GetViewModelOf<ContactsListContactItem>(sender);
      if (viewModelOf == null)
        return;
      IMO.ContactsManager.RemoveContactFromFavorites(viewModelOf.Contact);
    }

    private void MuteGroup_Click(object sender, EventArgs e)
    {
      VisualUtils.GetViewModelOf<ContactsListContactItem>(sender)?.Contact.Mute();
    }

    private void UnmuteGroup_Click(object sender, EventArgs e)
    {
      VisualUtils.GetViewModelOf<ContactsListContactItem>(sender)?.Contact.Unmute();
    }

    private void Conversation_Holding(object sender, GestureEventArgs e)
    {
    }

    private void CreateGroup_Tapped(object sender, GestureEventArgs e)
    {
      IMO.NavigationManager.NavigateToCreateGroup();
    }

    private void ViewProfile_Click(object sender, EventArgs e)
    {
      ContactsListContactItem contactItem = VisualUtils.GetViewModelOf<ContactsListContactItem>(sender);
      if (contactItem == null)
        return;
      Utils.DelayExecute(50, (Action) (() => IMO.NavigationManager.NavigateToProfile(contactItem.Contact.Buid)));
    }

    private void Invite_Tapped(object sender, GestureEventArgs e)
    {
      IMO.NavigationManager.NavigateToInvitePage();
    }

    private void Invite_Loaded(object sender, RoutedEventArgs e)
    {
      if (ContactsList.constructorCalledCount == 1)
      {
        if (this.initialLoadedTimestamp == 0L)
          return;
        IMO.MonitorLog.Log("speed_metrics", "initial_first_contacts_item_loaded", (object) (Utils.GetTimestamp() - this.initialLoadedTimestamp));
        this.initialLoadedTimestamp = 0L;
      }
      else
      {
        if (this.loadedTimestamp == 0L)
          return;
        IMO.MonitorLog.Log("speed_metrics", "first_contacts_item_loaded", (object) (Utils.GetTimestamp() - this.loadedTimestamp));
        this.loadedTimestamp = 0L;
      }
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/ImoSilverlightApp;component/UI/Views/ContactsList.xaml", UriKind.Relative));
      this.contactsList = (LongListSelector) this.FindName("contactsList");
    }
  }
}
