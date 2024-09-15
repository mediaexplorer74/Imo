// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.Connection.ImoDNSManager
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Collections;
using ImoSilverlightApp.Helpers;
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;


namespace ImoSilverlightApp.Connection
{
  internal class ImoDNSManager
  {
    private static readonly Logger log = LogManager.GetLogger(typeof (ImoDNSManager).Name);
    private static ImoDNSManager instance;
    private static string GOOGLE_URL = "https://imodns-na.appspot.com/get";
    private static string AMAZON_URL = "https://d3r6lfw7jds5dy.cloudfront.net/get";
    private static readonly HashSet<string> bestCountries = new HashSet<string>((IEnumerable<string>) new string[16]
    {
      "AE",
      "OM",
      "DM",
      "ET",
      "TD",
      "NP",
      "CG",
      "CD",
      "DJ",
      "TM",
      "HT",
      "ER",
      "SS",
      "CU",
      "SN",
      "YE"
    });
    private static string[] PORTS = new string[3]
    {
      "443",
      "5223",
      "5228"
    };
    private static Random random = new Random();
    private static HashSet<string> randomCountryAllSet = new HashSet<string>((IEnumerable<string>) new string[6]
    {
      "AE",
      "OM",
      "ET",
      "IR",
      "SA",
      "MA"
    });
    private static HashSet<string> randomCountrynonWifiSet = new HashSet<string>((IEnumerable<string>) new string[2]
    {
      "TM",
      "DJ"
    });
    private static readonly string DEFAULT_SESSION_PREFIX = "2";
    private const int SESSION_SIZE = 17;
    private int lastProviderIndex;
    private bool hasReturnedFirstIp;
    private bool hasRefreshIpsScheduled;
    private int scheduleId;
    private static int REFRESH_ON_FAIL_INTERVAL = 3000;
    private IList<ImoDNSProvider> endpointProviders;
    private ImoDNSEndpoint currentBackendEndpoint;

    private ImoDNSManager()
    {
      this.endpointProviders = (IList<ImoDNSProvider>) new List<ImoDNSProvider>();
      this.endpointProviders.Add((ImoDNSProvider) new HttpImoDNSProvider(ImoDNSManager.GOOGLE_URL));
      this.endpointProviders.Add((ImoDNSProvider) new HttpImoDNSProvider(ImoDNSManager.AMAZON_URL));
      this.endpointProviders.Add((ImoDNSProvider) new HttpImoDNSProvider((Func<string>) (() => "https://" + Constants.GetImoHost() + "/imodns/get")));
      this.endpointProviders.Add((ImoDNSProvider) new DispatcherImoDNSProvider());
      this.lastProviderIndex = Utils.GetUnsafeRandomInt(this.endpointProviders.Count);
    }

    public void Init()
    {
      IMO.Network.Connected += new EventHandler<EventArg<ConnectionData>>(this.Network_Connected);
      IMO.Network.ConnectionFailed += new EventHandler<EventArgs>(this.Network_ConnectionFailed);
    }

    private void Network_Connected(object sender, EventArg<ConnectionData> e)
    {
      if (this.currentBackendEndpoint == null)
        return;
      this.currentBackendEndpoint.BumpSuccessConnectionCount();
    }

    private void Network_ConnectionFailed(object sender, EventArgs e)
    {
      if (this.currentBackendEndpoint == null)
        return;
      this.currentBackendEndpoint.BumpFailConnectionCount();
    }

    public static ImoDNSManager Instance
    {
      get
      {
        if (ImoDNSManager.instance == null)
          ImoDNSManager.instance = new ImoDNSManager();
        return ImoDNSManager.instance;
      }
    }

    public void HandleMessage(JObject jObj)
    {
      string str = jObj.Value<string>((object) "name");
      JObject jObj1 = jObj.Value<JObject>((object) "edata");
      if (!str.Equals("get_ips"))
        return;
      this.HandleIpsResponse(jObj1, true);
    }

    private void HandleIpsResponse(JObject jObj, bool forceSchedule = false)
    {
      JArray jarray = jObj.Value<JArray>((object) "ips");
      List<ImoDNSEndpoint> imoDnsEndpointList = new List<ImoDNSEndpoint>();
      foreach (JToken jtoken in jarray)
      {
        string ip = jtoken.Value<string>((object) "ip");
        string sessionPrefix = jtoken.Value<string>((object) "session_prefix");
        List<int> list = jtoken.Value<JArray>((object) "ports").Select<JToken, int>((Func<JToken, int>) (x => x.ToObject<int>())).ToList<int>();
        long ttl = jtoken.Value<long>((object) "ttl") * 1000L;
        imoDnsEndpointList.Add(new ImoDNSEndpoint(ip, (IList<int>) list, ttl, sessionPrefix));
      }
      int num = jObj.Value<int>((object) "ask_again") * 1000;
      if (imoDnsEndpointList.Count == 0)
        ImoDNSManager.log.Error("Endpoints count is 0", 123, nameof (HandleIpsResponse));
      else
        IMO.ApplicationSettings.LastImoDNSResult = (IList<ImoDNSEndpoint>) imoDnsEndpointList;
      this.ScheduleRefreshEndpoints(imoDnsEndpointList.Count > 0 ? num : ImoDNSManager.REFRESH_ON_FAIL_INTERVAL, forceSchedule);
    }

    public async void RefreshEndpoints(bool forceSchedule = false)
    {
      try
      {
        ImoDNSProvider providerThatCanGetIps = this.GetFirstProviderThatCanGetIps();
        if (providerThatCanGetIps == null)
        {
          ImoDNSManager.log.Error("No valid provider found to get new ips, will retry", 139, nameof (RefreshEndpoints));
          this.ScheduleRefreshEndpoints(3000);
        }
        else
          this.HandleIpsResponse(await providerThatCanGetIps.GetIps(), forceSchedule);
      }
      catch (Exception ex)
      {
        ImoDNSManager.log.Error(ex, "Exception while getting ips", 149, nameof (RefreshEndpoints));
        this.ScheduleRefreshEndpoints(ImoDNSManager.REFRESH_ON_FAIL_INTERVAL);
      }
    }

    public string GetSessionPrefix()
    {
      return this.currentBackendEndpoint?.SessionPrefix ?? this.GetDefaultSessionPrefix();
    }

    private string GetDefaultSessionPrefix() => ImoDNSManager.DEFAULT_SESSION_PREFIX;

    public string GenerateSSID() => this.GenerateSSID(this.GetSessionPrefix());

    private string GenerateSSID(string prefix)
    {
      return prefix + Utils.GetRandomString(17 - prefix.Length);
    }

    private ImoDNSProvider GetFirstProviderThatCanGetIps()
    {
      for (int index = 0; index < this.endpointProviders.Count; ++index)
      {
        this.lastProviderIndex = (this.lastProviderIndex + 1) % this.endpointProviders.Count;
        ImoDNSProvider endpointProvider = this.endpointProviders[this.lastProviderIndex];
        if (endpointProvider.CanGetIps())
          return endpointProvider;
      }
      return (ImoDNSProvider) null;
    }

    private void ScheduleRefreshEndpoints(int milliseconds, bool force = false)
    {
      if (this.hasRefreshIpsScheduled && !force)
        return;
      this.hasRefreshIpsScheduled = true;
      int currentScheduleId = ++this.scheduleId;
      Utils.DelayExecute(milliseconds, (Action) (() =>
      {
        if (currentScheduleId != this.scheduleId)
          return;
        this.hasRefreshIpsScheduled = false;
        this.RefreshEndpoints();
      }));
    }

    public ImoDNSEndpoint GetFasterEndpoint()
    {
      this.currentBackendEndpoint = (ImoDNSEndpoint) null;
      foreach (ImoDNSEndpoint imoDnsEndpoint in (IEnumerable<ImoDNSEndpoint>) IMO.ApplicationSettings.LastImoDNSResult)
      {
        if (imoDnsEndpoint.IsValid() && (this.currentBackendEndpoint == null || imoDnsEndpoint.LastUsedTime < this.currentBackendEndpoint.LastUsedTime))
          this.currentBackendEndpoint = imoDnsEndpoint;
      }
      if (this.currentBackendEndpoint != null)
      {
        this.currentBackendEndpoint.UpdateLastUsedTime();
        return this.currentBackendEndpoint;
      }
      Pair<string, int> defaultFasterIp = ImoDNSManager.GetDefaultFasterIP();
      this.currentBackendEndpoint = new ImoDNSEndpoint(defaultFasterIp.First, (IList<int>) new int[1]
      {
        defaultFasterIp.Second
      }, long.MaxValue, this.GetDefaultSessionPrefix());
      return this.currentBackendEndpoint;
    }

    public static Pair<string, int> GetDefaultFasterIP()
    {
      string[] list = Constants.FASTER_IPS;
      string phoneCountryCode = Utils.GetPhoneCountryCode();
      if (phoneCountryCode == null || phoneCountryCode == "PH")
        list = Constants.NEW_IPS;
      if (ImoDNSManager.IsBestCountry())
        list = !Utils.IsUdidLucky(1, 2) ? Constants.IP_IP_IPS : Constants.NEW_IP_IP_IPS;
      if (phoneCountryCode == "DJ" || phoneCountryCode == "TM" || phoneCountryCode == "OM" || phoneCountryCode == "SA")
        list = new string[1]
        {
          ImoDNSManager.GetNewIp(Utils.ImoHash("afea6afe'" + IMO.ApplicationSettings.Udid + "'cb7egss"))
        };
      return new Pair<string, int>(ImoDNSManager.SelectRandom(list), int.Parse(ImoDNSManager.SelectRandom(ImoDNSManager.PORTS)));
    }

    private static string SelectRandom(string[] list)
    {
      return list[ImoDNSManager.random.Next(list.Length)];
    }

    private static string GetNewIp(ulong which)
    {
      return "5.150.156." + (object) (ulong) (11L + (long) (which % 160UL));
    }

    public static bool IsBestCountry()
    {
      string str = Utils.GetCarrierName();
      if (string.IsNullOrEmpty(str))
        str = "";
      string lowerInvariant = str.ToLowerInvariant();
      if (lowerInvariant.Contains("etisalat") || lowerInvariant == "du")
        return true;
      string phoneCountryCode = Utils.GetPhoneCountryCode();
      return ImoDNSManager.bestCountries.Contains(phoneCountryCode);
    }

    private bool IsRandomCountry()
    {
      string phoneCountryCode = Utils.GetPhoneCountryCode();
      if (ImoDNSManager.randomCountryAllSet.Contains(phoneCountryCode))
        return true;
      string str = Utils.GetCarrierName();
      if (string.IsNullOrEmpty(str))
        str = "";
      string lowerInvariant = str.ToLowerInvariant();
      if (lowerInvariant.Contains("etisalat") || lowerInvariant == "du")
        return true;
      return ImoDNSManager.randomCountrynonWifiSet.Contains(phoneCountryCode) && Utils.GetNetworkTypeAndSubtype() != "WIFI";
    }

    public string GetRandomPadding()
    {
      return this.IsRandomCountry() ? Convert.ToBase64String(Utils.GetRandomBytes(Utils.GetRandomInt(200))) : (string) null;
    }

    internal void Reset()
    {
      IMO.ApplicationSettings.LastImoDNSResult = (IList<ImoDNSEndpoint>) null;
      this.currentBackendEndpoint = (ImoDNSEndpoint) null;
    }
  }
}
