// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.UI.Views.InvitePeopleToGroupPageViewModel
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Storage.Models;
using System.Windows;


namespace ImoSilverlightApp.UI.Views
{
  internal class InvitePeopleToGroupPageViewModel : ViewModelBase
  {
    private bool isCreateEnabled;
    private bool showBusyIndicator;
    private Contact contact;

    public InvitePeopleToGroupPageViewModel(string buid, FrameworkElement el)
      : base(el)
    {
      this.contact = IMO.ContactsManager.GetOrCreateContact(buid);
    }

    public bool IsCreateEnabled
    {
      get => this.isCreateEnabled;
      set
      {
        if (this.isCreateEnabled == value)
          return;
        this.isCreateEnabled = value;
        this.OnPropertyChanged(nameof (IsCreateEnabled));
      }
    }

    public Contact Contact => this.contact;

    public bool ShowBusyIndicator
    {
      get => this.showBusyIndicator;
      set
      {
        if (this.showBusyIndicator == value)
          return;
        this.showBusyIndicator = value;
        this.OnPropertyChanged(nameof (ShowBusyIndicator));
      }
    }
  }
}
