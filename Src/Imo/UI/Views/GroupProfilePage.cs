// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.UI.Views.GroupProfilePage
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Helpers;
using ImoSilverlightApp.Storage.Models;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;


namespace ImoSilverlightApp.UI.Views
{
  public class GroupProfilePage : ImoPage
  {
    internal IconButton settingsButton;
    internal Border isUploadingIndicator;
    internal Imo.Phone.Controls.LongListSelector searchResultList;
    private bool _contentLoaded;

    public GroupProfilePage() => this.InitializeComponent();

    internal GroupProfilePageViewModel ViewModel { get; set; }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
      base.OnNavigatedTo(e);
      string buid = "";
      if (!this.NavigationContext.QueryString.TryGetValue("buid", out buid))
        return;
      this.DataContext = (object) (this.ViewModel = new GroupProfilePageViewModel(buid, (FrameworkElement) this));
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
      base.OnNavigatedFrom(e);
      this.ViewModel.Dispose();
    }

    private void MemberChat_Click(object sender, EventArgs e)
    {
      Contact viewModelOf = VisualUtils.GetViewModelOf<Contact>(sender);
      if (viewModelOf == null)
        return;
      IMO.NavigationManager.NavigateToConversation(viewModelOf.Buid);
    }

    private void MemberProfile_Click(object sender, EventArgs e)
    {
      Contact viewModelOf = VisualUtils.GetViewModelOf<Contact>(sender);
      if (viewModelOf == null)
        return;
      IMO.NavigationManager.NavigateToBuddyProfile(viewModelOf.Buid);
    }

    private void MemberRemove_Click(object sender, EventArgs e)
    {
      Contact viewModelOf = VisualUtils.GetViewModelOf<Contact>(sender);
      if (viewModelOf == null)
        return;
      IMO.ContactsManager.KickGroupMember(viewModelOf.Buid, this.ViewModel.Contact.Gid);
    }

    private void AddMembers_Tapped(object sender, System.Windows.Input.GestureEventArgs e)
    {
      IMO.NavigationManager.NavigateToInvitePeopleToGroupPage(this.ViewModel.Contact.Buid);
    }

    private void FavoriteButton_Tapped(object sender, System.Windows.Input.GestureEventArgs e)
    {
      if (!this.ViewModel.Contact.IsFavorite)
        IMO.ContactsManager.AddContactToFavorites(this.ViewModel.Contact);
      else
        IMO.ContactsManager.RemoveContactFromFavorites(this.ViewModel.Contact);
    }

    private void ViewGallery_Tapped(object sender, System.Windows.Input.GestureEventArgs e)
    {
      IMO.NavigationManager.NavigateToGallery(this.ViewModel.Contact.Buid);
    }

    private void Settings_Click(object sender, EventArgs e)
    {
      ContextMenuService.GetContextMenu((DependencyObject) this.settingsButton).IsOpen = true;
    }

    private void MenuItem_Click(object sender, RoutedEventArgs e)
    {
    }

    private async void LeaveGroup_Click(object sender, RoutedEventArgs e)
    {
      if (!await this.ViewModel.Contact.LeaveGroup(true))
        return;
      IMO.NavigationManager.NavigateToHome();
    }

    private void ChangePhoto_Click(object sender, RoutedEventArgs e)
    {
      ImageUtils.ChoosePhoto((Action<PhotoResult>) (async photoResult =>
      {
        if (photoResult.ChosenPhoto == null)
          return;
        this.isUploadingIndicator.Visibility = Visibility.Visible;
        string pathFromPhotoResult = await FSUtils.GetFilePathFromPhotoResult(photoResult, "group_photo");
        if (pathFromPhotoResult == null)
          this.isUploadingIndicator.Visibility = Visibility.Collapsed;
        else
          IMO.Pixel.UploadGroupProfile(pathFromPhotoResult, this.ViewModel.Contact.Gid, (Action<JToken>) (result => this.isUploadingIndicator.Visibility = Visibility.Collapsed));
      }));
    }

    private void Image_Tap(object sender, System.Windows.Input.GestureEventArgs e)
    {
      IMO.NavigationManager.NavigateToPhotoPreview(this.ViewModel.Contact.PhotoUrl, 0, 0);
    }

    private void Mute_Click(object sender, RoutedEventArgs e) => this.ViewModel.Contact.Mute();

    private void Unmute_Click(object sender, RoutedEventArgs e) => this.ViewModel.Contact.Unmute();

    private void Member_Tap(object sender, System.Windows.Input.GestureEventArgs e)
    {
      ContextMenuService.GetContextMenu((DependencyObject) (sender as Grid)).IsOpen = true;
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/ImoSilverlightApp;component/UI/Views/GroupProfilePage.xaml", UriKind.Relative));
      this.settingsButton = (IconButton) this.FindName("settingsButton");
      this.isUploadingIndicator = (Border) this.FindName("isUploadingIndicator");
      this.searchResultList = (Imo.Phone.Controls.LongListSelector) this.FindName("searchResultList");
    }
  }
}
