// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.Helpers.VideoUtils
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll


namespace ImoSilverlightApp.Helpers
{
  internal class VideoUtils
  {
    public static string GetVideoUrlFromId(string videoID)
    {
      return string.Format("s/object/{0}/?format=mp4", (object) videoID);
    }

    public static string GetVideoThumbnailUrlFromId(string videoID)
    {
      return "/s/object/" + videoID + "/?&format=thumbnail";
    }
  }
}
