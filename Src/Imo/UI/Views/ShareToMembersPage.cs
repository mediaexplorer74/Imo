// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.UI.Views.ShareToMembersPage
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Navigation;


namespace ImoSilverlightApp.UI.Views
{
  public class ShareToMembersPage : ImoPage
  {
    internal Microsoft.Phone.Shell.ApplicationBar appBar;
    internal ContactSelectorView selectorControl;
    private bool _contentLoaded;

    public ShareToMembersPage() => this.InitializeComponent();

    private ShareToMembersPageViewModel ViewModel { get; set; }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
      base.OnNavigatedTo(e);
      string filePath = (string) null;
      string objectId = (string) null;
      string objectType = (string) null;
      this.NavigationContext.QueryString.TryGetValue("filePath", out filePath);
      this.NavigationContext.QueryString.TryGetValue("objectId", out objectId);
      this.NavigationContext.QueryString.TryGetValue("objectType", out objectType);
      this.DataContext = (object) (this.ViewModel = new ShareToMembersPageViewModel((FrameworkElement) this, filePath, objectId, objectType, this.selectorControl));
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
      base.OnNavigatedFrom(e);
      this.ViewModel.Dispose();
    }

    private void Cancel_Click(object sender, EventArgs e)
    {
      IMO.NavigationManager.NavigateBackOrHome();
    }

    private async void ShareVideo_Click(object sender, EventArgs e)
    {
      await this.ViewModel.DoShare();
      string singleSelectedBuid = this.ViewModel.GetSingleSelectedBuid();
      if (singleSelectedBuid != null)
      {
        IMO.NavigationManager.NavigateToConversation(singleSelectedBuid);
        this.NavigationService.RemoveBackEntry();
      }
      else
        IMO.NavigationManager.NavigateBackOrHome();
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/ImoSilverlightApp;component/UI/Views/ShareToMembersPage.xaml", UriKind.Relative));
      this.appBar = (Microsoft.Phone.Shell.ApplicationBar) this.FindName("appBar");
      this.selectorControl = (ContactSelectorView) this.FindName("selectorControl");
    }
  }
}
