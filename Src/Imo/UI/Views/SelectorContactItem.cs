// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.UI.Views.SelectorContactItem
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Storage.Models;


namespace ImoSilverlightApp.UI.Views
{
  internal class SelectorContactItem : SelectorItemBase
  {
    private Contact contact;

    public SelectorContactItem(Contact contact) => this.contact = contact;

    public Contact Contact => this.contact;

    public override int GetHashCode() => this.contact.GetHashCode();

    public override bool Equals(object obj)
    {
      return obj is SelectorContactItem && this.contact.Equals((object) (obj as SelectorContactItem).Contact);
    }
  }
}
