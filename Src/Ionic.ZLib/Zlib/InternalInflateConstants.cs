// Decompiled with JetBrains decompiler
// Type: Ionic.Zlib.InternalInflateConstants
// Assembly: Ionic.ZLib, Version=2.0.0.14, Culture=neutral, PublicKeyToken=null
// MVID: AE1A51CF-981C-4AD1-9F5E-F78F9C4A3637
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\Ionic.ZLib.dll

#nullable disable
namespace Ionic.Zlib
{
  internal static class InternalInflateConstants
  {
    internal static readonly int[] InflateMask = new int[17]
    {
      0,
      1,
      3,
      7,
      15,
      31,
      63,
      (int) sbyte.MaxValue,
      (int) byte.MaxValue,
      511,
      1023,
      2047,
      4095,
      8191,
      16383,
      (int) short.MaxValue,
      (int) ushort.MaxValue
    };
  }
}
