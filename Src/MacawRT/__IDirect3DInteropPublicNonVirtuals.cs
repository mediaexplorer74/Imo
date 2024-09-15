// Decompiled with JetBrains decompiler
// Type: MacawRT.__IDirect3DInteropPublicNonVirtuals
// Assembly: MacawRT, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: CC361766-FEAE-4663-8B04-28F9E3929A51
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\MacawRT.winmd

using System.Runtime.InteropServices;
using Windows.Foundation;
using Windows.Foundation.Metadata;
using Windows.Phone.Graphics.Interop;


namespace MacawRT
{
  [Version(1)]
  [Guid(1397185897, 11744, 14563, 137, 135, 229, 240, 246, 89, 144, 30)]
  [WebHostHidden]
  [ExclusiveTo(typeof (Direct3DInterop))]
  internal interface __IDirect3DInteropPublicNonVirtuals
  {
    Size WindowBounds { get; [param: In] set; }

    Size NativeResolution { get; [param: In] set; }

    Size RenderResolution { get; [param: In] set; }

    IDrawingSurfaceContentProvider CreateContentProvider();

    void SetFrame(out byte data, [In] int width, [In] int height, [In] int angle, [In] int deviceAngle);

    void SetGroupFrame(
      out byte data,
      [In] int width,
      [In] int height,
      [In] int angle,
      [In] int slot,
      [In] int deviceAngle);

    void SetSelfFrame(out byte data, [In] int width, [In] int height, [In] int angle, [In] bool flipX);

    void SetIsGroupCall([In] bool value);
  }
}
