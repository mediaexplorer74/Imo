// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.UI.Views.GalleryPage
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Helpers;
using ImoSilverlightApp.Storage.Models;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;


namespace ImoSilverlightApp.UI.Views
{
  public class GalleryPage : ImoPage
  {
    private GalleryPageViewModel viewModel;
    public static readonly DependencyProperty ItemSizeProperty = DependencyProperty.Register(nameof (ItemSize), typeof (double), typeof (GalleryPage), new PropertyMetadata((object) 100.0));
    internal ImoPage rootGalleryPage;
    internal Grid LayoutRoot;
    private bool _contentLoaded;

    public double ItemSize
    {
      get => (double) this.GetValue(GalleryPage.ItemSizeProperty);
      set => this.SetValue(GalleryPage.ItemSizeProperty, (object) value);
    }

    public GalleryPage()
    {
      this.InitializeComponent();
      this.SizeChanged += new SizeChangedEventHandler(this.GalleryPage_SizeChanged);
    }

    private void GalleryPage_SizeChanged(object sender, SizeChangedEventArgs e)
    {
      this.ItemSize = (this.ActualWidth - 20.0) / 3.0;
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
      base.OnNavigatedTo(e);
      string buid = "";
      if (!this.NavigationContext.QueryString.TryGetValue("buid", out buid))
        return;
      this.DataContext = (object) (this.viewModel = new GalleryPageViewModel(buid, (FrameworkElement) this));
      IMO.PhotoStreamsManager.GetOrCreatePhotoStream(buid).SyncPhotos();
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
      base.OnNavigatedFrom(e);
      if (this.viewModel == null)
        return;
      this.viewModel.Dispose();
    }

    private void Photo_Tapped(object sender, GestureEventArgs e)
    {
      Photo viewModelOf = VisualUtils.GetViewModelOf<Photo>(sender);
      if (viewModelOf == null)
        return;
      if (viewModelOf.VideoUrl != null)
        IMO.NavigationManager.NavigateToVideoPlayerPage("videourl:" + viewModelOf.VideoUrl);
      else
        IMO.NavigationManager.NavigateToPhotoPreviewStream(this.viewModel.Buid, this.viewModel.PhotoStream.Photos.IndexOf(viewModelOf));
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/ImoSilverlightApp;component/UI/Views/GalleryPage.xaml", UriKind.Relative));
      this.rootGalleryPage = (ImoPage) this.FindName("rootGalleryPage");
      this.LayoutRoot = (Grid) this.FindName("LayoutRoot");
    }
  }
}
