// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.Converters.DoubleToPointConverter
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;


namespace ImoSilverlightApp.Converters
{
  public class DoubleToPointConverter : IValueConverter
  {
    public double Scale { get; set; }

    public DoubleToPointConverter() => this.Scale = 1.0;

    public virtual object Convert(
      object value,
      Type targetType,
      object parameter,
      CultureInfo culture)
    {
      return (object) new Point((double) value * this.Scale, (double) value * this.Scale);
    }

    public virtual object ConvertBack(
      object value,
      Type targetType,
      object parameter,
      CultureInfo culture)
    {
      return (object) (((Point) value).X / this.Scale);
    }
  }
}
