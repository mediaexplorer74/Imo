// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.UI.Views.ImageViewerPage
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Navigation;


namespace ImoSilverlightApp.UI.Views
{
  public class ImageViewerPage : ImoPage
  {
    internal ImageViewerControl imageViewer;
    private bool _contentLoaded;

    public ImageViewerPage() => this.InitializeComponent();

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
      string source = "";
      string s1 = "";
      string s2 = "";
      string buid = "";
      string objectId = "";
      if (!this.NavigationContext.QueryString.TryGetValue("url", out source) || !this.NavigationContext.QueryString.TryGetValue("width", out s1) || !this.NavigationContext.QueryString.TryGetValue("height", out s2) || !this.NavigationContext.QueryString.TryGetValue("buid", out buid) || !this.NavigationContext.QueryString.TryGetValue("objectId", out objectId))
        return;
      this.imageViewer.ViewPhoto(source, int.Parse(s1), int.Parse(s2), buid, objectId);
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/ImoSilverlightApp;component/UI/Views/ImageViewerPage.xaml", UriKind.Relative));
      this.imageViewer = (ImageViewerControl) this.FindName("imageViewer");
    }
  }
}
