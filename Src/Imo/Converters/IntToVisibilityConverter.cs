// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.Converters.IntToVisibilityConverter
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;


namespace ImoSilverlightApp.Converters
{
  public class IntToVisibilityConverter : IValueConverter
  {
    public int False { get; set; }

    public virtual object Convert(
      object value,
      Type targetType,
      object parameter,
      CultureInfo culture)
    {
      return (object) (Visibility) (value is int num1 && num1 != this.False || value is long num2 && num2 != (long) this.False ? 0 : 1);
    }

    public virtual object ConvertBack(
      object value,
      Type targetType,
      object parameter,
      CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
