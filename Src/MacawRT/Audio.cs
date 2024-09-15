// Decompiled with JetBrains decompiler
// Type: MacawRT.Audio
// Assembly: MacawRT, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: CC361766-FEAE-4663-8B04-28F9E3929A51
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\MacawRT.winmd

using System.Runtime.CompilerServices;
using Windows.Foundation;
using Windows.Foundation.Metadata;


namespace MacawRT
{
  [Version(1)]
  [Threading]
  [MarshalingBehavior]
  public sealed class Audio : IClosable, __IAudioPublicNonVirtuals
  {
    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public extern void Start();

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public extern void Stop();

    public extern int SampleRate { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; }

    public extern int MicSampleRate { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; }

    public extern int RenderSampleRate { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; }

    public extern AudioStateEnum State { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; }

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public extern void Close();
  }
}
