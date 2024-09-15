// Decompiled with JetBrains decompiler
// Type: MacawRT.__IAudioPublicNonVirtuals
// Assembly: MacawRT, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: CC361766-FEAE-4663-8B04-28F9E3929A51
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\MacawRT.winmd

using Windows.Foundation.Metadata;


namespace MacawRT
{
  [Guid(279299422, 55664, 16357, 163, 230, 61, 35, 5, 21, 125, 79)]
  [ExclusiveTo(typeof (Audio))]
  [Version(1)]
  internal interface __IAudioPublicNonVirtuals
  {
    void Start();

    void Stop();

    int SampleRate { get; }

    int MicSampleRate { get; }

    int RenderSampleRate { get; }

    AudioStateEnum State { get; }
  }
}
