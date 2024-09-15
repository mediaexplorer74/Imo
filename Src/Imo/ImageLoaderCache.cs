// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.ImageLoaderCache
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Helpers;
using NLog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Windows.Storage;


namespace ImoSilverlightApp
{
  internal class ImageLoaderCache
  {
    private static readonly Logger log = LogManager.GetLogger(typeof (ImageLoaderCache).Name);
    private Dictionary<string, WeakReference> cacheBitmaps;

    public ImageLoaderCache() => this.cacheBitmaps = new Dictionary<string, WeakReference>();

    public async Task Add(string path, byte[] bytes)
    {
      try
      {
        if (await this.Has(path))
          return;
        await FSUtils.WriteFileSafeAsync(FSUtils.GetImageFilePath(path), bytes);
      }
      catch (Exception ex)
      {
        ImageLoaderCache.log.Error(ex, "Error adding bitmap to cache", 41, nameof (Add));
      }
    }

    public async Task<bool> Has(string path) => await FSUtils.IsUserImageOnDisk(path);

    public async Task<BitmapImage> LoadBitmap(string path)
    {
      if (this.cacheBitmaps.ContainsKey(path))
      {
        BitmapImage target = (BitmapImage) this.cacheBitmaps[path].Target;
        if (target != null)
          return target;
      }
      byte[] bytes = await this.LoadBytes(path);
      try
      {
        if (bytes == null)
          return (BitmapImage) null;
        BitmapImage bitmapFromBytes = await ImageUtils.GetBitmapFromBytes(bytes);
        this.cacheBitmaps[path] = new WeakReference((object) bitmapFromBytes);
        return bitmapFromBytes;
      }
      catch (Exception ex)
      {
        ImageLoaderCache.log.Error(ex, "Error decoding image", 79, nameof (LoadBitmap));
        await this.DeleteFromDisk(path);
        return (BitmapImage) null;
      }
      BitmapImage bitmapImage;
      return bitmapImage;
    }

    public async Task<byte[]> LoadBytes(string path) => await this.LoadFromDisk(path);

    private async Task<byte[]> LoadFromDisk(string path, bool isRetry = false)
    {
      try
      {
        if (await FSUtils.IsUserImageOnDisk(path))
          return await FSUtils.ReadFileFromCacheAsync(FSUtils.GetImageFilePath(path));
        ImageLoaderCache.log.Error("Tried loading bytes that weren't on disk", 101, nameof (LoadFromDisk));
        return (byte[]) null;
      }
      catch (Exception ex)
      {
        if (isRetry)
        {
          ImageLoaderCache.log.Error(ex, "Error loading image from disk after retry", 109, nameof (LoadFromDisk));
          return (byte[]) null;
        }
        await Task.Delay(5000);
        return await this.LoadFromDisk(path, true);
      }
      byte[] numArray;
      return numArray;
    }

    private async Task DeleteFromDisk(string path)
    {
      try
      {
        await (await StorageFile.GetFileFromPathAsync(FSUtils.GetImageFilePath(path))).DeleteAsync();
      }
      catch (Exception ex)
      {
        ImageLoaderCache.log.Error(ex, "Error deleting cached image from disk", 131, nameof (DeleteFromDisk));
      }
    }
  }
}
