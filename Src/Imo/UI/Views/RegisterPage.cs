// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.UI.Views.RegisterPage
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using Microsoft.Phone.Controls;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;


namespace ImoSilverlightApp.UI.Views
{
  public class RegisterPage : PhoneApplicationPage
  {
    internal Grid ContentRoot;
    internal ImoTextBox ageTextBox;
    private bool _contentLoaded;

    public RegisterPage()
    {
      this.InitializeComponent();
      this.DataContext = (object) (this.ViewModel = new RegisterPageViewModel((FrameworkElement) this));
    }

    private RegisterPageViewModel ViewModel { get; set; }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
      string str = (string) null;
      if (this.NavigationContext.QueryString.TryGetValue("code", out str))
        this.ViewModel.Code = str;
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

    private void ContinueButton_Click(object sender, EventArgs e) => this.ViewModel.PhoneRegister();

    private void ImoTextBox_KeyDown(object sender, KeyEventArgs e)
    {
      if ((e.Key < Key.D0 || e.Key > Key.D9) && e.Key != Key.Back)
      {
        e.Handled = true;
      }
      else
      {
        if (this.ageTextBox.Text == null || this.ageTextBox.Text.Length != 2 || this.ageTextBox.Text[0] == '1')
          return;
        e.Handled = true;
      }
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/ImoSilverlightApp;component/UI/Views/RegisterPage.xaml", UriKind.Relative));
      this.ContentRoot = (Grid) this.FindName("ContentRoot");
      this.ageTextBox = (ImoTextBox) this.FindName("ageTextBox");
    }
  }
}
