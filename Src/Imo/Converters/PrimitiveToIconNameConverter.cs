// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.Converters.PrimitiveToIconNameConverter
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Storage.Models;
using System;
using System.Globalization;
using System.Windows.Data;


namespace ImoSilverlightApp.Converters
{
  public class PrimitiveToIconNameConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      switch ((Primitive) value)
      {
        case Primitive.Available:
          return (object) "PrimitiveAvailableIcon";
        case Primitive.Away:
          return (object) "PrimitiveAwayIcon";
        default:
          return (object) "PrimitiveOfflineIcon";
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
