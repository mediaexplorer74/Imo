// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.UI.Views.ChatsListGroupCallItem
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Storage.Models;


namespace ImoSilverlightApp.UI.Views
{
  internal class ChatsListGroupCallItem : ChatsListItemBase
  {
    public Contact Contact { get; private set; }

    public ChatsListGroupCallItem(Contact contact) => this.Contact = contact;

    public override int GetUIPriority() => 1;
  }
}
