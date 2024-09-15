// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.VideoLoader
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Helpers;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;


namespace ImoSilverlightApp
{
  public class VideoLoader
  {
    private static readonly Logger log = LogManager.GetLogger(typeof (VideoLoader).Name);
    public const string FAIL_REASON_DELETED = "deleted";
    public const string FAIL_REASON_NOT_FOUND = "not_found";
    private Dictionary<string, string> cache;
    private IDictionary<string, VideoLoader.QueuedItemData> downloadQueue;
    private static VideoLoader instance;
    private int currentyDownloadingCount;

    private VideoLoader()
    {
      this.cache = new Dictionary<string, string>();
      this.downloadQueue = (IDictionary<string, VideoLoader.QueuedItemData>) new Dictionary<string, VideoLoader.QueuedItemData>();
    }

    public static VideoLoader Instance
    {
      get
      {
        if (VideoLoader.instance == null)
          VideoLoader.instance = new VideoLoader();
        return VideoLoader.instance;
      }
    }

    public void LoadVideo(
      string path,
      Action<string, string> completedCallback,
      Action<int> progressCallback = null,
      bool useHttps = false,
      bool isPreload = false)
    {
      if (path.StartsWith("/"))
        path = path.Substring(1);
      if (completedCallback == null)
        throw new ArgumentException("callback is null");
      string path1 = path != null ? this.GetFilePath(path) : throw new ArgumentException("path is null");
      if (File.Exists(path1))
      {
        completedCallback((string) null, path1);
      }
      else
      {
        if (progressCallback != null)
          progressCallback(0);
        this.InternalLoadVideo(path, useHttps, false, completedCallback, progressCallback);
      }
    }

    public void PreloadVideo(string path, bool useHttps = false)
    {
      this.LoadVideo(path, (Action<string, string>) ((failReason, result) => { }), (Action<int>) (result => { }), useHttps, true);
    }

    private void InternalLoadVideo(
      string path,
      bool useHttps,
      bool isPreload,
      Action<string, string> callback,
      Action<int> progressCallback)
    {
      if (this.SetFromCache(path, callback))
        return;
      if (!this.downloadQueue.ContainsKey(path))
        this.DownloadItem(new VideoLoader.QueuedItemData(path, callback, progressCallback), useHttps, isPreload);
      else
        this.downloadQueue[path].AddCallback(callback, progressCallback);
    }

    private bool SetFromCache(string path, Action<string, string> callback)
    {
      string str;
      if (!this.cache.TryGetValue(path, out str))
        return false;
      callback((string) null, str);
      return true;
    }

    private void DownloadItem(VideoLoader.QueuedItemData itemData, bool useHttps, bool isPreload)
    {
      this.downloadQueue[itemData.Path] = itemData;
      Action<string, byte[]> callback = (Action<string, byte[]>) (async (failReason, bytes) =>
      {
        this.downloadQueue.Remove(itemData.Path);
        if (failReason != null)
        {
          VideoLoader.log.Info("Error downloading video: " + itemData.Path);
          itemData.TriggerCallbacks(failReason, (string) null);
        }
        else
        {
          string filePath = this.GetFilePath(itemData.Path);
          try
          {
            await FSUtils.WriteFileAsync(filePath, bytes);
          }
          catch (Exception ex)
          {
            VideoLoader.log.Error(ex, 181, nameof (DownloadItem));
            itemData.TriggerCallbacks("write_file_exception", (string) null);
            return;
          }
          this.cache[itemData.Path] = filePath;
          itemData.TriggerCallbacks((string) null, filePath);
          filePath = (string) null;
        }
      });
      Action<double> progressCallback = (Action<double>) (progress => itemData.TriggerProgressCallbacks((int) (progress * 100.0)));
      if (useHttps)
        IMO.HttpsDownloader.Download(itemData.Path, callback, progressCallback);
      else
        IMO.PixelDownloader.DownloadVideo(itemData.Path, isPreload, callback, progressCallback);
    }

    private string GetFilePath(string path)
    {
      string path2 = "imo" + IMO.User.Uid + "vid" + (object) path.GetHashCode() + ".mp4";
      return Path.Combine(FSUtils.GetTmpDir(), path2);
    }

    private class QueuedItemData
    {
      public IList<Action<string, string>> callbacks;
      public IList<Action<int>> progressCallbacks;
      public string Path;

      public QueuedItemData(
        string path,
        Action<string, string> completedCallback,
        Action<int> progressCallback)
      {
        this.callbacks = (IList<Action<string, string>>) new List<Action<string, string>>();
        this.progressCallbacks = (IList<Action<int>>) new List<Action<int>>();
        this.callbacks.Add(completedCallback);
        if (progressCallback != null)
          this.progressCallbacks.Add(progressCallback);
        this.Path = path;
      }

      public void AddCallback(Action<string, string> callback, Action<int> progressCallback = null)
      {
        this.callbacks.Add(callback);
        if (progressCallback == null)
          return;
        this.progressCallbacks.Add(progressCallback);
      }

      internal void TriggerCallbacks(string failReason, string filePath)
      {
        foreach (Action<string, string> callback in (IEnumerable<Action<string, string>>) this.callbacks)
          callback(failReason, filePath);
      }

      internal void TriggerProgressCallbacks(int progress)
      {
        foreach (Action<int> progressCallback in (IEnumerable<Action<int>>) this.progressCallbacks)
          progressCallback(progress);
      }
    }
  }
}
