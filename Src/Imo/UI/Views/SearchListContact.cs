// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.UI.Views.SearchListContact
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Storage.Models;


namespace ImoSilverlightApp.UI.Views
{
  internal class SearchListContact : SearchListItemBase
  {
    private Contact contact;

    public SearchListContact(Contact contact) => this.contact = contact;

    public Contact Contact => this.contact;
  }
}
