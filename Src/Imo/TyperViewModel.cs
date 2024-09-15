// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.TyperViewModel
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Storage.Models;


namespace ImoSilverlightApp
{
  public class TyperViewModel : ModelBase
  {
    private Contact contact;
    private string message;

    public TyperViewModel(Contact contact, string message)
    {
      this.contact = contact;
      this.message = message;
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

    public string Message
    {
      get => this.message;
      set
      {
        if (!(this.message != value))
          return;
        this.message = value;
        this.OnPropertyChanged(nameof (Message));
      }
    }
  }
}
