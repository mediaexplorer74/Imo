// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.UI.Views.GDPRAcceptPage
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Windows.System;


namespace ImoSilverlightApp.UI.Views
{
  public class GDPRAcceptPage : ImoPage
  {
    internal Grid LayoutRoot;
    private bool _contentLoaded;

    public GDPRAcceptPage() => this.InitializeComponent();

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
      base.OnNavigatedTo(e);
      while (this.NavigationService.CanGoBack)
        this.NavigationService.RemoveBackEntry();
    }

    private async void privacyPolicy_Click(object sender, RoutedEventArgs e)
    {
      int num = await Launcher.LaunchUriAsync(new Uri("https://imo.im/privacy")) ? 1 : 0;
    }

    private async void termsOfService_Click(object sender, RoutedEventArgs e)
    {
      int num = await Launcher.LaunchUriAsync(new Uri("https://imo.im/terms")) ? 1 : 0;
    }

    private void Accept_Clicked(object sender, RoutedEventArgs e) => IMO.ImoAccount.AcceptGDPR();

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/ImoSilverlightApp;component/UI/Views/GDPRAcceptPage.xaml", UriKind.Relative));
      this.LayoutRoot = (Grid) this.FindName("LayoutRoot");
    }
  }
}
