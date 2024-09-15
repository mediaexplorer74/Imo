// Decompiled with JetBrains decompiler
// Type: MacawRT.__IDirect3DInteropStatics
// Assembly: MacawRT, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: CC361766-FEAE-4663-8B04-28F9E3929A51
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\MacawRT.winmd

using System.Runtime.InteropServices;
using Windows.Foundation.Metadata;


namespace MacawRT
{
  [Version(1)]
  [Guid(2947430599, 62997, 14919, 150, 140, 128, 155, 139, 20, 25, 205)]
  [ExclusiveTo(typeof (Direct3DInterop))]
  [WebHostHidden]
  internal interface __IDirect3DInteropStatics
  {
    Direct3DInterop Instance { get; [param: In] set; }
  }
}
