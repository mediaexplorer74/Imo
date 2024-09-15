// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.UI.Views.CodeInputPage
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Helpers;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Windows.System;


namespace ImoSilverlightApp.UI.Views
{
  public class CodeInputPage : ImoPage
  {
    internal Microsoft.Phone.Shell.ApplicationBar appBar;
    internal Grid LayoutRoot;
    internal Grid ContentRoot;
    internal ImoTextBox codeTextBox;
    private bool _contentLoaded;

    public CodeInputPage()
    {
      this.InitializeComponent();
      this.DataContext = (object) (this.ViewModel = new CodeInputPageViewModel(this));
      this.Loaded += new RoutedEventHandler(this.CodeInputPage_Loaded);
    }

    public CodeInputPageViewModel ViewModel { get; set; }

    private void CodeInputPage_Loaded(object sender, RoutedEventArgs e) => this.codeTextBox.Focus();

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
      base.OnNavigatedTo(e);
      string str = (string) null;
      if (this.NavigationContext.QueryString.TryGetValue("reason", out str))
        this.ViewModel.Reason = (CodeInputPageReason) Enum.Parse(typeof (CodeInputPageReason), str);
      if (IMO.ApplicationSettings.CodeRequestedTimestamp == 0L)
        IMO.MonitorLog.Log("sign_in", "code_input_shown");
      IMO.ApplicationSettings.CodeRequestedTimestamp = Utils.GetTimestamp();
      IMO.AccountManager.SignedOn += new EventHandler<EventArg<SignOnData>>(this.AccountManager_SignedOn);
    }

    protected override void OnBackKeyPress(CancelEventArgs e)
    {
      base.OnBackKeyPress(e);
      IMO.ApplicationSettings.CodeRequestedTimestamp = 0L;
    }

    private void AccountManager_SignedOn(object sender, EventArg<SignOnData> e)
    {
      IMO.NavigationManager.NavigateToHome();
    }

    private async void privacyPolicy_Click(object sender, RoutedEventArgs e)
    {
      int num = await Launcher.LaunchUriAsync(new Uri("https://imo.im/privacy")) ? 1 : 0;
    }

    private async void termsOfService_Click(object sender, RoutedEventArgs e)
    {
      int num = await Launcher.LaunchUriAsync(new Uri("https://imo.im/terms")) ? 1 : 0;
    }

    private void Continue_Click(object sender, EventArgs e)
    {
      if (string.IsNullOrEmpty(this.ViewModel.CodeText))
        Utils.ShowPopup("Please fill in your verification code!");
      else
        this.ViewModel.CheckPhoneCode();
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/ImoSilverlightApp;component/UI/Views/CodeInputPage.xaml", UriKind.Relative));
      this.appBar = (Microsoft.Phone.Shell.ApplicationBar) this.FindName("appBar");
      this.LayoutRoot = (Grid) this.FindName("LayoutRoot");
      this.ContentRoot = (Grid) this.FindName("ContentRoot");
      this.codeTextBox = (ImoTextBox) this.FindName("codeTextBox");
    }
  }
}
