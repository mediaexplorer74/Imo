// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.UI.Views.ChatsListChatItem
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Helpers;
using ImoSilverlightApp.Storage.Models;
using System;
using System.ComponentModel;


namespace ImoSilverlightApp.UI.Views
{
  internal class ChatsListChatItem : ChatsListItemBase
  {
    private Conversation conversation;

    public ChatsListChatItem(Conversation conversation)
    {
      this.conversation = conversation;
      this.conversation.PropertyChanged += new PropertyChangedEventHandler(this.Conversation_PropertyChanged);
    }

    private void Conversation_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      this.OnPropertyChanged(string.Empty);
    }

    public Conversation Conversation => this.conversation;

    public string LastMessageTimestamp
    {
      get
      {
        return this.conversation.LastMessageTimestamp == -1L ? "" : Utils.ToFormattedTimestamp(this.conversation.LastMessageTimestamp);
      }
    }

    public int UnreadMessagesCountUI => Math.Max(1, this.Conversation.UnreadMessagesCount);

    public override int GetHashCode() => this.conversation.GetHashCode();

    public override bool Equals(object obj)
    {
      return obj is ChatsListChatItem && this.conversation.Equals((object) (obj as ChatsListChatItem).Conversation);
    }

    public override void Dispose()
    {
      this.conversation.PropertyChanged -= new PropertyChangedEventHandler(this.Conversation_PropertyChanged);
    }

    public override int GetUIPriority() => 2;
  }
}
