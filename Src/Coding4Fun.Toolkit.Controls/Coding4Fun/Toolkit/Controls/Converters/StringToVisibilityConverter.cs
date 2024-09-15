// Decompiled with JetBrains decompiler
// Type: Coding4Fun.Toolkit.Controls.Converters.StringToVisibilityConverter
// Assembly: Coding4Fun.Toolkit.Controls, Version=2.1.7.0, Culture=neutral, PublicKeyToken=c5fd7b72b1a17ce4
// MVID: 213D8A75-4B66-4004-95C2-D82C49ACB764
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\Coding4Fun.Toolkit.Controls.dll

using System;
using System.Globalization;
using System.Windows;


namespace Coding4Fun.Toolkit.Controls.Converters
{
  public class StringToVisibilityConverter : ValueConverter
  {
    public bool Inverted { get; set; }

    public override object Convert(
      object value,
      Type targetType,
      object parameter,
      CultureInfo culture,
      string language)
    {
      return this.Inverted ? (object) (Visibility) (string.IsNullOrEmpty(value as string) ? 0 : 1) : (object) (Visibility) (string.IsNullOrEmpty(value as string) ? 1 : 0);
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
