// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.UI.Views.ImoAudioInline
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Storage.Models;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;


namespace ImoSilverlightApp.UI.Views
{
  public class ImoAudioInline : UserControl
  {
    public static readonly DependencyProperty ObservedDataContextProperty = DependencyProperty.Register("ObservedDataContext", typeof (object), typeof (ImoVideoInline), new PropertyMetadata((object) null, (PropertyChangedCallback) ((d, e) => ((ImoAudioInline) d).OnDataContextChanged())));
    public static readonly DependencyProperty AudioUrlProperty = DependencyProperty.Register(nameof (AudioUrl), typeof (string), typeof (ImoAudioInline), new PropertyMetadata((object) string.Empty, new PropertyChangedCallback(ImoAudioInline.AudioUrlChangedCallback)));
    public static readonly DependencyProperty IsPlayingProperty = DependencyProperty.Register(nameof (IsPlaying), typeof (bool), typeof (ImoAudioInline), new PropertyMetadata((object) false));
    public static readonly DependencyProperty MessageProperty = DependencyProperty.Register(nameof (Message), typeof (string), typeof (ImoAudioInline), new PropertyMetadata((object) string.Empty));
    private AudioMessage audioMessage;
    private Uri uriSource;
    private DispatcherTimer timer;
    private static ImoAudioInline currentlyPlaying;
    internal UserControl imoVideoInlineRoot;
    internal MediaElement player;
    internal IconCircle playButton;
    internal ImoAudioAmpsElement ampsElement;
    private bool _contentLoaded;

    public bool IsPlaying
    {
      get => (bool) this.GetValue(ImoAudioInline.IsPlayingProperty);
      set => this.SetValue(ImoAudioInline.IsPlayingProperty, (object) value);
    }

    public string AudioUrl
    {
      get => this.GetValue(ImoAudioInline.AudioUrlProperty).ToString();
      set => this.SetValue(ImoAudioInline.AudioUrlProperty, (object) value);
    }

    public string Message
    {
      get => this.GetValue(ImoAudioInline.MessageProperty).ToString();
      private set => this.SetValue(ImoAudioInline.MessageProperty, (object) value);
    }

    public ImoAudioInline()
    {
      this.InitializeComponent();
      this.timer = new DispatcherTimer();
      this.timer.Interval = TimeSpan.FromMilliseconds(25.0);
      this.timer.Tick += (EventHandler) ((s, e) => this.ampsElement.CurrentPosMs = (int) this.player.Position.TotalMilliseconds);
      this.Loaded += new RoutedEventHandler(this.ImoAudioInline_Loaded);
      this.Unloaded += new RoutedEventHandler(this.ImoAudioInline_Unloaded);
    }

    private void OnDataContextChanged()
    {
      if (this.audioMessage != null)
        this.audioMessage.PropertyChanged -= new PropertyChangedEventHandler(this.AudioMessage_PropertyChanged);
      this.audioMessage = this.DataContext as AudioMessage;
      if (this.audioMessage != null)
        this.audioMessage.PropertyChanged += new PropertyChangedEventHandler(this.AudioMessage_PropertyChanged);
      this.SyncAudioProperties();
    }

    private void ImoAudioInline_Unloaded(object sender, RoutedEventArgs e)
    {
      if (this.audioMessage == null)
        return;
      this.StopPlayback();
      this.audioMessage.PropertyChanged -= new PropertyChangedEventHandler(this.AudioMessage_PropertyChanged);
    }

    private void ImoAudioInline_Loaded(object sender, RoutedEventArgs e)
    {
      this.audioMessage = this.DataContext as AudioMessage;
      if (this.audioMessage == null)
        return;
      this.audioMessage.PropertyChanged -= new PropertyChangedEventHandler(this.AudioMessage_PropertyChanged);
      this.audioMessage.PropertyChanged += new PropertyChangedEventHandler(this.AudioMessage_PropertyChanged);
      this.SyncAudioProperties();
    }

    private void SyncAudioProperties()
    {
      if (this.audioMessage == null)
        return;
      if (this.audioMessage.IsLocal)
        this.uriSource = new Uri(this.audioMessage.LocalPath, UriKind.Absolute);
      else
        this.AudioUrl = this.audioMessage.AudioUrl;
      this.ampsElement.Amps = this.audioMessage.Amps;
    }

    private void AudioMessage_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      this.SyncAudioProperties();
    }

    private void playButton_Click(object sender, GestureEventArgs e)
    {
      if (!(this.uriSource != (Uri) null))
        return;
      if (this.IsPlaying)
        this.StopPlayback();
      else
        this.StartPlayback();
    }

    private void StartPlayback()
    {
      if (ImoAudioInline.currentlyPlaying != null && ImoAudioInline.currentlyPlaying != this)
        ImoAudioInline.currentlyPlaying.StopPlayback();
      this.player.Source = (Uri) null;
      this.player.Source = this.uriSource;
      this.player.Play();
      ImoAudioInline.currentlyPlaying = this;
      this.ampsElement.CurrentPosMs = 0;
      this.timer.Start();
      this.IsPlaying = true;
    }

    private void StopPlayback()
    {
      if (!this.IsPlaying)
        return;
      this.timer.Stop();
      this.player.Stop();
      this.player.Source = (Uri) null;
      this.IsPlaying = false;
      ImoAudioInline.currentlyPlaying = (ImoAudioInline) null;
      this.ampsElement.CurrentPosMs = int.MaxValue;
    }

    private static void AudioUrlChangedCallback(
      DependencyObject dependencyObject,
      DependencyPropertyChangedEventArgs args)
    {
      ImoAudioInline audioInline = (ImoAudioInline) dependencyObject;
      if (!string.IsNullOrEmpty(args.NewValue as string))
      {
        Action<string, string> completedCallback = (Action<string, string>) ((failReason, localPath) =>
        {
          if (failReason == null)
          {
            audioInline.audioMessage.ProgressPercent = -1;
            audioInline.uriSource = new Uri(localPath, UriKind.Absolute);
          }
          else
            audioInline.Message = "Error loading message";
        });
        Action<int> progressCallback = (Action<int>) (progress => audioInline.audioMessage.ProgressPercent = progress);
        IMO.VideoLoader.LoadVideo(audioInline.AudioUrl, completedCallback, progressCallback);
      }
      else
        audioInline.uriSource = (Uri) null;
    }

    private void Delete_Click(object sender, RoutedEventArgs e)
    {
      if (!(this.DataContext is AudioMessage dataContext))
        return;
      IMO.IM.DeleteMessage((ImoSilverlightApp.Storage.Models.Message) dataContext);
    }

    private void player_MediaEnded(object sender, RoutedEventArgs e) => this.StopPlayback();

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/ImoSilverlightApp;component/UI/Views/ImoAudioInline.xaml", UriKind.Relative));
      this.imoVideoInlineRoot = (UserControl) this.FindName("imoVideoInlineRoot");
      this.player = (MediaElement) this.FindName("player");
      this.playButton = (IconCircle) this.FindName("playButton");
      this.ampsElement = (ImoAudioAmpsElement) this.FindName("ampsElement");
    }
  }
}
