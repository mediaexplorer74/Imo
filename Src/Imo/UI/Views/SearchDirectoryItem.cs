// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.UI.Views.SearchDirectoryItem
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll


namespace ImoSilverlightApp.UI.Views
{
  internal class SearchDirectoryItem : SearchListItemBase
  {
    private static SearchDirectoryItem instance;

    private SearchDirectoryItem()
    {
    }

    public static SearchDirectoryItem Instance
    {
      get
      {
        if (SearchDirectoryItem.instance == null)
          SearchDirectoryItem.instance = new SearchDirectoryItem();
        return SearchDirectoryItem.instance;
      }
    }
  }
}
