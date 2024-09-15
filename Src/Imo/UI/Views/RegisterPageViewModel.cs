// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.UI.Views.RegisterPageViewModel
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Helpers;
using ImoSilverlightApp.Storage.Models;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.Windows;


namespace ImoSilverlightApp.UI.Views
{
  internal class RegisterPageViewModel : ViewModelBase
  {
    private bool showBusyIndicator;

    public RegisterPageViewModel(FrameworkElement element)
      : base(element)
    {
    }

    protected override void OnLoaded(object sender, RoutedEventArgs e)
    {
      this.ApplicationSettings.PropertyChanged += new PropertyChangedEventHandler(this.ApplicationSettings_PropertyChanged);
    }

    protected override void OnUnloaded(object sender, RoutedEventArgs e)
    {
      this.ApplicationSettings.PropertyChanged -= new PropertyChangedEventHandler(this.ApplicationSettings_PropertyChanged);
    }

    public ApplicationSettings ApplicationSettings => IMO.ApplicationSettings;

    private void ApplicationSettings_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      if (!(e.PropertyName == "UserFullName") && !(e.PropertyName == "UserAge"))
        return;
      this.OnPropertyChanged("CanContinue");
    }

    internal void PhoneRegister()
    {
      this.ShowBusyIndicator = true;
      IMO.AccountManager.PhoneRegister(this.Code, (Action<JToken>) (result =>
      {
        if (!(result.Value<string>((object) nameof (result)) == "fail"))
          return;
        switch (result.Value<string>((object) "reason"))
        {
          case "tooyoung":
            Utils.ShowPopup("Sorry, we are unable to process your registration");
            break;
          case "full_name":
            Utils.ShowPopup("Please enter your first and last name");
            break;
          default:
            Utils.ShowPopup("There was a problem during registration, please try again later");
            break;
        }
        this.ShowBusyIndicator = false;
      }));
    }

    public string Code { get; set; }

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

    public bool CanContinue
    {
      get
      {
        return !string.IsNullOrEmpty(this.ApplicationSettings.UserFullName) && !string.IsNullOrEmpty(this.ApplicationSettings.UserAge);
      }
    }
  }
}
