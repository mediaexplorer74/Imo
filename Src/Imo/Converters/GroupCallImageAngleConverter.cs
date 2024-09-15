// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.Converters.GroupCallImageAngleConverter
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using System;
using System.Globalization;
using System.Windows.Data;


namespace ImoSilverlightApp.Converters
{
  public class GroupCallImageAngleConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      int num = (int) value;
      switch (num)
      {
        case 0:
        case 180:
          return (object) num;
        default:
          return (object) 0;
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
