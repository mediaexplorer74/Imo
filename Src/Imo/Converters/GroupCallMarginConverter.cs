// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.Converters.GroupCallMarginConverter
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Helpers;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;


namespace ImoSilverlightApp.Converters
{
  public class GroupCallMarginConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      switch ((int) value)
      {
        case 0:
        case 180:
          return (object) new Thickness();
        default:
          int num = (Utils.GetGroupCallSlotHeight() - Utils.GetGroupCallSlotWidth()) / 2;
          return (object) new Thickness((double) -num, (double) num, (double) -num, (double) num);
      }
    }

    public object ConvertBack(
      object value,
      Type targetType,
      object parameter,
      CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
