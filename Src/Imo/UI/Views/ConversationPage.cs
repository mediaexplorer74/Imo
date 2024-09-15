// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.UI.Views.ConversationPage
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Helpers;
using ImoSilverlightApp.Storage.Models;
using Microsoft.Phone.Tasks;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.UI.ViewManagement;


namespace ImoSilverlightApp.UI.Views
{
  public sealed class ConversationPage : ImoPage
  {
    private CoreApplicationView coreApplicationView;
    private bool keyboardVisible;
    private PhotoResult pendingSendPhoto;
    internal Grid LayoutRoot;
    internal Grid ContentRoot;
    internal ChatLog chatLog;
    internal TextBox messageTextBox;
    internal StickersPickerView stickersPickerView;
    internal Border fix10margin;
    private bool _contentLoaded;

    public ConversationPage()
    {
      this.InitializeComponent();
      this.Loaded += new RoutedEventHandler(this.ConversationPage_Loaded);
      this.Unloaded += new RoutedEventHandler(this.ConversationPage_Unloaded);
      this.coreApplicationView = CoreApplication.GetCurrentView();
      this.stickersPickerView.StickerPicked += new EventHandler<EventArg<Sticker>>(this.StickersPickerView_StickerPicked);
      if (Environment.OSVersion.Version.Major != 10)
        return;
      this.fix10margin.Height = 7.0;
    }

    protected override void OnBackKeyPress(CancelEventArgs e)
    {
      e.Cancel = true;
      if (this.ViewModel != null && this.ViewModel.ShowStickers)
        this.ViewModel.ShowStickers = false;
      else
        IMO.NavigationManager.NavigateToHome();
      base.OnBackKeyPress(e);
    }

    private void StickersPickerView_StickerPicked(object sender, EventArg<Sticker> e)
    {
      if (this.ViewModel == null)
        return;
      this.ViewModel.Conversation.SendSticker(e.Arg);
      this.ViewModel.ShowStickers = false;
    }

    private ConversationPageViewModel ViewModel { get; set; }

    private void ConversationPage_Loaded(object sender, RoutedEventArgs e)
    {
      this.stickersPickerView.Height = this.ActualHeight * 0.5;
      InputPane forCurrentView1 = InputPane.GetForCurrentView();
      // ISSUE: method pointer
      WindowsRuntimeMarshal.AddEventHandler<TypedEventHandler<InputPane, InputPaneVisibilityEventArgs>>(new Func<TypedEventHandler<InputPane, InputPaneVisibilityEventArgs>, EventRegistrationToken>(forCurrentView1.add_Showing), new Action<EventRegistrationToken>(forCurrentView1.remove_Showing), new TypedEventHandler<InputPane, InputPaneVisibilityEventArgs>((object) this, __methodptr(Keyboard_Showing)));
      InputPane forCurrentView2 = InputPane.GetForCurrentView();
      // ISSUE: method pointer
      WindowsRuntimeMarshal.AddEventHandler<TypedEventHandler<InputPane, InputPaneVisibilityEventArgs>>(new Func<TypedEventHandler<InputPane, InputPaneVisibilityEventArgs>, EventRegistrationToken>(forCurrentView2.add_Hiding), new Action<EventRegistrationToken>(forCurrentView2.remove_Hiding), new TypedEventHandler<InputPane, InputPaneVisibilityEventArgs>((object) this, __methodptr(Keyboard_Hidding)));
    }

    private void ConversationPage_Unloaded(object sender, RoutedEventArgs e)
    {
      // ISSUE: method pointer
      WindowsRuntimeMarshal.RemoveEventHandler<TypedEventHandler<InputPane, InputPaneVisibilityEventArgs>>(new Action<EventRegistrationToken>(InputPane.GetForCurrentView().remove_Showing), new TypedEventHandler<InputPane, InputPaneVisibilityEventArgs>((object) this, __methodptr(Keyboard_Showing)));
      // ISSUE: method pointer
      WindowsRuntimeMarshal.RemoveEventHandler<TypedEventHandler<InputPane, InputPaneVisibilityEventArgs>>(new Action<EventRegistrationToken>(InputPane.GetForCurrentView().remove_Hiding), new TypedEventHandler<InputPane, InputPaneVisibilityEventArgs>((object) this, __methodptr(Keyboard_Hidding)));
    }

    private void Keyboard_Hidding(InputPane sender, InputPaneVisibilityEventArgs args)
    {
      this.keyboardVisible = false;
    }

    private void Keyboard_Showing(InputPane sender, InputPaneVisibilityEventArgs args)
    {
      if (this.ViewModel == null)
        return;
      this.keyboardVisible = true;
      this.ViewModel.ShowStickers = false;
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
      base.OnNavigatedTo(e);
      if (this.NavigationService == null || !this.NavigationService.CanGoBack)
        IMO.MonitorLog.Log("application", "open_from_push");
      string buid = "";
      if (!this.NavigationContext.QueryString.TryGetValue("buid", out buid))
        return;
      Conversation conversation = IMO.ConversationsManager.GetOrCreateConversation(buid);
      conversation.MarkMessagesAsRead();
      conversation.OpenChat();
      this.DataContext = (object) (this.ViewModel = new ConversationPageViewModel(buid, (FrameworkElement) this, this.chatLog));
      if (this.pendingSendPhoto == null)
        return;
      this.ViewModel.SendPhoto(this.pendingSendPhoto);
      this.pendingSendPhoto = (PhotoResult) null;
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
      base.OnNavigatedFrom(e);
      if (this.ViewModel == null)
        return;
      this.ViewModel.Conversation.CloseChat();
      this.ViewModel.Dispose();
      this.ViewModel = (ConversationPageViewModel) null;
    }

    private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
      if (this.ViewModel == null)
        return;
      (sender as TextBox).GetBindingExpression(TextBox.TextProperty)?.UpdateSource();
      IMO.ConversationsManager.SendTypingState("typing", this.ViewModel.Conversation.Buid, this.messageTextBox.Text.Trim());
      this.ViewModel.Conversation.UpdateLastUserActivity();
    }

    private void Page_Tapped(object sender, GestureEventArgs e)
    {
      if (this.ViewModel == null)
        return;
      this.ViewModel.Conversation.UpdateLastUserActivity();
    }

    private void SendMessage()
    {
      if (this.ViewModel == null)
        return;
      this.ViewModel.SendMesage();
      this.messageTextBox.Text = string.Empty;
    }

    private void SendButton_Click(object sender, EventArgs e)
    {
      this.SendMessage();
      this.messageTextBox.Focus();
    }

    private void SendMessage_Click(object sender, EventArgs e) => this.SendMessage();

    private void messageTextBox_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key != Key.Enter)
        return;
      e.Handled = true;
      this.SendMessage();
    }

    private void SendButton_Tap(object sender, GestureEventArgs e) => this.SendMessage();

    private async void Blocked_Tapped(object sender, GestureEventArgs e)
    {
      if (this.ViewModel == null)
        return;
      if (await ImoMessageBox.Show("Block this user?", ImoMessageBoxButton.YesNo) != ImoMessageBoxResult.Yes)
        return;
      this.ViewModel.Contact.Block();
      IMO.ConversationsManager.GetOrCreateConversation(this.ViewModel.Contact.Buid).HideFromChatList();
      IMO.NavigationManager.NavigateToHome();
    }

    private void Add_Tapped(object sender, GestureEventArgs e)
    {
      if (this.ViewModel == null)
        return;
      IMO.ContactsManager.AddImoContact(this.ViewModel.Contact.Buid, "direct");
    }

    private void AudioCall_Click(object sender, EventArgs e)
    {
      if (this.ViewModel == null)
        return;
      IMO.AVManager.StartAudioCall(this.ViewModel.Conversation.Contact.Buid, "conversation_header-" + sender.GetType().Name);
    }

    private void VideoCall_Click(object sender, EventArgs e)
    {
      if (this.ViewModel == null)
        return;
      if (!this.ViewModel.Conversation.IsGroup)
        IMO.AVManager.StartVideoCall(this.ViewModel.Conversation.Contact.Buid, "conversation_header-" + sender.GetType().Name);
      else
        IMO.AVManager.InitiateGroupCall(this.ViewModel.Conversation.Contact.Buid, "conversation_header-" + sender.GetType().Name);
    }

    private void PhotoGallery_Tapped(object sender, EventArgs e)
    {
      if (this.ViewModel == null)
        return;
      IMO.NavigationManager.NavigateToGallery(this.ViewModel.Conversation.Contact.Buid);
    }

    private void sendVideo_Tapped(object sender, GestureEventArgs e)
    {
      if (this.ViewModel == null)
        return;
      IMO.NavigationManager.NavigateToVideoCapturePage("buid:" + this.ViewModel.Conversation.Buid);
    }

    private void SendImage_Click(object sender, EventArgs e)
    {
      ImageUtils.ChoosePhoto((Action<PhotoResult>) (photoResult =>
      {
        if (this.ViewModel != null)
          this.ViewModel.SendPhoto(photoResult);
        else
          this.pendingSendPhoto = photoResult;
      }));
    }

    private void Stickers_Click(object sender, EventArgs e)
    {
      if (this.ViewModel == null)
        return;
      this.ViewModel.ShowStickers = !this.ViewModel.ShowStickers;
      if (this.ViewModel.ShowStickers)
        this.stickersPickerView.ScrollSelectedStickerPackIntoView();
      InputPane.GetForCurrentView().TryHide();
    }

    private void Header_Tapped(object sender, GestureEventArgs e)
    {
      if (this.ViewModel == null)
        return;
      if (Contact.IsGroupBuid(this.ViewModel.Contact.Buid))
        IMO.NavigationManager.NavigateToGroupProfile(this.ViewModel.Contact.Buid);
      else
        IMO.NavigationManager.NavigateToBuddyProfile(this.ViewModel.Contact.Buid);
    }

    private void messageTextBox_GotFocus(object sender, RoutedEventArgs e)
    {
      if (this.ViewModel == null)
        return;
      this.ViewModel.ShowStickers = false;
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/ImoSilverlightApp;component/UI/Views/ConversationPage.xaml", UriKind.Relative));
      this.LayoutRoot = (Grid) this.FindName("LayoutRoot");
      this.ContentRoot = (Grid) this.FindName("ContentRoot");
      this.chatLog = (ChatLog) this.FindName("chatLog");
      this.messageTextBox = (TextBox) this.FindName("messageTextBox");
      this.stickersPickerView = (StickersPickerView) this.FindName("stickersPickerView");
      this.fix10margin = (Border) this.FindName("fix10margin");
    }
  }
}
