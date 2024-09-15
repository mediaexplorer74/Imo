// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.UI.Views.ImoImage
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Helpers;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace ImoSilverlightApp.UI.Views
{
  public class ImoImage : UserControl
  {
    private bool isLoading;
    public static readonly DependencyProperty UrlProperty = DependencyProperty.Register(nameof (Url), typeof (string), typeof (ImoImage), new PropertyMetadata((object) string.Empty, new PropertyChangedCallback(ImoImage.UrlChangedCallback)));
    public static readonly DependencyProperty enableBusyIndicatorProperty = DependencyProperty.Register(nameof (EnableBusyIndicator), typeof (bool), typeof (ImoImage), new PropertyMetadata((object) false));
    public static readonly DependencyProperty isShowingBusyIndicatorProperty = DependencyProperty.Register(nameof (IsShowingBusyIndicator), typeof (bool), typeof (ImoImage), new PropertyMetadata((object) false));
    public static readonly DependencyProperty stretchProperty = DependencyProperty.Register(nameof (Stretch), typeof (Stretch), typeof (ImoImage), new PropertyMetadata((object) Stretch.Uniform));
    internal UserControl imoImageRoot;
    internal Image image;
    internal Grid busyIndicator;
    private bool _contentLoaded;

    public event EventHandler ImageOpened;

    public ImoImage() => this.InitializeComponent();

    public string Url
    {
      get => this.GetValue(ImoImage.UrlProperty) as string;
      set => this.SetValue(ImoImage.UrlProperty, (object) value);
    }

    public bool EnableBusyIndicator
    {
      get => (bool) this.GetValue(ImoImage.enableBusyIndicatorProperty);
      set => this.SetValue(ImoImage.enableBusyIndicatorProperty, (object) value);
    }

    public bool IsShowingBusyIndicator
    {
      get => (bool) this.GetValue(ImoImage.isShowingBusyIndicatorProperty);
      set
      {
        this.busyIndicator.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
        this.SetValue(ImoImage.isShowingBusyIndicatorProperty, (object) value);
      }
    }

    public Stretch Stretch
    {
      get => (Stretch) this.GetValue(ImoImage.stretchProperty);
      set => this.SetValue(ImoImage.stretchProperty, (object) value);
    }

    private static void UrlChangedCallback(
      DependencyObject dependencyObject,
      DependencyPropertyChangedEventArgs args)
    {
      ((ImoImage) dependencyObject).ReloadImage();
    }

    private async void ReloadImage()
    {
      this.image.Source = (ImageSource) null;
      if (!string.IsNullOrEmpty(this.Url))
      {
        this.isLoading = true;
        if (this.EnableBusyIndicator)
          Utils.DelayExecute(1000, (Action) (() =>
          {
            if (!this.isLoading)
              return;
            this.IsShowingBusyIndicator = true;
          }));
        bool useHttps = this.Url.StartsWith("s/stickers/v1/packs/");
        string currentUrl = this.Url;
        BitmapImage bitmapImage = await IMO.ImageLoader.LoadImage(this.Url, useHttps);
        if (currentUrl == this.Url)
        {
          this.isLoading = false;
          this.IsShowingBusyIndicator = false;
          this.image.Source = (ImageSource) bitmapImage;
          Utils.BeginInvokeOnUI((Action) (() =>
          {
            EventHandler imageOpened = this.ImageOpened;
            if (imageOpened == null)
              return;
            imageOpened((object) this, EventArgs.Empty);
          }));
        }
        currentUrl = (string) null;
      }
      else
        this.image.Source = (ImageSource) null;
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/ImoSilverlightApp;component/UI/Views/ImoImage.xaml", UriKind.Relative));
      this.imoImageRoot = (UserControl) this.FindName("imoImageRoot");
      this.image = (Image) this.FindName("image");
      this.busyIndicator = (Grid) this.FindName("busyIndicator");
    }
  }
}
