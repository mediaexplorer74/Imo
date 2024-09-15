// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.Converters.VisibilityConverter
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;


namespace ImoSilverlightApp.Converters
{
  public class VisibilityConverter : IValueConverter
  {
    public object VisibleValue { get; set; }

    public object CollapsedValue { get; set; }

    public virtual object Convert(
      object value,
      Type targetType,
      object parameter,
      CultureInfo culture)
    {
      if (this.VisibleValue != null && value != null)
        return (object) (Visibility) (this.VisibleValue.ToString() == value.ToString() ? 0 : 1);
      return this.CollapsedValue != null && value != null ? (object) (Visibility) (this.CollapsedValue.ToString() == value.ToString() ? 1 : 0) : (object) Visibility.Visible;
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
