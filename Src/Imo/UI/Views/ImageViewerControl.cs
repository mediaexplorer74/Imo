// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.UI.Views.ImageViewerControl
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Helpers;
using Microsoft.Phone.Controls;
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;


namespace ImoSilverlightApp.UI.Views
{
  public class ImageViewerControl : UserControl
  {
    private static readonly Logger log = LogManager.GetLogger(typeof (ImageViewerControl).Name);
    public static readonly DependencyProperty PhotoUrlProperty = DependencyProperty.Register(nameof (PhotoUrl), typeof (string), typeof (ImageViewerControl), new PropertyMetadata((object) string.Empty, new PropertyChangedCallback(ImageViewerControl.OnPhotoUrlChanged)));
    public static readonly DependencyProperty BuidProperty = DependencyProperty.Register(nameof (Buid), typeof (string), typeof (ImageViewerControl), new PropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty ObjectIdProperty = DependencyProperty.Register(nameof (ObjectId), typeof (string), typeof (ImageViewerControl), new PropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty ImageSizeProperty = DependencyProperty.Register(nameof (ImageSize), typeof (Size), typeof (ImageViewerControl), new PropertyMetadata((object) Size.Empty, new PropertyChangedCallback(ImageViewerControl.OnImageSizeChanged)));
    private const double MaxScale = 10.0;
    private double _scale = 1.0;
    private double _minScale;
    private double _coercedScale;
    private double _originalScale;
    private Size _viewportSize;
    private bool _pinching;
    private Point _screenMidpoint;
    private Point _relativeMidpoint;
    private BitmapImage _bitmap;
    private ScaleTransform xform = new ScaleTransform();
    private DateTime lastGridTap;
    private bool isSavingImage;
    internal UserControl imageViewerControlRoot;
    internal Grid LayoutRoot;
    internal Grid ContentPanel;
    internal ViewportControl viewport;
    internal Canvas canvas;
    internal ImoImage imoImage;
    internal Grid topBar;
    internal IconButton settingsButton;
    private bool _contentLoaded;

    private static void OnPhotoUrlChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      ImageViewerControl imageViewerControl = (ImageViewerControl) d;
      imageViewerControl.imoImage.Url = imageViewerControl.PhotoUrl;
    }

    private static void OnImageSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      ImageViewerControl imageViewerControl = (ImageViewerControl) d;
      imageViewerControl.viewport.Bounds = new Rect(new Point(0.0, 0.0), imageViewerControl.ImageSize);
    }

    public string PhotoUrl
    {
      get => (string) this.GetValue(ImageViewerControl.PhotoUrlProperty);
      set => this.SetValue(ImageViewerControl.PhotoUrlProperty, (object) value);
    }

    public string Buid
    {
      get => this.GetValue(ImageViewerControl.BuidProperty) as string;
      set => this.SetValue(ImageViewerControl.BuidProperty, (object) value);
    }

    public string ObjectId
    {
      get => this.GetValue(ImageViewerControl.ObjectIdProperty) as string;
      set => this.SetValue(ImageViewerControl.ObjectIdProperty, (object) value);
    }

    public Size ImageSize
    {
      get => (Size) this.GetValue(ImageViewerControl.ImageSizeProperty);
      set => this.SetValue(ImageViewerControl.ImageSizeProperty, (object) value);
    }

    public ImageViewerControl()
    {
      this.InitializeComponent();
      this.imoImage.RenderTransform = (Transform) this.xform;
      this.Loaded += (RoutedEventHandler) ((s, e) => this.topBar.Visibility = Visibility.Collapsed);
    }

    public void ViewPhoto(
      string source,
      int imageWidth,
      int imageHeight,
      string buid = "",
      string objectId = "")
    {
      this.ImageSize = new Size((double) imageWidth, (double) imageHeight);
      this.PhotoUrl = source;
      this.Buid = buid;
      this.ObjectId = objectId;
    }

    private void HideWindow() => IMO.NavigationManager.NavigateBackOrExit();

    private void viewport_ViewportChanged(object sender, ViewportChangedEventArgs e)
    {
      Size size;
      ref Size local = ref size;
      Rect viewport = this.viewport.Viewport;
      double width = viewport.Width;
      viewport = this.viewport.Viewport;
      double height = viewport.Height;
      local = new Size(width, height);
      if (!(size != this._viewportSize))
        return;
      this._viewportSize = size;
      this.CoerceScale(true);
      this.ResizeImage(false);
    }

    private void OnManipulationStarted(object sender, ManipulationStartedEventArgs e)
    {
      this._pinching = false;
      this._originalScale = this._scale;
    }

    private void OnManipulationDelta(object sender, ManipulationDeltaEventArgs e)
    {
      if (e.PinchManipulation != null)
      {
        e.Handled = true;
        if (!this._pinching)
        {
          this._pinching = true;
          Point center = e.PinchManipulation.Original.Center;
          this._relativeMidpoint = new Point(center.X / this.imoImage.ActualWidth, center.Y / this.imoImage.ActualHeight);
          this._screenMidpoint = this.imoImage.TransformToVisual((UIElement) this.viewport).Transform(center);
        }
        this._scale = this._originalScale * e.PinchManipulation.CumulativeScale;
        this.CoerceScale(false);
        this.ResizeImage(false);
      }
      else
      {
        if (!this._pinching)
          return;
        this._pinching = false;
        this._originalScale = this._scale = this._coercedScale;
      }
    }

    private void OnManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
    {
      this._pinching = false;
      this._scale = this._coercedScale;
    }

    private void OnImageOpened(object sender, EventArgs e)
    {
      this._bitmap = (BitmapImage) this.imoImage.image.Source;
      this._scale = 0.0;
      this.CoerceScale(true);
      this._scale = this._coercedScale;
      this.ResizeImage(true);
    }

    private void ResizeImage(bool center)
    {
      if (this._coercedScale == 0.0 || this._bitmap == null)
        return;
      double width = this.canvas.Width = Math.Round((double) this._bitmap.PixelWidth * this._coercedScale);
      double height = this.canvas.Height = Math.Round((double) this._bitmap.PixelHeight * this._coercedScale);
      this.xform.ScaleX = this.xform.ScaleY = this._coercedScale;
      this.viewport.Bounds = new Rect(0.0, 0.0, width, height);
      if (center)
      {
        this.viewport.SetViewportOrigin(new Point(Math.Round((width - this.viewport.ActualWidth) / 2.0), Math.Round((height - this.viewport.ActualHeight) / 2.0)));
      }
      else
      {
        Point point = new Point(width * this._relativeMidpoint.X, height * this._relativeMidpoint.Y);
        this.viewport.SetViewportOrigin(new Point(point.X - this._screenMidpoint.X, point.Y - this._screenMidpoint.Y));
      }
    }

    private void CoerceScale(bool recompute)
    {
      if (recompute && this._bitmap != null && this.viewport != null)
        this._minScale = Math.Min(this.viewport.ActualWidth / (double) this._bitmap.PixelWidth, this.viewport.ActualHeight / (double) this._bitmap.PixelHeight);
      this._coercedScale = Math.Min(10.0, Math.Max(this._scale, this._minScale));
    }

    private void LayoutRoot_DoubleTap(object sender, System.Windows.Input.GestureEventArgs e)
    {
      this.lastGridTap = DateTime.Now;
      TimeSpan timeSpan = TimeSpan.FromMilliseconds(10.0);
      Point position = e.GetPosition((UIElement) this.viewport);
      this._relativeMidpoint = new Point(position.X / this.imoImage.ActualWidth, position.Y / this.imoImage.ActualHeight);
      this._screenMidpoint = this.imoImage.TransformToVisual((UIElement) this.viewport).Transform(position);
      if (this._scale > this._minScale + double.Epsilon)
      {
        double step = Math.Pow(this._minScale / this._scale, 1.0 / 3.0);
        int stepsRemaining = 3;
        DispatcherTimer timer = new DispatcherTimer();
        timer.Interval = timeSpan;
        timer.Tick += (EventHandler) ((s, ea) =>
        {
          if (--stepsRemaining == 0)
          {
            this._scale = this._minScale;
            timer.Stop();
          }
          else
            this._scale *= step;
          this.CoerceScale(false);
          this.ResizeImage(false);
        });
        timer.Start();
      }
      else
      {
        double step = Math.Pow(3.0 * this._minScale / this._scale, 1.0 / 3.0);
        int stepsRemaining = 3;
        DispatcherTimer timer = new DispatcherTimer();
        timer.Interval = timeSpan;
        timer.Tick += (EventHandler) ((s, ea) =>
        {
          if (--stepsRemaining == 0)
            timer.Stop();
          this._scale *= step;
          this.CoerceScale(false);
          this.ResizeImage(false);
        });
        timer.Start();
      }
    }

    private void Grid_Tap(object sender, System.Windows.Input.GestureEventArgs e)
    {
      this.lastGridTap = DateTime.Now;
      Utils.DelayExecute(500, (Action) (() =>
      {
        if ((DateTime.Now - this.lastGridTap).TotalMilliseconds <= 499.0)
          return;
        this.topBar.Visibility = this.topBar.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
      }));
    }

    private void Settings_Click(object sender, EventArgs e) => this.ShowContextMenu();

    private void Grid_Hold(object sender, System.Windows.Input.GestureEventArgs e)
    {
      this.ShowContextMenu();
    }

    private void ShowContextMenu()
    {
      this.topBar.Visibility = Visibility.Visible;
      ContextMenu contextMenu = ContextMenuService.GetContextMenu((DependencyObject) this.settingsButton);
      foreach (object obj in (PresentationFrameworkCollection<object>) contextMenu.Items)
      {
        MenuItem menuItem = obj as MenuItem;
        if (menuItem.Tag is string && menuItem.Tag as string == "delete")
          menuItem.Visibility = string.IsNullOrEmpty(this.ObjectId) || string.IsNullOrEmpty(this.Buid) ? Visibility.Collapsed : Visibility.Visible;
      }
      contextMenu.IsOpen = true;
    }

    private async void Download_Click(object sender, RoutedEventArgs e)
    {
      if (this.isSavingImage)
        return;
      this.isSavingImage = true;
      try
      {
        BitmapImage bitmap = await IMO.ImageLoader.LoadImage(this.PhotoUrl);
        if (bitmap == null)
          return;
        try
        {
          await ImageUtils.SaveImageAs(bitmap);
          ImoMessageBoxResult messageBoxResult = await ImoMessageBox.Show("Image saved to library!");
        }
        catch (Exception ex)
        {
          ImageViewerControl.log.Error(ex, 352, nameof (Download_Click));
        }
        this.isSavingImage = false;
      }
      catch (Exception ex)
      {
        this.isSavingImage = false;
        ImageViewerControl.log.Error(ex, 360, nameof (Download_Click));
      }
    }

    private async void Delete_Click(object sender, RoutedEventArgs e)
    {
      if (string.IsNullOrEmpty(this.ObjectId) || string.IsNullOrEmpty(this.Buid))
        return;
      if (await ImoMessageBox.Show("Delete this photo?", ImoMessageBoxButton.OKCancel) != ImoMessageBoxResult.OK)
        return;
      IMO.Pixel.DeletePhoto(this.Buid, this.ObjectId, (Action<JToken>) (r =>
      {
        IMO.PhotoStreamsManager.GetOrCreatePhotoStream(this.Buid).InvalidateIsSynced();
        IMO.ConversationsManager.GetOrCreateConversation(this.Buid).RemovePhotoMessage(this.ObjectId);
        try
        {
          switch (((ContentControl) Application.Current.RootVisual).Content)
          {
            case PhotoStreamViewerPage _:
            case ImageViewerPage _:
              IMO.NavigationManager.NavigateBackOrExit();
              break;
          }
        }
        catch (Exception ex)
        {
          ImageViewerControl.log.Error(ex, 386, nameof (Delete_Click));
        }
      }));
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/ImoSilverlightApp;component/UI/Views/ImageViewerControl.xaml", UriKind.Relative));
      this.imageViewerControlRoot = (UserControl) this.FindName("imageViewerControlRoot");
      this.LayoutRoot = (Grid) this.FindName("LayoutRoot");
      this.ContentPanel = (Grid) this.FindName("ContentPanel");
      this.viewport = (ViewportControl) this.FindName("viewport");
      this.canvas = (Canvas) this.FindName("canvas");
      this.imoImage = (ImoImage) this.FindName("imoImage");
      this.topBar = (Grid) this.FindName("topBar");
      this.settingsButton = (IconButton) this.FindName("settingsButton");
    }
  }
}
