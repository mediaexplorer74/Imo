// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.UI.Views.AddToContactsItem
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll


namespace ImoSilverlightApp.UI.Views
{
  internal class AddToContactsItem : SearchListItemBase
  {
    private static AddToContactsItem instance;

    private AddToContactsItem()
    {
    }

    public static AddToContactsItem Instance
    {
      get
      {
        if (AddToContactsItem.instance == null)
          AddToContactsItem.instance = new AddToContactsItem();
        return AddToContactsItem.instance;
      }
    }
  }
}
