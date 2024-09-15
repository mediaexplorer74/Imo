// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.UI.Views.CreateGroupViewModel
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using System.Windows;


namespace ImoSilverlightApp.UI.Views
{
  internal class CreateGroupViewModel : ViewModelBase
  {
    private bool isCreateEnabled;
    private bool showNoNameError;

    public CreateGroupViewModel(FrameworkElement el)
      : base(el)
    {
    }

    protected override void OnLoaded(object sender, RoutedEventArgs e)
    {
      this.ShowNoNameError = false;
      IMO.MonitorLog.Log("group_chat", "create_group_opened");
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

    public bool ShowNoNameError
    {
      get => this.showNoNameError;
      set
      {
        if (this.showNoNameError == value)
          return;
        this.showNoNameError = value;
        this.OnPropertyChanged(nameof (ShowNoNameError));
      }
    }
  }
}
