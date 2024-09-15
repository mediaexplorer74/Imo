// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.Helpers.BitConverterHelper
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using System;
using System.Collections.Generic;
using System.Linq;


namespace ImoSilverlightApp.Helpers
{
  internal class BitConverterHelper
  {
    public static byte[] GetBytes(int value)
    {
      return BitConverter.IsLittleEndian ? ((IEnumerable<byte>) BitConverter.GetBytes(value)).Reverse<byte>().ToArray<byte>() : BitConverter.GetBytes(value);
    }

    public static int ToInt32(byte[] bytes)
    {
      return !BitConverter.IsLittleEndian ? BitConverter.ToInt32(bytes, 0) : BitConverter.ToInt32(((IEnumerable<byte>) bytes).Reverse<byte>().ToArray<byte>(), 0);
    }
  }
}
