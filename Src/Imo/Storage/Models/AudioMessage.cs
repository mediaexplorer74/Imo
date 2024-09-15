// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.Storage.Models.AudioMessage
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Helpers;
using Newtonsoft.Json.Linq;
using System.IO;


namespace ImoSilverlightApp.Storage.Models
{
  internal class AudioMessage : Message
  {
    private const string TAG = "AudioMessage";
    private string audioId;
    private string audioUrl;
    private string localPath;
    private int[] amps;
    private int progressPercent;
    private string transcribed = "";

    public string Transcribed
    {
      get => this.transcribed;
      private set
      {
        this.transcribed = value;
        this.OnPropertyChanged(nameof (Transcribed));
      }
    }

    public string AudioUrl
    {
      get => this.audioUrl;
      private set
      {
        this.audioUrl = value;
        this.OnPropertyChanged(nameof (AudioUrl));
      }
    }

    public string AudioID
    {
      get => this.audioId;
      private set
      {
        this.audioId = value;
        this.OnPropertyChanged(nameof (AudioID));
      }
    }

    public int[] Amps
    {
      get => this.amps;
      private set
      {
        this.amps = value;
        this.OnPropertyChanged("amps");
      }
    }

    public bool IsLocal { get; private set; }

    public string LocalPath
    {
      get => this.localPath;
      private set
      {
        this.localPath = value;
        this.OnPropertyChanged(nameof (LocalPath));
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

    public AudioMessage(MessageOrigin origin)
      : base(origin)
    {
    }

    protected override void Init()
    {
      this.msg = "sent voice";
      JArray jarray1 = this.imdata.Value<JArray>((object) "objects");
      this.audioId = "";
      foreach (JObject jobject in jarray1)
      {
        this.AudioID = jobject.Value<string>((object) "object_id");
        jobject.Value<JObject>((object) "type_specific_params");
      }
      if (this.audioId != null)
        this.AudioUrl = VideoUtils.GetVideoUrlFromId(this.audioId);
      this.localPath = this.imdata.Value<string>((object) "local_path");
      this.IsLocal = !string.IsNullOrEmpty(this.localPath) && File.Exists(this.localPath);
      JArray jarray2 = this.imdata.Value<JArray>((object) "amps");
      if (jarray2 != null)
        this.Amps = jarray2.ToObject<int[]>();
      this.Transcribed = this.imdata.Value<string>((object) "transcribed");
      if (this.IsLocal)
        return;
      IMO.VideoLoader.PreloadVideo(this.audioUrl);
    }

    public static AudioMessage MakeAudioMessage(string buid, string filePath, int[] amps)
    {
      JObject edata = new JObject();
      edata.Add(nameof (buid), (JToken) buid);
      edata.Add("msg", (JToken) "");
      JObject jobject = new JObject();
      jobject.Add("local_path", (JToken) filePath);
      jobject.Add("msg_id", (JToken) Utils.GetRandomString(8));
      jobject.Add(nameof (amps), (JToken) new JArray((object) amps));
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
                "duration",
                (JToken) 0
              }
            }
          }
        }
      });
      AudioMessage audioMessage = new AudioMessage(MessageOrigin.SEND);
      audioMessage.PopulateFromEdata(edata);
      audioMessage.amps = amps;
      audioMessage.msg = "sending";
      return audioMessage;
    }

    internal void SetVideoId(string videoId, JObject specificParams)
    {
      this.audioId = videoId;
      foreach (JObject jobject in this.imdata.Value<JArray>((object) "objects"))
        jobject["object_id"] = (JToken) videoId;
    }
  }
}
