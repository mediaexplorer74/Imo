// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.UI.Views.VideoPlayerPage
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;


namespace ImoSilverlightApp.UI.Views
{
  public class VideoPlayerPage : ImoPage
  {
    private string uri;
    public static readonly DependencyProperty progressMessageProperty = DependencyProperty.Register(nameof (ProgressMessage), typeof (string), typeof (VideoPlayerPage), new PropertyMetadata((object) string.Empty));
    internal ImoPage videoPlayerPageRoot;
    internal Grid LayoutRoot;
    internal MediaElement player;
    private bool _contentLoaded;

    public VideoPlayerPage() => this.InitializeComponent();

    public string ProgressMessage
    {
      get => this.GetValue(VideoPlayerPage.progressMessageProperty).ToString();
      set => this.SetValue(VideoPlayerPage.progressMessageProperty, (object) value);
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
      this.ProgressMessage = (string) null;
      this.uri = (string) null;
      this.NavigationContext.QueryString.TryGetValue("filePath", out this.uri);
      string currentUri = this.uri;
      if (this.uri.StartsWith("videourl:"))
        IMO.VideoLoader.LoadVideo(this.uri.Substring("videourl:".Length), (Action<string, string>) ((failReason, localPath) =>
        {
          if (!(currentUri == this.uri))
            return;
          if (localPath != null)
          {
            this.ProgressMessage = (string) null;
            if (this.Visibility != Visibility.Visible)
              return;
            this.PlayVideo(new Uri(localPath, UriKind.Absolute), 0);
          }
          else
            this.ProgressMessage = "Error loading video";
        }), (Action<int>) (progress =>
        {
          if (!(currentUri == this.uri))
            return;
          this.ProgressMessage = "Downloading " + (object) progress + "%...";
        }));
      else
        this.PlayVideo(new Uri(this.uri, UriKind.Absolute), 0);
    }

    private void player_Tapped(object sender, GestureEventArgs e) => this.HideWindow();

    private void PlayVideo(Uri source, int orientation)
    {
      this.player.Source = source;
      this.player.Play();
    }

    private void Player_MediaEnded(object sender, RoutedEventArgs e) => this.HideWindow();

    private void HideWindow()
    {
      this.player.Stop();
      this.player.Source = (Uri) null;
      IMO.NavigationManager.NavigateBackOrExit();
    }

    private void player_MediaOpened(object sender, RoutedEventArgs e)
    {
      double actualHeight = this.LayoutRoot.ActualHeight;
      double actualWidth = this.LayoutRoot.ActualWidth;
      double naturalVideoHeight = (double) this.player.NaturalVideoHeight;
      double num = actualHeight / naturalVideoHeight * (double) this.player.NaturalVideoWidth;
      this.player.Margin = new Thickness((actualWidth - num) / 2.0, 0.0, 0.0, 0.0);
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/ImoSilverlightApp;component/UI/Views/VideoPlayerPage.xaml", UriKind.Relative));
      this.videoPlayerPageRoot = (ImoPage) this.FindName("videoPlayerPageRoot");
      this.LayoutRoot = (Grid) this.FindName("LayoutRoot");
      this.player = (MediaElement) this.FindName("player");
    }
  }
}
