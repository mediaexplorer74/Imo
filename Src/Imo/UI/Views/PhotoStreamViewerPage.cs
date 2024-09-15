// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.UI.Views.PhotoStreamViewerPage
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using Imo.Phone.Controls;
using ImoSilverlightApp.Helpers;
using ImoSilverlightApp.Storage.Models;
using NLog;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;


namespace ImoSilverlightApp.UI.Views
{
  public class PhotoStreamViewerPage : ImoPage
  {
    private static readonly Logger log = LogManager.GetLogger(typeof (PhotoStreamViewerPage).Name);
    internal ImoPage photoStreamViewerRoot;
    internal FlipView flipView;
    private bool _contentLoaded;

    public PhotoStreamViewerPage() => this.InitializeComponent();

    internal PhotoStreamViewerPageViewModel ViewModel { get; set; }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
      string buid = "";
      string s = "";
      if (!this.NavigationContext.QueryString.TryGetValue("buid", out buid) || !this.NavigationContext.QueryString.TryGetValue("index", out s))
        return;
      PhotoStream photoStream = IMO.PhotoStreamsManager.GetOrCreatePhotoStream(buid);
      this.DataContext = (object) (this.ViewModel = new PhotoStreamViewerPageViewModel((FrameworkElement) this, buid, photoStream));
      this.flipView.SelectedIndex = int.Parse(s);
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
      base.OnNavigatedFrom(e);
      this.ViewModel.Dispose();
    }

    private void Video_Tapped(object sender, GestureEventArgs e)
    {
      Photo viewModelOf = VisualUtils.GetViewModelOf<Photo>(sender);
      if (viewModelOf == null)
        return;
      IMO.NavigationManager.NavigateToVideoPlayerPage("videourl:" + viewModelOf.VideoUrl);
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/ImoSilverlightApp;component/UI/Views/PhotoStreamViewerPage.xaml", UriKind.Relative));
      this.photoStreamViewerRoot = (ImoPage) this.FindName("photoStreamViewerRoot");
      this.flipView = (FlipView) this.FindName("flipView");
    }
  }
}
