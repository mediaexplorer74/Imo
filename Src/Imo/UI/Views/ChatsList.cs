// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.UI.Views.ChatsList
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using Imo.Phone.Controls;
using ImoSilverlightApp.Helpers;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace ImoSilverlightApp.UI.Views
{
  public class ChatsList : UserControl
  {
    private static int constructorCalledCount;
    private long initialLoadedTimestamp;
    private long loadedTimestamp;
    internal LongListSelector chatsList;
    private bool _contentLoaded;

    public ChatsList()
    {
      ++ChatsList.constructorCalledCount;
      this.InitializeComponent();
      this.DataContext = (object) (this.ViewModel = new ChatsListViewModel((FrameworkElement) this));
      this.Loaded += new RoutedEventHandler(this.LoadedHandler);
      this.Unloaded += new RoutedEventHandler(this.UnloadedHandler);
    }

    private ChatsListViewModel ViewModel { get; set; }

    private void LoadedHandler(object sender, RoutedEventArgs e)
    {
      if (ChatsList.constructorCalledCount == 1)
        this.initialLoadedTimestamp = Utils.GetTimestamp();
      else
        this.loadedTimestamp = Utils.GetTimestamp();
    }

    private void UnloadedHandler(object sender, RoutedEventArgs e)
    {
    }

    private void Conversation_Tapped(object sender, GestureEventArgs e)
    {
      ChatsListChatItem viewModelOf = VisualUtils.GetViewModelOf<ChatsListChatItem>(sender);
      if (viewModelOf == null)
        return;
      IMO.NavigationManager.NavigateToConversation(viewModelOf.Conversation.Buid, true);
      e.Handled = true;
    }

    private async void RemoveContact_Click(object sender, EventArgs e)
    {
      ChatsListChatItem viewModelOf = VisualUtils.GetViewModelOf<ChatsListChatItem>(sender);
      if (viewModelOf == null)
        return;
      int num = await IMO.ContactsManager.DeleteContact(viewModelOf.Conversation.Contact, true) ? 1 : 0;
    }

    private void LeaveGroup_Click(object sender, EventArgs e)
    {
      VisualUtils.GetViewModelOf<ChatsListChatItem>(sender)?.Conversation.Contact.LeaveGroup(true);
    }

    private void HideConversation_Click(object sender, EventArgs e)
    {
      VisualUtils.GetViewModelOf<ChatsListChatItem>(sender)?.Conversation.HideFromChatList();
    }

    private void SendMessage_Click(object sender, EventArgs e)
    {
      ChatsListChatItem viewModelOf = VisualUtils.GetViewModelOf<ChatsListChatItem>(sender);
      if (viewModelOf == null)
        return;
      IMO.NavigationManager.NavigateToConversation(viewModelOf.Conversation.Buid, true);
    }

    private void AudioCall_Click(object sender, EventArgs e)
    {
      ChatsListChatItem viewModelOf = VisualUtils.GetViewModelOf<ChatsListChatItem>(sender);
      if (viewModelOf == null)
        return;
      IMO.AVManager.StartAudioCall(viewModelOf.Conversation.Buid, "chats_list-" + sender.GetType().Name);
    }

    private void VideoCall_Click(object sender, EventArgs e)
    {
      ChatsListChatItem viewModelOf = VisualUtils.GetViewModelOf<ChatsListChatItem>(sender);
      if (viewModelOf == null)
        return;
      IMO.AVManager.StartVideoCall(viewModelOf.Conversation.Buid, "chats_list-" + sender.GetType().Name);
    }

    private void AddToFavorites_Click(object sender, EventArgs e)
    {
      ChatsListChatItem viewModelOf = VisualUtils.GetViewModelOf<ChatsListChatItem>(sender);
      if (viewModelOf == null)
        return;
      IMO.ContactsManager.AddContactToFavorites(viewModelOf.Conversation.Contact);
    }

    private void RemoveFromFavorites_Click(object sender, EventArgs e)
    {
      ChatsListChatItem viewModelOf = VisualUtils.GetViewModelOf<ChatsListChatItem>(sender);
      if (viewModelOf == null)
        return;
      IMO.ContactsManager.RemoveContactFromFavorites(viewModelOf.Conversation.Contact);
    }

    private void AddContact_Click(object sender, EventArgs e)
    {
      ChatsListChatItem viewModelOf = VisualUtils.GetViewModelOf<ChatsListChatItem>(sender);
      if (viewModelOf == null)
        return;
      IMO.ContactsManager.AddImoContact(viewModelOf.Conversation.Buid, "direct");
    }

    private void BlockContact_Click(object sender, EventArgs e)
    {
      ChatsListChatItem viewModelOf = VisualUtils.GetViewModelOf<ChatsListChatItem>(sender);
      if (viewModelOf == null)
        return;
      viewModelOf.Conversation.Contact.Block();
      viewModelOf.Conversation.HideFromChatList();
      IMO.NavigationManager.NavigateToBlockedContacts();
    }

    private void MuteGroup_Click(object sender, EventArgs e)
    {
      VisualUtils.GetViewModelOf<ChatsListChatItem>(sender)?.Conversation.Contact.Mute();
    }

    private void UnmuteGroup_Click(object sender, EventArgs e)
    {
      VisualUtils.GetViewModelOf<ChatsListChatItem>(sender)?.Conversation.Contact.Unmute();
    }

    private void Invite_Tapped(object sender, GestureEventArgs e)
    {
      IMO.NavigationManager.NavigateToInvitePage();
    }

    private void Profile_Click(object sender, RoutedEventArgs e)
    {
      ChatsListChatItem viewModelOf = VisualUtils.GetViewModelOf<ChatsListChatItem>(sender);
      if (viewModelOf == null)
        return;
      IMO.NavigationManager.NavigateToProfile(viewModelOf.Conversation.Buid);
    }

    private async void DeleteConversation_Click(object sender, RoutedEventArgs e)
    {
      ChatsListChatItem chatItem = VisualUtils.GetViewModelOf<ChatsListChatItem>(sender);
      if (chatItem == null)
        return;
      if (await ImoMessageBox.Show("Delete chat with " + chatItem.Conversation.Alias + "?", ImoMessageBoxButton.YesNo) != ImoMessageBoxResult.Yes)
        return;
      IMO.IM.DeleteChatHistory(chatItem.Conversation.Buid, (Action<JToken>) (jObj =>
      {
        chatItem.Conversation.ClearMessages();
        chatItem.Conversation.HideFromChatList();
      }));
    }

    private void Invite_Loaded(object sender, RoutedEventArgs e)
    {
      if (ChatsList.constructorCalledCount == 1)
      {
        if (this.initialLoadedTimestamp == 0L)
          return;
        IMO.MonitorLog.Log("speed_metrics", "initial_first_chats_item_loaded", (object) (Utils.GetTimestamp() - this.initialLoadedTimestamp));
        this.initialLoadedTimestamp = 0L;
      }
      else
      {
        if (this.loadedTimestamp == 0L)
          return;
        IMO.MonitorLog.Log("speed_metrics", "first_chats_item_loaded", (object) (Utils.GetTimestamp() - this.loadedTimestamp));
        this.loadedTimestamp = 0L;
      }
    }

    private void JoinGroupCall_Tapped(object sender, GestureEventArgs e)
    {
      ChatsListGroupCallItem viewModelOf = VisualUtils.GetViewModelOf<ChatsListGroupCallItem>(sender);
      if (viewModelOf == null)
        return;
      IMO.AVManager.InitiateGroupCall(viewModelOf.Contact.Buid, "chats_list-" + sender.GetType().Name);
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/ImoSilverlightApp;component/UI/Views/ChatsList.xaml", UriKind.Relative));
      this.chatsList = (LongListSelector) this.FindName("chatsList");
    }
  }
}
