// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.UI.Views.SignInPage
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Helpers;
using ImoSilverlightApp.Pages;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Windows.System;


namespace ImoSilverlightApp.UI.Views
{
  public class SignInPage : ImoPage
  {
    internal ImoPage root;
    internal Microsoft.Phone.Shell.ApplicationBar appBar;
    internal Grid LayoutRoot;
    internal Grid ContentRoot;
    internal Path Arrow;
    internal ImoTextBox phoneInputTextBox;
    internal Popup countriesPopup;
    internal Grid popupGrid;
    internal ListBox listView;
    private bool _contentLoaded;

    public SignInPage()
    {
      this.InitializeComponent();
      this.DataContext = (object) (this.ViewModel = new SignInPageViewModel(this));
      this.Loaded += new RoutedEventHandler(this.SignInPage_Loaded);
    }

    private SignInPageViewModel ViewModel { get; set; }

    private void SignInPage_Loaded(object sender, RoutedEventArgs e)
    {
      this.popupGrid.Height = Application.Current.Host.Content.ActualHeight;
      this.popupGrid.Width = Application.Current.Host.Content.ActualWidth;
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
      base.OnNavigatedTo(e);
      while (this.NavigationService.CanGoBack)
        this.NavigationService.RemoveBackEntry();
      if (IMO.ApplicationSettings.CodeRequestedTimestamp != 0L)
        this.ViewModel.GetStarted(true);
      IMO.AccountManager.SignedOn += new EventHandler<EventArg<SignOnData>>(this.AccountManager_SignedOn);
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
      IMO.AccountManager.SignedOn -= new EventHandler<EventArg<SignOnData>>(this.AccountManager_SignedOn);
    }

    private void AccountManager_SignedOn(object sender, EventArg<SignOnData> e)
    {
      IMO.NavigationManager.NavigateToHome();
    }

    protected override void OnBackKeyPress(CancelEventArgs e)
    {
      if (this.countriesPopup.IsOpen)
      {
        this.countriesPopup.IsOpen = false;
        e.Cancel = true;
      }
      base.OnBackKeyPress(e);
    }

    private void country_Tapped(object sender, GestureEventArgs e)
    {
      CountryCodeData viewModelOf = VisualUtils.GetViewModelOf<CountryCodeData>(sender);
      if (viewModelOf == null)
        return;
      this.ViewModel.SelectedCountryCodeData = viewModelOf;
      this.HideCountriesPopup();
    }

    private void SelectCountry_Click(object sender, RoutedEventArgs e) => this.ShowCountriesPopup();

    private void ShowCountriesPopup()
    {
      this.countriesPopup.IsOpen = true;
      this.ApplicationBar.IsVisible = false;
    }

    private void HideCountriesPopup()
    {
      this.countriesPopup.IsOpen = false;
      this.ApplicationBar.IsVisible = true;
    }

    private async void privacyPolicy_Click(object sender, RoutedEventArgs e)
    {
      int num = await Launcher.LaunchUriAsync(new Uri("https://imo.im/privacy")) ? 1 : 0;
    }

    private async void termsOfService_Click(object sender, RoutedEventArgs e)
    {
      int num = await Launcher.LaunchUriAsync(new Uri("https://imo.im/terms")) ? 1 : 0;
    }

    private async void Continue_Click(object sender, EventArgs e)
    {
      IMO.ApplicationSettings.HasAcceptedGdpr = true;
      if (this.ViewModel.SelectedCountryCodeData == null)
        Utils.ShowPopup("Please select your country!");
      else if (string.IsNullOrEmpty(IMO.ApplicationSettings.PhoneNumber))
      {
        Utils.ShowPopup("Please fill in your phone number!");
      }
      else
      {
        switch (CountryCodesHelper.VerifyPhoneLength(IMO.ApplicationSettings.CountryCode, IMO.ApplicationSettings.PhoneNumber))
        {
          case PhoneLengthResult.OK:
            if (await ImoMessageBox.Show(string.Format("Please confirm your phone number: \n\n+{0} {1}", (object) this.ViewModel.PhoneCCText, (object) IMO.ApplicationSettings.PhoneNumber), ImoMessageBoxButton.OKCancel) != ImoMessageBoxResult.OK)
              break;
            this.ViewModel.GetStarted();
            break;
          case PhoneLengthResult.TooLong:
            Utils.ShowPopup("Phone number is too long!");
            break;
          case PhoneLengthResult.TooShort:
            Utils.ShowPopup("Phone number is too short!");
            break;
        }
      }
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/ImoSilverlightApp;component/UI/Views/SignInPage.xaml", UriKind.Relative));
      this.root = (ImoPage) this.FindName("root");
      this.appBar = (Microsoft.Phone.Shell.ApplicationBar) this.FindName("appBar");
      this.LayoutRoot = (Grid) this.FindName("LayoutRoot");
      this.ContentRoot = (Grid) this.FindName("ContentRoot");
      this.Arrow = (Path) this.FindName("Arrow");
      this.phoneInputTextBox = (ImoTextBox) this.FindName("phoneInputTextBox");
      this.countriesPopup = (Popup) this.FindName("countriesPopup");
      this.popupGrid = (Grid) this.FindName("popupGrid");
      this.listView = (ListBox) this.FindName("listView");
    }
  }
}
