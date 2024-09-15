// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.Helpers.DateTimeExtensions
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using System;
using System.Globalization;


namespace ImoSilverlightApp.Helpers
{
  public static class DateTimeExtensions
  {
    public static string ToShortTimeString(this DateTime dateTime)
    {
      return dateTime.ToString(DateTimeFormatInfo.CurrentInfo.ShortTimePattern);
    }

    public static string ToShortDateString(this DateTime dateTime)
    {
      return dateTime.ToString(DateTimeFormatInfo.CurrentInfo.ShortDatePattern);
    }

    public static string ToLongTimeString(this DateTime dateTime)
    {
      return dateTime.ToString(DateTimeFormatInfo.CurrentInfo.LongTimePattern);
    }
  }
}
