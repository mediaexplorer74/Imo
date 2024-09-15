// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.UI.Views.AVCallPage
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.AV;
using MacawRT;
using NLog;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;


namespace ImoSilverlightApp.UI.Views
{
  public sealed class AVCallPage : ImoPage
  {
    private static readonly Logger log = LogManager.GetLogger(typeof (AVCallPage).Name);
    private DateTime lastUserActivity = DateTime.Now;
    private const int HIDE_PANELS_TIMEOUT = 4000;
    private bool arePanelsIn;
    private bool flippingCamera;
    private Direct3DInterop d3dInterop;
    internal MediaElement ringtoneElement;
    internal MediaElement callOutElement;
    internal DrawingSurface videoBuddySurface;
    internal Grid headerGrid;
    internal TextBlock buddyNameText;
    internal TextBlock callingText;
    internal Grid footerGrid;
    internal IconCircleButton buttonSpeaker;
    internal IconCircleButton buttonFlipCamera;
    internal IconCircleButton buttonReject;
    internal IconCircleButton buttonAnswer;
    private bool _contentLoaded;

    public AVCallPage() => this.InitializeComponent();

    private void VideoBuddySurface_Loaded(object sender, RoutedEventArgs e)
    {
      if (this.d3dInterop != null)
        return;
      this.d3dInterop = new Direct3DInterop();
      this.d3dInterop.WindowBounds = new Windows.Foundation.Size(this.videoBuddySurface.ActualWidth, this.videoBuddySurface.ActualHeight);
      this.d3dInterop.NativeResolution = new Windows.Foundation.Size(Math.Floor(this.videoBuddySurface.ActualWidth * (double) Application.Current.Host.Content.ScaleFactor / 100.0 + 0.5), Math.Floor(this.videoBuddySurface.ActualHeight * (double) Application.Current.Host.Content.ScaleFactor / 100.0 + 0.5));
      this.d3dInterop.RenderResolution = this.d3dInterop.NativeResolution;
      this.videoBuddySurface.SetContentProvider((object) this.d3dInterop.CreateContentProvider());
    }

    protected override void OnBackKeyPress(CancelEventArgs e)
    {
      IMO.NavigationManager.NavigateToHome();
      IMO.AVManager.IsCallOpened = false;
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
      base.OnNavigatedTo(e);
      if (IMO.AVManager.IsInCall)
      {
        this.DataContext = (object) (this.ViewModel = new AVCallPageViewModel((FrameworkElement) this, IMO.AVManager.CallController));
        IMO.AVManager.CallController.RingtoneElement = this.ringtoneElement;
        IMO.AVManager.CallController.CallOutElement = this.callOutElement;
      }
      else
      {
        if (e.NavigationMode != NavigationMode.Back)
          return;
        IMO.NavigationManager.NavigateToHome();
      }
    }

    private AVCallPageViewModel ViewModel { get; set; }

    public object CallState { get; private set; }

    private void buttonSpeaker_Click(object sender, EventArgs e)
    {
      if (IMO.AVManager.CallController == null || IMO.AVManager.CallController.CallHandler == null)
        return;
      IMO.AVManager.CallController.ToggleSpeaker();
    }

    private async void buttonFlipCamera_Click(object sender, EventArgs e)
    {
      if (IMO.AVManager.CallController == null || IMO.AVManager.CallController.CallHandler == null || this.flippingCamera)
        return;
      this.flippingCamera = true;
      await IMO.AVManager.CallController.CallHandler.ChangeCamera();
      this.flippingCamera = false;
    }

    private void buttonAnswer_Click(object sender, EventArgs e)
    {
      if (IMO.AVManager.CallController != null && IMO.AVManager.CallController.CallState == AVState.RECEIVING)
        IMO.AVManager.SelfAcceptCall();
      else
        IMO.NavigationManager.NavigateBackOrHome();
    }

    private void buttonReject_Click(object sender, EventArgs e)
    {
      if (IMO.AVManager.CallController != null)
      {
        if (IMO.AVManager.CallController.CallState == AVState.TALKING)
          IMO.AVManager.SelfEndCall();
        else if (IMO.AVManager.CallController.CallState == AVState.RECEIVING)
          IMO.AVManager.SelfRejectCall();
        else
          IMO.AVManager.SelfCancelCall();
      }
      else
        IMO.NavigationManager.NavigateBackOrHome();
    }

    private void Grid_Tap(object sender, GestureEventArgs e)
    {
      if (IMO.AVManager.CallController != null && IMO.AVManager.CallController.IsVideoCall && IMO.AVManager.CallController.IsTalking)
      {
        IMO.AVManager.CallController.ToggleFooter();
      }
      else
      {
        if (IMO.AVManager.CallController != null)
          return;
        IMO.NavigationManager.NavigateBackOrHome();
      }
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/ImoSilverlightApp;component/UI/Views/AVCallPage.xaml", UriKind.Relative));
      this.ringtoneElement = (MediaElement) this.FindName("ringtoneElement");
      this.callOutElement = (MediaElement) this.FindName("callOutElement");
      this.videoBuddySurface = (DrawingSurface) this.FindName("videoBuddySurface");
      this.headerGrid = (Grid) this.FindName("headerGrid");
      this.buddyNameText = (TextBlock) this.FindName("buddyNameText");
      this.callingText = (TextBlock) this.FindName("callingText");
      this.footerGrid = (Grid) this.FindName("footerGrid");
      this.buttonSpeaker = (IconCircleButton) this.FindName("buttonSpeaker");
      this.buttonFlipCamera = (IconCircleButton) this.FindName("buttonFlipCamera");
      this.buttonReject = (IconCircleButton) this.FindName("buttonReject");
      this.buttonAnswer = (IconCircleButton) this.FindName("buttonAnswer");
    }
  }
}
