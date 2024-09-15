// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.UI.Views.InvitePageViewModel
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using System.Windows;


namespace ImoSilverlightApp.UI.Views
{
  internal class InvitePageViewModel : ViewModelBase
  {
    private bool isInviteEnabled;

    public InvitePageViewModel(FrameworkElement el)
      : base(el)
    {
    }

    public bool IsInviteEnabled
    {
      get => this.isInviteEnabled;
      set
      {
        if (this.isInviteEnabled == value)
          return;
        this.isInviteEnabled = value;
        this.OnPropertyChanged(nameof (IsInviteEnabled));
      }
    }
  }
}
