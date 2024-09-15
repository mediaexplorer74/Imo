// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.UI.Views.SelectorPhonebookContactItem
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll


namespace ImoSilverlightApp.UI.Views
{
  internal class SelectorPhonebookContactItem : SelectorItemBase
  {
    private string phoneNumber;
    private string name;

    public SelectorPhonebookContactItem(string phoneNumber, string name)
    {
      this.phoneNumber = phoneNumber;
      this.name = name;
    }

    public string Name => this.name;

    public string PhoneNumber => this.phoneNumber;

    public override int GetHashCode()
    {
      return (1073741827 * 16777619 ^ this.phoneNumber.GetHashCode()) * 16777619 ^ this.name.GetHashCode();
    }

    public override bool Equals(object obj)
    {
      return obj is SelectorPhonebookContactItem phonebookContactItem && this.name.Equals(phonebookContactItem.Name) && this.phoneNumber.Equals(phonebookContactItem.PhoneNumber);
    }
  }
}
