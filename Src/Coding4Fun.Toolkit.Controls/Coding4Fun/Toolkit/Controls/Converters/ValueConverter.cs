// Decompiled with JetBrains decompiler
// Type: Coding4Fun.Toolkit.Controls.Converters.ValueConverter
// Assembly: Coding4Fun.Toolkit.Controls, Version=2.1.7.0, Culture=neutral, PublicKeyToken=c5fd7b72b1a17ce4
// MVID: 213D8A75-4B66-4004-95C2-D82C49ACB764
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\Coding4Fun.Toolkit.Controls.dll

using System;
using System.Globalization;
using System.Windows.Data;


namespace Coding4Fun.Toolkit.Controls.Converters
{
  public abstract class ValueConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return this.Convert(value, targetType, parameter, culture, (string) null);
    }

    public object ConvertBack(
      object value,
      Type targetType,
      object parameter,
      CultureInfo culture)
    {
      return this.ConvertBack(value, targetType, parameter, culture, (string) null);
    }

    public object Convert(object value, Type targetType, object parameter, string language)
    {
      return this.Convert(value, targetType, parameter, (CultureInfo) null, language);
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
      return this.ConvertBack(value, targetType, parameter, (CultureInfo) null, language);
    }

    public virtual object Convert(
      object value,
      Type targetType,
      object parameter,
      CultureInfo culture,
      string language)
    {
      throw new NotImplementedException();
    }

    public virtual object ConvertBack(
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
