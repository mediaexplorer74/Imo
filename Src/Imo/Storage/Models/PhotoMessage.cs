// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.Storage.Models.PhotoMessage
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Helpers;
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.IO;
using System.Windows.Media.Imaging;


namespace ImoSilverlightApp.Storage.Models
{
  internal class PhotoMessage : Message
  {
    private static readonly Logger log = LogManager.GetLogger(typeof (PhotoMessage).Name);
    private string localPath;
    private int width;
    private bool isLocalImage;
    private string photoID;
    private int height;

    public string PhotoUrl
    {
      get
      {
        return this.IsLocalImage ? string.Format("file:///{0}", (object) this.LocalPath) : ImageUtils.GetPhotoUrlFromId(this.photoID, PictureSize.Large);
      }
    }

    public string PhotoUrlMedium
    {
      get
      {
        return this.IsLocalImage ? string.Format("file:///{0}", (object) this.LocalPath) : ImageUtils.GetPhotoUrlFromId(this.photoID, PictureSize.Medium);
      }
    }

    public string LocalPath => this.localPath;

    public int Height => this.height;

    public int Width => this.width;

    public bool IsLocalImage => this.isLocalImage;

    public string PhotoID => this.photoID;

    public void SetPhotoId(string photoID, JObject specificParams)
    {
      this.photoID = photoID;
      this.imdata["objects"] = (JToken) new JArray()
      {
        (JToken) new JObject()
        {
          {
            "object_id",
            (JToken) photoID
          },
          {
            "type_specific_params",
            (JToken) specificParams
          }
        }
      };
      this.Init();
    }

    protected override void Init()
    {
      this.msg = "sent a photo";
      JArray jarray = this.imdata.Value<JArray>((object) "objects");
      if (jarray != null)
      {
        JObject jobject1 = (JObject) jarray[0];
        JObject jobject2 = jobject1.Value<JObject>((object) "type_specific_params");
        this.photoID = jobject1.Value<string>((object) "object_id");
        this.width = jobject2.Value<int>((object) "original_width");
        this.height = jobject2.Value<int>((object) "original_height");
      }
      this.localPath = this.imdata.Value<string>((object) "local_path");
      this.isLocalImage = !string.IsNullOrEmpty(this.localPath) && File.Exists(this.localPath);
      if (this.IsLocalImage)
      {
        try
        {
          BitmapImage bitmapImage = new BitmapImage(new Uri(this.localPath));
          this.width = bitmapImage.PixelWidth;
          this.height = bitmapImage.PixelHeight;
        }
        catch (Exception ex)
        {
          PhotoMessage.log.Error(ex, "Error", 130, nameof (Init));
        }
      }
      else
        IMO.ImageLoader.PreloadImage(this.PhotoUrl);
      this.OnPropertyChanged((string) null);
    }

    public PhotoMessage(MessageOrigin origin)
      : base(origin)
    {
    }

    public static PhotoMessage MakePhotoMessage(string buid, string filePath)
    {
      JObject edata = new JObject();
      edata.Add(nameof (buid), (JToken) buid);
      edata.Add("msg", (JToken) "");
      edata.Add("imdata", (JToken) new JObject()
      {
        {
          "local_path",
          (JToken) filePath
        }
      });
      PhotoMessage photoMessage = new PhotoMessage(MessageOrigin.SEND);
      photoMessage.PopulateFromEdata(edata);
      photoMessage.msg = "sending";
      return photoMessage;
    }

    public static PhotoMessage MakePhotoMessage(
      string buid,
      string objectId,
      int width,
      int height)
    {
      JObject edata = new JObject();
      edata.Add(nameof (buid), (JToken) buid);
      edata.Add("msg", (JToken) "");
      JObject jobject1 = new JObject();
      edata.Add("imdata", (JToken) jobject1);
      PhotoMessage photoMessage = new PhotoMessage(MessageOrigin.SEND);
      JArray jarray = new JArray();
      JObject jobject2 = new JObject();
      JObject jobject3 = new JObject();
      jobject3.Add("original_width", (JToken) width);
      jobject3.Add("original_height", (JToken) height);
      jobject2.Add("object_id", (JToken) objectId);
      jobject2.Add("type_specific_params", (JToken) jobject3);
      jarray.Add((JToken) jobject2);
      jobject1.Add("objects", (JToken) jarray);
      photoMessage.PopulateFromEdata(edata);
      photoMessage.msg = "sending";
      return photoMessage;
    }
  }
}
