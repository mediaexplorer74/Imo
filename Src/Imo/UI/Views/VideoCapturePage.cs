// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.UI.Views.VideoCapturePage
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Helpers;
using Microsoft.Devices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using Windows.Devices.Enumeration;
using Windows.Graphics.Display;
using Windows.Media;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Phone.Media.Capture;
using Windows.Storage;


namespace ImoSilverlightApp.UI.Views
{
  public class VideoCapturePage : ImoPage
  {
    private const int MAX_VIDEO_DURATION = 15000;
    private const int MIN_SPACE = 104857600;
    private MediaCapture mediaCapture;
    private StorageFile storageFile;
    private int recordId;
    private bool isNavigatedTo;
    private string receiver;
    private bool isBackCamera;
    public static readonly DependencyProperty isRecordingProperty = DependencyProperty.Register(nameof (IsRecording), typeof (bool), typeof (VideoCapturePage), new PropertyMetadata((object) false, new PropertyChangedCallback(VideoCapturePage.IsRecording_Changed)));
    private bool isFlipping;
    internal ImoPage videoCapturePageRoot;
    internal Storyboard progressStoryboard;
    internal DoubleAnimation progressAnimation;
    internal ScaleTransform capturePreviewScaleTransform;
    internal Canvas viewfinderCanvas;
    internal VideoBrush viewfinderBrush;
    internal TextBlock titleText;
    internal CompositeTransform progressTransform;
    private bool _contentLoaded;

    public VideoCapturePage()
    {
      this.InitializeComponent();
      this.Loaded += new RoutedEventHandler(this.VideoCapturePage_Loaded);
    }

    private void VideoCapturePage_Loaded(object sender, RoutedEventArgs e)
    {
      this.UpdateHorizontalFlip();
    }

    private static void IsRecording_Changed(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      VideoCapturePage videoCapturePage = (VideoCapturePage) d;
      if (videoCapturePage.IsRecording)
        videoCapturePage.progressStoryboard.Begin();
      else
        videoCapturePage.progressStoryboard.Stop();
    }

    public bool IsRecording
    {
      get => (bool) this.GetValue(VideoCapturePage.isRecordingProperty);
      set => this.SetValue(VideoCapturePage.isRecordingProperty, (object) value);
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
      if (IsolatedStorageFile.GetUserStoreForApplication().AvailableFreeSpace < 104857600L)
      {
        Utils.ShowPopup("Not enough space on disk to capture video!");
        IMO.NavigationManager.NavigateBackOrExit();
      }
      else
      {
        this.isNavigatedTo = true;
        this.receiver = (string) null;
        this.NavigationContext.QueryString.TryGetValue("receiver", out this.receiver);
        this.titleText.Text = this.receiver == null || !this.receiver.StartsWith("buid:") ? (string) null : "Send to " + IMO.ContactsManager.GetOrCreateContact(this.receiver.Substring("buid:".Length)).AliasShort;
        IMO.ApplicationSettings.PropertyChanged += new PropertyChangedEventHandler(this.ApplicationSettings_PropertyChanged);
        this.StartRecording();
      }
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
      this.isNavigatedTo = false;
      this.StopRecording();
      IMO.ApplicationSettings.PropertyChanged -= new PropertyChangedEventHandler(this.ApplicationSettings_PropertyChanged);
    }

    private void UpdateHorizontalFlip()
    {
      double actualHeight = Application.Current.Host.Content.ActualHeight;
      double actualWidth = Application.Current.Host.Content.ActualWidth;
      double num = actualHeight * actualHeight / (actualWidth * actualWidth);
      this.capturePreviewScaleTransform.CenterX = this.ActualWidth / 2.0;
      if (IMO.ApplicationSettings.IsFrontCameraCapture)
        this.capturePreviewScaleTransform.ScaleX = -1.6;
      else
        this.capturePreviewScaleTransform.ScaleX = num;
    }

    private void ApplicationSettings_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      if (!(e.PropertyName == "IsFrontCameraCapture"))
        return;
      this.UpdateHorizontalFlip();
    }

    private void SetupCaptureOrientation(DisplayOrientations orientation)
    {
      this.mediaCapture.SetPreviewRotation(this.VideoRotationLookup(orientation, IMO.ApplicationSettings.IsFrontCameraCapture));
      this.mediaCapture.SetRecordRotation(this.VideoRotationLookup(orientation, IMO.ApplicationSettings.IsFrontCameraCapture));
    }

    private async Task StopRecording()
    {
      if (this.mediaCapture != null)
      {
        if (this.IsRecording)
        {
          this.IsRecording = false;
          await this.mediaCapture.StopRecordAsync();
        }
        WindowsRuntimeMarshal.RemoveEventHandler<RecordLimitationExceededEventHandler>(new Action<EventRegistrationToken>(this.mediaCapture.remove_RecordLimitationExceeded), new RecordLimitationExceededEventHandler(this.MediaCapture_RecordLimitationExceeded));
        await this.mediaCapture.StopPreviewAsync();
        this.mediaCapture.Dispose();
        this.mediaCapture = (MediaCapture) null;
      }
      this.IsRecording = false;
    }

    private async Task<DeviceInformation> GetCameraID(Panel desired)
    {
      return ((IEnumerable<DeviceInformation>) await DeviceInformation.FindAllAsync((DeviceClass) 4)).FirstOrDefault<DeviceInformation>((Func<DeviceInformation, bool>) (x => x.EnclosureLocation != null && x.EnclosureLocation.Panel == desired));
    }

    private async Task StartRecording()
    {
      try
      {
        await this.StopRecording();
        DeviceInformation cameraId = await this.GetCameraID(IMO.ApplicationSettings.IsFrontCameraCapture ? (Panel) 1 : (Panel) 2);
        if (cameraId == null)
        {
          IMO.ApplicationSettings.IsFrontCameraCapture = false;
          cameraId = await this.GetCameraID((Panel) 2);
        }
        if (cameraId == null)
          return;
        this.mediaCapture = new MediaCapture();
        MediaCapture mediaCapture1 = this.mediaCapture;
        MediaCaptureInitializationSettings initializationSettings = new MediaCaptureInitializationSettings();
        initializationSettings.put_VideoDeviceId(cameraId.Id);
        await mediaCapture1.InitializeAsync(initializationSettings);
        MediaCapture mediaCapture2 = this.mediaCapture;
        WindowsRuntimeMarshal.AddEventHandler<RecordLimitationExceededEventHandler>(new Func<RecordLimitationExceededEventHandler, EventRegistrationToken>(mediaCapture2.add_RecordLimitationExceeded), new Action<EventRegistrationToken>(mediaCapture2.remove_RecordLimitationExceeded), new RecordLimitationExceededEventHandler(this.MediaCapture_RecordLimitationExceeded));
        await this.StartPreview();
        this.SetupCaptureOrientation((DisplayOrientations) 2);
        VideoCapturePage videoCapturePage = this;
        StorageFile storageFile = videoCapturePage.storageFile;
        StorageFile fileAsync = await ApplicationData.Current.LocalCacheFolder.CreateFileAsync("imoCameraCapture.mp4", (CreationCollisionOption) 1);
        videoCapturePage.storageFile = fileAsync;
        videoCapturePage = (VideoCapturePage) null;
        MediaEncodingProfile profile = MediaEncodingProfile.CreateMp4((VideoEncodingQuality) 7);
        if (this.IsRecording)
        {
          this.IsRecording = false;
          await this.mediaCapture.StopRecordAsync();
        }
        await this.mediaCapture.StartRecordToStorageFileAsync(profile, (IStorageFile) this.storageFile);
        this.IsRecording = true;
        ++this.recordId;
        int currentrecordId = this.recordId;
        Utils.DelayExecute(15000, (Action) (async () =>
        {
          if (!this.isNavigatedTo || currentrecordId != this.recordId)
            return;
          await this.SendRecordedVideo();
        }));
        profile = (MediaEncodingProfile) null;
      }
      catch (Exception ex)
      {
        if (this.mediaCapture != null)
          this.mediaCapture.Dispose();
        this.mediaCapture = (MediaCapture) null;
      }
    }

    private async Task StartPreview()
    {
      MediaCapturePreviewSink previewSink = new MediaCapturePreviewSink();
      List<string> supportedVideoFormats = new List<string>()
      {
        "nv12",
        "rgb32"
      };
      VideoEncodingProperties previewFormat = this.mediaCapture.VideoDeviceController.GetAvailableMediaStreamProperties((MediaStreamType) 0).OfType<VideoEncodingProperties>().Where<VideoEncodingProperties>((Func<VideoEncodingProperties, bool>) (p => p != null && !string.IsNullOrEmpty(p.Subtype) && supportedVideoFormats.Contains(p.Subtype.ToLower()))).ToList<VideoEncodingProperties>().FirstOrDefault<VideoEncodingProperties>();
      await this.mediaCapture.VideoDeviceController.SetMediaStreamPropertiesAsync((MediaStreamType) 0, (IMediaEncodingProperties) previewFormat);
      MediaCapture mediaCapture = this.mediaCapture;
      MediaEncodingProfile mediaEncodingProfile = new MediaEncodingProfile();
      mediaEncodingProfile.put_Video(previewFormat);
      MediaCapturePreviewSink capturePreviewSink = previewSink;
      await mediaCapture.StartPreviewToCustomSinkAsync(mediaEncodingProfile, (IMediaExtension) capturePreviewSink);
      this.viewfinderBrush.SetSource((object) previewSink);
    }

    private async void SendVideo_Tapped(object sender, GestureEventArgs e)
    {
      await this.SendRecordedVideo();
    }

    private async Task SendRecordedVideo()
    {
      if (this.IsRecording)
      {
        this.IsRecording = false;
        await this.mediaCapture.StopRecordAsync();
        if (this.recordId == -1 || this.recordId == this.recordId)
        {
          if (this.receiver != null && this.receiver.StartsWith("buid:"))
          {
            IMO.ConversationsManager.GetOrCreateConversation(this.receiver.Substring("buid:".Length)).SendVideo(this.storageFile.Path);
            IMO.NavigationManager.NavigateBackOrExit();
            return;
          }
          IMO.NavigationManager.NavigateToShareVideoToMembersPage(this.storageFile.Path);
          this.NavigationService.RemoveBackEntry();
          return;
        }
      }
      IMO.NavigationManager.NavigateBackOrExit();
    }

    private void MediaCapture_RecordLimitationExceeded(MediaCapture sender)
    {
      this.IsRecording = false;
    }

    private VideoRotation VideoRotationLookup(
      DisplayOrientations displayOrientation,
      bool counterclockwise)
    {
      switch (displayOrientation - 1)
      {
        case 0:
          return (VideoRotation) 0;
        case 1:
          return !counterclockwise ? (VideoRotation) 1 : (VideoRotation) 3;
        case 2:
          return (VideoRotation) 0;
        case 3:
          return (VideoRotation) 2;
        default:
          if (displayOrientation == 8)
            return !counterclockwise ? (VideoRotation) 3 : (VideoRotation) 1;
          goto case 2;
      }
    }

    private async void FlipCamera_Tapped(object sender, GestureEventArgs e)
    {
      if (this.isFlipping)
        return;
      this.isFlipping = true;
      await this.StopRecording();
      IMO.ApplicationSettings.IsFrontCameraCapture = !IMO.ApplicationSettings.IsFrontCameraCapture;
      await this.StartRecording();
      this.isFlipping = false;
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/ImoSilverlightApp;component/UI/Views/VideoCapturePage.xaml", UriKind.Relative));
      this.videoCapturePageRoot = (ImoPage) this.FindName("videoCapturePageRoot");
      this.progressStoryboard = (Storyboard) this.FindName("progressStoryboard");
      this.progressAnimation = (DoubleAnimation) this.FindName("progressAnimation");
      this.capturePreviewScaleTransform = (ScaleTransform) this.FindName("capturePreviewScaleTransform");
      this.viewfinderCanvas = (Canvas) this.FindName("viewfinderCanvas");
      this.viewfinderBrush = (VideoBrush) this.FindName("viewfinderBrush");
      this.titleText = (TextBlock) this.FindName("titleText");
      this.progressTransform = (CompositeTransform) this.FindName("progressTransform");
    }
  }
}
