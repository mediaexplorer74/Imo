// Decompiled with JetBrains decompiler
// Type: Coding4Fun.Toolkit.Controls.Converters.VisibilityToBooleanConverter
// Assembly: Coding4Fun.Toolkit.Controls, Version=2.1.7.0, Culture=neutral, PublicKeyToken=c5fd7b72b1a17ce4
// MVID: 213D8A75-4B66-4004-95C2-D82C49ACB764
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\Coding4Fun.Toolkit.Controls.dll

using System;
using System.Globalization;


namespace Coding4Fun.Toolkit.Controls.Converters
{
  public class VisibilityToBooleanConverter : ValueConverter
  {
    private static readonly BooleanToVisibilityConverter BoolToVis = new BooleanToVisibilityConverter();

    public override object Convert(
      object value,
      Type targetType,
      object parameter,
      CultureInfo culture,
      string language)
    {
      return VisibilityToBooleanConverter.BoolToVis.ConvertBack(value, targetType, parameter, culture);
    }

    public override object ConvertBack(
      object value,
      Type targetType,
      object parameter,
      CultureInfo culture,
      string language)
    {
      return VisibilityToBooleanConverter.BoolToVis.Convert(value, targetType, parameter, culture);
    }
  }
}
