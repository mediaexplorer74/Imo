// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.Storage.Models.VideoMessage
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Helpers;
using Newtonsoft.Json.Linq;


namespace ImoSilverlightApp.Storage.Models
{
  internal class VideoMessage : Message
  {
    private const string TAG = "VideoMessage";
    private int width;
    private int height;
    private int orientation;
    private string thumbnailUrl;
    private string videoUrl;
    private string videoId;
    private bool isLarge;
    private string localPath;
    private int progressPercent;

    public int Width => this.width;

    public int Height => this.height;

    public int Orientation => this.orientation;

    public string ThumbnailUrl
    {
      get => this.thumbnailUrl;
      set
      {
        this.thumbnailUrl = value;
        this.OnPropertyChanged(nameof (ThumbnailUrl));
      }
    }

    public string VideoUrl
    {
      get => this.videoUrl;
      set
      {
        this.videoUrl = value;
        this.OnPropertyChanged(nameof (VideoUrl));
      }
    }

    public string VideoID
    {
      get => this.videoId;
      set
      {
        this.videoId = value;
        this.OnPropertyChanged(nameof (VideoID));
        this.VideoUrl = VideoUtils.GetVideoUrlFromId(this.videoId);
        this.ThumbnailUrl = VideoUtils.GetVideoThumbnailUrlFromId(this.videoId);
      }
    }

    public int ProgressPercent
    {
      get => this.progressPercent;
      set
      {
        if (this.progressPercent == value)
          return;
        this.progressPercent = value;
        this.OnPropertyChanged(nameof (ProgressPercent));
      }
    }

    public string LocalPath => this.localPath;

    public VideoMessage(MessageOrigin origin)
      : base(origin)
    {
    }

    protected override void Init()
    {
      this.msg = "sent a video";
      JArray jarray = this.imdata.Value<JArray>((object) "objects");
      this.videoId = "";
      JObject jobject1 = (JObject) null;
      foreach (JObject jobject2 in jarray)
      {
        this.videoId = jobject2.Value<string>((object) "object_id");
        jobject1 = jobject2.Value<JObject>((object) "type_specific_params");
        this.isLarge = jobject2.Value<int>((object) "filesize") == -1;
      }
      JToken jtoken;
      this.height = jobject1.TryGetValue("height", out jtoken) ? jtoken.Value<int>() : 720;
      this.width = jobject1.TryGetValue("width", out jtoken) ? jtoken.Value<int>() : 1280;
      this.orientation = jobject1.TryGetValue("orientation", out jtoken) ? jtoken.Value<int?>() ?? 0 : 0;
      this.localPath = this.imdata.Value<string>((object) "local_path");
      if (this.videoId == null)
        return;
      this.videoUrl = VideoUtils.GetVideoUrlFromId(this.videoId);
      this.thumbnailUrl = VideoUtils.GetVideoThumbnailUrlFromId(this.videoId);
      IMO.ImageLoader.PreloadImage(this.thumbnailUrl);
      IMO.VideoLoader.PreloadVideo(this.videoUrl);
    }

    public static VideoMessage MakeVideoMessage(string buid, string filePath = null)
    {
      int num1 = 280;
      int num2 = 200;
      JObject edata = new JObject();
      edata.Add(nameof (buid), (JToken) buid);
      edata.Add("msg", (JToken) "");
      JObject jobject = new JObject();
      jobject.Add("local_path", (JToken) filePath);
      edata.Add("imdata", (JToken) jobject);
      jobject.Add("objects", (JToken) new JArray()
      {
        (JToken) new JObject()
        {
          {
            "type_specific_params",
            (JToken) new JObject()
            {
              {
                "width",
                (JToken) num1
              },
              {
                "height",
                (JToken) num2
              },
              {
                "duration",
                (JToken) 0
              }
            }
          }
        }
      });
      VideoMessage videoMessage = new VideoMessage(MessageOrigin.SEND);
      videoMessage.PopulateFromEdata(edata);
      videoMessage.msg = "sending";
      return videoMessage;
    }

    internal void SetVideoId(string videoId, JObject specificParams)
    {
      this.videoId = videoId;
      JToken jtoken;
      this.width = specificParams.TryGetValue("width", out jtoken) ? jtoken.Value<int>() : 280;
      this.height = specificParams.TryGetValue("height", out jtoken) ? jtoken.Value<int>() : 200;
      this.orientation = specificParams.TryGetValue("orientation", out jtoken) ? jtoken.Value<int?>() ?? 0 : 0;
      this.videoUrl = VideoUtils.GetVideoUrlFromId(videoId);
      this.thumbnailUrl = VideoUtils.GetVideoThumbnailUrlFromId(videoId);
      this.OnPropertyChanged("");
    }
  }
}
