// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.Managers.Pixel
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Helpers;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;


namespace ImoSilverlightApp.Managers
{
  public class Pixel : BaseManager
  {
    public void DeletePhoto(string buid, string photoID, Action<JToken> callback = null)
    {
      Dictionary<string, object> data = new Dictionary<string, object>();
      MessageFactory.AddSsidUidProto(data);
      data.Add("stream_id", (object) Utils.GenerateStreamId(IMO.User.Uid, buid));
      data.Add("object_ids", (object) new string[1]
      {
        photoID
      });
      BaseManager.Send("pixel", "delete_objects", data, callback);
    }

    internal void SharePhoto(string buddyKey, string photoID, Action<JToken> callback = null)
    {
      BaseManager.Send("pixel", "copy_objects", new Dictionary<string, object>()
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
          (object) Utils.GenerateStreamId(IMO.User.Uid, buddyKey)
        },
        {
          "send_reflect",
          (object) false
        },
        {
          "object_ids",
          (object) new string[1]{ photoID }
        }
      }, callback);
    }

    internal void ShareVideo(string buid, string photoID, Action<JToken> callback = null)
    {
      BaseManager.Send("pixel", "copy_objects", new Dictionary<string, object>()
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
          (object) Utils.GenerateStreamId(IMO.User.Uid, buid)
        },
        {
          "send_reflect",
          (object) false
        },
        {
          "object_ids",
          (object) new string[1]{ photoID }
        },
        {
          "imdata",
          (object) new Dictionary<string, object>()
          {
            {
              "is_video_message",
              (object) true
            }
          }
        }
      }, callback);
    }

    public async void UploadOwnProfile(string path, Action successCallback = null)
    {
      string streamId = "profile:" + IMO.User.Uid;
      string resizedImage = await ImageUtils.GetResizedImage(path);
      FileInfo file = new FileInfo(resizedImage);
      byte[] inArray = await FSUtils.ReadFileAsync(resizedImage);
      if (inArray == null)
      {
        successCallback();
      }
      else
      {
        string base64String = Convert.ToBase64String(inArray, 0, inArray.Length);
        BaseManager.Send("pixelupload", "upload", new Dictionary<string, object>()
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
            "filenames",
            (object) new string[1]{ file.Name }
          },
          {
            "files_base64",
            (object) new string[1]{ base64String }
          },
          {
            "send_reflect",
            (object) false
          }
        }, (Action<JToken>) (result =>
        {
          IMO.User.ProfilePhotoId = ((JArray) result)[0].Value<string>((object) "object_id");
          IMO.MonitorLog.Log("upload_profile_pic", "success");
          if (successCallback == null)
            return;
          successCallback();
        }));
        IMO.MonitorLog.Log("upload_profile_pic", "attempt");
      }
    }

    public async void UploadGroupProfile(string path, string gid, Action<JToken> successCallback = null)
    {
      string streamId = "gicon:" + gid;
      string resizedImage = await ImageUtils.GetResizedImage(path);
      FileInfo file = new FileInfo(resizedImage);
      byte[] inArray = await FSUtils.ReadFileAsync(resizedImage);
      string base64String = Convert.ToBase64String(inArray, 0, inArray.Length);
      BaseManager.Send("pixelupload", "upload", new Dictionary<string, object>()
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
          "filenames",
          (object) new string[1]{ file.Name }
        },
        {
          "files_base64",
          (object) new string[1]{ base64String }
        },
        {
          "send_reflect",
          (object) false
        }
      }, (Action<JToken>) (result =>
      {
        if (successCallback != null)
          successCallback(result);
        IMO.MonitorLog.Log("upload_group_pic", "success");
      }));
      IMO.MonitorLog.Log("upload_group_pic", "attempt");
    }

    public void DownloadChunk(JObject data, Action<JToken> successCallback = null)
    {
      BaseManager.Send("pixeldownload", "get_photo", data, successCallback);
    }

    public void GetObjects(string buid, Action<JToken> callback)
    {
      BaseManager.Send("pixel", "get_objects", new Dictionary<string, object>()
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
          (object) Utils.GenerateStreamId(IMO.User.Uid, buid)
        }
      }, callback);
    }
  }
}
