// Decompiled with JetBrains decompiler
// Type: MacawRT.__IMacawStatics
// Assembly: MacawRT, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: CC361766-FEAE-4663-8B04-28F9E3929A51
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\MacawRT.winmd

using System.Runtime.InteropServices;
using Windows.Foundation.Metadata;


namespace MacawRT
{
  [Guid(3132487715, 17952, 14326, 169, 211, 204, 243, 248, 171, 70, 27)]
  [ExclusiveTo(typeof (Macaw))]
  [Version(1)]
  internal interface __IMacawStatics
  {
    void init([In] bool loudSpeaker);

    int runThread();

    void StartAudio();

    void StopAudio();

    void SendImageRGB32(
      [In] int context,
      [In] int width,
      [In] int height,
      [In] int stride,
      [In] int[] data,
      [In] int length,
      [In] int milliseconds,
      [In] int angle);

    void SendImageNV12(
      [In] int context,
      [In] int width,
      [In] int height,
      [In] byte[] data,
      [In] int milliseconds,
      [In] int angle);

    void ConvertNv12ToArgb([In] int width, [In] int height, [In] byte[] data, [Out] int[] output);

    void SetPreviewFrame([In] byte[] data, [In] int width, [In] int height, [In] int angle, [In] bool flipX);

    int PhoneRotation { get; [param: In] set; }

    void SetOnSelfConnect([In] VoidCallback onSelfConnect);

    void SetOnSelfDisconnect([In] VoidCallback onSelfDisconnect);

    void SetOnBuddyConnect([In] VoidCallback onBuddyConnect);

    void SetOnBuddyDisconnect([In] VoidCallback onBuddyDisconnect);

    void SetOnExit([In] VoidCallback onExit);

    void SetGetIsVideoCall([In] BoolCallback getIsVideoCall);

    void SetReportStats([In] VoidStringCallback reportStats);

    void SetSetFrame([In] SetFrameCallback setFrame);

    void SetGetConvId([In] StringCallback getConvId);

    void SetGetLogPath([In] StringCallback getLogPath);

    void SetIsRefl([In] BoolCallback isRefl);

    void SetIsInitiator([In] BoolCallback isInitiator);

    void SetIsABTestEnabled([In] BoolIntCallback isABTestEnabled);

    void SetGetSharedKey([In] ByteArrayCallback getSharedKey);

    void SetGetServerCbcKey([In] ByteArrayCallback getServerCbcKey);

    void SetGetPeerCbcKey([In] ByteArrayCallback getPeerCbcKey);

    void SetGetServerKey([In] ByteArrayCallback getServerKey);

    void SetGetConnServerTickets([In] IndexByteArrayCallback getConnServerTickets);

    void SetGetConnServerTicketsLengths([In] IndexIntArrayCallback getConnServerTicketsLengths);

    void SetGetInitiatorProtocolMask([In] ByteArrayCallback getInitiatorProtocolMask);

    void SetGetReceiverProtocolMask([In] ByteArrayCallback getReceiverProtocolMask);

    void SetGetConnectionType([In] StringCallback getConnectionType);

    void SetGetIPv6Pipe([In] StringCallback getIPv6Pipe);

    void SetGetLocalIPv6Address([In] StringCallback getLocalIPv6Address);

    void SetGetMaxVideoBitrateKbps([In] IntCallback getMaxVideoBitrateKbps);

    void SetGetMaxVideoSlots([In] IntCallback getMaxVideoSlots);

    void SetGetBitrateParams([In] DoubleArrayCallback getBitrateParams);

    void SetIsErrorCorrectionAllowed([In] BoolCallback isErrorCorrectionAllowed);

    void SetGetErrorCorrectionParams([In] DoubleArrayCallback getErrorCorrectionParams);

    void SetLogMonitor([In] VoidStringCallback logMonitor);

    void SetPollUI([In] UlongBoolCallback pollUI);

    void SetCalculateBufferDelay([In] IntCallback calculateBufferDelay);

    void SetSetBufferDelay([In] VoidIntCallback setBufferDelay);

    void SetReportException([In] VoidStringCallback reportException);

    void SetIsGroupCall([In] BoolCallback isGroupCall);

    void SetGetQualityConfigParams([In] IndexDoubleArrayCallback getQualityConfigParams);

    void SetGetCallParams([In] DoubleArrayCallback getCallParams);

    void SetGetStreamId([In] UshortCallback getStreamId);

    void SetGetNumConnections([In] IntCallback getNumConnections);

    void SetGetConnServerName([In] StringIntCallback getConnServerName);

    void SetGetConnSourcePort([In] IntIntCallback getConnSourcePort);

    void SetGetConnServerPort([In] IntIntCallback getConnServerPort);

    void SetGetConnNetParams([In] IndexDoubleArrayCallback getConnNetParams);

    void SetGetConnStringParams([In] StringIntCallback getConnStringParams);

    void SetGetConnStringParamsLengths([In] IndexIntArrayCallback getConnStringParamsLengths);

    void SetSetContext([In] VoidIntCallback setContext);
  }
}
