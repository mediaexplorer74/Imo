// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.Converters.ContactListToStringConverter
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Storage.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;


namespace ImoSilverlightApp.Converters
{
  public class ContactListToStringConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (!(value is IEnumerable<Contact> contacts))
        return (object) string.Empty;
      StringBuilder stringBuilder = new StringBuilder();
      foreach (Contact contact in contacts)
      {
        if (contact != null && !string.IsNullOrEmpty(contact.Alias))
        {
          string str = contact.Alias;
          if (str.Contains(" "))
            str = str.Substring(0, str.IndexOf(" "));
          if (stringBuilder.Length > 0)
            stringBuilder.Append(", ");
          stringBuilder.Append(str);
        }
      }
      return (object) stringBuilder.ToString();
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
