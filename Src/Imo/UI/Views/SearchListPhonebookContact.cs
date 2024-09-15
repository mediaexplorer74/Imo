// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.UI.Views.SearchListPhonebookContact
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll


namespace ImoSilverlightApp.UI.Views
{
  internal class SearchListPhonebookContact : SearchListItemBase
  {
    public string Name { get; private set; }

    public string PhoneNumber { get; private set; }

    public SearchListPhonebookContact(string name, string phoneNumber)
    {
      this.Name = name;
      this.PhoneNumber = phoneNumber;
    }
  }
}
