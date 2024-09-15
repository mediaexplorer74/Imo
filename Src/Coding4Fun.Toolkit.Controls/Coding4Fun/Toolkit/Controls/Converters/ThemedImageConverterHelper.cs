// Decompiled with JetBrains decompiler
// Type: Coding4Fun.Toolkit.Controls.Converters.ThemedImageConverterHelper
// Assembly: Coding4Fun.Toolkit.Controls, Version=2.1.7.0, Culture=neutral, PublicKeyToken=c5fd7b72b1a17ce4
// MVID: 213D8A75-4B66-4004-95C2-D82C49ACB764
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\Coding4Fun.Toolkit.Controls.dll

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Imaging;


namespace Coding4Fun.Toolkit.Controls.Converters
{
  public static class ThemedImageConverterHelper
  {
    private static readonly Dictionary<string, BitmapImage> ImageCache = new Dictionary<string, BitmapImage>();

    public static BitmapImage GetImage(string path, bool negateResult = false)
    {
      if (string.IsNullOrEmpty(path))
        return (BitmapImage) null;
      bool flag = Application.Current.Resources.Contains((object) "PhoneDarkThemeVisibility") && (Visibility) Application.Current.Resources[(object) "PhoneDarkThemeVisibility"] == Visibility.Visible;
      if (negateResult)
        flag = !flag;
      path = string.Format(path, flag ? (object) "dark" : (object) "light");
      BitmapImage image;
      if (!ThemedImageConverterHelper.ImageCache.TryGetValue(path, out image))
      {
        image = new BitmapImage(new Uri(path, UriKind.Relative));
        ThemedImageConverterHelper.ImageCache.Add(path, image);
      }
      return image;
    }
  }
}
