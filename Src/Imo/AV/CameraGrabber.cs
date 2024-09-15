// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.AV.CameraGrabber
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Helpers;
using MacawRT;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Media.MediaProperties;
using Windows.Phone.Media.Capture;


namespace ImoSilverlightApp.AV
{
  public class CameraGrabber
  {
    private static readonly Logger log = LogManager.GetLogger(typeof (CameraGrabber).Name);
    private bool isRendering;
    private byte[] currentFrameBuffer;
    private DateTime startCapture;
    private int captures;
    private int snapshotNumber;
    private double fps;
    private string[] preferredEncodings = new string[2]
    {
      MediaEncodingSubtypes.Rgb32,
      MediaEncodingSubtypes.Nv12
    };
    private const int targetWidth = 640;
    private const int targetHeight = 360;
    private const int targetSize = 230400;
    private PhotoCaptureDevice photoCaptureDevice;

    public CameraSensorLocation CameraFacing { get; private set; }

    public bool IsCapturing { get; private set; }

    public event EventHandler<SampleGrabberEventArgs> SampleCaptured;

    public int VideoWidth { get; private set; }

    public int VideoHeight { get; private set; }

    public int Stride { get; private set; }

    public int ImageSize { get; private set; }

    public CameraGrabber.EncodingType Encoding { get; private set; }

    public static CameraGrabber CreateInstance(CameraSensorLocation cameraFacing = 1)
    {
      try
      {
        CameraGrabber instance = new CameraGrabber();
        if (!PhotoCaptureDevice.AvailableSensorLocations.Contains<CameraSensorLocation>(cameraFacing))
          cameraFacing = cameraFacing == 1 ? (CameraSensorLocation) 0 : (CameraSensorLocation) 1;
        instance.CameraFacing = cameraFacing;
        return instance;
      }
      catch (Exception ex)
      {
        CameraGrabber.log.Error(ex, "Exception during initialization of camera.", 89, nameof (CreateInstance));
        return (CameraGrabber) null;
      }
    }

    private CameraGrabber()
    {
    }

    private bool IsSupported(Size size) => size.Width * size.Height > 0.0;

    private int CompareSizes(Size l, Size r)
    {
      bool flag1 = this.IsSupported(l);
      bool flag2 = this.IsSupported(r);
      if (flag1 != flag2)
        return !flag1 ? -1 : 1;
      if (!flag1 && !flag2)
        return 0;
      int num = Math.Abs((int) (l.Width * l.Height) - 230400);
      return Math.Abs((int) (r.Width * r.Height) - 230400) - num;
    }

    private Size SelectPreviewSize(List<Size> excludedSizes)
    {
      IReadOnlyList<Size> previewResolutions = PhotoCaptureDevice.GetAvailablePreviewResolutions(this.CameraFacing);
      Size l = previewResolutions.First<Size>();
      foreach (Size r in (IEnumerable<Size>) previewResolutions)
      {
        if (this.CompareSizes(l, r) < 0 && !excludedSizes.Contains(r))
          l = r;
      }
      return l;
    }

    public async Task Start()
    {
      this.IsCapturing = true;
      if (!PhotoCaptureDevice.AvailableSensorLocations.Contains<CameraSensorLocation>(this.CameraFacing))
        return;
      bool success = false;
      List<Size> excudedSizes = new List<Size>();
      for (int i = 0; i < 30 && !success; ++i)
      {
        Size bestSize = this.SelectPreviewSize(excudedSizes);
        try
        {
          CameraGrabber cameraGrabber = this;
          PhotoCaptureDevice photoCaptureDevice1 = cameraGrabber.photoCaptureDevice;
          PhotoCaptureDevice photoCaptureDevice2 = await PhotoCaptureDevice.OpenAsync(this.CameraFacing, bestSize);
          cameraGrabber.photoCaptureDevice = photoCaptureDevice2;
          cameraGrabber = (CameraGrabber) null;
          await this.photoCaptureDevice.SetPreviewResolutionAsync(bestSize);
          success = true;
          this.SaveSizeInfo(bestSize);
        }
        catch
        {
          excudedSizes.Add(bestSize);
        }
        bestSize = new Size();
      }
      PhotoCaptureDevice photoCaptureDevice = this.photoCaptureDevice;
      // ISSUE: method pointer
      WindowsRuntimeMarshal.AddEventHandler<TypedEventHandler<ICameraCaptureDevice, object>>(new Func<TypedEventHandler<ICameraCaptureDevice, object>, EventRegistrationToken>(photoCaptureDevice.add_PreviewFrameAvailable), new Action<EventRegistrationToken>(photoCaptureDevice.remove_PreviewFrameAvailable), new TypedEventHandler<ICameraCaptureDevice, object>((object) this, __methodptr(OnFrameAvailable)));
    }

    private async void OnFrameAvailable(object sender, object args)
    {
      if (this.isRendering)
        return;
      this.isRendering = true;
      if (this.snapshotNumber == 0)
        this.startCapture = DateTime.Now;
      else if ((DateTime.Now - this.startCapture).TotalSeconds > 3.0)
      {
        this.fps = (double) this.captures / (DateTime.Now - this.startCapture).TotalSeconds;
        this.startCapture = DateTime.Now;
        this.captures = 0;
      }
      try
      {
        this.photoCaptureDevice.GetPreviewBufferYCbCr(this.currentFrameBuffer);
      }
      catch (Exception ex)
      {
        this.isRendering = false;
        return;
      }
      ++this.captures;
      ++this.snapshotNumber;
      await Utils.InvokeOnUI((Action) (() =>
      {
        if (IMO.AVManager.IsInCall)
        {
          byte[] currentFrameBuffer = this.currentFrameBuffer;
          int videoWidth = this.VideoWidth;
          int videoHeight = this.VideoHeight;
          CallController callController = IMO.AVManager.CallController;
          int meCameraAngle = callController != null ? callController.MeCameraAngle : 0;
          int num = IMO.AVManager.CallController == null ? 0 : (IMO.AVManager.CallController.FlipXRatio == -1 ? 1 : 0);
          Macaw.SetPreviewFrame(currentFrameBuffer, videoWidth, videoHeight, meCameraAngle, num != 0);
        }
        EventHandler<SampleGrabberEventArgs> sampleCaptured = this.SampleCaptured;
        if (sampleCaptured == null)
          return;
        sampleCaptured((object) this, new SampleGrabberEventArgs(this.currentFrameBuffer));
      }));
      this.isRendering = false;
    }

    public void Stop()
    {
      if (this.photoCaptureDevice == null)
        return;
      // ISSUE: method pointer
      WindowsRuntimeMarshal.RemoveEventHandler<TypedEventHandler<ICameraCaptureDevice, object>>(new Action<EventRegistrationToken>(this.photoCaptureDevice.remove_PreviewFrameAvailable), new TypedEventHandler<ICameraCaptureDevice, object>((object) this, __methodptr(OnFrameAvailable)));
      this.photoCaptureDevice.Dispose();
    }

    private void SaveSizeInfo(Size size)
    {
      this.VideoWidth = (int) size.Width;
      this.VideoHeight = (int) size.Height;
      this.Stride = this.VideoWidth * 3;
      this.ImageSize = this.VideoWidth * this.VideoHeight * 4;
      this.Encoding = CameraGrabber.EncodingType.RGB32;
      this.currentFrameBuffer = new byte[this.VideoWidth * this.VideoHeight * 3 / 2];
    }

    public enum EncodingType
    {
      YUY2,
      I420,
      RGB24,
      RGB32,
    }
  }
}
