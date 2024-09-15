// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.UI.Views.ChatLogViewModel
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Storage.Models;
using System;
using System.ComponentModel;
using System.Windows;


namespace ImoSilverlightApp.UI.Views
{
  public class ChatLogViewModel : ViewModelBase
  {
    private Conversation conversation;
    private ChatLog chatLog;

    public ChatLogViewModel(string buid, ChatLog chatLog)
      : base((FrameworkElement) chatLog)
    {
      this.chatLog = chatLog;
      this.conversation = IMO.ConversationsManager.GetOrCreateConversation(buid);
    }

    protected override void OnLoaded(object sender, RoutedEventArgs e)
    {
      this.Conversation.MessageAdded += new EventHandler<EventArg<Message>>(this.Conversation_MessageAdded);
      this.Conversation.PropertyChanged += new PropertyChangedEventHandler(this.Conversation_PropertyChanged);
    }

    protected override void OnUnloaded(object sender, RoutedEventArgs e)
    {
      this.Conversation.MessageAdded -= new EventHandler<EventArg<Message>>(this.Conversation_MessageAdded);
      this.Conversation.PropertyChanged -= new PropertyChangedEventHandler(this.Conversation_PropertyChanged);
    }

    private void Conversation_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
    }

    public Conversation Conversation => this.conversation;

    private void Conversation_MessageAdded(object sender, EventArg<Message> e)
    {
      if (!this.Conversation.IsUserWatchingIt() || !this.chatLog.IsAnchoredBottom)
        return;
      this.Conversation.MarkMessagesAsRead();
    }
  }
}
