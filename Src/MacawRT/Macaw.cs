// Decompiled with JetBrains decompiler
// Type: MacawRT.Macaw
// Assembly: MacawRT, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: CC361766-FEAE-4663-8B04-28F9E3929A51
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\MacawRT.winmd

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Windows.Foundation.Metadata;


namespace MacawRT
{
  [Threading]
  [Static(typeof (__IMacawStatics), 1)]
  [MarshalingBehavior]
  [Version(1)]
  [Activatable(1)]
  public sealed class Macaw : __IMacawPublicNonVirtuals
  {
    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public static extern void init([In] bool loudSpeaker);

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public static extern int runThread();

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public static extern void StartAudio();

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public static extern void StopAudio();

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public static extern void SendImageRGB32(
      [In] int context,
      [In] int width,
      [In] int height,
      [In] int stride,
      [In] int[] data,
      [In] int length,
      [In] int milliseconds,
      [In] int angle);

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public static extern void SendImageNV12(
      [In] int context,
      [In] int width,
      [In] int height,
      [In] byte[] data,
      [In] int milliseconds,
      [In] int angle);

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public static extern void ConvertNv12ToArgb([In] int width, [In] int height, [In] byte[] data, [Out] int[] output);

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public static extern void SetPreviewFrame(
      [In] byte[] data,
      [In] int width,
      [In] int height,
      [In] int angle,
      [In] bool flipX);

    public static extern int PhoneRotation { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public static extern void SetOnSelfConnect([In] VoidCallback onSelfConnect);

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public static extern void SetOnSelfDisconnect([In] VoidCallback onSelfDisconnect);

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public static extern void SetOnBuddyConnect([In] VoidCallback onBuddyConnect);

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public static extern void SetOnBuddyDisconnect([In] VoidCallback onBuddyDisconnect);

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public static extern void SetOnExit([In] VoidCallback onExit);

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public static extern void SetGetIsVideoCall([In] BoolCallback getIsVideoCall);

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public static extern void SetReportStats([In] VoidStringCallback reportStats);

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public static extern void SetSetFrame([In] SetFrameCallback setFrame);

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public static extern void SetGetConvId([In] StringCallback getConvId);

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public static extern void SetGetLogPath([In] StringCallback getLogPath);

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public static extern void SetIsRefl([In] BoolCallback isRefl);

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public static extern void SetIsInitiator([In] BoolCallback isInitiator);

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public static extern void SetIsABTestEnabled([In] BoolIntCallback isABTestEnabled);

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public static extern void SetGetSharedKey([In] ByteArrayCallback getSharedKey);

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public static extern void SetGetServerCbcKey([In] ByteArrayCallback getServerCbcKey);

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public static extern void SetGetPeerCbcKey([In] ByteArrayCallback getPeerCbcKey);

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public static extern void SetGetServerKey([In] ByteArrayCallback getServerKey);

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public static extern void SetGetConnServerTickets([In] IndexByteArrayCallback getConnServerTickets);

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public static extern void SetGetConnServerTicketsLengths(
      [In] IndexIntArrayCallback getConnServerTicketsLengths);

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public static extern void SetGetInitiatorProtocolMask([In] ByteArrayCallback getInitiatorProtocolMask);

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public static extern void SetGetReceiverProtocolMask([In] ByteArrayCallback getReceiverProtocolMask);

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public static extern void SetGetConnectionType([In] StringCallback getConnectionType);

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public static extern void SetGetIPv6Pipe([In] StringCallback getIPv6Pipe);

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public static extern void SetGetLocalIPv6Address([In] StringCallback getLocalIPv6Address);

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public static extern void SetGetMaxVideoBitrateKbps([In] IntCallback getMaxVideoBitrateKbps);

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public static extern void SetGetMaxVideoSlots([In] IntCallback getMaxVideoSlots);

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public static extern void SetGetBitrateParams([In] DoubleArrayCallback getBitrateParams);

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public static extern void SetIsErrorCorrectionAllowed([In] BoolCallback isErrorCorrectionAllowed);

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public static extern void SetGetErrorCorrectionParams(
      [In] DoubleArrayCallback getErrorCorrectionParams);

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public static extern void SetLogMonitor([In] VoidStringCallback logMonitor);

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public static extern void SetPollUI([In] UlongBoolCallback pollUI);

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public static extern void SetCalculateBufferDelay([In] IntCallback calculateBufferDelay);

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public static extern void SetSetBufferDelay([In] VoidIntCallback setBufferDelay);

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public static extern void SetReportException([In] VoidStringCallback reportException);

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public static extern void SetIsGroupCall([In] BoolCallback isGroupCall);

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public static extern void SetGetQualityConfigParams(
      [In] IndexDoubleArrayCallback getQualityConfigParams);

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public static extern void SetGetCallParams([In] DoubleArrayCallback getCallParams);

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public static extern void SetGetStreamId([In] UshortCallback getStreamId);

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public static extern void SetGetNumConnections([In] IntCallback getNumConnections);

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public static extern void SetGetConnServerName([In] StringIntCallback getConnServerName);

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public static extern void SetGetConnSourcePort([In] IntIntCallback getConnSourcePort);

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public static extern void SetGetConnServerPort([In] IntIntCallback getConnServerPort);

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public static extern void SetGetConnNetParams([In] IndexDoubleArrayCallback getConnNetParams);

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public static extern void SetGetConnStringParams([In] StringIntCallback getConnStringParams);

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public static extern void SetGetConnStringParamsLengths(
      [In] IndexIntArrayCallback getConnStringParamsLengths);

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public static extern void SetSetContext([In] VoidIntCallback setContext);

    [Overload("CreateInstance1")]
    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public extern Macaw();
  }
}
