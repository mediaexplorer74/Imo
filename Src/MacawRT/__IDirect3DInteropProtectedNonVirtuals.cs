// Decompiled with JetBrains decompiler
// Type: MacawRT.__IDirect3DInteropProtectedNonVirtuals
// Assembly: MacawRT, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: CC361766-FEAE-4663-8B04-28F9E3929A51
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\MacawRT.winmd

using System.Runtime.InteropServices;
using Windows.Foundation.Metadata;
using Windows.Phone.Input.Interop;
using Windows.UI.Core;


namespace MacawRT
{
  [Version(1)]
  [ExclusiveTo(typeof (Direct3DInterop))]
  [WebHostHidden]
  [Guid(773995109, 35303, 14414, 139, 185, 250, 173, 213, 28, 155, 220)]
  internal interface __IDirect3DInteropProtectedNonVirtuals
  {
    void OnPointerPressed([In] DrawingSurfaceManipulationHost sender, [In] PointerEventArgs args);

    void OnPointerMoved([In] DrawingSurfaceManipulationHost sender, [In] PointerEventArgs args);

    void OnPointerReleased([In] DrawingSurfaceManipulationHost sender, [In] PointerEventArgs args);
  }
}
