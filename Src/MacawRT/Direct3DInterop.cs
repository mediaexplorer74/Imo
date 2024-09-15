// Decompiled with JetBrains decompiler
// Type: MacawRT.Direct3DInterop
// Assembly: MacawRT, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: CC361766-FEAE-4663-8B04-28F9E3929A51
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\MacawRT.winmd

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Windows.Foundation;
using Windows.Foundation.Metadata;
using Windows.Phone.Graphics.Interop;
using Windows.Phone.Input.Interop;
using Windows.UI.Core;


namespace MacawRT
{
  [Threading]
  [Activatable(1)]
  [MarshalingBehavior]
  [WebHostHidden]
  [Static(typeof (__IDirect3DInteropStatics), 1)]
  [Version(1)]
  public sealed class Direct3DInterop : 
    IDrawingSurfaceManipulationHandler,
    __IDirect3DInteropPublicNonVirtuals,
    __IDirect3DInteropProtectedNonVirtuals
  {
    [Overload("CreateInstance1")]
    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public extern Direct3DInterop();

    public static extern Direct3DInterop Instance { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public extern void SetManipulationHost([In] DrawingSurfaceManipulationHost manipulationHost);

    public extern Size WindowBounds { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    public extern Size NativeResolution { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    public extern Size RenderResolution { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public extern IDrawingSurfaceContentProvider CreateContentProvider();

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public extern void SetFrame(out byte data, [In] int width, [In] int height, [In] int angle, [In] int deviceAngle);

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public extern void SetGroupFrame(
      out byte data,
      [In] int width,
      [In] int height,
      [In] int angle,
      [In] int slot,
      [In] int deviceAngle);

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public extern void SetSelfFrame(out byte data, [In] int width, [In] int height, [In] int angle, [In] bool flipX);

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public extern void SetIsGroupCall([In] bool value);

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    extern void __IDirect3DInteropProtectedNonVirtuals.OnPointerPressed(
      [In] DrawingSurfaceManipulationHost sender,
      [In] PointerEventArgs args);

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    extern void __IDirect3DInteropProtectedNonVirtuals.OnPointerMoved(
      [In] DrawingSurfaceManipulationHost sender,
      [In] PointerEventArgs args);

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    extern void __IDirect3DInteropProtectedNonVirtuals.OnPointerReleased(
      [In] DrawingSurfaceManipulationHost sender,
      [In] PointerEventArgs args);
  }
}
