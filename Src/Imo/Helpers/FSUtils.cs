// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.Helpers.FSUtils
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using Microsoft.Phone.Tasks;
using Microsoft.Xna.Framework.Media;
using NLog;
using System;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage;
using Windows.Storage.Streams;


namespace ImoSilverlightApp.Helpers
{
  internal static class FSUtils
  {
    private static readonly Logger log = LogManager.GetLogger(typeof (FSUtils).Name);

    internal static string GetOrCreateCacheDir()
    {
      FSUtils.EnsureDirectoryExists(ApplicationData.Current.LocalCacheFolder, "data");
      return Path.Combine(ApplicationData.Current.LocalCacheFolder.Path, "data");
    }

    internal static void EnsureDirectoryExists(StorageFolder parentDir, string directory)
    {
      parentDir.CreateFolderAsync(directory, (CreationCollisionOption) 3).AsTask<StorageFolder>().Wait();
    }

    internal static string GetOrCreateUserCacheDir()
    {
      string directory = Path.Combine(FSUtils.GetOrCreateCacheDir(), IMO.ApplicationSettings.CookieUid ?? throw new Exception("No user signed on"));
      FSUtils.EnsureDirectoryExists(directory);
      return directory;
    }

    internal static void EnsureDirectoryExists(string directory)
    {
      if (Directory.Exists(directory))
        return;
      try
      {
        Directory.CreateDirectory(directory);
      }
      catch (IOException ex)
      {
      }
    }

    internal static async Task<byte[]> ReadFileAsync(string filePath)
    {
      try
      {
        return (await FileIO.ReadBufferAsync((IStorageFile) await StorageFile.GetFileFromPathAsync(filePath))).ToArray();
      }
      catch (Exception ex)
      {
        FSUtils.log.Error(ex, 89, nameof (ReadFileAsync));
        return (byte[]) null;
      }
    }

    internal static async Task<byte[]> ReadFileFromCacheAsync(string filePathShort)
    {
      return await FSUtils.ReadFileAsync(Path.Combine(ApplicationData.Current.LocalCacheFolder.Path, filePathShort));
    }

    public static async Task WriteFileAsync(
      string filePath,
      byte[] bytes,
      CreationCollisionOption collisionOption = 3)
    {
      await FileIO.WriteBytesAsync((IStorageFile) await (await StorageFolder.GetFolderFromPathAsync(Path.GetDirectoryName(filePath))).CreateFileAsync(Path.GetFileName(filePath), collisionOption), bytes);
    }

    internal static async void ReadFileFromCache(string filePathShort, Action<byte[]> callback)
    {
      byte[] numArray = await FSUtils.ReadFileAsync(Path.Combine(ApplicationData.Current.LocalCacheFolder.Path, filePathShort));
      callback(numArray);
    }

    public static async Task WriteFileSafeAsync(string filePath, byte[] bytes)
    {
      int lastDirCharIndex = filePath.LastIndexOf(Path.DirectorySeparatorChar);
      StorageFolder storageFolder = ApplicationData.Current.LocalCacheFolder;
      string str1 = filePath;
      if (lastDirCharIndex != -1)
      {
        string str2 = filePath.Substring(0, lastDirCharIndex);
        storageFolder = await storageFolder.CreateFolderAsync(str2, (CreationCollisionOption) 3);
        str1 = filePath.Substring(lastDirCharIndex + 1);
      }
      StorageFile fileAsync = await storageFolder.CreateFileAsync(str1, (CreationCollisionOption) 1);
      try
      {
        using (Stream stream = await ((IStorageFile) fileAsync).OpenStreamForWriteAsync())
          stream.Write(bytes, 0, bytes.Length);
      }
      catch (IOException ex)
      {
        await Utils.ShowPopup("Your phone memory is full and IMO might not function properly! Please free up some space.");
      }
    }

    public static string GetImageCacheDir() => "images";

    internal static async Task<bool> IsUserImageOnDisk(string path)
    {
      StorageFolder localCacheFolder = ApplicationData.Current.LocalCacheFolder;
      try
      {
        StorageFile fileAsync = await localCacheFolder.GetFileAsync(FSUtils.GetImageFilePath(path));
      }
      catch
      {
        return false;
      }
      return true;
    }

    internal static string GetImageFilePath(string path)
    {
      string imageFileName = FSUtils.GetImageFileName(path);
      return Path.Combine(FSUtils.GetImageCacheDir(), imageFileName);
    }

    internal static string GetTmpDir()
    {
      string directory = Path.Combine(ApplicationData.Current.LocalCacheFolder.Path, "tmp");
      FSUtils.EnsureDirectoryExists(directory);
      return directory;
    }

    internal static bool EmptyTmpDir()
    {
      try
      {
        DirectoryInfo directoryInfo = new DirectoryInfo(FSUtils.GetTmpDir());
        if (directoryInfo.Exists)
        {
          FileInfo[] files = directoryInfo.GetFiles();
          if ((files != null ? (files.Length != 0 ? 1 : 0) : 0) != 0)
          {
            foreach (FileInfo file in directoryInfo.GetFiles())
            {
              try
              {
                file.Delete();
              }
              catch (Exception ex)
              {
              }
            }
            return true;
          }
        }
      }
      catch (Exception ex)
      {
      }
      return false;
    }

    internal static bool EmptyImagesDir()
    {
      try
      {
        DirectoryInfo directoryInfo = new DirectoryInfo(Path.Combine(ApplicationData.Current.LocalCacheFolder.Path, FSUtils.GetImageCacheDir()));
        if (directoryInfo.Exists)
        {
          FileInfo[] files = directoryInfo.GetFiles();
          if ((files != null ? (files.Length != 0 ? 1 : 0) : 0) != 0)
          {
            foreach (FileInfo file in directoryInfo.GetFiles())
            {
              try
              {
                file.Delete();
              }
              catch (Exception ex)
              {
              }
            }
          }
        }
      }
      catch (Exception ex)
      {
      }
      return false;
    }

    public static long GetDirectorySize(DirectoryInfo dirInfo)
    {
      long directorySize = 0;
      foreach (FileInfo file in dirInfo.GetFiles())
        directorySize += file.Length;
      foreach (DirectoryInfo directory in dirInfo.GetDirectories())
        directorySize += FSUtils.GetDirectorySize(directory);
      return directorySize;
    }

    internal static string GetImageFileName(string path)
    {
      StringBuilder stringBuilder = new StringBuilder();
      IBuffer binary = CryptographicBuffer.ConvertStringToBinary(path, (BinaryStringEncoding) 0);
      foreach (byte num in HashAlgorithmProvider.OpenAlgorithm("SHA256").HashData(binary).ToArray())
        stringBuilder.Append(num.ToString("x2"));
      return stringBuilder.ToString();
    }

    internal static string GetExtension(string path)
    {
      int num = path.LastIndexOf('.');
      return num == -1 || num == path.Length - 1 ? (string) null : path.Substring(num + 1);
    }

    internal static async Task<string> GetFilePathFromPhotoResult(
      PhotoResult photoResult,
      string fileName)
    {
      using (MemoryStream ms = new MemoryStream())
      {
        photoResult.ChosenPhoto.CopyTo((Stream) ms);
        byte[] array = ms.ToArray();
        string filePath = Path.Combine(ApplicationData.Current.LocalCacheFolder.Path, fileName + "." + (FSUtils.GetExtension(photoResult.OriginalFileName) ?? "jpg"));
        try
        {
          await FSUtils.WriteFileAsync(filePath, array);
          return filePath;
        }
        catch (Exception ex)
        {
          ImoMessageBoxResult messageBoxResult = await ImoMessageBox.Show("Failed to open the photo because your phone memory is full!");
          return (string) null;
        }
        filePath = (string) null;
      }
      string pathFromPhotoResult;
      return pathFromPhotoResult;
    }

    internal static async Task<string> GetFilePathFromPicture(Picture picture, string fileName)
    {
      using (MemoryStream ms = new MemoryStream())
      {
        picture.GetImage().CopyTo((Stream) ms);
        byte[] array = ms.ToArray();
        string filePath = Path.Combine(ApplicationData.Current.LocalCacheFolder.Path, fileName + "." + (FSUtils.GetExtension(picture.Name) ?? "jpg"));
        try
        {
          await FSUtils.WriteFileAsync(filePath, array);
          return filePath;
        }
        catch (Exception ex)
        {
          ImoMessageBoxResult messageBoxResult = await ImoMessageBox.Show("Failed to open the photo because your phone memory is full!");
          return (string) null;
        }
        filePath = (string) null;
      }
      string filePathFromPicture;
      return filePathFromPicture;
    }

    private class ReadFileStateObject
    {
      public FileStream FileStream;
      public byte[] ByteArray;
      public string FilePath;
      public Action<byte[]> Callback;

      public ReadFileStateObject(string filePath, long byteArraySize)
      {
        this.FilePath = filePath;
        this.ByteArray = new byte[byteArraySize];
      }
    }
  }
}
