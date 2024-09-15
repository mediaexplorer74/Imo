// Decompiled with JetBrains decompiler
// Type: Coding4Fun.Toolkit.Controls.Converters.ThemedImageConverter
// Assembly: Coding4Fun.Toolkit.Controls, Version=2.1.7.0, Culture=neutral, PublicKeyToken=c5fd7b72b1a17ce4
// MVID: 213D8A75-4B66-4004-95C2-D82C49ACB764
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\Coding4Fun.Toolkit.Controls.dll

using System;
using System.Globalization;


namespace Coding4Fun.Toolkit.Controls.Converters
{
  public class ThemedImageConverter : ValueConverter
  {
    public override object Convert(
      object value,
      Type targetType,
      object parameter,
      CultureInfo culture,
      string language)
    {
      string path = parameter as string;
      if (string.IsNullOrEmpty(path))
        path = value as string;
      return (object) ThemedImageConverterHelper.GetImage(path);
    }

    public override object ConvertBack(
      object value,
      Type targetType,
      object parameter,
      CultureInfo culture,
      string language)
    {
      throw new NotImplementedException();
    }
  }
}
