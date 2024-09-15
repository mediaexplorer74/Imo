// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.Constants
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using System;
using System.Collections.Generic;
using System.Text;


namespace ImoSilverlightApp
{
  public static class Constants
  {
    public const string USER_AGENT_NAME = "imoWindowsPhone";
    public const string PUSH_CHANNEL_NAME = "ImoSilverlightApp.PUSH_AND_VOIP";
    public const int COOKIE_LENGTH = 32;
    public const int UDID_LENGTH = 32;
    public const int IMO_API_LEVEL = 0;
    public const int FASTER_PORT_PRODUCTION = 443;
    public const int WARPY_PORT_PRODUCTON = 443;
    public const string PROTO_IMO = "imo";
    public const string AVCHAT_TYPE_VIDEO = "video_chat";
    public const string AVCHAT_TYPE_AUDIO = "audio_chat";
    public const string SYSTEM_PIN = "pin";
    public const string SYSTEM_IM = "im";
    public const string SYSTEM_PREFERENCE = "preference";
    public const string SYSTEM_IMOGROUPS = "imogroups";
    public const string MACAW_CLIENT_TYPE = "macaw";
    public const string DEFAULT_CLIENT_TYPE = "macaw";
    public const string INITIATE_CALL_CLIENT_TYPE = "macaw";
    public const string IMO_GOOGLE_PLAY_URL = "https://play.google.com/store/apps/details?id=com.imo.android.imoim";
    public const string IMO_ITUNES_URL = "https://itunes.apple.com/app/id336435697";
    public const string INVITE_TWITTER_URL = "https://twitter.com/home?status=I'm%20using%20%23imoim%20%23messenger%20to%20make%20free%20%23videocalls%20and%20%23chat%20on%20%23android,%20%23iOS%20and%20%23windows%20%23desktop%20Get%20it%20here%3A%20http%3A//imo.im   ";
    public const string INVITE_FACEBOOK_URL = "https://www.facebook.com/sharer/sharer.php?u=http%3A//imo.im";
    public const string STICKER_PACK_THUMBNAIL_PREFIX = "s/stickers/v1/packs/";
    public const string NOT_IN_CALL_MUTEX_NAME = "ImoSilverlightAppNotInCallHandle";
    public static readonly HashSet<string> EU_CCS = new HashSet<string>((IEnumerable<string>) new string[29]
    {
      "AT",
      "BE",
      "BG",
      "HR",
      "CY",
      "CZ",
      "DK",
      "EE",
      "FI",
      "FR",
      "DE",
      "GR",
      "HU",
      "IE",
      "IT",
      "LV",
      "LT",
      "LU",
      "MT",
      "NL",
      "PL",
      "PT",
      "RO",
      "SK",
      "SI",
      "ES",
      "SE",
      "GB",
      "CH"
    });
    public static readonly string[] FASTER_IPS;
    public static readonly string[] COGENT_IPS;
    public static readonly string[] NEW_IPS = new string[10]
    {
      "38.90.96.74",
      "38.90.96.75",
      "38.90.96.76",
      "38.90.96.77",
      "38.90.96.78",
      "38.90.96.109",
      "38.90.96.110",
      "38.90.96.111",
      "38.90.96.112",
      "38.90.96.113"
    };
    public static readonly string[] BBR_IPS = new string[4]
    {
      "192.12.31.81",
      "192.12.31.91",
      "192.12.31.101",
      "192.12.31.111"
    };
    public static readonly string[] IP_IP_IPS;
    public static readonly string[] NEW_IP_IP_IPS;
    public static readonly string[] NEW_IP_IP_IPS2;
    private static readonly string blob = "NTIuOC4xOTguMTkxLDU0LjY3LjI5LjE0Nyw1Mi44LjI0NS4xNDIsNTIuOC4xODQuMjEsNTIuOC43My4yLDUyLjguMTU2LjExNyw1Mi44LjE1NS41OSw1Mi44LjIwMC40MCw1Mi44LjIxMC4xNTUsNTIuOC41OS4xNDk=";
    private static readonly string blob2 = "NTIuOS4yMi4yNTIsNTIuOS40OC4xMTUsNTIuOS40OC4xMzYsNTIuOS40OC4xNTUsNTIuOS40OC4yMjQsNTIuOS40OS43LDUyLjkuNDkuNjAsNTIuOS40OS43OCw1Mi45LjQ5LjEwNSw1Mi45LjQ5LjEzNw==";
    private static readonly string blob3 = "MzguOTAuOTYuNzQsMzguOTAuOTYuNzksMzguOTAuOTYuODQsMzguOTAuOTYuODksMzguOTAuOTYuOTQsMzguOTAuOTYuOTksMzguOTAuOTYuMTA0LDM4LjkwLjk2LjEwOQ==";

    public static string GetImoHost() => "imo.im";

    static Constants()
    {
      int length1 = 40;
      Constants.FASTER_IPS = new string[length1];
      for (int index = 0; index < length1; ++index)
        Constants.FASTER_IPS[index] = "192.12.31." + (object) (74 + index);
      int length2 = 40;
      Constants.COGENT_IPS = new string[length2];
      for (int index = 0; index < length2; ++index)
        Constants.COGENT_IPS[index] = "38.90.96." + (object) (74 + index);
      byte[] bytes1 = Convert.FromBase64String(Constants.blob);
      Constants.IP_IP_IPS = Encoding.UTF8.GetString(bytes1, 0, bytes1.Length).Split(',');
      byte[] bytes2 = Convert.FromBase64String(Constants.blob2);
      Constants.NEW_IP_IP_IPS = Encoding.UTF8.GetString(bytes2, 0, bytes2.Length).Split(',');
      byte[] bytes3 = Convert.FromBase64String(Constants.blob3);
      Constants.NEW_IP_IP_IPS2 = Encoding.UTF8.GetString(bytes3, 0, bytes3.Length).Split(',');
    }
  }
}
