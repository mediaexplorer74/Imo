// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.Helpers.Utils
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Storage.Models;
using Microsoft.Phone.Net.NetworkInformation;
using NLog;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Windows.ApplicationModel.Core;
using Windows.Networking.Connectivity;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;
using Windows.System;
using Windows.UI.Core;


namespace ImoSilverlightApp.Helpers
{
  public static class Utils
  {
    private static readonly Logger log = LogManager.GetLogger(typeof (Utils).Name);
    private const string PUBLIC_KEY_1024 = "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCYAoB3djV8hefWBBQIEuq54qDke4wWek/WulorSc1FOk6Gt9tu50swlxN1kXpCQDfZPGlNASY0UxXKZ0qKqaO2+ZW3CpDp+UYsiARBbEJ3y47HJ1zPVsEq2mMGUnz++35DXzdexmIQbOR4aMbSzXwBAbf+ZaggECe5V21U65tyzQIDAQAB";
    private static string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
    private static Random random = new Random();

    public static string GetRandomString(int length)
    {
      return new string(Enumerable.Repeat<string>(Utils.chars, length).Select<string, char>((Func<string, char>) (s => s[Utils.GetRandomInt(s.Length)])).ToArray<char>());
    }

    public static string GetUnsafeRandomString(int length)
    {
      return new string(Enumerable.Repeat<string>(Utils.chars, length).Select<string, char>((Func<string, char>) (s => s[Utils.GetUnsafeRandomInt(s.Length)])).ToArray<char>());
    }

    public static int GetRandomInt(int maxValue)
    {
      byte[] numArray = new byte[4];
      uint uint32;
      do
      {
        CryptographicBuffer.CopyToByteArray(CryptographicBuffer.GenerateRandom((uint) numArray.Length), ref numArray);
        uint32 = BitConverter.ToUInt32(numArray, 0);
      }
      while ((long) uint32 >= (long) uint.MaxValue / (long) maxValue * (long) maxValue);
      return (int) (uint32 % (uint) maxValue);
    }

    public static int GetUnsafeRandomInt(int maxValue) => Utils.random.Next(maxValue);

    public static string EncryptSecretKey(byte[] secretKey)
    {
      IBuffer ibuffer = CryptographicBuffer.DecodeFromBase64String("MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCYAoB3djV8hefWBBQIEuq54qDke4wWek/WulorSc1FOk6Gt9tu50swlxN1kXpCQDfZPGlNASY0UxXKZ0qKqaO2+ZW3CpDp+UYsiARBbEJ3y47HJ1zPVsEq2mMGUnz++35DXzdexmIQbOR4aMbSzXwBAbf+ZaggECe5V21U65tyzQIDAQAB");
      byte[] inArray;
      CryptographicBuffer.CopyToByteArray(CryptographicEngine.Encrypt(AsymmetricKeyAlgorithmProvider.OpenAlgorithm(AsymmetricAlgorithmNames.RsaOaepSha1).ImportPublicKey(ibuffer, (CryptographicPublicKeyBlobType) 0), CryptographicBuffer.CreateFromByteArray(secretKey), (IBuffer) null), ref inArray);
      return Utils.InsertLineBreaks(Convert.ToBase64String(inArray));
    }

    private static string InsertLineBreaks(string str)
    {
      StringBuilder stringBuilder = new StringBuilder();
      for (int startIndex = 0; startIndex < str.Length; startIndex += 76)
      {
        stringBuilder.Append(str.Substring(startIndex, Math.Min(76, str.Length - startIndex)));
        stringBuilder.Append('\n');
      }
      return stringBuilder.ToString();
    }

    public static byte[] ReadStreamFully(Stream stream, int streamLength = 2147483647)
    {
      int val2 = streamLength;
      byte[] buffer = new byte[2048];
      using (MemoryStream memoryStream = new MemoryStream())
      {
        int count;
        do
        {
          count = stream.Read(buffer, 0, Math.Min(buffer.Length, val2));
          memoryStream.Write(buffer, 0, count);
          val2 -= count;
        }
        while (count != 0 && val2 > 0);
        return memoryStream.ToArray();
      }
    }

    public static string NormalizePhoneNumber(string phoneNumber)
    {
      return string.IsNullOrEmpty(phoneNumber) ? string.Empty : phoneNumber.Replace("(", "").Replace(")", "").Replace(" ", "").Replace("-", "").Replace("#", "").Replace("/", "");
    }

    public static string GenerateStreamId(string uid, string buid)
    {
      string gid = Utils.GetGid(buid);
      string streamId;
      if (Contact.IsGroupBuid(buid))
      {
        streamId = "groups:" + gid;
      }
      else
      {
        string str1 = uid;
        string strB = gid;
        if (str1.CompareTo(strB) > 0)
        {
          string str2 = str1;
          str1 = strB;
          strB = str2;
        }
        streamId = "pair:imo#" + str1 + "#" + strB;
      }
      return streamId;
    }

    private static int Clip(int a)
    {
      if (a > (int) byte.MaxValue)
        return (int) byte.MaxValue;
      return a >= 0 ? a : 0;
    }

    private static int GetPixel(byte y, byte u, byte v)
    {
      return -16777216 ^ Utils.Clip((int) y + (91811 * (int) v >> 16) - 179) << 16 ^ Utils.Clip((int) y - (22544 * (int) u + 46793 * (int) v >> 16) + 135) << 8 ^ Utils.Clip((int) y + (116129 * (int) u >> 16) - 226);
    }

    internal static void ConvertNv12ToArgb(int width, int height, byte[] nv12, int[] rgb)
    {
      int num1 = 0;
      int num2 = width * height;
      for (int index1 = 0; index1 < height; ++index1)
      {
        for (int index2 = 0; index2 < width; ++index2)
          rgb[num1 + index2] = Utils.GetPixel(nv12[num1 + index2], nv12[num2 + (index2 & -2)], nv12[num2 + (index2 | 1)]);
        num1 += width;
        num2 += index1 % 2 * width;
      }
    }

    public static string GetGid(string buid) => buid.Split(';')[0];

    internal static async Task ShowPopup(string message)
    {
      ImoMessageBoxResult messageBoxResult = await ImoMessageBox.Show(message);
    }

    public static long GetTimestamp()
    {
      return (long) DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
    }

    public static DateTime TimestampToDateTime(long timestamp_ms)
    {
      DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).Add(TimeSpan.FromMilliseconds((double) timestamp_ms));
      dateTime = dateTime.ToLocalTime();
      return dateTime;
    }

    public static string ToFormattedTimestamp(long timestamp_ms)
    {
      DateTime dateTime1 = Utils.TimestampToDateTime(timestamp_ms);
      if (!(DateTime.Today != dateTime1.Date))
        return dateTime1.ToShortTimeString();
      DateTime dateTime2 = DateTime.Today;
      int year1 = dateTime2.Year;
      dateTime2 = dateTime1.Date;
      int year2 = dateTime2.Year;
      return year1 != year2 ? dateTime1.ToString("MMM d, yyyy") : dateTime1.ToString("MMM d, ") + dateTime1.ToShortTimeString();
    }

    public static string ToFormattedTimespan(long span_ms)
    {
      TimeSpan timeSpan = TimeSpan.FromMilliseconds((double) span_ms);
      return string.Format("{0}d {1}h {2}m {3}s", (object) timeSpan.Days, (object) timeSpan.Hours, (object) timeSpan.Minutes, (object) timeSpan.Seconds);
    }

    internal static string ToFullFormattedTimestamp(long timestamp_ms)
    {
      DateTime dateTime = Utils.TimestampToDateTime(timestamp_ms);
      return dateTime.ToString("MMM d, yyyy, ") + dateTime.ToLongTimeString();
    }

    internal static string ToFormattedSystemTimestamp(long timestamp_ms)
    {
      return Utils.TimestampToDateTime(timestamp_ms).ToString("hh:mm:ss.fff dd/MM/yyyy", (IFormatProvider) CultureInfo.InvariantCulture);
    }

    public static string ToDateTimeString(long timestamp_ms)
    {
      DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).Add(TimeSpan.FromMilliseconds((double) timestamp_ms));
      dateTime = dateTime.ToLocalTime();
      return dateTime.ToShortDateString() + " " + dateTime.ToShortTimeString();
    }

    public static byte[] GetRandomBytes(int length)
    {
      byte[] buffer = new byte[length];
      Utils.random.NextBytes(buffer);
      return buffer;
    }

    public static string GetIconPath(string icon) => Utils.GetIconPath(icon, PictureSize.Small);

    internal static Action WaitCallbackExecute(int timeout, Action callback)
    {
      bool callbackCalled = false;
      Action wrappedCallback = (Action) (() =>
      {
        if (callbackCalled)
          return;
        callbackCalled = true;
        callback();
      });
      Utils.DelayExecute(timeout, (Action) (() => wrappedCallback()));
      return wrappedCallback;
    }

    public static string GetIconPath(string icon, PictureSize pictureSize)
    {
      return icon != null ? string.Format("/s/object/{0}/?size_type={1}", (object) icon, (object) pictureSize.ToString().ToLower()) : (string) null;
    }

    public static void DelayExecute(int intervalMilliseconds, Action action)
    {
      DispatcherTimer timer = new DispatcherTimer();
      timer.Tick += (EventHandler) ((s, e) =>
      {
        timer.Stop();
        action();
      });
      timer.Interval = TimeSpan.FromMilliseconds((double) intervalMilliseconds);
      timer.Start();
    }

    internal static void Swap<T>(ref T first, ref T second)
    {
      T obj = first;
      first = second;
      second = obj;
    }

    internal static async Task InvokeOnUI(Action action)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: method pointer
      await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync((CoreDispatcherPriority) 0, new DispatchedHandler((object) new Utils.\u003C\u003Ec__DisplayClass31_0()
      {
        action = action
      }, __methodptr(\u003CInvokeOnUI\u003Eb__0)));
    }

    internal static void BeginInvokeOnUI(Action action)
    {
      Deployment.Current.Dispatcher.BeginInvoke(action);
    }

    internal static bool Assert(bool condition, string message = null, bool throwException = true)
    {
      if (condition)
        return true;
      string message1 = "Assertion failed" + (message != null ? ": " + message : "");
      if (throwException)
        throw new Exception(message1);
      Utils.log.Error(message1, 329, nameof (Assert));
      return false;
    }

    internal static string ReadEmbeddedResourceFile(string fileName)
    {
      Assembly assembly = typeof (Utils).GetTypeInfo().Assembly;
      string name = string.Format("{0}.{1}", (object) assembly.GetName().Name, (object) fileName);
      using (Stream manifestResourceStream = assembly.GetManifestResourceStream(name))
      {
        using (StreamReader streamReader = new StreamReader(manifestResourceStream))
          return streamReader.ReadToEnd();
      }
    }

    public static string GetFileNameFromUrl(string url)
    {
      int num = url.LastIndexOf("/");
      if (num != -1)
        url = url.Substring(num + 1);
      return WebUtility.UrlDecode(url);
    }

    public static string GetFileNameFromFilePath(string filePath) => Path.GetFileName(filePath);

    internal static void OpenUrl(string url)
    {
      try
      {
        Launcher.LaunchUriAsync(new Uri(url, UriKind.RelativeOrAbsolute));
      }
      catch (Exception ex)
      {
        Utils.log.Error(ex, "Open url", 373, nameof (OpenUrl));
      }
    }

    internal static void OpenEmailInvite()
    {
      try
      {
        string stringToEscape1 = "Invitation to imo";
        string stringToEscape2 = Utils.ReadEmbeddedResourceFile("Resources.MailInviteTemplate.txt");
        Utils.OpenUrl(string.Format("mailto:?subject={0}&body={1}", (object) Uri.EscapeDataString(stringToEscape1), (object) Uri.EscapeDataString(stringToEscape2)));
      }
      catch (Exception ex)
      {
        Utils.log.Error(ex, "Generate email link", 390, nameof (OpenEmailInvite));
      }
    }

    internal static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
    {
      return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(unixTimeStamp).ToLocalTime();
    }

    internal static string GetNetworkTypeAndSubtype()
    {
      ConnectionProfile connectionProfile = Windows.Networking.Connectivity.NetworkInformation.GetInternetConnectionProfile();
      if (connectionProfile == null)
        return (string) null;
      if (connectionProfile.IsWlanConnectionProfile)
        return "WIFI";
      return connectionProfile.IsWwanConnectionProfile ? string.Format("MOBILE[{0}]", (object) connectionProfile.WwanConnectionProfileDetails.GetCurrentDataClass().ToString().ToUpper()) : (string) null;
    }

    internal static string GetPhoneCountryCode()
    {
      return string.IsNullOrEmpty(IMO.User.PhoneCC) ? IMO.ApplicationSettings.CountryCode : IMO.User.PhoneCC.ToUpper();
    }

    internal static string GetCarrierName() => DeviceNetworkInformation.CellularMobileOperator;

    internal static int GetScreenWidth() => (int) Application.Current.Host.Content.ActualWidth;

    internal static int GetScreenHeight() => (int) Application.Current.Host.Content.ActualHeight;

    internal static int GetGroupCallSlotWidth() => Utils.GetScreenWidth() / 2;

    internal static int GetGroupCallSlotHeight() => Utils.GetScreenHeight() / 2;

    internal static int GetNumberOfCores() => Environment.ProcessorCount;

    public static bool IsUdidLucky(int luckyNumber, int sample)
    {
      return Math.Abs(IMO.ApplicationSettings.Udid.GetHashCode()) % sample == luckyNumber;
    }

    public static ulong ImoHash(string key)
    {
      ulong num1 = 0;
      foreach (ulong num2 in key)
      {
        ulong num3 = num2 + num1 & (ulong) uint.MaxValue;
        ulong num4 = (num3 << 10) + num3 & (ulong) uint.MaxValue;
        num1 = num4 ^ num4 >> 6;
      }
      ulong num5 = (num1 << 3) + num1 & (ulong) uint.MaxValue;
      ulong num6 = num5 ^ num5 >> 11;
      return (num6 << 15) + num6 & (ulong) uint.MaxValue;
    }
  }
}
