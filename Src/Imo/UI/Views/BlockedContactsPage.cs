// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.UI.Views.BlockedContactsPage
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Helpers;
using ImoSilverlightApp.Storage.Models;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;


namespace ImoSilverlightApp.UI.Views
{
  public class BlockedContactsPage : ImoPage
  {
    internal Grid LayoutRoot;
    private bool _contentLoaded;

    public BlockedContactsPage()
    {
      this.InitializeComponent();
      this.DataContext = (object) (this.ViewModel = new BlockedContactsPageViewModel((FrameworkElement) this));
    }

    private BlockedContactsPageViewModel ViewModel { get; set; }

    private async void Unblock_Click(object sender, EventArgs e)
    {
      Contact contact = VisualUtils.GetViewModelOf<Contact>(sender);
      if (contact == null)
        return;
      if (await ImoMessageBox.Show("Unblock " + contact.Alias + "?", ImoMessageBoxButton.YesNo) != ImoMessageBoxResult.Yes)
        return;
      contact.Unblock();
      IMO.NavigationManager.NavigateToConversation(contact.Buid);
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/ImoSilverlightApp;component/UI/Views/BlockedContactsPage.xaml", UriKind.Relative));
      this.LayoutRoot = (Grid) this.FindName("LayoutRoot");
    }
  }
}
