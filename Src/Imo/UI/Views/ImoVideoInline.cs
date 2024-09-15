// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.UI.Views.ImoVideoInline
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Helpers;
using ImoSilverlightApp.Storage.Models;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Streams;


namespace ImoSilverlightApp.UI.Views
{
  public class ImoVideoInline : UserControl
  {
    public static readonly DependencyProperty ObservedDataContextProperty = DependencyProperty.Register("ObservedDataContext", typeof (object), typeof (ImoVideoInline), new PropertyMetadata((object) null, (PropertyChangedCallback) ((d, e) => ((ImoVideoInline) d).OnDataContextChanged())));
    public static readonly DependencyProperty VideoUrlProperty = DependencyProperty.Register(nameof (VideoUrl), typeof (string), typeof (ImoVideoInline), new PropertyMetadata((object) string.Empty, new PropertyChangedCallback(ImoVideoInline.VideoUrlChangedCallback)));
    public static readonly DependencyProperty ThumbUrlProperty = DependencyProperty.Register(nameof (ThumbUrl), typeof (string), typeof (ImoVideoInline), new PropertyMetadata((object) string.Empty, new PropertyChangedCallback(ImoVideoInline.ThumbUrlChangedCallback)));
    public static readonly DependencyProperty IsPlayingProperty = DependencyProperty.Register(nameof (IsPlaying), typeof (bool), typeof (ImoVideoInline), new PropertyMetadata((object) false));
    public static readonly DependencyProperty messageProperty = DependencyProperty.Register(nameof (Message), typeof (string), typeof (ImoVideoInline), new PropertyMetadata((object) string.Empty));
    private VideoMessage videoMessage;
    private Uri uriSource;
    internal UserControl imoVideoInlineRoot;
    internal Image thumbnail;
    internal IconCircle playButton;
    private bool _contentLoaded;

    public bool IsPlaying
    {
      get => (bool) this.GetValue(ImoVideoInline.IsPlayingProperty);
      set => this.SetValue(ImoVideoInline.IsPlayingProperty, (object) value);
    }

    public string VideoUrl
    {
      get => this.GetValue(ImoVideoInline.VideoUrlProperty).ToString();
      set => this.SetValue(ImoVideoInline.VideoUrlProperty, (object) value);
    }

    public string ThumbUrl
    {
      get => this.GetValue(ImoVideoInline.ThumbUrlProperty).ToString();
      set => this.SetValue(ImoVideoInline.ThumbUrlProperty, (object) value);
    }

    public string Message
    {
      get => this.GetValue(ImoVideoInline.messageProperty).ToString();
      set => this.SetValue(ImoVideoInline.messageProperty, (object) value);
    }

    public ImoVideoInline()
    {
      this.InitializeComponent();
      this.Loaded += new RoutedEventHandler(this.ImoVideoInline_Loaded);
      this.Unloaded += new RoutedEventHandler(this.ImoVideoInline_Unloaded);
    }

    private void OnDataContextChanged()
    {
      if (this.videoMessage != null)
        this.videoMessage.PropertyChanged -= new PropertyChangedEventHandler(this.VideoMessage_PropertyChanged);
      this.videoMessage = this.DataContext as VideoMessage;
      if (this.videoMessage != null)
        this.videoMessage.PropertyChanged += new PropertyChangedEventHandler(this.VideoMessage_PropertyChanged);
      this.SyncVideoProperties();
    }

    private async void SetThumbnailFromLocalFile()
    {
      try
      {
        StorageFile fileFromPathAsync = await StorageFile.GetFileFromPathAsync(this.videoMessage.LocalPath);
        if (fileFromPathAsync == null)
          return;
        BitmapImage image = new BitmapImage();
        StorageItemThumbnail thumbnailAsync = await fileFromPathAsync.GetThumbnailAsync((ThumbnailMode) 1);
        image.SetSource(((IRandomAccessStream) thumbnailAsync).AsStream());
        this.thumbnail.Source = (ImageSource) image;
        image = (BitmapImage) null;
      }
      catch (Exception ex)
      {
      }
    }

    private void ImoVideoInline_Unloaded(object sender, RoutedEventArgs e)
    {
      if (this.videoMessage == null)
        return;
      this.videoMessage.PropertyChanged -= new PropertyChangedEventHandler(this.VideoMessage_PropertyChanged);
    }

    private void ImoVideoInline_Loaded(object sender, RoutedEventArgs e)
    {
      this.videoMessage = this.DataContext as VideoMessage;
      if (this.videoMessage == null)
        return;
      this.videoMessage.PropertyChanged -= new PropertyChangedEventHandler(this.VideoMessage_PropertyChanged);
      this.videoMessage.PropertyChanged += new PropertyChangedEventHandler(this.VideoMessage_PropertyChanged);
      this.SyncVideoProperties();
    }

    private void SyncVideoProperties()
    {
      if (this.videoMessage == null)
        return;
      this.Width = 280.0;
      this.Height = 200.0;
      this.VideoUrl = this.videoMessage.VideoUrl;
      this.ThumbUrl = this.videoMessage.ThumbnailUrl;
    }

    private void VideoMessage_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      this.SyncVideoProperties();
    }

    private static async void ThumbUrlChangedCallback(
      DependencyObject dependencyObject,
      DependencyPropertyChangedEventArgs args)
    {
      ImoVideoInline imoVideo = (ImoVideoInline) dependencyObject;
      if (args.NewValue != null)
      {
        BitmapImage bitmapImage = await IMO.ImageLoader.LoadImage((string) args.NewValue);
        imoVideo.thumbnail.Source = (ImageSource) bitmapImage;
      }
      else
        imoVideo.thumbnail.Source = (ImageSource) null;
    }

    private static void VideoUrlChangedCallback(
      DependencyObject dependencyObject,
      DependencyPropertyChangedEventArgs args)
    {
      ImoVideoInline imoVideo = (ImoVideoInline) dependencyObject;
      if (args.NewValue != null)
      {
        Action<string, string> completedCallback = (Action<string, string>) ((failReason, localPath) =>
        {
          if (failReason == null)
          {
            imoVideo.videoMessage.ProgressPercent = -1;
            imoVideo.uriSource = new Uri(localPath, UriKind.Absolute);
          }
          else
            imoVideo.Message = "Error loading video";
        });
        Action<int> progressCallback = (Action<int>) (progress => imoVideo.videoMessage.ProgressPercent = progress);
        IMO.VideoLoader.LoadVideo(imoVideo.VideoUrl, completedCallback, progressCallback);
      }
      else
        imoVideo.uriSource = (Uri) null;
    }

    private void Video_Tapped(object sender, GestureEventArgs e)
    {
      if (!(this.uriSource != (Uri) null))
        return;
      IMO.NavigationManager.NavigateToVideoPlayerPage(this.uriSource.AbsolutePath);
    }

    private void Share_Click(object sender, RoutedEventArgs e)
    {
      if (this.videoMessage == null)
        return;
      IMO.NavigationManager.NavigateToShareObjectToMembersPage(this.videoMessage.VideoID, "video");
    }

    private async void Download_Click(object sender, RoutedEventArgs e)
    {
      if (!(this.uriSource != (Uri) null))
        return;
      string localPath = this.uriSource.LocalPath;
      if (!File.Exists(localPath))
        return;
      StorageFile fileFromPathAsync = await StorageFile.GetFileFromPathAsync(localPath);
      int num = 0;
      object obj;
      try
      {
        StorageFile storageFile = await fileFromPathAsync.CopyAsync((IStorageFolder) KnownFolders.SavedPictures);
      }
      catch (Exception ex)
      {
        obj = (object) ex;
        num = 1;
      }
      if (num == 1)
      {
        ImoMessageBoxResult messageBoxResult1 = await ImoMessageBox.Show("Saving video failed!");
      }
      else
      {
        obj = (object) null;
        IMO.MonitorLog.Log("media_messages", "save_video");
        ImoMessageBoxResult messageBoxResult2 = await ImoMessageBox.Show("Video saved successfully!");
      }
    }

    private void Delete_Click(object sender, RoutedEventArgs e)
    {
      if (this.videoMessage == null)
        return;
      IMO.IM.DeleteMessage((ImoSilverlightApp.Storage.Models.Message) this.videoMessage);
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/ImoSilverlightApp;component/UI/Views/ImoVideoInline.xaml", UriKind.Relative));
      this.imoVideoInlineRoot = (UserControl) this.FindName("imoVideoInlineRoot");
      this.thumbnail = (Image) this.FindName("thumbnail");
      this.playButton = (IconCircle) this.FindName("playButton");
    }
  }
}
