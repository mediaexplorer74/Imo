// Decompiled with JetBrains decompiler
// Type: MacawRT.SetFrameCallback
// Assembly: MacawRT, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: CC361766-FEAE-4663-8B04-28F9E3929A51
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\MacawRT.winmd

using System.Runtime.InteropServices;
using Windows.Foundation.Metadata;


namespace MacawRT
{
  [Guid(1117971309, 64178, 13178, 170, 211, 192, 41, 21, 96, 198, 218)]
  [Version(1)]
  public delegate void SetFrameCallback(
    [In] byte[] __param0,
    [In] int __param1,
    [In] int __param2,
    [In] int __param3,
    [In] int __param4);
}
