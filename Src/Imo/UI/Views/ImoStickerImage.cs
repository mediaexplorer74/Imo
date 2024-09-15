// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.UI.Views.ImoStickerImage
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Helpers;
using ImoSilverlightApp.Storage.Models;
using NLog;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;


namespace ImoSilverlightApp.UI.Views
{
  public class ImoStickerImage : UserControl
  {
    private static readonly Logger log = LogManager.GetLogger(typeof (ImoStickerImage).Name);
    private Storyboard storyboard;
    private double imageScale;
    private bool isLoadingImage;
    public static readonly DependencyProperty urlProperty = DependencyProperty.Register("Url", typeof (string), typeof (ImoStickerImage), new PropertyMetadata((object) string.Empty, new PropertyChangedCallback(ImoStickerImage.UrlChangedCallback)));
    public static readonly DependencyProperty stickerProperty = DependencyProperty.Register("Sticker", typeof (Sticker), typeof (ImoStickerImage), new PropertyMetadata((PropertyChangedCallback) null));
    private Storyboard marginStoryboard;
    private bool isLoaded;
    public static readonly DependencyProperty autoPlayProperty = DependencyProperty.Register(nameof (AutoPlay), typeof (bool), typeof (ImoStickerImage), new PropertyMetadata((object) false));
    public static readonly DependencyProperty isShowingBusyIndicatorProperty = DependencyProperty.Register(nameof (IsShowingBusyIndicator), typeof (bool), typeof (ImoStickerImage), new PropertyMetadata((object) false));
    internal UserControl imoStickerRoot;
    internal Canvas canvas;
    internal RectangleGeometry canvasClip;
    internal Image image;
    internal ScaleTransform imageScaleTransform;
    private bool _contentLoaded;

    public bool AutoPlay
    {
      get => (bool) this.GetValue(ImoStickerImage.autoPlayProperty);
      set => this.SetValue(ImoStickerImage.autoPlayProperty, (object) value);
    }

    public bool IsShowingBusyIndicator
    {
      get => (bool) this.GetValue(ImoStickerImage.isShowingBusyIndicatorProperty);
      set => this.SetValue(ImoStickerImage.isShowingBusyIndicatorProperty, (object) value);
    }

    public ImoStickerImage()
    {
      this.InitializeComponent();
      this.Loaded += new RoutedEventHandler(this.ImoStickerImage_Loaded);
      this.Unloaded += new RoutedEventHandler(this.ImoStickerImage_Unloaded);
    }

    private Sticker ViewModel => (Sticker) this.DataContext;

    private void ImoStickerImage_Loaded(object sender, RoutedEventArgs e)
    {
      this.isLoaded = true;
      Sticker viewModel = this.ViewModel;
      if (viewModel == null)
        return;
      this.imageScale = this.ActualHeight / (double) viewModel.Height;
      this.imageScaleTransform.ScaleX = this.imageScale;
      this.imageScaleTransform.ScaleY = this.imageScale;
      this.canvasClip.Rect = new Rect(0.0, 0.0, this.ActualWidth, this.ActualHeight);
      this.ReloadImage();
    }

    private void ImoStickerImage_Unloaded(object sender, RoutedEventArgs e)
    {
      this.isLoaded = false;
    }

    private static void UrlChangedCallback(
      DependencyObject dependencyObject,
      DependencyPropertyChangedEventArgs args)
    {
      ((ImoStickerImage) dependencyObject).ReloadImage();
    }

    private async void ReloadImage()
    {
      this.image.Source = (ImageSource) null;
      Sticker viewModel = this.ViewModel;
      if (!this.isLoaded || viewModel == null)
        return;
      Utils.DelayExecute(1000, (Action) (() =>
      {
        if (!this.isLoadingImage)
          return;
        this.IsShowingBusyIndicator = true;
      }));
      this.isLoadingImage = true;
      this.StickerDownloaded(await IMO.ImageLoader.LoadImage(viewModel.Url, true));
    }

    public void StickerDownloaded(BitmapImage result)
    {
      this.IsShowingBusyIndicator = false;
      this.isLoadingImage = false;
      if (result == null)
        return;
      this.image.Source = (ImageSource) result;
      Sticker viewModel = this.ViewModel;
      if (viewModel == null || !viewModel.Animated)
        return;
      int num1 = result.PixelHeight / viewModel.Height;
      int num2 = result.PixelWidth / viewModel.Width;
      if (this.storyboard != null)
      {
        this.storyboard.Stop();
        this.storyboard = (Storyboard) null;
      }
      this.storyboard = new Storyboard();
      this.storyboard.RepeatBehavior = RepeatBehavior.Forever;
      DoubleAnimationUsingKeyFrames element1 = new DoubleAnimationUsingKeyFrames();
      element1.BeginTime = new TimeSpan?(TimeSpan.FromSeconds(0.0));
      element1.Duration = (Duration) viewModel.GetDuration();
      DoubleAnimationUsingKeyFrames element2 = new DoubleAnimationUsingKeyFrames();
      element2.BeginTime = new TimeSpan?(TimeSpan.FromSeconds(0.0));
      element2.Duration = (Duration) viewModel.GetDuration();
      Storyboard.SetTarget((Timeline) element1, (DependencyObject) this.image);
      Storyboard.SetTargetProperty((Timeline) element1, new PropertyPath("(Canvas.Left)", new object[0]));
      Storyboard.SetTarget((Timeline) element2, (DependencyObject) this.image);
      Storyboard.SetTargetProperty((Timeline) element2, new PropertyPath("(Canvas.Top)", new object[0]));
      TimeSpan timeSpan = TimeSpan.Zero;
      for (int index1 = 0; index1 < num1; ++index1)
      {
        for (int index2 = 0; index2 < num2; ++index2)
        {
          int i = index1 * num2 + index2;
          if (i < viewModel.FramesCount)
          {
            DiscreteDoubleKeyFrame discreteDoubleKeyFrame1 = new DiscreteDoubleKeyFrame();
            discreteDoubleKeyFrame1.Value = (double) (-index2 * viewModel.Width) * this.imageScale;
            discreteDoubleKeyFrame1.KeyTime = (KeyTime) timeSpan;
            DiscreteDoubleKeyFrame discreteDoubleKeyFrame2 = discreteDoubleKeyFrame1;
            DiscreteDoubleKeyFrame discreteDoubleKeyFrame3 = new DiscreteDoubleKeyFrame();
            discreteDoubleKeyFrame3.Value = (double) (-index1 * viewModel.Height) * this.imageScale;
            discreteDoubleKeyFrame3.KeyTime = (KeyTime) timeSpan;
            DiscreteDoubleKeyFrame discreteDoubleKeyFrame4 = discreteDoubleKeyFrame3;
            element1.KeyFrames.Add((DoubleKeyFrame) discreteDoubleKeyFrame2);
            element2.KeyFrames.Add((DoubleKeyFrame) discreteDoubleKeyFrame4);
            timeSpan = timeSpan.Add(viewModel.GetFrameDuration(i));
          }
          else
            break;
        }
      }
      DoubleKeyFrameCollection keyFrames1 = element1.KeyFrames;
      DiscreteDoubleKeyFrame discreteDoubleKeyFrame5 = new DiscreteDoubleKeyFrame();
      discreteDoubleKeyFrame5.Value = 0.0;
      discreteDoubleKeyFrame5.KeyTime = (KeyTime) viewModel.GetDuration();
      keyFrames1.Add((DoubleKeyFrame) discreteDoubleKeyFrame5);
      this.storyboard.Children.Add((Timeline) element1);
      DoubleKeyFrameCollection keyFrames2 = element2.KeyFrames;
      DiscreteDoubleKeyFrame discreteDoubleKeyFrame6 = new DiscreteDoubleKeyFrame();
      discreteDoubleKeyFrame6.Value = 0.0;
      discreteDoubleKeyFrame6.KeyTime = (KeyTime) viewModel.GetDuration();
      keyFrames2.Add((DoubleKeyFrame) discreteDoubleKeyFrame6);
      this.storyboard.Children.Add((Timeline) element2);
      this.storyboard.Begin();
      if (this.AutoPlay)
        return;
      this.storyboard.Stop();
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/ImoSilverlightApp;component/UI/Views/ImoStickerImage.xaml", UriKind.Relative));
      this.imoStickerRoot = (UserControl) this.FindName("imoStickerRoot");
      this.canvas = (Canvas) this.FindName("canvas");
      this.canvasClip = (RectangleGeometry) this.FindName("canvasClip");
      this.image = (Image) this.FindName("image");
      this.imageScaleTransform = (ScaleTransform) this.FindName("imageScaleTransform");
    }
  }
}
