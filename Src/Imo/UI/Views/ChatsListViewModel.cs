// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.UI.Views.ChatsListViewModel
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Collections;
using ImoSilverlightApp.Storage.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;


namespace ImoSilverlightApp.UI.Views
{
  internal class ChatsListViewModel : ViewModelBase
  {
    private SortedObservableCollection<ChatsListItemBase> chatItems;

    public ChatsListViewModel(FrameworkElement el)
      : base(el)
    {
      this.chatItems = new SortedObservableCollection<ChatsListItemBase>((Comparer<ChatsListItemBase>) new ChatsListViewModel.ChatsListItemComparer());
      this.InternalReset();
    }

    private void AVManager_GroupCallsListChanged(object sender, EventArgs e)
    {
      foreach (ChatsListItemBase chatsListItemBase in this.chatItems.Where<ChatsListItemBase>((Func<ChatsListItemBase, bool>) (x => x is ChatsListGroupCallItem)).ToList<ChatsListItemBase>())
        this.chatItems.Remove(chatsListItemBase);
      foreach (Contact currentGroupCall in IMO.AVManager.CurrentGroupCalls)
        this.chatItems.Add((ChatsListItemBase) new ChatsListGroupCallItem(currentGroupCall));
    }

    private void InternalReset()
    {
      IEnumerable<Conversation> source = IMO.ConversationsManager.GetAllConversationsList().Where<Conversation>((Func<Conversation, bool>) (x => x.ShowInChatsList));
      this.chatItems.SetItems(source.Select<Conversation, ChatsListChatItem>((Func<Conversation, ChatsListChatItem>) (x => !x.IsGroup ? new ChatsListChatItem(x) : (ChatsListChatItem) new ChatsListGroupChatItem(x))).Cast<ChatsListItemBase>());
      this.chatItems.Add((ChatsListItemBase) new ChatsListInviteItem());
      foreach (Contact currentGroupCall in IMO.AVManager.CurrentGroupCalls)
        this.chatItems.Add((ChatsListItemBase) new ChatsListGroupCallItem(currentGroupCall));
      foreach (ModelBase modelBase in source)
        modelBase.PropertyChanged += new PropertyChangedEventHandler(this.Conversation_PropertyChanged);
    }

    private void InternalReload()
    {
      IEnumerable<Conversation> conversations = IMO.ConversationsManager.GetAllConversationsList().Where<Conversation>((Func<Conversation, bool>) (x => x.ShowInChatsList));
      IEnumerable<Contact> currentGroupCalls = IMO.AVManager.CurrentGroupCalls;
      Dictionary<string, ChatsListChatItem> dictionary1 = new Dictionary<string, ChatsListChatItem>();
      Dictionary<string, Conversation> dictionary2 = new Dictionary<string, Conversation>();
      Dictionary<string, ChatsListGroupCallItem> dictionary3 = new Dictionary<string, ChatsListGroupCallItem>();
      Dictionary<string, Contact> dictionary4 = new Dictionary<string, Contact>();
      foreach (ChatsListItemBase chatItem in this.chatItems)
      {
        if (chatItem is ChatsListChatItem)
          dictionary1.Add(((ChatsListChatItem) chatItem).Conversation.Buid, (ChatsListChatItem) chatItem);
        else if (chatItem is ChatsListGroupCallItem)
          dictionary3.Add(((ChatsListGroupCallItem) chatItem).Contact.Buid, (ChatsListGroupCallItem) chatItem);
      }
      foreach (Conversation conversation in conversations)
        dictionary2.Add(conversation.Buid, conversation);
      foreach (Contact contact in currentGroupCalls)
        dictionary4.Add(contact.Buid, contact);
      foreach (KeyValuePair<string, ChatsListChatItem> keyValuePair in dictionary1)
      {
        if (!dictionary2.ContainsKey(keyValuePair.Key))
          this.RemoveConversation(keyValuePair.Value.Conversation);
      }
      foreach (KeyValuePair<string, ChatsListGroupCallItem> keyValuePair in dictionary3)
      {
        if (!dictionary4.ContainsKey(keyValuePair.Key))
          this.chatItems.Remove((ChatsListItemBase) keyValuePair.Value);
      }
      foreach (KeyValuePair<string, Conversation> keyValuePair in dictionary2)
      {
        if (!dictionary1.ContainsKey(keyValuePair.Key))
          this.AddConversation(keyValuePair.Value);
      }
      foreach (KeyValuePair<string, Contact> keyValuePair in dictionary4)
      {
        if (!dictionary3.ContainsKey(keyValuePair.Key))
          this.chatItems.Add((ChatsListItemBase) new ChatsListGroupCallItem(keyValuePair.Value));
      }
    }

    protected override void OnLoaded(object sender, RoutedEventArgs e)
    {
      this.InternalReload();
      IMO.ConversationsManager.ConversationActivated += new EventHandler<EventArg<Conversation>>(this.ConversationActivatedHandler);
      IMO.ConversationsManager.ConversationRemoved += new EventHandler<EventArg<Conversation>>(this.ConversationsManager_ConversationRemoved);
      IMO.ConversationsManager.ConversationsReset += new EventHandler<EventArgs>(this.ConversationsManager_ConversationsReset);
      IMO.AVManager.GroupCallsListChanged += new EventHandler(this.AVManager_GroupCallsListChanged);
    }

    protected override void OnUnloaded(object sender, RoutedEventArgs e)
    {
      IMO.ConversationsManager.ConversationActivated -= new EventHandler<EventArg<Conversation>>(this.ConversationActivatedHandler);
      IMO.ConversationsManager.ConversationRemoved -= new EventHandler<EventArg<Conversation>>(this.ConversationsManager_ConversationRemoved);
      IMO.ConversationsManager.ConversationsReset -= new EventHandler<EventArgs>(this.ConversationsManager_ConversationsReset);
      IMO.AVManager.GroupCallsListChanged -= new EventHandler(this.AVManager_GroupCallsListChanged);
    }

    private void ConversationsManager_ConversationsReset(object sender, EventArgs e)
    {
      this.ClearChatItems();
      this.InternalReset();
    }

    private void ClearChatItems()
    {
      foreach (ChatsListItemBase chatsListItemBase in this.chatItems.ToList<ChatsListItemBase>())
      {
        if (chatsListItemBase is ChatsListChatItem)
        {
          ChatsListChatItem chatsListChatItem = chatsListItemBase as ChatsListChatItem;
          chatsListChatItem.Conversation.PropertyChanged -= new PropertyChangedEventHandler(this.Conversation_PropertyChanged);
          chatsListChatItem.Dispose();
        }
      }
      this.chatItems.Clear();
    }

    private void ConversationsManager_ConversationRemoved(object sender, EventArg<Conversation> e)
    {
      this.RemoveConversation(e.Arg);
    }

    private void Conversation_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      Conversation conversation = (Conversation) sender;
      if (!(e.PropertyName == "ShowInChatsList") || conversation.ShowInChatsList)
        return;
      this.RemoveConversation(conversation);
    }

    public SortedObservableCollection<ChatsListItemBase> ChatItems => this.chatItems;

    private void ConversationActivatedHandler(object sender, EventArg<Conversation> e)
    {
      this.AddConversation(e.Arg);
    }

    private void AddConversation(Conversation conversation)
    {
      ChatsListChatItem chatsListChatItem = conversation.IsGroup ? (ChatsListChatItem) new ChatsListGroupChatItem(conversation) : new ChatsListChatItem(conversation);
      if (this.chatItems.Contains((ChatsListItemBase) chatsListChatItem))
        return;
      this.chatItems.Add((ChatsListItemBase) chatsListChatItem);
      conversation.PropertyChanged += new PropertyChangedEventHandler(this.Conversation_PropertyChanged);
    }

    private void RemoveConversation(Conversation conversation)
    {
      ChatsListItemBase chatsListItemBase = this.chatItems.Where<ChatsListItemBase>((Func<ChatsListItemBase, bool>) (x => x is ChatsListChatItem && ((ChatsListChatItem) x).Conversation == conversation)).FirstOrDefault<ChatsListItemBase>();
      conversation.PropertyChanged -= new PropertyChangedEventHandler(this.Conversation_PropertyChanged);
      if (chatsListItemBase == null)
        return;
      this.chatItems.Remove(chatsListItemBase);
      chatsListItemBase.Dispose();
    }

    private class ChatsListItemComparer : Comparer<ChatsListItemBase>
    {
      public override int Compare(ChatsListItemBase x, ChatsListItemBase y)
      {
        if (x.GetUIPriority() != y.GetUIPriority())
          return x.GetUIPriority().CompareTo(y.GetUIPriority());
        return x is ChatsListChatItem && y is ChatsListChatItem ? -(x as ChatsListChatItem).Conversation.LastActivityTimestamp.CompareTo((y as ChatsListChatItem).Conversation.LastActivityTimestamp) : 0;
      }
    }
  }
}
