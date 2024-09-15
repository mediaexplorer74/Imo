// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.UI.Views.DownloadAccountPage
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Helpers;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;


namespace ImoSilverlightApp.UI.Views
{
  public class DownloadAccountPage : ImoPage
  {
    private string url;
    internal Grid LayoutRoot;
    internal Grid ContentPanel;
    internal Button btnDownloadData;
    internal ProgressBar waitingBar;
    internal TextBlock statusTextBlock;
    internal RichTextBox linkRtb;
    internal Run linkTextRun;
    private bool _contentLoaded;

    public DownloadAccountPage() => this.InitializeComponent();

    private void downloadButton_Click(object sender, RoutedEventArgs e)
    {
      this.waitingBar.Visibility = Visibility.Visible;
      this.btnDownloadData.Visibility = Visibility.Collapsed;
      IMO.IM.GetAccountData((Action<JToken>) (res =>
      {
        this.waitingBar.Visibility = Visibility.Collapsed;
        if (!(res is JObject jobject2))
          this.btnDownloadData.Visibility = Visibility.Visible;
        else if (jobject2.Value<string>((object) "result") == "ok")
        {
          string str = jobject2.Value<string>((object) "url");
          this.url = str;
          this.statusTextBlock.Visibility = Visibility.Visible;
          this.statusTextBlock.Text = "Your link is ready";
          this.linkRtb.Visibility = Visibility.Visible;
          this.linkTextRun.Text = str;
        }
        else
        {
          string formattedTimespan = Utils.ToFormattedTimespan(jobject2.Value<long>((object) "wait_time"));
          this.statusTextBlock.Visibility = Visibility.Visible;
          this.statusTextBlock.Text = "Link will be ready in " + formattedTimespan;
        }
      }));
    }

    private void downloadLink_Click(object sender, RoutedEventArgs e)
    {
      if (string.IsNullOrEmpty(this.url))
        return;
      Utils.OpenUrl(this.url);
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/ImoSilverlightApp;component/UI/Views/DownloadAccountPage.xaml", UriKind.Relative));
      this.LayoutRoot = (Grid) this.FindName("LayoutRoot");
      this.ContentPanel = (Grid) this.FindName("ContentPanel");
      this.btnDownloadData = (Button) this.FindName("btnDownloadData");
      this.waitingBar = (ProgressBar) this.FindName("waitingBar");
      this.statusTextBlock = (TextBlock) this.FindName("statusTextBlock");
      this.linkRtb = (RichTextBox) this.FindName("linkRtb");
      this.linkTextRun = (Run) this.FindName("linkTextRun");
    }
  }
}
