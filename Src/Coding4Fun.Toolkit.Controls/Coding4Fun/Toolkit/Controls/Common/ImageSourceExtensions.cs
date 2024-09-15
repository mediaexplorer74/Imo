// Decompiled with JetBrains decompiler
// Type: Coding4Fun.Toolkit.Controls.Common.ImageSourceExtensions
// Assembly: Coding4Fun.Toolkit.Controls, Version=2.1.7.0, Culture=neutral, PublicKeyToken=c5fd7b72b1a17ce4
// MVID: 213D8A75-4B66-4004-95C2-D82C49ACB764
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\Coding4Fun.Toolkit.Controls.dll

using System.IO;
using System.IO.IsolatedStorage;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace Coding4Fun.Toolkit.Controls.Common
{
  public static class ImageSourceExtensions
  {
    private const string IsoStoreScheme = "isostore:/";
    private const string MsAppXScheme = "ms-appx:///";

    public static ImageSource ToBitmapImage(this ImageSource imageSource)
    {
      if (imageSource == null)
        return (ImageSource) null;
      if (!(imageSource is BitmapImage bitmapImage))
        return imageSource;
      string lower = bitmapImage.UriSource.ToString().ToLower();
      if (lower.StartsWith("isostore:/") || lower.StartsWith("ms-appx:///"))
      {
        string path = lower.Replace("isostore:/", string.Empty).TrimEnd('.').Replace("ms-appx:///", string.Empty).TrimEnd('.');
        bitmapImage = new BitmapImage();
        if (!ApplicationSpace.IsDesignMode)
        {
          using (IsolatedStorageFile storeForApplication = IsolatedStorageFile.GetUserStoreForApplication())
          {
            if (storeForApplication.FileExists(path))
            {
              using (IsolatedStorageFileStream streamSource = storeForApplication.OpenFile(path, FileMode.Open))
                bitmapImage.SetSource((Stream) streamSource);
            }
          }
        }
      }
      return (ImageSource) bitmapImage;
    }
  }
}
