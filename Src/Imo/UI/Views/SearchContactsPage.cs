// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.UI.Views.SearchContactsPage
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using Imo.Phone.Controls;
using ImoSilverlightApp.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using Windows.UI.ViewManagement;


namespace ImoSilverlightApp.UI.Views
{
  public class SearchContactsPage : ImoPage
  {
    internal ImoTextBox searchTextBox;
    internal LongListSelector searchList;
    private bool _contentLoaded;

    public SearchContactsPage()
    {
      this.InitializeComponent();
      this.DataContext = (object) (this.ViewModel = new SearchContactsPageViewModel((FrameworkElement) this));
      this.Loaded += new RoutedEventHandler(this.SearchContactsPage_Loaded);
    }

    private void SearchContactsPage_Loaded(object sender, RoutedEventArgs e)
    {
      this.searchTextBox.Focus();
    }

    private SearchContactsPageViewModel ViewModel { get; set; }

    private void VideoCall_Click(object sender, GestureEventArgs e)
    {
    }

    private void SearchResult_Tapped(object sender, GestureEventArgs e)
    {
      SearchListContact viewModelOf = VisualUtils.GetViewModelOf<SearchListContact>(sender);
      if (viewModelOf == null)
        return;
      this.NavigationService.Navigate(new Uri("/UI/Views/ConversationPage.xaml?buid=" + viewModelOf.Contact.Buid, UriKind.Relative));
      e.Handled = true;
    }

    private void SearchDirectory_Click(object sender, GestureEventArgs e)
    {
      this.ViewModel.SearchDirectory();
    }

    private async void SearchPhonebookContact_Click(object sender, GestureEventArgs e)
    {
      SearchListPhonebookContact phonebookContact = VisualUtils.GetViewModelOf<SearchListPhonebookContact>(sender);
      if (phonebookContact == null)
        return;
      if (await ImoMessageBox.Show("Invite " + phonebookContact.Name + " to imo. Standard SMS rates may apply.", ImoMessageBoxButton.OKCancel) != ImoMessageBoxResult.OK)
        return;
      await IMO.PhonebookManager.SendSms((IList<string>) new List<string>()
      {
        phonebookContact.PhoneNumber
      });
    }

    private void AddToContacts_Click(object sender, GestureEventArgs e)
    {
      this.ViewModel.AddContact();
    }

    private void searchList_MouseEnter(object sender, MouseEventArgs e)
    {
      this.searchList.Focus();
      InputPane.GetForCurrentView().TryHide();
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/ImoSilverlightApp;component/UI/Views/SearchContactsPage.xaml", UriKind.Relative));
      this.searchTextBox = (ImoTextBox) this.FindName("searchTextBox");
      this.searchList = (LongListSelector) this.FindName("searchList");
    }
  }
}
