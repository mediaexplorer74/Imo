// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.Helpers.ImageUtils
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using Microsoft.Phone.Tasks;
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;


namespace ImoSilverlightApp.Helpers
{
  internal class ImageUtils
  {
    private const int TARGET_IMAGE_SIZE = 1024;
    private const int IMAGES_CACHE_MAX_SIZE = 52428800;
    private static readonly Logger log = LogManager.GetLogger(typeof (ImageUtils).Name);
    private static int currentId = 0;
    private static bool isShowingPhotoChooser = false;

    public static string GetPhotoThumbnailPath(string icon, PictureSize pictureSize)
    {
      string objectId = ImageUtils.GetObjectId(icon);
      return objectId == null ? (string) null : "/s/object/" + objectId + "/?width=200&height=200&fit=1";
    }

    internal static void EnsureImagesCacheSizeLimit()
    {
      try
      {
        DirectoryInfo dirInfo = new DirectoryInfo(FSUtils.GetImageCacheDir());
        long directorySize = FSUtils.GetDirectorySize(dirInfo);
        if (directorySize <= 52428800L)
          return;
        ImageUtils.log.Info("Images cache is exceeding limit. Reducing...");
        List<FileInfo> list = ((IEnumerable<FileInfo>) dirInfo.GetFiles()).OrderBy<FileInfo, DateTime>((Func<FileInfo, DateTime>) (x => x.LastAccessTime)).ToList<FileInfo>();
        long num1 = directorySize;
        foreach (FileInfo fileInfo in (IEnumerable<FileInfo>) list)
        {
          directorySize -= fileInfo.Length;
          fileInfo.Delete();
          if (directorySize <= 52428800L)
            break;
        }
        long num2 = directorySize;
        ImageUtils.log.Info("Images cache reduced from : " + (object) num1 + " to " + (object) num2);
        IMO.MonitorLog.Log("images_cache", new JObject()
        {
          {
            "cache_reduced",
            (JToken) 1
          },
          {
            "cache_reduced_from",
            (JToken) num1
          },
          {
            "cache_reduced_to",
            (JToken) num2
          }
        });
      }
      catch (Exception ex)
      {
        ImageUtils.log.Error(ex, "Ensuring images cache limit", 99, nameof (EnsureImagesCacheSizeLimit));
      }
    }

    internal static string GetPhotoUrlFromId(string icon, PictureSize pictureSize = PictureSize.Small)
    {
      string objectId = ImageUtils.GetObjectId(icon);
      return objectId == null ? (string) null : string.Format("s/object/{0}/?size_type={1}", (object) objectId, (object) pictureSize.ToString().ToLower());
    }

    public static string GetObjectId(string icon)
    {
      if (string.IsNullOrEmpty(icon))
        return (string) null;
      string str = icon;
      if (str.StartsWith("imo/"))
        str = str.Replace("imo/", "");
      else if (str.StartsWith("ngc/"))
        str = str.Replace("ngc/", "");
      return str.Length < 28 ? (string) null : str.Substring(0, 28);
    }

    internal static async Task<BitmapImage> GetBitmapFromBytes(byte[] bytes)
    {
      try
      {
        using (new MemoryStream(bytes))
        {
          BitmapImage bitmapImage = new BitmapImage();
          InMemoryRandomAccessStream stream = new InMemoryRandomAccessStream();
          int num = (int) await stream.WriteAsync(bytes.AsBuffer());
          stream.Seek(0UL);
          bitmapImage.SetSource(((IRandomAccessStream) stream).AsStream());
          return bitmapImage;
        }
      }
      catch (Exception ex)
      {
        ImageUtils.log.Error(ex, "Error", 154, nameof (GetBitmapFromBytes));
        return (BitmapImage) null;
      }
    }

    internal static string GetResizedImageBlocking(string path, bool crop = false)
    {
      Task<string> resizedImage = ImageUtils.GetResizedImage(path, crop);
      resizedImage.Wait();
      return resizedImage.Result;
    }

    private static string CreateDestinationPath()
    {
      string str = "IMG_R_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + "_" + (object) ImageUtils.currentId;
      ++ImageUtils.currentId;
      return new FileInfo(FSUtils.GetTmpDir() + "\\" + str + ".jpeg").FullName;
    }

    internal static async Task<string> GetResizedImage(string path, bool crop = false)
    {
      BitmapDecoder decoder = await BitmapDecoder.CreateAsync(await (await StorageFile.GetFileFromPathAsync(path)).OpenAsync((FileAccessMode) 0));
      string filePath = ImageUtils.CreateDestinationPath();
      IRandomAccessStream ras = await (await (await StorageFolder.GetFolderFromPathAsync(Path.GetDirectoryName(filePath))).CreateFileAsync(Path.GetFileName(filePath), (CreationCollisionOption) 3)).OpenAsync((FileAccessMode) 1);
      BitmapEncoder transcodingAsync = await BitmapEncoder.CreateForTranscodingAsync(ras, decoder);
      uint num1;
      uint num2;
      if (decoder.PixelWidth > decoder.PixelHeight)
      {
        float num3 = 1024f / (float) decoder.PixelWidth;
        num1 = 1024U;
        num2 = (uint) ((double) decoder.PixelHeight * (double) num3);
      }
      else
      {
        float num4 = 1024f / (float) decoder.PixelHeight;
        num2 = 1024U;
        num1 = (uint) ((double) decoder.PixelWidth * (double) num4);
      }
      transcodingAsync.BitmapTransform.put_ScaledHeight(num2);
      transcodingAsync.BitmapTransform.put_ScaledWidth(num1);
      if (crop)
      {
        uint num5 = num1;
        uint num6 = num2;
        uint num7;
        uint num8 = num7 = 0U;
        uint num9 = num5;
        uint num10 = num6;
        if ((int) num5 * 9 != 16 * (int) num6 && (int) num6 * 9 != 16 * (int) num5)
        {
          if (num5 > num6)
          {
            if (num5 * 9U > 16U * num6)
            {
              num9 = num6 * 16U / 9U;
              num8 = (num5 - num9) / 2U;
            }
            else if (num5 * 9U < 16U * num6)
            {
              num10 = num5 * 9U / 16U;
              num7 = (num6 - num10) / 2U;
            }
          }
          else if (num6 * 9U > 16U * num5)
          {
            num10 = num5 * 16U / 9U;
            num7 = (num6 - num10) / 2U;
          }
          else
          {
            num9 = num6 * 9U / 16U;
            num8 = (num5 - num9) / 2U;
          }
        }
        transcodingAsync.BitmapTransform.put_Bounds(new BitmapBounds()
        {
          Width = num9,
          Height = num10,
          X = num8,
          Y = num7
        });
      }
      try
      {
        await transcodingAsync.FlushAsync();
      }
      catch (Exception ex)
      {
        ImageUtils.log.Error(ex, "Error transcoding image", 270, nameof (GetResizedImage));
      }
      ((IDisposable) ras).Dispose();
      return filePath;
    }

    public static async Task SaveImageAs(byte[] bytes, DateTime dateTime)
    {
      await FSUtils.WriteFileAsync(Path.Combine(KnownFolders.SavedPictures.Path, ImageUtils.GetFileName(dateTime)), bytes, (CreationCollisionOption) 0);
    }

    public static async Task SaveImageAs(BitmapImage bitmap)
    {
      DateTime dateTime = DateTime.Now;
      WriteableBitmap bmp = new WriteableBitmap((BitmapSource) bitmap);
      using (Stream targetStream = await ((IStorageFile) await (await StorageFolder.GetFolderFromPathAsync(Path.GetDirectoryName(KnownFolders.SavedPictures.Path))).CreateFileAsync(ImageUtils.GetFileName(dateTime), (CreationCollisionOption) 0)).OpenStreamForWriteAsync())
        bmp.SaveJpeg(targetStream, bitmap.PixelWidth, bitmap.PixelHeight, 0, 100);
    }

    public static string GetFileName(DateTime dateTime)
    {
      return string.Format("Picture-" + dateTime.ToString("yyyy-MM-dd HH.mm.ss", (IFormatProvider) CultureInfo.InvariantCulture) + ".jpg");
    }

    public static void ChoosePhoto(Action<PhotoResult> callback)
    {
      try
      {
        if (ImageUtils.isShowingPhotoChooser)
          return;
        ImageUtils.isShowingPhotoChooser = true;
        PhotoChooserTask photoChooserTask = new PhotoChooserTask();
        photoChooserTask.ShowCamera = true;
        photoChooserTask.Completed += (EventHandler<PhotoResult>) ((s, e) =>
        {
          ImageUtils.isShowingPhotoChooser = false;
          callback(e);
        });
        photoChooserTask.Show();
      }
      catch (Exception ex)
      {
        ImageUtils.isShowingPhotoChooser = false;
        ImageUtils.log.Error(ex, 324, nameof (ChoosePhoto));
      }
    }
  }
}
