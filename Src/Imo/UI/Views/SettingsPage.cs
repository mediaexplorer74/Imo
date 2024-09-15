// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.UI.Views.SettingsPage
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Helpers;
using Microsoft.Phone.Tasks;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;


namespace ImoSilverlightApp.UI.Views
{
  public class SettingsPage : ImoPage
  {
    private bool _contentLoaded;

    public SettingsPage()
    {
      this.InitializeComponent();
      this.DataContext = (object) (this.ViewModel = new SettingsPageViewModel((FrameworkElement) this));
    }

    private SettingsPageViewModel ViewModel { get; set; }

    private void BlockedContacts_Click(object sender, RoutedEventArgs e)
    {
      IMO.NavigationManager.NavigateToBlockedContacts();
    }

    private void ChangeImage_Tap(object sender, GestureEventArgs e) => this.ChangePhoto();

    private void ChangePhoto_Click(object sender, RoutedEventArgs e) => this.ChangePhoto();

    private void Dispatcher_Click(object sender, RoutedEventArgs e)
    {
    }

    private void ChangePhoto()
    {
      ImageUtils.ChoosePhoto((Action<PhotoResult>) (async photoResult =>
      {
        if (photoResult.ChosenPhoto == null)
          return;
        this.ViewModel.IsUploading = true;
        string pathFromPhotoResult = await FSUtils.GetFilePathFromPhotoResult(photoResult, "profile_photo");
        if (pathFromPhotoResult == null)
          this.ViewModel.IsUploading = false;
        else
          IMO.Pixel.UploadOwnProfile(pathFromPhotoResult, (Action) (() => this.ViewModel.IsUploading = false));
      }));
    }

    private void DeleteAccount_Click(object sender, RoutedEventArgs e)
    {
      IMO.NavigationManager.NavigateToDeleteAccount();
    }

    private async void DeleteChatHistory_Click(object sender, RoutedEventArgs e)
    {
      if (await ImoMessageBox.Show("Clicking \"Yes\" will permanently delete ALL your chat history. Are you sure you want to continue?", ImoMessageBoxButton.YesNo) != ImoMessageBoxResult.Yes)
        return;
      IMO.IM.ClearHistory((Action<JToken>) (r =>
      {
        IMO.ConversationsManager.ClearConversationsMessages();
        Utils.ShowPopup("Chat history deleted!");
      }));
    }

    private void Image_Tap(object sender, GestureEventArgs e)
    {
      if (IMO.User.ProfilePhotoId == null)
        return;
      IMO.NavigationManager.NavigateToPhotoPreview(ImageUtils.GetPhotoUrlFromId(IMO.User.ProfilePhotoId, PictureSize.Large), 0, 0);
    }

    private void DownloadAccountData_Click(object sender, RoutedEventArgs e)
    {
      IMO.NavigationManager.NavigateToDownloadAccountPage();
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/ImoSilverlightApp;component/UI/Views/SettingsPage.xaml", UriKind.Relative));
    }
  }
}
