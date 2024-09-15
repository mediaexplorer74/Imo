// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.NavigationManager
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Helpers;
using ImoSilverlightApp.Storage.Models;
using NLog;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;


namespace ImoSilverlightApp
{
  internal class NavigationManager
  {
    private static NavigationManager instance;
    private static readonly Logger log = LogManager.GetLogger(typeof (NavigationManager).Name);
    private long navigateToConversationTimestamp;

    private NavigationManager()
    {
    }

    public static NavigationManager Instance
    {
      get
      {
        if (NavigationManager.instance == null)
          NavigationManager.instance = new NavigationManager();
        return NavigationManager.instance;
      }
    }

    private static NavigationService NavigationService
    {
      get
      {
        return !((Application.Current.RootVisual is Frame rootVisual ? rootVisual.Content : (object) null) is Page content) ? (NavigationService) null : content.NavigationService;
      }
    }

    internal void NavigateToVideoCapturePage(string receiver = null)
    {
      this.TryNavigateTo(new Uri("/UI/Views/VideoCapturePage.xaml?receiver=" + receiver, UriKind.Relative));
    }

    internal void NavigateToCodeInput(string reason)
    {
      this.TryNavigateTo(new Uri("/UI/Views/CodeInputPage.xaml?reason=" + reason, UriKind.Relative));
    }

    internal void NavigateToVideoPlayerPage(string filePath)
    {
      this.TryNavigateTo(new Uri("/UI/Views/VideoPlayerPage.xaml?filePath=" + filePath, UriKind.Relative));
    }

    internal void NavigateToShareVideoToMembersPage(string filePath)
    {
      this.TryNavigateTo(new Uri("/UI/Views/ShareToMembersPage.xaml?filePath=" + filePath, UriKind.Relative));
    }

    internal void NavigateToShareObjectToMembersPage(string objectId, string objectType)
    {
      this.TryNavigateTo(new Uri("/UI/Views/ShareToMembersPage.xaml?objectId=" + objectId + "&objectType=" + objectType, UriKind.Relative));
    }

    internal void NavigateToRegisterPage(string codeText)
    {
      this.TryNavigateTo(new Uri("/UI/Views/RegisterPage.xaml?code=" + codeText, UriKind.Relative));
    }

    internal void NavigateToDeleteAccount()
    {
      this.TryNavigateTo(new Uri("/UI/Views/DeleteAccountPage.xaml", UriKind.Relative));
    }

    internal void NavigateToBlockedContacts()
    {
      this.TryNavigateTo(new Uri("/UI/Views/BlockedContactsPage.xaml", UriKind.Relative));
    }

    internal void NavigateToInvitePage()
    {
      this.TryNavigateTo(new Uri("/UI/Views/InvitePage.xaml", UriKind.Relative));
    }

    internal void NavigateToHome()
    {
      while (NavigationManager.NavigationService.CanGoBack)
      {
        if (NavigationManager.NavigationService.BackStack.First<JournalEntry>().Source.OriginalString.Contains("HomePage.xaml"))
        {
          NavigationManager.NavigationService.GoBack();
          return;
        }
        NavigationManager.NavigationService.RemoveBackEntry();
      }
      this.TryNavigateTo(new Uri("/UI/Views/HomePage.xaml", UriKind.Relative));
    }

    internal void NavigateToDownloadAccountPage()
    {
      this.TryNavigateTo(new Uri("/UI/Views/DownloadAccountPage.xaml", UriKind.Relative));
    }

    internal void NavigateToInvitePeopleToGroupPage(string buid)
    {
      this.TryNavigateTo(new Uri("/UI/Views/InvitePeopleToGroupPage.xaml?buid=" + buid, UriKind.Relative));
    }

    internal void NavigateToPhotoPreviewStream(string buid, int index)
    {
      this.TryNavigateTo(new Uri(string.Format("/UI/Views/PhotoStreamViewerPage.xaml?buid={0}&index={1}", (object) buid, (object) index), UriKind.Relative));
    }

    internal void NavigateToPhotoPreview(
      string photoUrl,
      int width,
      int height,
      string buid = "",
      string objectId = "")
    {
      this.TryNavigateTo(new Uri(string.Format("/UI/Views/ImageViewerPage.xaml?url={0}&width={1}&height={2}&buid={3}&objectId={4}", (object) photoUrl, (object) width, (object) height, (object) buid, (object) objectId), UriKind.Relative));
    }

    internal void LogMessageLoaded(bool isFromStorage)
    {
      if (this.navigateToConversationTimestamp == 0L)
        return;
      long num = Utils.GetTimestamp() - this.navigateToConversationTimestamp;
      IMO.MonitorLog.Log("speed_metrics", "message_loaded_from_" + (isFromStorage ? "storage" : "server"), (object) num);
      this.navigateToConversationTimestamp = 0L;
    }

    public void NavigateToConversation(string buid, bool focusTextInput = false)
    {
      this.navigateToConversationTimestamp = Utils.GetTimestamp();
      IMO.ConversationsManager.GetOrCreateConversation(buid)?.ReduceMessages();
      this.TryNavigateTo(new Uri("/UI/Views/ConversationPage.xaml?buid=" + buid, UriKind.Relative));
    }

    public void NavigateToProfileSettings()
    {
      this.TryNavigateTo(new Uri("/UI/Views/SettingsPage.xaml", UriKind.Relative));
    }

    internal void NavigateToAVCallPage()
    {
      IMO.AVManager.IsCallOpened = this.TryNavigateTo(new Uri("/UI/Views/AVCallPage.xaml", UriKind.Relative));
    }

    internal void NavigateToCreateGroup()
    {
      this.TryNavigateTo(new Uri("/UI/Views/CreateGroupPage.xaml", UriKind.Relative));
    }

    internal void NavigateToBuddyProfile(string buid)
    {
      this.TryNavigateTo(new Uri("/UI/Views/BuddyProfilePage.xaml?buid=" + buid, UriKind.Relative));
    }

    internal void NavigateToProfile(string buid)
    {
      if (Contact.IsGroupBuid(buid))
        this.NavigateToGroupProfile(buid);
      else
        this.NavigateToBuddyProfile(buid);
    }

    internal void NavigateToGroupProfile(string buid)
    {
      this.TryNavigateTo(new Uri("/UI/Views/GroupProfilePage.xaml?buid=" + buid, UriKind.Relative));
    }

    internal void NavigateToGallery(string buid)
    {
      this.TryNavigateTo(new Uri("/UI/Views/GalleryPage.xaml?buid=" + buid, UriKind.Relative));
    }

    internal void NavigateToSignInPage()
    {
      this.TryNavigateTo(new Uri("/UI/Views/SignInPage.xaml", UriKind.Relative));
    }

    internal void NavigateToDispatcher()
    {
      this.TryNavigateTo(new Uri("/UI/Views/DispatcherPage.xaml", UriKind.Relative));
    }

    internal void NavigateToGDPRAccept()
    {
      this.TryNavigateTo(new Uri("/UI/Views/GDPRAcceptPage.xaml", UriKind.Relative));
    }

    private bool TryNavigateTo(Uri uri)
    {
      if (uri == (Uri) null)
        return false;
      try
      {
        NavigationService navigationService = NavigationManager.NavigationService;
        return navigationService != null && navigationService.Navigate(uri);
      }
      catch (Exception ex)
      {
        NavigationManager.log.Error(ex, "Error navigating to " + uri.ToString(), 218, nameof (TryNavigateTo));
      }
      return false;
    }

    internal void NavigateBackOrExit()
    {
      if (NavigationManager.NavigationService != null)
      {
        if (NavigationManager.NavigationService.CanGoBack && NavigationManager.NavigationService.BackStack.First<JournalEntry>().Source.OriginalString.Contains("MainPage.xaml"))
          Application.Current.Terminate();
        else if (NavigationManager.NavigationService.CanGoBack)
          NavigationManager.NavigationService?.GoBack();
        else
          Application.Current.Terminate();
      }
      IMO.AVManager.IsCallOpened = false;
    }

    internal void NavigateBackOrHome()
    {
      if (NavigationManager.NavigationService != null)
      {
        if (NavigationManager.NavigationService.CanGoBack && NavigationManager.NavigationService.BackStack.First<JournalEntry>().Source.OriginalString.Contains("MainPage.xaml"))
          this.NavigateToHome();
        else if (NavigationManager.NavigationService.CanGoBack)
          NavigationManager.NavigationService?.GoBack();
        else
          this.NavigateToHome();
      }
      IMO.AVManager.IsCallOpened = false;
    }

    internal void NavigatePastOpenChat(string buid)
    {
      if (!NavigationManager.NavigationService.CanGoBack || !NavigationManager.NavigationService.BackStack.First<JournalEntry>().Source.OriginalString.Contains(buid))
        return;
      this.NavigateToHome();
    }
  }
}
