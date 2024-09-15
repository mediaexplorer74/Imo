﻿// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.Converters.LowercaseEnumConverter
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using Newtonsoft.Json;
using System;


namespace ImoSilverlightApp.Converters
{
  internal class LowercaseEnumConverter : JsonConverter
  {
    public override bool CanConvert(Type objectType) => objectType.IsEnum;

    public override object ReadJson(
      JsonReader reader,
      Type objectType,
      object existingValue,
      JsonSerializer serializer)
    {
      string str = (string) reader.Value;
      return Enum.Parse(objectType, str, true);
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      writer.WriteValue(value.ToString().ToLower());
    }
  }
}