// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.Managers.MediaUploader
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Helpers;
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;


namespace ImoSilverlightApp.Managers
{
  internal class MediaUploader : BaseManager
  {
    private static readonly Logger log = LogManager.GetLogger(typeof (MediaUploader).Name);
    public const string VIDEO_LOG_FILE = "upload_videos";
    public const string PHOTO_LOG_FILE = "upload_photos";
    private const string MEDIA_TYPE_IMAGE = "image/";
    private const string MEDIA_TYPE_VIDEO = "video/";
    private const int CHUNK_SIZE = 12000;
    private const int PARALLEL_CHUNKS = 4;
    private const long MAX_FILE_SIZE = 52428800;
    private HashSet<string> cancelRequests = new HashSet<string>();

    private bool IsCanceled(string uploadId)
    {
      lock (this.cancelRequests)
        return this.cancelRequests.Contains(uploadId);
    }

    public void CancelPhoto(string streamingUploadId)
    {
      lock (this.cancelRequests)
        this.cancelRequests.Add(streamingUploadId);
    }

    public async Task<string> UploadImage(
      string path,
      Action<string> successCallback = null,
      Action<double> progressCallback = null,
      bool shouldResize = false,
      bool shouldCrop = false)
    {
      if (new FileInfo(path).Length <= 52428800L)
        return this.UploadMedia(path, "image/", successCallback, progressCallback, shouldResize, shouldCrop);
      ImoMessageBoxResult messageBoxResult = await ImoMessageBox.Show("The file you have selected is too large! The maximum size is " + string.Format("{0}MB.", (object) 50L));
      return string.Empty;
    }

    public async Task<string> UploadVideo(
      string path,
      Action<string> successCallback = null,
      Action<double> progressCallback = null)
    {
      if (new FileInfo(path).Length <= 52428800L)
        return this.UploadMedia(path, "video/", successCallback, progressCallback);
      ImoMessageBoxResult messageBoxResult = await ImoMessageBox.Show("The file you have selected is too large! The maximum size is " + string.Format("{0}MB.", (object) 50L));
      return string.Empty;
    }

    private string UploadMedia(
      string path,
      string mediaType,
      Action<string> successCallback = null,
      Action<double> progressCallback = null,
      bool shouldResize = false,
      bool shouldCrop = false)
    {
      string filePathShort = path;
      if (mediaType == "image/" & shouldResize)
        filePathShort = ImageUtils.GetResizedImageBlocking(path, shouldCrop);
      string streamingUploadId = Utils.GetRandomString(16);
      string streamId = Utils.GenerateStreamId(IMO.User.Uid, IMO.User.Uid);
      FSUtils.ReadFileFromCache(filePathShort, (Action<byte[]>) (bytes =>
      {
        if (bytes == null)
        {
          MediaUploader.log.Error("Read no bytes while uplpoading photo", 89, nameof (UploadMedia));
        }
        else
        {
          long timestamp = Utils.GetTimestamp();
          MediaUploader.LogUploadStatus(bytes.Length, timestamp, "start", mediaType);
          int numChunks = (bytes.Length + 12000 - 1) / 12000;
          int sentChunks = 0;
          for (int chunkNum = 0; chunkNum < 4; ++chunkNum)
            this.UploadChunk(chunkNum, bytes, streamingUploadId, streamId, timestamp, mediaType, successCallback, (Action) (() =>
            {
              ++sentChunks;
              if (progressCallback == null || this.IsCanceled(streamingUploadId))
                return;
              progressCallback((double) sentChunks / (double) numChunks);
            }));
          lock (this.cancelRequests)
          {
            if (!this.cancelRequests.Contains(streamingUploadId))
              return;
            MediaUploader.LogUploadStatus(bytes.Length, timestamp, "cancel", mediaType);
            this.cancelRequests.Remove(streamingUploadId);
          }
        }
      }));
      return streamingUploadId;
    }

    private void UploadChunk(
      int chunkNum,
      byte[] bytes,
      string streamingUploadId,
      string streamId,
      long startTime,
      string mediaType,
      Action<string> successCallback = null,
      Action ackCallback = null)
    {
      if (this.IsCanceled(streamingUploadId))
        return;
      int num = (bytes.Length + 12000 - 1) / 12000;
      if (chunkNum == 4)
        MediaUploader.LogUploadStatus(bytes.Length, startTime, "first", mediaType);
      if (chunkNum >= num)
        return;
      int sourceIndex = chunkNum * 12000;
      int length = Math.Min(bytes.Length, sourceIndex + 12000) - sourceIndex;
      byte[] numArray = new byte[length];
      Array.Copy((Array) bytes, sourceIndex, (Array) numArray, 0, length);
      string base64String = Convert.ToBase64String(numArray);
      BaseManager.Send("pixelupload", "upload_chunk", new Dictionary<string, object>()
      {
        {
          "ssid",
          (object) IMO.Dispatcher.GetSSID()
        },
        {
          "uid",
          (object) IMO.User.Uid
        },
        {
          "proto",
          (object) "imo"
        },
        {
          "stream_id",
          (object) streamId
        },
        {
          "streaming_upload_id",
          (object) streamingUploadId
        },
        {
          "data",
          (object) base64String
        },
        {
          "offset",
          (object) sourceIndex
        },
        {
          "chunk_size",
          (object) length
        },
        {
          "total_size",
          (object) bytes.Length
        }
      }, (Action<JToken>) (token =>
      {
        try
        {
          if (token.Value<JToken>((object) "object_data") == null)
            return;
          MediaUploader.LogUploadStatus(bytes.Length, startTime, "success", mediaType);
          successCallback(token.Value<JArray>((object) "object_data")[0].Value<string>((object) "object_id"));
        }
        catch (Exception ex)
        {
          MediaUploader.log.Error(ex, "UploadChunk error in response", 182, nameof (UploadChunk));
        }
      }), (Action) (() =>
      {
        if (ackCallback != null)
          ackCallback();
        this.UploadChunk(chunkNum + 4, bytes, streamingUploadId, streamId, startTime, mediaType, successCallback, ackCallback);
      }));
    }

    public void StreamChunk(
      int chunkNum,
      byte[] chunkBytes,
      string streamingUploadId,
      string streamId,
      long startTime,
      string mediaType,
      Action<string> successCallback = null,
      Action ackCallback = null)
    {
      int num = chunkNum * 12000;
      int length = chunkBytes.Length;
      string base64String = Convert.ToBase64String(chunkBytes);
      BaseManager.Send("pixelupload", "upload_chunk", new Dictionary<string, object>()
      {
        {
          "ssid",
          (object) IMO.Dispatcher.GetSSID()
        },
        {
          "uid",
          (object) IMO.User.Uid
        },
        {
          "proto",
          (object) "imo"
        },
        {
          "stream_id",
          (object) streamId
        },
        {
          "streaming_upload_id",
          (object) streamingUploadId
        },
        {
          "data",
          (object) base64String
        },
        {
          "offset",
          (object) num
        },
        {
          "chunk_size",
          (object) length
        },
        {
          "total_size",
          (object) -1
        }
      }, onDispatcherAckCallback: (Action) (() =>
      {
        if (ackCallback == null)
          return;
        ackCallback();
      }));
    }

    public void FinishChunk(
      int fileSize,
      string streamingUploadId,
      string streamId,
      long startTime,
      string mediaType,
      Action<string> successCallback = null,
      Action ackCallback = null)
    {
      BaseManager.Send("pixelupload", "upload_chunk", new Dictionary<string, object>()
      {
        {
          "ssid",
          (object) IMO.Dispatcher.GetSSID()
        },
        {
          "uid",
          (object) IMO.User.Uid
        },
        {
          "proto",
          (object) "imo"
        },
        {
          "stream_id",
          (object) streamId
        },
        {
          "streaming_upload_id",
          (object) streamingUploadId
        },
        {
          "offset",
          (object) fileSize
        },
        {
          "data",
          (object) ""
        },
        {
          "total_size",
          (object) -1
        },
        {
          "chunk_size",
          (object) 12000
        }
      }, (Action<JToken>) (token =>
      {
        MediaUploader.LogUploadStatus(fileSize, startTime, "success", mediaType);
        try
        {
          if (token.Value<JToken>((object) "object_data") == null)
            return;
          string str = token.Value<JArray>((object) "object_data")[0].Value<string>((object) "object_id");
          if (successCallback == null)
            return;
          successCallback(str);
        }
        catch (Exception ex)
        {
          MediaUploader.log.Error(ex, "UploadChunk error in response", 260, nameof (FinishChunk));
        }
      }));
    }

    private static void LogUploadStatus(int fileSize, long startTime, string step, string type)
    {
      JObject eventsMap = new JObject();
      eventsMap.Add(step, (JToken) 1);
      eventsMap.Add("time_milis", (JToken) (Utils.GetTimestamp() - startTime));
      eventsMap.Add("file_size", (JToken) fileSize);
      eventsMap.Add(nameof (type), (JToken) type);
      if (type == "video/")
        IMO.MonitorLog.Log("upload_videos", eventsMap);
      else
        IMO.MonitorLog.Log("upload_photos", eventsMap);
    }
  }
}
