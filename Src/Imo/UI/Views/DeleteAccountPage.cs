// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.UI.Views.DeleteAccountPage
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Helpers;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;


namespace ImoSilverlightApp.UI.Views
{
  public class DeleteAccountPage : ImoPage
  {
    internal Grid LayoutRoot;
    internal TextBlock textBlockPhoneCC;
    internal ImoTextBox textBoxPhoneNumber;
    private bool _contentLoaded;

    public DeleteAccountPage()
    {
      this.InitializeComponent();
      CountryCodeData countryDataByCc = CountryCodesHelper.GetCountryDataByCC(IMO.User.PhoneCC);
      this.textBlockPhoneCC.Text = countryDataByCc != null ? countryDataByCc.PhoneCC : "";
    }

    private void DeleteAccount_Click(object sender, RoutedEventArgs e)
    {
      string str = Utils.NormalizePhoneNumber(this.textBoxPhoneNumber.Text);
      if (IMO.User.PhoneNumber == null)
        Utils.ShowPopup("Cannot validate phone number! Please try again later.");
      else if (str == IMO.User.PhoneNumber.Substring(this.textBlockPhoneCC.Text.Length))
        IMO.ImoAccount.DeleteAccount((Action<JToken>) (async result =>
        {
          await Utils.ShowPopup("Account deleted!");
          IMO.AccountManager.SignOff("deleted_account");
        }));
      else
        Utils.ShowPopup("Invalid phone!");
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/ImoSilverlightApp;component/UI/Views/DeleteAccountPage.xaml", UriKind.Relative));
      this.LayoutRoot = (Grid) this.FindName("LayoutRoot");
      this.textBlockPhoneCC = (TextBlock) this.FindName("textBlockPhoneCC");
      this.textBoxPhoneNumber = (ImoTextBox) this.FindName("textBoxPhoneNumber");
    }
  }
}
