// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.Managers.PixelDownloader
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;


namespace ImoSilverlightApp.Managers
{
  internal class PixelDownloader
  {
    private static readonly Logger log = LogManager.GetLogger(typeof (PixelDownloader).Name);
    private const int MAX_PARALLEL_DOWNLOADS = 10;
    private const int CHUNK_SIZE = 12000;
    private const int MAX_DOWNLOAD_SIZE = 104857600;
    private int currentlyDownloading;
    private IDictionary<string, PixelDownloader.QueuedItemData> queue;

    internal bool ElevatePhotoPriority(
      string path,
      int oldDownloadIndex,
      bool oldIsPreload,
      int newDownloadIndex,
      bool isPreload)
    {
      string sortedListKey1 = PixelDownloader.QueuedItemData.GetSortedListKey(this.GetDownloadPriority(PixelObjectType.Photo, oldIsPreload), path, oldDownloadIndex);
      PixelDownloader.QueuedItemData queuedItemData;
      this.queue.TryGetValue(sortedListKey1, out queuedItemData);
      if (queuedItemData != null)
      {
        PixelDownloadPriority downloadPriority = this.GetDownloadPriority(PixelObjectType.Photo, isPreload);
        string sortedListKey2 = PixelDownloader.QueuedItemData.GetSortedListKey(downloadPriority, path, newDownloadIndex);
        if (sortedListKey2.CompareTo(sortedListKey1) < 0)
        {
          this.queue.Remove(sortedListKey1);
          queuedItemData.DownloadIndex = newDownloadIndex;
          queuedItemData.Priority = downloadPriority;
          this.queue.Add(sortedListKey2, queuedItemData);
          return true;
        }
      }
      else
        PixelDownloader.log.Error("No item found to elevate", 316, nameof (ElevatePhotoPriority));
      return false;
    }

    public PixelDownloader()
    {
      this.queue = (IDictionary<string, PixelDownloader.QueuedItemData>) new SortedDictionary<string, PixelDownloader.QueuedItemData>();
      IMO.Dispatcher.Resetted += new EventHandler(this.Dispatcher_Resetted);
    }

    private void Dispatcher_Resetted(object sender, EventArgs e)
    {
      foreach (PixelDownloader.QueuedItemData queuedItemData in (IEnumerable<PixelDownloader.QueuedItemData>) this.queue.Values)
        queuedItemData.ResetDownloadingStates();
      this.currentlyDownloading = 0;
      for (int index = 0; index < 10; ++index)
        this.DownloadNext();
    }

    public void DownloadPhoto(
      string path,
      int downloadIndex,
      bool isPreload,
      Action<string, byte[]> callback,
      Action<double> progressCallback = null)
    {
      this.InternalDownload(path, PixelObjectType.Photo, downloadIndex, isPreload, callback, progressCallback);
    }

    public void DownloadVideo(
      string path,
      bool isPreload,
      Action<string, byte[]> callback,
      Action<double> progressCallback = null)
    {
      this.InternalDownload(path, PixelObjectType.Video, 0, isPreload, callback, progressCallback);
    }

    private PixelDownloadPriority GetDownloadPriority(PixelObjectType objectType, bool isPreload)
    {
      switch (objectType)
      {
        case PixelObjectType.Photo:
          return !isPreload ? PixelDownloadPriority.Photo : PixelDownloadPriority.PhotoPreload;
        case PixelObjectType.Video:
          return !isPreload ? PixelDownloadPriority.Video : PixelDownloadPriority.VideoPreload;
        default:
          return PixelDownloadPriority.Default;
      }
    }

    private void InternalDownload(
      string path,
      PixelObjectType objectType,
      int downloadIndex,
      bool isPreload,
      Action<string, byte[]> callback,
      Action<double> progressCallback = null)
    {
      PixelDownloader.QueuedItemData queuedItemData = new PixelDownloader.QueuedItemData(path, objectType, this.GetDownloadPriority(objectType, isPreload), downloadIndex, callback, progressCallback);
      if (string.IsNullOrEmpty(queuedItemData.ObjectId))
      {
        PixelDownloader.log.Error("ObjectId is null or empty: " + path, 372, nameof (InternalDownload));
        callback("client_null_object_id", (byte[]) null);
      }
      else
      {
        this.queue.Add(queuedItemData.GetSortedListKey(), queuedItemData);
        IMO.MonitorLog.Log(queuedItemData.GetLogNamespace(), "start");
        if (this.currentlyDownloading >= 10)
          return;
        this.DownloadNext();
      }
    }

    private bool DownloadNext()
    {
      PixelDownloader.QueuedItemData nextItemData = this.queue.Values.FirstOrDefault<PixelDownloader.QueuedItemData>((Func<PixelDownloader.QueuedItemData, bool>) (x => x.HasQueuedChunks()));
      if (nextItemData == null)
        return false;
      int nextQueuedChunk = nextItemData.GetNextQueuedChunk();
      JObject data = this.GetData(nextItemData, nextQueuedChunk);
      ++this.currentlyDownloading;
      if (this.currentlyDownloading > 10)
        throw new Exception("Attempted to init to many downloads");
      nextItemData.SetIsDownloading(nextQueuedChunk);
      IMO.Pixel.DownloadChunk(data, (Action<JToken>) (result =>
      {
        this.HandleDownloadedChunk(nextItemData, nextQueuedChunk, result);
        --this.currentlyDownloading;
        int currentlyDownloading = this.currentlyDownloading;
        while (currentlyDownloading < 10 && this.DownloadNext())
          ++currentlyDownloading;
      }));
      return true;
    }

    private void CancelItem(PixelDownloader.QueuedItemData itemData, string reason)
    {
      string sortedListKey = itemData.GetSortedListKey();
      if (!this.queue.ContainsKey(sortedListKey))
        return;
      this.queue.Remove(sortedListKey);
      IMO.MonitorLog.LogFail(itemData.GetLogNamespace(), reason);
      itemData.Callback(reason, (byte[]) null);
    }

    private void HandleDownloadedChunk(
      PixelDownloader.QueuedItemData itemData,
      int chunkIndex,
      JToken result)
    {
      if (result.HasValues)
      {
        if (result.Value<string>((object) nameof (result)) == "fail")
        {
          string reason = result.Value<string>((object) "reason");
          PixelDownloader.log.Warn((itemData.IsPhoto ? "Photo" : "Video") + " download failed: " + reason + ", " + itemData.Path);
          this.CancelItem(itemData, reason);
        }
        else
        {
          if (!(result.Value<string>((object) nameof (result)) == "success"))
            return;
          if (chunkIndex == 0 && !itemData.InitFromFirstChunk(result.Value<int>((object) "total")))
          {
            this.CancelItem(itemData, "too_big");
          }
          else
          {
            itemData.HandleDownloadedChunk(chunkIndex, result);
            if (!itemData.HasFinishedAllChunks)
              return;
            this.queue.Remove(itemData.GetSortedListKey());
            IMO.MonitorLog.Log(itemData.GetLogNamespace(), "success");
            itemData.Callback((string) null, itemData.Bytes);
          }
        }
      }
      else
      {
        PixelDownloader.log.Error("result has no values", 468, nameof (HandleDownloadedChunk));
        this.CancelItem(itemData, "handle_chunk_error");
      }
    }

    private JObject GetData(PixelDownloader.QueuedItemData itemData, int nextQueuedChunk)
    {
      JObject data = new JObject();
      data.Add("object_id", (JToken) itemData.ObjectId);
      data.Add("uid", (JToken) IMO.User.Uid);
      data.Add("transform", (JToken) itemData.Transform);
      int num = nextQueuedChunk * 12000;
      data.Add("ranges", (JToken) new JArray()
      {
        (JToken) num,
        (JToken) (num + 12000)
      });
      return data;
    }

    private static JObject GetTransform(string path)
    {
      JObject transform = new JObject();
      if (path.Contains("large"))
        transform.Add("size_type", (JToken) "large");
      else if (path.Contains("small"))
        transform.Add("size_type", (JToken) "small");
      else if (path.Contains("medium"))
        transform.Add("size_type", (JToken) "medium");
      else if (path.EndsWith("width=200&height=200&fit=1"))
      {
        transform.Add("width", (JToken) 200);
        transform.Add("height", (JToken) 200);
        transform.Add("fit", (JToken) 1);
      }
      if (path.Contains("thumbnail"))
        transform.Add("format", (JToken) "thumbnail");
      return transform;
    }

    private class QueuedItemData
    {
      private string objectId;
      private string path;
      private Action<string, byte[]> callback;
      private Action<double> progressCallback;
      private IList<PixelChunkState> chunkStates;
      private JObject transform;
      private int chunksCount;
      private int finishedChunksCount;
      private int finishedBytesCount;
      private int totalSize = -1;
      private bool finishedFirstChunk;
      private byte[] bytes;
      private PixelObjectType type;
      public PixelDownloadPriority Priority;
      public int DownloadIndex;

      public QueuedItemData(
        string path,
        PixelObjectType type,
        PixelDownloadPriority priority,
        int downloadIndex,
        Action<string, byte[]> callback,
        Action<double> progressCallback)
      {
        this.objectId = path.Split('/')[2];
        this.type = type;
        this.callback = callback;
        this.progressCallback = progressCallback;
        this.transform = PixelDownloader.GetTransform(path);
        this.path = path;
        this.Priority = priority;
        this.DownloadIndex = downloadIndex;
        this.chunkStates = (IList<PixelChunkState>) Enumerable.Repeat<PixelChunkState>(PixelChunkState.Queued, 1).ToList<PixelChunkState>();
      }

      public void HandleDownloadedChunk(int index, JToken result)
      {
        if (this.chunkStates[index] != PixelChunkState.Downloading)
          throw new Exception("Finished a chunk that wasn't downloading");
        byte[] sourceArray = Convert.FromBase64String(result.Value<string>((object) "base64"));
        Array.Copy((Array) sourceArray, 0, (Array) this.bytes, index * 12000, sourceArray.Length);
        this.finishedBytesCount += sourceArray.Length;
        ++this.finishedChunksCount;
        this.chunkStates[index] = PixelChunkState.Finished;
        if (this.progressCallback == null)
          return;
        if (this.finishedChunksCount == this.chunksCount)
          this.progressCallback(1.0);
        else
          this.progressCallback((double) this.finishedBytesCount / (double) this.totalSize);
      }

      public bool InitFromFirstChunk(int totalSize)
      {
        this.totalSize = totalSize;
        if (totalSize > 104857600)
        {
          PixelDownloader.log.Error("Attempted to download photo that is too big: " + (object) totalSize, 107, nameof (InitFromFirstChunk));
          return false;
        }
        this.totalSize = totalSize;
        this.chunksCount = (totalSize - 1) / 12000 + 1;
        this.finishedFirstChunk = true;
        this.chunkStates = (IList<PixelChunkState>) Enumerable.Repeat<PixelChunkState>(PixelChunkState.Queued, this.chunksCount).ToList<PixelChunkState>();
        this.chunkStates[0] = PixelChunkState.Downloading;
        this.bytes = new byte[totalSize];
        return true;
      }

      public string ObjectId => this.objectId;

      public string Path => this.path;

      public JObject Transform => this.transform;

      public Action<string, byte[]> Callback => this.callback;

      public int TotalSize => this.totalSize;

      public int ChunksCount => this.chunksCount;

      public byte[] Bytes => this.bytes;

      public PixelObjectType Type => this.type;

      public string GetLogNamespace()
      {
        return this.type != PixelObjectType.Photo ? "pixel_video_download" : "pixel_photo_download";
      }

      public bool IsPhoto => this.type == PixelObjectType.Photo;

      internal void ResetDownloadingStates()
      {
        for (int index = 0; index < this.chunkStates.Count; ++index)
        {
          if (this.chunkStates[index] == PixelChunkState.Downloading)
            this.chunkStates[index] = PixelChunkState.Queued;
        }
      }

      public bool HasQueuedChunks()
      {
        return !this.StartedDownloadingFirstChunk || this.chunkStates.Any<PixelChunkState>((Func<PixelChunkState, bool>) (x => x == PixelChunkState.Queued));
      }

      internal int GetNextQueuedChunk()
      {
        if (!this.finishedFirstChunk)
          return 0;
        for (int index = 1; index < this.chunksCount; ++index)
        {
          if (this.chunkStates[index] == PixelChunkState.Queued)
            return index;
        }
        throw new Exception("No queued chunks when getting next");
      }

      public bool StartedDownloadingFirstChunk => this.chunkStates[0] != 0;

      public bool HasFinishedAllChunks => this.finishedChunksCount == this.chunksCount;

      internal void SetIsDownloading(int nextQueuedChunk)
      {
        if (this.chunkStates[nextQueuedChunk] != PixelChunkState.Queued)
          throw new Exception("Cannot set to downloaded unqueued chunk");
        this.chunkStates[nextQueuedChunk] = PixelChunkState.Downloading;
      }

      internal string GetSortedListKey()
      {
        return PixelDownloader.QueuedItemData.GetSortedListKey(this.Priority, this.path, this.DownloadIndex);
      }

      public static string GetSortedListKey(
        PixelDownloadPriority priority,
        string path,
        int downloadIndex)
      {
        return PixelDownloader.QueuedItemData.GetPriorityString(priority, downloadIndex) + path;
      }

      private static string GetPriorityString(PixelDownloadPriority priority, int downloadIndex)
      {
        string str;
        switch (priority)
        {
          case PixelDownloadPriority.Photo:
            str = "0";
            break;
          case PixelDownloadPriority.Video:
            str = "1";
            break;
          case PixelDownloadPriority.PhotoPreload:
            str = "2";
            break;
          case PixelDownloadPriority.VideoPreload:
            str = "3";
            break;
          default:
            str = "9";
            break;
        }
        return str + Convert.ToString(1000000 - downloadIndex % 1000000).PadLeft(6, '0');
      }
    }
  }
}
