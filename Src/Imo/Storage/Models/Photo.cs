// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.Storage.Models.Photo
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Windows;


namespace ImoSilverlightApp.Storage.Models
{
  public class Photo : ModelBase
  {
    [JsonProperty(PropertyName = "object_id")]
    private string photoId;
    [JsonProperty(PropertyName = "filename")]
    private string filename;
    [JsonProperty(PropertyName = "uploader")]
    private string uploader;
    [JsonProperty(PropertyName = "type")]
    private string type;
    private string buid;
    private int duration = -1;
    private int width = -1;
    private int height = -1;

    public Size Size { get; private set; }

    public Photo(string buid, JObject json)
    {
      this.buid = buid;
      JsonConvert.PopulateObject(json.ToString(), (object) this);
      JObject jobject = json.Value<JObject>((object) "properties");
      if (jobject == null)
        return;
      if (this.Type == PhotoType.Video)
      {
        if (jobject.Value<JToken>((object) nameof (duration)) == null)
          return;
        this.duration = jobject.Value<int>((object) nameof (duration));
      }
      else
      {
        if (jobject.Value<JToken>((object) nameof (width)) != null)
          this.width = jobject.Value<int>((object) nameof (width));
        if (jobject.Value<JToken>((object) nameof (height)) != null)
          this.height = jobject.Value<int>((object) nameof (height));
        this.Size = new Size((double) this.width, (double) this.height);
      }
    }

    public string Buid => this.buid;

    public string PhotoUrl
    {
      get => this.Type != PhotoType.Photo ? "" : Utils.GetIconPath(this.photoId, PictureSize.Large);
    }

    public string VideoUrl
    {
      get
      {
        return this.Type != PhotoType.Video ? (string) null : VideoUtils.GetVideoUrlFromId(this.photoId);
      }
    }

    public string ThumbnailUrl
    {
      get
      {
        return this.IsVideo ? VideoUtils.GetVideoThumbnailUrlFromId(this.photoId) : ImageUtils.GetPhotoThumbnailPath(this.photoId, PictureSize.Small);
      }
    }

    public string PhotoId => this.photoId;

    public string Filename => this.filename;

    public string Uploader => this.uploader;

    public PhotoType Type => !(this.type == "video") ? PhotoType.Photo : PhotoType.Video;

    public bool IsVideo => this.Type == PhotoType.Video;

    public int Duration => this.duration;

    public int Width => this.width;

    public int Height => this.height;
  }
}
