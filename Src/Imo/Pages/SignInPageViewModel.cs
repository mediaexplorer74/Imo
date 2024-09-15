// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.Pages.SignInPageViewModel
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Connection;
using ImoSilverlightApp.Helpers;
using ImoSilverlightApp.Storage.Models;
using ImoSilverlightApp.UI.Views;
using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;


namespace ImoSilverlightApp.Pages
{
  public class SignInPageViewModel : ViewModelBase
  {
    private static readonly Logger log = LogManager.GetLogger(typeof (SignInPageViewModel).Name);
    private const string DISCONNECTED_MESSAGE = "Unable to connect";
    private const string DISPATCHER_RESET_MESSAGE = "Connection error, try again";
    private const string INVALID_COUNTRY_MESSAGE = "Invalid country";
    private const int CONNECTION_RESET_TIMEOUT = 30000;
    private DateTime lastShowBusyIndicator = DateTime.MinValue;
    private IList<CountryCodeData> countryCodesData;
    private CountryCodeData selectedCountryCodeData;
    private string phoneCCText;
    private bool isCountryValid;
    private bool showBusyIndicator;
    private string errorMessage;
    private bool isConnected;
    private bool getStartedCalled;
    private bool isLoaded;
    private bool isPendingGetStarted;
    private SignInPage page;
    private IList<CountryCodeData> filteredCountryCodesData;
    private string searchText;

    public SignInPageViewModel(SignInPage page)
      : base((FrameworkElement) page)
    {
      this.page = page;
      this.countryCodesData = (IList<CountryCodeData>) CountryCodesHelper.GetCountryCodesData().OrderBy<CountryCodeData, string>((Func<CountryCodeData, string>) (x => x.Country)).ToList<CountryCodeData>();
      this.selectedCountryCodeData = this.countryCodesData.Where<CountryCodeData>((Func<CountryCodeData, bool>) (x => x.CC == this.ApplicationSettings.CountryCode)).FirstOrDefault<CountryCodeData>();
      this.isCountryValid = this.selectedCountryCodeData != null;
      this.countryCodesData = (IList<CountryCodeData>) CountryCodesHelper.GetCountryCodesData().OrderBy<CountryCodeData, string>((Func<CountryCodeData, string>) (x => x.Country)).ToList<CountryCodeData>();
      this.filteredCountryCodesData = this.countryCodesData;
    }

    protected override void OnLoaded(object sender, RoutedEventArgs e)
    {
      this.isLoaded = true;
      this.IsConnected = IMO.Network.IsConnected;
      IMO.AccountManager.SignedOff += new EventHandler(this.SignedOffHandler);
      IMO.Network.Connected += new EventHandler<EventArg<ConnectionData>>(this.Network_Connected);
      IMO.Network.Disconnected += new EventHandler<EventArgs>(this.Network_Disconnected);
      IMO.Dispatcher.Resetted += new EventHandler(this.Dispatcher_Resetted);
      this.SetPhoneCCText(this.selectedCountryCodeData != null ? this.selectedCountryCodeData.PhoneCCNoPlus : "");
      this.ApplicationSettings.PropertyChanged += new PropertyChangedEventHandler(this.ApplicationSettings_PropertyChanged);
    }

    protected override void OnUnloaded(object sender, RoutedEventArgs e)
    {
      this.isLoaded = false;
      IMO.AccountManager.SignedOff -= new EventHandler(this.SignedOffHandler);
      IMO.Network.Connected -= new EventHandler<EventArg<ConnectionData>>(this.Network_Connected);
      IMO.Network.Disconnected -= new EventHandler<EventArgs>(this.Network_Disconnected);
      IMO.Dispatcher.Resetted -= new EventHandler(this.Dispatcher_Resetted);
      this.ApplicationSettings.PropertyChanged -= new PropertyChangedEventHandler(this.ApplicationSettings_PropertyChanged);
    }

    private void Dispatcher_Resetted(object sender, EventArgs e)
    {
      this.isPendingGetStarted = false;
    }

    private void ApplicationSettings_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      if (!(e.PropertyName == "PhoneNumber"))
        return;
      this.OnPropertyChanged("CanContinue");
    }

    private void Network_Connected(object sender, EventArg<ConnectionData> e)
    {
      this.IsConnected = true;
      if (!e.Arg.ResetDispatcher)
        return;
      this.Reset();
    }

    private void Network_Disconnected(object sender, EventArgs e) => this.IsConnected = false;

    private void SignedOffHandler(object sender, EventArgs e) => this.ShowBusyIndicator = false;

    public IEnumerable<CountryCodeData> CountryCodesData
    {
      get => (IEnumerable<CountryCodeData>) this.countryCodesData;
    }

    public bool IsConnected
    {
      get => this.isConnected;
      set
      {
        if (this.isConnected == value)
          return;
        this.isConnected = value;
        if (!value && this.getStartedCalled)
          this.Reset(this.errorMessage ?? "Unable to connect");
        this.OnPropertyChanged(nameof (IsConnected));
      }
    }

    public CountryCodeData SelectedCountryCodeData
    {
      get => this.selectedCountryCodeData;
      set
      {
        if (this.selectedCountryCodeData == value)
          return;
        this.selectedCountryCodeData = value;
        if (this.selectedCountryCodeData != null)
        {
          IMO.ApplicationSettings.CountryCode = this.selectedCountryCodeData.CC;
          this.SetPhoneCCText(this.selectedCountryCodeData.PhoneCCNoPlus);
          this.IsCountryValid = true;
        }
        else
          this.IsCountryValid = false;
        this.OnPropertyChanged(nameof (SelectedCountryCodeData));
        this.OnPropertyChanged("CanContinue");
      }
    }

    public ApplicationSettings ApplicationSettings => IMO.ApplicationSettings;

    public bool IsCountryValid
    {
      get => this.isCountryValid;
      set
      {
        if (this.isCountryValid == value)
          return;
        this.isCountryValid = value;
        if (this.isCountryValid)
        {
          if (this.ErrorMessage == "Invalid country")
            this.ErrorMessage = (string) null;
        }
        else
          this.ErrorMessage = "Invalid country";
        this.OnPropertyChanged(nameof (IsCountryValid));
      }
    }

    public string ErrorMessage
    {
      get => this.errorMessage;
      set
      {
        this.errorMessage = value;
        this.OnPropertyChanged(nameof (ErrorMessage));
      }
    }

    public string PhoneCCText
    {
      get => this.phoneCCText;
      set
      {
        if (!(this.phoneCCText != value))
          return;
        this.phoneCCText = value;
        CountryCodeData countryDataByPhoneCc = CountryCodesHelper.GetCountryDataByPhoneCC(this.phoneCCText);
        this.SelectedCountryCodeData = countryDataByPhoneCc == null ? (CountryCodeData) null : countryDataByPhoneCc;
        this.OnPropertyChanged(nameof (PhoneCCText));
      }
    }

    private void SetPhoneCCText(string val)
    {
      this.phoneCCText = val;
      this.OnPropertyChanged("PhoneCCText");
    }

    public bool ShowBusyIndicator
    {
      get => this.showBusyIndicator;
      set
      {
        if (this.showBusyIndicator == value)
          return;
        if (value)
        {
          this.ErrorMessage = (string) null;
          this.lastShowBusyIndicator = DateTime.Now;
          Utils.DelayExecute(30000, (Action) (() =>
          {
            if (!this.showBusyIndicator || IMO.ApplicationProperties.IsSignedOn || !this.isLoaded || (DateTime.Now - this.lastShowBusyIndicator).TotalMilliseconds <= 29900.0)
              return;
            this.Reset("Connection error, try again");
            IMO.Network.Reconnect(true);
          }));
        }
        this.showBusyIndicator = value;
        this.OnPropertyChanged(nameof (ShowBusyIndicator));
      }
    }

    public void GetStarted(bool ignoreDisconnected = false)
    {
      SignInPageViewModel.log.Info(nameof (GetStarted));
      this.getStartedCalled = true;
      if (this.isPendingGetStarted)
        return;
      if (!ignoreDisconnected && !this.isConnected)
      {
        this.Reset("Unable to connect");
      }
      else
      {
        if (string.IsNullOrEmpty(IMO.ApplicationSettings.PhoneNumber) || !this.IsCountryValid)
          return;
        this.ShowBusyIndicator = true;
        this.isPendingGetStarted = true;
        IMO.AccountManager.GetStarted(IMO.ApplicationSettings.PhoneNumber, IMO.ApplicationSettings.CountryCode, (Action<LoginResultData>) (result =>
        {
          this.isPendingGetStarted = false;
          this.ShowBusyIndicator = false;
          if (result.ErrorMessage != null)
          {
            SignInPageViewModel.log.Info("GetStarted failed: " + result.ErrorMessage);
            Utils.ShowPopup(result.ErrorMessage);
          }
          else if (result.CodeRequested || result.NeedRegister)
          {
            string str = result.NeedRegister ? "register" : "login";
            SignInPageViewModel.log.Info("Showing code input to " + str);
            IMO.NavigationManager.NavigateToCodeInput(string.Concat((object) (CodeInputPageReason) (result.NeedRegister ? 1 : 0)));
          }
          else if (result.DoPhoneLogin)
          {
            this.ShowBusyIndicator = true;
            IMO.AccountManager.PhoneLogin((string) null, (Action<LoginResultData>) (loginResult =>
            {
              this.ShowBusyIndicator = false;
              if (loginResult.ErrorMessage == null)
                return;
              Utils.ShowPopup(loginResult.ErrorMessage);
            }));
          }
          else
          {
            if (!result.DoRegister)
              return;
            IMO.NavigationManager.NavigateToRegisterPage((string) null);
          }
        }));
      }
    }

    public void Reset(string errorMessage = null)
    {
      SignInPageViewModel.log.Info("Reset: " + errorMessage);
      this.ShowBusyIndicator = false;
      this.ErrorMessage = errorMessage;
    }

    public bool CanContinue
    {
      get
      {
        return this.selectedCountryCodeData != null && !string.IsNullOrEmpty(this.ApplicationSettings.PhoneNumber);
      }
    }

    public IList<CountryCodeData> FilteredCountryCodesData => this.filteredCountryCodesData;

    public string SearchText
    {
      get => this.searchText;
      set
      {
        if (!(this.searchText != value))
          return;
        this.searchText = value.Trim();
        this.filteredCountryCodesData = string.IsNullOrEmpty(this.searchText) ? this.countryCodesData : (IList<CountryCodeData>) this.countryCodesData.Where<CountryCodeData>((Func<CountryCodeData, bool>) (x => x.Country.ToLower().Contains(this.searchText.ToLower()))).ToList<CountryCodeData>();
        this.OnPropertyChanged("FilteredCountryCodesData");
        this.OnPropertyChanged(nameof (SearchText));
      }
    }
  }
}
