// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.UI.Views.CodeInputPageViewModel
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Helpers;
using Newtonsoft.Json.Linq;
using System;
using System.Windows;


namespace ImoSilverlightApp.UI.Views
{
  public class CodeInputPageViewModel : ViewModelBase
  {
    private string codeText;
    private bool showBusyIndicator;
    private CodeInputPage page;

    public CodeInputPageViewModel(CodeInputPage page)
      : base((FrameworkElement) page)
    {
      this.page = page;
    }

    internal void CheckPhoneCode()
    {
      this.ShowBusyIndicator = true;
      IMO.AccountManager.CheckPhoneCode(this.CodeText, (Action<JToken>) (result =>
      {
        this.ShowBusyIndicator = false;
        if (result.Value<string>((object) nameof (result)) != "ok")
          Utils.ShowPopup("Wrong verification code!");
        else if (this.Reason == CodeInputPageReason.Login)
          IMO.AccountManager.PhoneLogin(this.CodeText, (Action<LoginResultData>) (loginResult =>
          {
            if (loginResult.ErrorMessage == null)
              return;
            Utils.ShowPopup(loginResult.ErrorMessage);
          }));
        else
          IMO.NavigationManager.NavigateToRegisterPage(this.CodeText);
      }));
    }

    public string CodeText
    {
      get => this.codeText;
      set
      {
        if (!(this.codeText != value))
          return;
        this.codeText = value;
        this.OnPropertyChanged(nameof (CodeText));
        this.OnPropertyChanged("CanContinue");
      }
    }

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

    public CodeInputPageReason Reason { get; set; }

    public bool CanContinue => !string.IsNullOrEmpty(this.codeText);

    public string PhoneNumber
    {
      get
      {
        return string.Format("({0}){1}", (object) CountryCodesHelper.GetPhoneCC(IMO.ApplicationSettings.CountryCode), (object) IMO.ApplicationSettings.PhoneNumber);
      }
    }
  }
}
