// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.ChatHeadViewModel
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Storage.Models;


namespace ImoSilverlightApp
{
  public class ChatHeadViewModel : ModelBase
  {
    private bool isInChat;
    private Contact contact;

    public ChatHeadViewModel(Contact contact, bool isInChat)
    {
      this.contact = contact;
      this.isInChat = isInChat;
    }

    public bool IsInChat
    {
      get => this.isInChat;
      set
      {
        if (this.isInChat == value)
          return;
        this.isInChat = value;
        this.OnPropertyChanged(nameof (IsInChat));
      }
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
  }
}
