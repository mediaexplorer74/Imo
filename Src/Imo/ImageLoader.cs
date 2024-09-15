// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.ImageLoader
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Helpers;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;


namespace ImoSilverlightApp
{
  public class ImageLoader
  {
    private static readonly Logger log = LogManager.GetLogger(typeof (ImageLoader).Name);
    private const string LOCAL_FILE_PREFIX = "file:///";
    private static int downloadIndex;
    private ImageLoaderCache cache;
    private IDictionary<string, ImageLoader.QueuedItemData> downloadQueue;
    private static ImageLoader instance;
    private int currentyDownloadingCount;

    private ImageLoader()
    {
      this.cache = new ImageLoaderCache();
      this.downloadQueue = (IDictionary<string, ImageLoader.QueuedItemData>) new Dictionary<string, ImageLoader.QueuedItemData>();
    }

    public static ImageLoader Instance
    {
      get
      {
        if (ImageLoader.instance == null)
          ImageLoader.instance = new ImageLoader();
        return ImageLoader.instance;
      }
    }

    public async Task<BitmapImage> LoadImage(string path, bool useHttps = false, bool isPreload = false)
    {
      ++ImageLoader.downloadIndex;
      if (path == null)
        throw new ArgumentException("path is null");
      if (path.StartsWith("file:///"))
        return await this.LoadImageFromDisk(path.Substring("file:///".Length));
      if (path.StartsWith("/"))
        path = path.Substring(1);
      return await this.InternalLoadImage(path, useHttps, isPreload);
    }

    public void PreloadImage(string path, bool useHttps = false)
    {
      this.LoadImage(path, useHttps, true);
    }

    private async Task<BitmapImage> LoadImageFromDisk(string path)
    {
      try
      {
        byte[] bytes = await FSUtils.ReadFileAsync(path);
        return bytes == null ? (BitmapImage) null : await ImageUtils.GetBitmapFromBytes(bytes);
      }
      catch (Exception ex)
      {
        ImageLoader.log.Error(ex, "Loading image from disk", 126, nameof (LoadImageFromDisk));
        return (BitmapImage) null;
      }
    }

    private async Task<BitmapImage> InternalLoadImage(string path, bool useHttps, bool isPreload)
    {
      if (await this.cache.Has(path))
        return !isPreload ? await this.cache.LoadBitmap(path) : (BitmapImage) null;
      if (!this.downloadQueue.ContainsKey(path) | useHttps)
      {
        TaskCompletionSource<BitmapImage> callback = new TaskCompletionSource<BitmapImage>();
        this.DownloadItem(new ImageLoader.QueuedItemData(path, ImageLoader.downloadIndex, isPreload, callback), useHttps, isPreload);
        return await callback.Task;
      }
      if (useHttps)
        return (BitmapImage) null;
      ImageLoader.QueuedItemData download = this.downloadQueue[path];
      TaskCompletionSource<BitmapImage> callback1 = new TaskCompletionSource<BitmapImage>();
      download.AddCallback(callback1);
      if (IMO.PixelDownloader.ElevatePhotoPriority(path, download.DownloadIndex, download.IsPreload, ImageLoader.downloadIndex, isPreload))
      {
        download.DownloadIndex = ImageLoader.downloadIndex;
        download.IsPreload = isPreload;
      }
      return await callback1.Task;
    }

    private void DownloadItem(ImageLoader.QueuedItemData itemData, bool useHttps, bool isPreload)
    {
      ImageLoader.log.Debug("Downloading: " + itemData.Path);
      this.downloadQueue[itemData.Path] = itemData;
      Action<string, byte[]> callback = (Action<string, byte[]>) (async (failReason, bytes) =>
      {
        this.downloadQueue.Remove(itemData.Path);
        if (bytes == null)
        {
          ImageLoader.log.Info("Error downloading image: " + itemData.Path);
        }
        else
        {
          await this.cache.Add(itemData.Path, bytes);
          BitmapImage bitmap = await this.cache.LoadBitmap(itemData.Path);
          itemData.TriggerCallbacks(bitmap);
        }
      });
      if (useHttps)
        IMO.HttpsDownloader.Download(itemData.Path, callback);
      else
        IMO.PixelDownloader.DownloadPhoto(itemData.Path, ImageLoader.downloadIndex, isPreload, callback);
    }

    public async Task SaveImageAs(string path, DateTime dateTime)
    {
      if (path == null)
        ImageLoader.log.Error("Attempted to open an image with null path", 203, nameof (SaveImageAs));
      else if (path.StartsWith("file:///"))
      {
        try
        {
          path = path.Substring("file:///".Length);
          await ImageUtils.SaveImageAs(await FSUtils.ReadFileAsync(path), dateTime);
        }
        catch (FileNotFoundException ex)
        {
          ImoMessageBoxResult messageBoxResult = await ImoMessageBox.Show(ex.Message);
        }
      }
      else if (!await this.cache.Has(path))
        ImageLoader.log.Error("Cannot open not loaded image", 225, nameof (SaveImageAs));
      else
        await ImageUtils.SaveImageAs(await this.cache.LoadBytes(path), dateTime);
    }

    private class QueuedItemData
    {
      public IList<TaskCompletionSource<BitmapImage>> callbacks;
      public string Path;
      public int DownloadIndex;
      public bool IsPreload;

      public QueuedItemData(
        string path,
        int downloadIndex,
        bool isPreload,
        TaskCompletionSource<BitmapImage> callback)
      {
        this.callbacks = (IList<TaskCompletionSource<BitmapImage>>) new List<TaskCompletionSource<BitmapImage>>();
        this.callbacks.Add(callback);
        this.Path = path;
        this.DownloadIndex = downloadIndex;
        this.IsPreload = isPreload;
      }

      public void AddCallback(TaskCompletionSource<BitmapImage> callback)
      {
        this.callbacks.Add(callback);
      }

      internal void TriggerCallbacks(BitmapImage bitmap)
      {
        foreach (TaskCompletionSource<BitmapImage> callback in (IEnumerable<TaskCompletionSource<BitmapImage>>) this.callbacks)
          callback.SetResult(bitmap);
      }
    }
  }
}
