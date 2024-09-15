// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.ChatHeadsViewModel
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Storage.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;


namespace ImoSilverlightApp
{
  public class ChatHeadsViewModel : ViewModelBase
  {
    private ObservableCollection<ChatHeadViewModel> chatHeads;
    private ObservableCollection<TyperViewModel> typers;
    private Conversation conversation;
    private IList<Contact> seenAllContacts;
    private IList<Contact> inChatContacts;

    public ChatHeadsViewModel(string buid, FrameworkElement element)
      : base(element)
    {
      this.chatHeads = new ObservableCollection<ChatHeadViewModel>();
      this.typers = new ObservableCollection<TyperViewModel>();
      this.conversation = IMO.ConversationsManager.GetOrCreateConversation(buid);
    }

    private ChatHeadViewModel GetChatHead(Contact contact)
    {
      return this.chatHeads.Where<ChatHeadViewModel>((Func<ChatHeadViewModel, bool>) (x => x.Contact == contact)).FirstOrDefault<ChatHeadViewModel>();
    }

    private TyperViewModel GetTyper(Contact contact)
    {
      return this.typers.Where<TyperViewModel>((Func<TyperViewModel, bool>) (x => x.Contact == contact)).FirstOrDefault<TyperViewModel>();
    }

    protected override void OnLoaded(object sender, RoutedEventArgs e)
    {
      this.seenAllContacts = this.conversation.GetSeenAllContacts();
      this.inChatContacts = this.conversation.GetInChatContacts();
      this.ChatHeads = new ObservableCollection<ChatHeadViewModel>(this.inChatContacts.Select<Contact, ChatHeadViewModel>((Func<Contact, ChatHeadViewModel>) (x => new ChatHeadViewModel(x, true))));
      foreach (Contact seenAllContact in (IEnumerable<Contact>) this.seenAllContacts)
      {
        if (this.GetChatHead(seenAllContact) == null)
          this.chatHeads.Add(new ChatHeadViewModel(seenAllContact, false));
      }
      this.conversation.InChatContactAdded += new EventHandler<EventArg<Contact>>(this.Conversation_InChatContactAdded);
      this.conversation.InChatContactRemoved += new EventHandler<EventArg<Contact>>(this.Conversation_InChatContactRemoved);
      this.conversation.SeenAllContactAdded += new EventHandler<EventArg<Contact>>(this.Conversation_SeenAllContactAdded);
      this.conversation.SeenAllContactRemoved += new EventHandler<EventArg<Contact>>(this.Conversation_SeenAllContactRemoved);
      this.typers.Clear();
      foreach (KeyValuePair<Contact, string> typer in (IEnumerable<KeyValuePair<Contact, string>>) this.conversation.GetTypers())
        this.AddTyper(typer.Key, typer.Value);
      this.conversation.TyperAdded += new EventHandler<EventArg<KeyValuePair<Contact, string>>>(this.Conversation_TyperAdded);
      this.conversation.TyperRemoved += new EventHandler<EventArg<Contact>>(this.Conversation_TyperRemoved);
    }

    protected override void OnUnloaded(object sender, RoutedEventArgs e)
    {
      this.conversation.InChatContactAdded -= new EventHandler<EventArg<Contact>>(this.Conversation_InChatContactAdded);
      this.conversation.InChatContactRemoved -= new EventHandler<EventArg<Contact>>(this.Conversation_InChatContactRemoved);
      this.conversation.SeenAllContactAdded -= new EventHandler<EventArg<Contact>>(this.Conversation_SeenAllContactAdded);
      this.conversation.SeenAllContactRemoved -= new EventHandler<EventArg<Contact>>(this.Conversation_SeenAllContactRemoved);
      this.conversation.TyperAdded -= new EventHandler<EventArg<KeyValuePair<Contact, string>>>(this.Conversation_TyperAdded);
      this.conversation.TyperRemoved -= new EventHandler<EventArg<Contact>>(this.Conversation_TyperRemoved);
    }

    private void AddTyper(Contact contact, string message)
    {
      TyperViewModel typer = this.GetTyper(contact);
      if (typer != null)
        typer.Message = message;
      else
        this.typers.Add(new TyperViewModel(contact, message));
      ChatHeadViewModel chatHead = this.GetChatHead(contact);
      if (chatHead == null)
        return;
      this.chatHeads.Remove(chatHead);
    }

    private void Conversation_TyperAdded(object sender, EventArg<KeyValuePair<Contact, string>> e)
    {
      KeyValuePair<Contact, string> keyValuePair = e.Arg;
      Contact key = keyValuePair.Key;
      keyValuePair = e.Arg;
      string message = keyValuePair.Value;
      this.AddTyper(key, message);
    }

    private void Conversation_TyperRemoved(object sender, EventArg<Contact> e)
    {
      Contact contact = e.Arg;
      TyperViewModel typer = this.GetTyper(contact);
      if (typer == null)
        return;
      this.typers.Remove(typer);
      ChatHeadViewModel chatHeadViewModel = new ChatHeadViewModel(contact, false);
      if (this.inChatContacts.Contains(contact))
        chatHeadViewModel.IsInChat = true;
      if (!this.seenAllContacts.Contains(contact) && !chatHeadViewModel.IsInChat)
        return;
      this.chatHeads.Add(chatHeadViewModel);
    }

    public ObservableCollection<ChatHeadViewModel> ChatHeads
    {
      get => this.chatHeads;
      set
      {
        if (this.chatHeads == value)
          return;
        this.chatHeads = value;
        this.OnPropertyChanged(nameof (ChatHeads));
      }
    }

    public ObservableCollection<TyperViewModel> Typers
    {
      get => this.typers;
      set
      {
        if (this.typers == value)
          return;
        this.typers = value;
        this.OnPropertyChanged(nameof (Typers));
      }
    }

    private void Conversation_SeenAllContactAdded(object sender, EventArg<Contact> e)
    {
      Contact contact = e.Arg;
      if (!this.seenAllContacts.Contains(contact))
        this.seenAllContacts.Add(contact);
      ChatHeadViewModel chatHead = this.GetChatHead(contact);
      TyperViewModel typer = this.GetTyper(contact);
      if (chatHead != null || typer != null)
        return;
      this.chatHeads.Add(new ChatHeadViewModel(contact, false));
    }

    private void Conversation_SeenAllContactRemoved(object sender, EventArg<Contact> e)
    {
      Contact contact = e.Arg;
      if (this.seenAllContacts.Contains(contact))
        this.seenAllContacts.Remove(contact);
      if (this.inChatContacts.Contains(contact))
        return;
      ChatHeadViewModel chatHead = this.GetChatHead(contact);
      if (chatHead == null)
        return;
      this.chatHeads.Remove(chatHead);
    }

    private void Conversation_InChatContactAdded(object sender, EventArg<Contact> e)
    {
      Contact contact = e.Arg;
      if (!this.inChatContacts.Contains(contact))
        this.inChatContacts.Add(contact);
      ChatHeadViewModel chatHead = this.GetChatHead(contact);
      if (this.GetTyper(contact) != null)
        return;
      if (chatHead == null)
        this.chatHeads.Add(new ChatHeadViewModel(e.Arg, true));
      else
        chatHead.IsInChat = true;
    }

    private void Conversation_InChatContactRemoved(object sender, EventArg<Contact> e)
    {
      Contact contact = e.Arg;
      if (this.inChatContacts.Contains(contact))
        this.inChatContacts.Remove(contact);
      ChatHeadViewModel chatHead = this.GetChatHead(contact);
      if (chatHead == null)
        return;
      chatHead.IsInChat = false;
      if (this.seenAllContacts.Contains(contact))
        return;
      this.chatHeads.Remove(chatHead);
    }
  }
}
