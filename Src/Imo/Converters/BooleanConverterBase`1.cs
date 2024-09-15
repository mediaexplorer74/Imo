// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.Converters.BooleanConverterBase`1
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;


namespace ImoSilverlightApp.Converters
{
  public abstract class BooleanConverterBase<T> : IValueConverter
  {
    public BooleanConverterBase()
    {
    }

    public BooleanConverterBase(T trueValue, T falseValue)
    {
      this.True = trueValue;
      this.False = falseValue;
    }

    public virtual T True { get; set; }

    public virtual T False { get; set; }

    public virtual object Convert(
      object value,
      Type targetType,
      object parameter,
      CultureInfo culture)
    {
      return (object) (!(value is bool flag) || !flag ? this.False : this.True);
    }

    public virtual object ConvertBack(
      object value,
      Type targetType,
      object parameter,
      CultureInfo culture)
    {
      return (object) (bool) (!(value is T x) ? 0 : (EqualityComparer<T>.Default.Equals(x, this.True) ? 1 : 0));
    }
  }
}
