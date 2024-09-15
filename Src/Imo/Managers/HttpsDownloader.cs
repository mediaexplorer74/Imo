// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.Managers.HttpsDownloader
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Helpers;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;


namespace ImoSilverlightApp.Managers
{
  internal class HttpsDownloader
  {
    private static readonly Logger log = LogManager.GetLogger(typeof (HttpsDownloader).Name);
    private const int MAX_PARALLEL_DOWNLOADS = 5;
    private int currentlyDownloading;
    private IList<HttpsDownloader.QueuedItemData> queue;

    public HttpsDownloader()
    {
      this.queue = (IList<HttpsDownloader.QueuedItemData>) new List<HttpsDownloader.QueuedItemData>();
    }

    public void Download(
      string path,
      Action<string, byte[]> callback,
      Action<double> progressCallback = null)
    {
      this.queue.Add(new HttpsDownloader.QueuedItemData(path, callback, progressCallback));
      IMO.MonitorLog.Log("https_download", "start");
      if (this.currentlyDownloading >= 5)
        return;
      this.DownloadNext();
    }

    private bool DownloadNext()
    {
      HttpsDownloader.QueuedItemData queuedItemData = this.queue.FirstOrDefault<HttpsDownloader.QueuedItemData>((Func<HttpsDownloader.QueuedItemData, bool>) (x => !x.IsDownloading));
      if (queuedItemData == null)
        return false;
      ++this.currentlyDownloading;
      if (this.currentlyDownloading > 5)
        throw new Exception("Attempted to init to many downloads");
      queuedItemData.IsDownloading = true;
      string warpyHost = IMO.Network.ConnectionData.WarpyHost;
      int warpyPort = IMO.Network.ConnectionData.WarpyPort;
      string str = queuedItemData.Path;
      if (str.StartsWith("/"))
        str = str.Substring(1);
      string requestUriString = string.Format("https://{0}:{1}/{2}", (object) warpyHost, (object) warpyPort, (object) str);
      HttpsDownloader.log.Info("Downloading image over https: " + requestUriString);
      WebRequest webRequest = WebRequest.Create(requestUriString);
      webRequest.Headers["User-Agent"] = UserAgentHelper.GetUA();
      this.RunDownloadTask(webRequest.GetResponseAsync(), queuedItemData);
      return true;
    }

    private void CancelItem(string reason, HttpsDownloader.QueuedItemData item)
    {
      item.Callback(reason, (byte[]) null);
      this.queue.Remove(item);
      --this.currentlyDownloading;
    }

    private async void RunDownloadTask(Task<WebResponse> task, HttpsDownloader.QueuedItemData item)
    {
      try
      {
        WebResponse webResponse = await task;
        byte[] numArray = (byte[]) null;
        using (Stream responseStream = webResponse.GetResponseStream())
          numArray = Utils.ReadStreamFully(responseStream, (int) webResponse.ContentLength);
        item.Callback((string) null, numArray);
        this.queue.Remove(item);
        --this.currentlyDownloading;
      }
      catch (WebException ex)
      {
        if (ex.Status != WebExceptionStatus.RequestCanceled)
        {
          HttpsDownloader.log.Error((Exception) ex, 136, nameof (RunDownloadTask));
          this.CancelItem("web_exception", item);
        }
      }
      int currentlyDownloading = this.currentlyDownloading;
      while (currentlyDownloading < 5 && this.DownloadNext())
        ++currentlyDownloading;
    }

    private class QueuedItemData
    {
      public string Path;
      public bool IsDownloading;
      private Action<string, byte[]> callback;
      private Action<double> progressCallback;

      public QueuedItemData(
        string path,
        Action<string, byte[]> callback,
        Action<double> progressCallback)
      {
        this.Path = path;
        this.callback = callback;
      }

      public Action<string, byte[]> Callback => this.callback;

      public Action<double> ProgressCallback => this.progressCallback;
    }
  }
}
