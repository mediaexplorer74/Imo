// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.Converters.BuidToIdentIconFillConverter
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Helpers;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;


namespace ImoSilverlightApp.Converters
{
  public class BuidToIdentIconFillConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return (object) new SolidColorBrush(IdentIconHelper.GetBuddyColor(System.Convert.ToString(value)));
    }

    public object ConvertBack(
      object value,
      Type targetType,
      object parameter,
      CultureInfo culture)
    {
      return (object) "";
    }
  }
}
