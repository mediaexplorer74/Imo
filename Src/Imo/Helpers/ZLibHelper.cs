// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.Helpers.ZLibHelper
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using Ionic.Zlib;
using NLog;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace ImoSilverlightApp.Helpers
{
  internal class ZLibHelper
  {
    private static readonly Logger log = LogManager.GetLogger(typeof (ZLibHelper).Name);
    private const string nc2 = "{\"headers\":{\"key2\":\"\",\"compression\":[\"zlib\"],\"no_b64\":true},\"data\":{\"name\":\"wsHXm.0\"},\"method\":\"name_channel\"}";
    private const string keep_alive = "{\"method\":\"keep_alive\"}";
    private const string keep_alive2 = "{\"headers\":{\"route_num\":23,\"c\":{\"UDID\":\"\",\"iat\":\"\"},\"ua\":\"imoAndroid/7.3.4; 4.2.2; REL; GT-I9195; samsung; play; phone\"},\"data\":{\"ack\":2,\"ssid\":\"\",\"messages\":[{\"to\":{\"system\":\"dispatcher\"},\"data\":{\"data\":{\"ssid\":\"\"},\"method\":\"keep_alive\"},\"seq\":63,\"from\":{\"system\":\"client\",\"ssid\":\"\"}}]},\"method\":\"forward_to_server\"}";
    private const string ack2 = "{\"data\":{\"ack\":2,\"ssid\":\"\",\"messages\":[]},\"method\":\"forward_to_server\"}";
    private const string cookie2 = "{\"headers\":{\"route_num\":1,\"c\":{\"UDID\":\"\",\"iat\":\"\"},\"ua\":\"imoAndroid/7.3.0; 4.1.2; REL; GT-I9300; samsung; play; phone\"},\"data\":{\"ack\":0,\"ssid\":\"\",\"messages\":[{\"to\":{\"system\":\"session\"},\"data\":{\"data\":{\"active\":false,\"ssid\":\"\",\"lang\":\"en-GB\"},\"method\":\"cookie_login\"},\"seq\":0,\"from\":{\"system\":\"client\",\"ssid\":\"\"}}]},\"method\":\"forward_to_server\"}";
    private const string ka2 = "{\"data\":{\"ack\":1,\"ssid\":\"\",\"messages\":[{\"to\":{\"system\":\"dispatcher\"},\"data\":{\"data\":{\"ssid\":\"\"},\"method\":\"keep_alive\"},\"seq\":4,\"from\":{\"system\":\"client\",\"ssid\":\"\"}}]},\"method\":\"forward_to_server\"}";
    private const string inv2 = "{\"data\":{\"ack\":4,\"ssid\":\"\",\"messages\":[{\"to\":{\"system\":\"monitor\"},\"data\":{\"data\":{\"events\":[{\"data\":{\"result\":\"SMS generic failure\",\"phone\":\"\",\"inviter_phone\":\"+\",\"is_resend\":true,\"data\":\" 1423871975351\",\"batch_size\":-1,\"num_tries\":2,\"message\":\"Let's video chat and text on imo! Get the free app http://imo.im\",\"clean_phone\":\"\",\"carrier_code\":\"\",\"inviter_uid\":\"\",\"errorCode\":28,\"delay\":474544,\"start_time_ms\":1423871975351,\"carrier_name\":\"VIVO\",\"phone_cc\":\"br\"},\"namespace\":\"sms_delivery\"}],\"ssid\":\"\"},\"method\":\"log_event\"},\"seq\":692,\"from\":{\"system\":\"client\",\"ssid\":\"\"}}]},\"method\":\"forward_to_server\"}";
    private const string open = "{\"data\":{\"ack\":2,\"ssid\":\"\",\"messages\":[{\"to\":{\"system\":\"im\"},\"data\":{\"data\":{\"uid\":\"\",\"buid\":\"\",\"ssid\":\"\",\"proto\":\"imo\"},\"method\":\"open_chat\"},\"seq\":2,\"from\":{\"system\":\"client\",\"ssid\":\"\"}}]},\"method\":\"forward_to_server\"}";
    private const string unread = "{\"data\":{\"ack\":1,\"ssid\":\"\",\"messages\":[{\"to\":{\"system\":\"im\"},\"data\":{\"data\":{\"uid\":\"\",\"ssid\":\"\",\"proto\":\"imo\"},\"method\":\"get_unread_msgs\"},\"seq\":1,\"from\":{\"system\":\"client\",\"ssid\":\"\"}}]},\"method\":\"forward_to_server\"}";
    public static byte[] CLIENT_TO_SERVER_DICT = Encoding.UTF8.GetBytes("{\"data\":{\"ack\":4,\"ssid\":\"\",\"messages\":[{\"to\":{\"system\":\"monitor\"},\"data\":{\"data\":{\"events\":[{\"data\":{\"result\":\"SMS generic failure\",\"phone\":\"\",\"inviter_phone\":\"+\",\"is_resend\":true,\"data\":\" 1423871975351\",\"batch_size\":-1,\"num_tries\":2,\"message\":\"Let's video chat and text on imo! Get the free app http://imo.im\",\"clean_phone\":\"\",\"carrier_code\":\"\",\"inviter_uid\":\"\",\"errorCode\":28,\"delay\":474544,\"start_time_ms\":1423871975351,\"carrier_name\":\"VIVO\",\"phone_cc\":\"br\"},\"namespace\":\"sms_delivery\"}],\"ssid\":\"\"},\"method\":\"log_event\"},\"seq\":692,\"from\":{\"system\":\"client\",\"ssid\":\"\"}}]},\"method\":\"forward_to_server\"}{\"data\":{\"ack\":2,\"ssid\":\"\",\"messages\":[{\"to\":{\"system\":\"im\"},\"data\":{\"data\":{\"uid\":\"\",\"buid\":\"\",\"ssid\":\"\",\"proto\":\"imo\"},\"method\":\"open_chat\"},\"seq\":2,\"from\":{\"system\":\"client\",\"ssid\":\"\"}}]},\"method\":\"forward_to_server\"}{\"headers\":{\"route_num\":23,\"c\":{\"UDID\":\"\",\"iat\":\"\"},\"ua\":\"imoAndroid/7.3.4; 4.2.2; REL; GT-I9195; samsung; play; phone\"},\"data\":{\"ack\":2,\"ssid\":\"\",\"messages\":[{\"to\":{\"system\":\"dispatcher\"},\"data\":{\"data\":{\"ssid\":\"\"},\"method\":\"keep_alive\"},\"seq\":63,\"from\":{\"system\":\"client\",\"ssid\":\"\"}}]},\"method\":\"forward_to_server\"}{\"method\":\"keep_alive\"}{\"data\":{\"ack\":1,\"ssid\":\"\",\"messages\":[{\"to\":{\"system\":\"im\"},\"data\":{\"data\":{\"uid\":\"\",\"ssid\":\"\",\"proto\":\"imo\"},\"method\":\"get_unread_msgs\"},\"seq\":1,\"from\":{\"system\":\"client\",\"ssid\":\"\"}}]},\"method\":\"forward_to_server\"}{\"headers\":{\"route_num\":1,\"c\":{\"UDID\":\"\",\"iat\":\"\"},\"ua\":\"imoAndroid/7.3.0; 4.1.2; REL; GT-I9300; samsung; play; phone\"},\"data\":{\"ack\":0,\"ssid\":\"\",\"messages\":[{\"to\":{\"system\":\"session\"},\"data\":{\"data\":{\"active\":false,\"ssid\":\"\",\"lang\":\"en-GB\"},\"method\":\"cookie_login\"},\"seq\":0,\"from\":{\"system\":\"client\",\"ssid\":\"\"}}]},\"method\":\"forward_to_server\"}{\"data\":{\"ack\":1,\"ssid\":\"\",\"messages\":[{\"to\":{\"system\":\"dispatcher\"},\"data\":{\"data\":{\"ssid\":\"\"},\"method\":\"keep_alive\"},\"seq\":4,\"from\":{\"system\":\"client\",\"ssid\":\"\"}}]},\"method\":\"forward_to_server\"}{\"headers\":{\"key2\":\"\",\"compression\":[\"zlib\"],\"no_b64\":true},\"data\":{\"name\":\"wsHXm.0\"},\"method\":\"name_channel\"}{\"data\":{\"ack\":2,\"ssid\":\"\",\"messages\":[]},\"method\":\"forward_to_server\"}");
    private const string nc = "{\"headers\":{\"heartbeat_new\":true,\"compression\":[\"zlib\"]},\"data\":{\"name\":\"Faster.1\"},\"method\":\"name_channel\"}";
    private const string ack = "{\"headers\":{\"http_headers\":{}},\"data\":{\"ack\":1,\"messages\":[]},\"method\":null}";
    private const string cookie = "{\"headers\":{\"http_headers\":{}},\"data\":{\"ack\":1,\"messages\":[{\"to\":{\"system\":\"client\",\"ssid\":\"2MdbftmOBo8bpV8sk\"},\"seq\":0,\"data\":{\"uid\":\"1007494312522139\",\"name\":\"signed_on\",\"edata\":{\"signup_date\":1424741225,\"alias\":\"Bbb\",\"is_email_verified\":false,\"premium\":false,\"profile_created\":true,\"inviter_client_select_all\":false,\"state\":\"active\",\"inviter_show_select_all\":false,\"verified_phone\":\"+16508629140\",\"premium_exp_date\":null,\"show_pin_tutorial\":false,\"username\":\"None\",\"premium_subscribed\":false,\"is_activated\":true,\"email\":null,\"invites_left\":0,\"max_points\":50,\"show_meet_new_people\":false,\"points\":50,\"is_phone_verified\":true,\"inviter_preselected\":10,\"phone_cc\":\"us\"},\"type\":\"account\",\"proto\":\"imo\"},\"from\":{\"system\":\"im\"}}]},\"method\":null}";
    private const string gotunread = "{\"headers\":{\"http_headers\":{}},\"data\":{\"ack\":2,\"messages\":[{\"to\":{\"system\":\"client\",\"ssid\":\"2MdbftmOBo8bpV8sk\"},\"seq\":1,\"data\":{\"uid\":\"1007494312522139\",\"name\":\"recv_unread_msgs\",\"edata\":{},\"type\":\"conv\",\"proto\":\"imo\"},\"from\":{\"system\":\"im\"}}]},\"method\":null}";
    private const string chatopen = "{\"headers\":{\"http_headers\":{}},\"data\":{\"ack\":7,\"messages\":[{\"to\":{\"system\":\"client\",\"ssid\":\"2MdbftmOBo8bpV8sk\"},\"seq\":4,\"data\":{\"uid\":\"1007494312522139\",\"name\":\"chat_opened\",\"edata\":{\"buid\":\"1007494312522139\"},\"type\":\"conv\",\"proto\":\"imo\"},\"from\":{\"system\":\"im\"}}]},\"method\":null}";
    public static byte[] SERVER_TO_CLIENT_DICT = Encoding.UTF8.GetBytes("{\"headers\":{\"http_headers\":{}},\"data\":{\"ack\":7,\"messages\":[{\"to\":{\"system\":\"client\",\"ssid\":\"2MdbftmOBo8bpV8sk\"},\"seq\":4,\"data\":{\"uid\":\"1007494312522139\",\"name\":\"chat_opened\",\"edata\":{\"buid\":\"1007494312522139\"},\"type\":\"conv\",\"proto\":\"imo\"},\"from\":{\"system\":\"im\"}}]},\"method\":null}{\"headers\":{\"http_headers\":{}},\"data\":{\"ack\":2,\"messages\":[{\"to\":{\"system\":\"client\",\"ssid\":\"2MdbftmOBo8bpV8sk\"},\"seq\":1,\"data\":{\"uid\":\"1007494312522139\",\"name\":\"recv_unread_msgs\",\"edata\":{},\"type\":\"conv\",\"proto\":\"imo\"},\"from\":{\"system\":\"im\"}}]},\"method\":null}{\"headers\":{\"http_headers\":{}},\"data\":{\"ack\":1,\"messages\":[{\"to\":{\"system\":\"client\",\"ssid\":\"2MdbftmOBo8bpV8sk\"},\"seq\":0,\"data\":{\"uid\":\"1007494312522139\",\"name\":\"signed_on\",\"edata\":{\"signup_date\":1424741225,\"alias\":\"Bbb\",\"is_email_verified\":false,\"premium\":false,\"profile_created\":true,\"inviter_client_select_all\":false,\"state\":\"active\",\"inviter_show_select_all\":false,\"verified_phone\":\"+16508629140\",\"premium_exp_date\":null,\"show_pin_tutorial\":false,\"username\":\"None\",\"premium_subscribed\":false,\"is_activated\":true,\"email\":null,\"invites_left\":0,\"max_points\":50,\"show_meet_new_people\":false,\"points\":50,\"is_phone_verified\":true,\"inviter_preselected\":10,\"phone_cc\":\"us\"},\"type\":\"account\",\"proto\":\"imo\"},\"from\":{\"system\":\"im\"}}]},\"method\":null}{\"headers\":{\"http_headers\":{}},\"data\":{\"ack\":1,\"messages\":[]},\"method\":null}{\"method\":\"keep_alive\"}{\"headers\":{\"heartbeat_new\":true,\"compression\":[\"zlib\"]},\"data\":{\"name\":\"Faster.1\"},\"method\":\"name_channel\"}");
    private const int bufferSize = 16384;
    private static byte[] decompressBuffer = new byte[16384];
    private static byte[] compressBuffer = new byte[16384];

    public static string Decompress(byte[] compressedBytes)
    {
      ZlibCodec z = new ZlibCodec();
      z.InputBuffer = compressedBytes;
      z.NextIn = 0;
      z.AvailableBytesIn = compressedBytes.Length;
      int err1 = z.InitializeInflate();
      ZLibHelper.CheckForError(z, err1, "inflateInit");
      MemoryStream memoryStream = new MemoryStream();
      do
      {
        z.OutputBuffer = ZLibHelper.decompressBuffer;
        z.NextOut = 0;
        z.AvailableBytesOut = ZLibHelper.decompressBuffer.Length;
        int num = z.Inflate(FlushType.Finish);
        switch (num)
        {
          case 0:
          case 1:
          case 2:
            if (num == 2)
            {
              int err2 = z.SetDictionary(ZLibHelper.SERVER_TO_CLIENT_DICT);
              ZLibHelper.CheckForError(z, err2, "inflateInit");
            }
            memoryStream.Write(ZLibHelper.decompressBuffer, 0, ZLibHelper.decompressBuffer.Length - z.AvailableBytesOut);
            continue;
          default:
            ZLibHelper.log.Error("inflate should report Z_STREAM_END or Z_NEED_DICT", 67, nameof (Decompress));
            Task.Delay(3000).Wait();
            Application.Current.Terminate();
            goto case 0;
        }
      }
      while (z.AvailableBytesIn != 0 || z.AvailableBytesOut == 0);
      int err3 = z.EndInflate();
      ZLibHelper.CheckForError(z, err3, "EndInflate");
      byte[] array = memoryStream.ToArray();
      memoryStream.Dispose();
      return Encoding.UTF8.GetString(((IEnumerable<byte>) array).ToArray<byte>(), 0, array.Length);
    }

    internal static void CheckForError(ZlibCodec z, int err, string msg)
    {
      if (err == 0)
        return;
      string message = z.Message;
      ZLibHelper.log.Error("message: " + msg + " error: " + (object) err, 106, nameof (CheckForError));
      Task.Delay(3000).Wait();
      Application.Current.Terminate();
    }

    public static byte[] Compress(byte[] input)
    {
      ZlibCodec z = new ZlibCodec();
      int err1 = z.InitializeDeflate();
      ZLibHelper.CheckForError(z, err1, "InitializeDeflate");
      int err2 = z.SetDictionary(ZLibHelper.CLIENT_TO_SERVER_DICT);
      ZLibHelper.CheckForError(z, err2, "SetDeflateDictionary");
      int adler32 = z.Adler32;
      z.InputBuffer = input;
      z.NextIn = 0;
      z.AvailableBytesIn = input.Length;
      MemoryStream memoryStream = new MemoryStream();
      do
      {
        z.OutputBuffer = ZLibHelper.compressBuffer;
        z.NextOut = 0;
        z.AvailableBytesOut = 16384;
        switch (z.Deflate(FlushType.Finish))
        {
          case 0:
          case 1:
            memoryStream.Write(ZLibHelper.compressBuffer, 0, ZLibHelper.compressBuffer.Length - z.AvailableBytesOut);
            continue;
          default:
            ZLibHelper.log.Error("deflate should report Z_STREAM_END", 140, nameof (Compress));
            Task.Delay(3000).Wait();
            Application.Current.Terminate();
            goto case 0;
        }
      }
      while (z.AvailableBytesIn != 0 || z.AvailableBytesOut == 0);
      z.EndDeflate();
      byte[] array = memoryStream.ToArray();
      memoryStream.Dispose();
      return array;
    }
  }
}
