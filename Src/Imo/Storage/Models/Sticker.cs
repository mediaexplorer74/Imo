// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.Storage.Models.Sticker
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Helpers;
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;


namespace ImoSilverlightApp.Storage.Models
{
  public class Sticker
  {
    private static readonly Logger log = LogManager.GetLogger(typeof (Sticker).Name);
    private string stickerId;
    private string name;
    private int width;
    private int height;
    private bool animated;
    private int framesCount;
    private int framesInterval;
    private int[] intervals;
    private bool isAnimating;

    public string StickerId => this.stickerId;

    public string Name => this.name;

    public int Width => this.width;

    public int Height => this.height;

    public bool Animated => this.animated;

    public int FramesCount => this.framesCount;

    public int FramesInterval => this.framesInterval;

    public int[] Intervals => this.intervals;

    public bool IsAnimating => this.isAnimating;

    public Sticker(string stickerId, string name, int width, int height)
    {
      this.stickerId = stickerId;
      this.name = name;
      this.width = width;
      this.height = height;
      this.animated = false;
    }

    public Sticker(
      string stickerId,
      string name,
      int width,
      int height,
      bool animated,
      int framesCount,
      int framesInterval)
    {
      this.stickerId = stickerId;
      this.name = name;
      this.width = width;
      this.height = height;
      this.animated = animated;
      this.framesCount = framesCount;
      this.framesInterval = framesInterval;
    }

    public Sticker(
      string stickerId,
      string name,
      int width,
      int height,
      bool animated,
      int framesCount,
      int[] intervals)
    {
      this.stickerId = stickerId;
      this.name = name;
      this.width = width;
      this.height = height;
      this.animated = animated;
      this.framesCount = framesCount;
      this.intervals = intervals;
    }

    public JObject GetIMData()
    {
      JObject imData = new JObject();
      imData.Add("type", (JToken) "sticker");
      imData.Add("name", (JToken) this.name);
      imData.Add("sticker_id", (JToken) this.stickerId);
      imData.Add("width", (JToken) this.width);
      imData.Add("height", (JToken) this.height);
      imData.Add("animated", (JToken) this.animated);
      if (this.animated)
      {
        imData.Add("num_frames", (JToken) this.framesCount);
        if (this.framesInterval != 0)
          imData.Add("frame_interval", (JToken) this.framesInterval);
        else if (this.intervals != null)
        {
          JArray jarray = new JArray();
          for (int index = 0; index < this.intervals.Length; ++index)
            jarray.Add((JToken) this.intervals[index]);
          imData.Add("intervals", (JToken) jarray);
        }
      }
      return imData;
    }

    public TimeSpan GetDuration()
    {
      if (this.framesInterval != 0)
        return TimeSpan.FromMilliseconds((double) (this.framesInterval * this.framesCount));
      return this.intervals != null ? TimeSpan.FromMilliseconds((double) ((IEnumerable<int>) this.intervals).Sum()) : TimeSpan.Zero;
    }

    public TimeSpan GetFrameDuration(int i)
    {
      if (this.framesInterval != 0)
        return TimeSpan.FromMilliseconds((double) this.framesInterval);
      return this.intervals != null && this.intervals.Length > i ? TimeSpan.FromMilliseconds((double) this.intervals[i]) : TimeSpan.Zero;
    }

    public static Sticker GetSticker(JObject sticker)
    {
      try
      {
        string stickerId = sticker.Value<string>((object) "sticker_id");
        string name = sticker.Value<string>((object) "name") ?? "";
        int width = sticker.Value<int>((object) "width");
        int height = sticker.Value<int>((object) "height");
        bool animated = "true".Equals(sticker.Value<string>((object) "animated").ToLower());
        if (!animated)
          return new Sticker(stickerId, name, width, height);
        int framesCount = sticker.Value<int>((object) "num_frames");
        if (sticker["frame_interval"] != null)
        {
          int framesInterval = sticker.Value<int>((object) "frame_interval");
          return new Sticker(stickerId, name, width, height, animated, framesCount, framesInterval);
        }
        if (sticker["intervals"] != null)
        {
          JArray jarray = sticker.Value<JArray>((object) "intervals");
          int[] intervals = new int[framesCount];
          for (int index = 0; index < framesCount; ++index)
            intervals[index] = jarray[index].Value<int>();
          return new Sticker(stickerId, name, width, height, animated, framesCount, intervals);
        }
        Sticker.log.Error("Animated sticker doesn't have frame_interval or intervals", 222, nameof (GetSticker));
      }
      catch (Exception ex)
      {
        Sticker.log.Error(ex, 227, nameof (GetSticker));
      }
      return (Sticker) null;
    }

    public string Url
    {
      get
      {
        return StickersUtils.GetSourceUrl(StickerResource.stickers, this.stickerId, StickerSize.sticker);
      }
    }
  }
}
