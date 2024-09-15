// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.Storage.Models.ApplicationSettings
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Connection;
using ImoSilverlightApp.Helpers;
using Microsoft.Phone.Info;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Collections.Generic;
using System.Globalization;
using Windows.System.UserProfile;


namespace ImoSilverlightApp.Storage.Models
{
  public class ApplicationSettings : ModelBase
  {
    private static readonly Logger log = LogManager.GetLogger(typeof (ApplicationSettings).Name);
    private static ApplicationSettings instance;
    private const string COOKIE_UID_KEY = "cookie_uid";
    private const string PHONE_NUMBER_KEY = "phone_number";
    private const string COUNTRY_CODE_KEY = "phone_cc";
    private const string COOKIE_KEY = "cookie";
    private const string UDID_KEY = "udid";
    private const string IS_COOKIE_SIGNED_ON_KEY = "is_cookie_signed_on";
    private const string SHOW_DEV_TOOLS_KEY = "show_dev_tools";
    private const string USER_FULL_NAME_KEY = "user_full_name";
    private const string USER_AGE_KEY = "user_age";
    private const string SHOW_NOTIFICATIONS_KEY = "show_notifications";
    private const string PLAY_SOUNDS_KEY = "play_sounds";
    private const string IS_CHATS_TAB_ACTIVE_KEY = "is_chats_tab_open";
    private const string NOTIFICATION_CHANNEL_KEY = "notification_channel";
    private const string SELECTED_STICKERS_PACK_KEY = "selected_stickers_pack";
    private const string IS_FRONT_CAMERA_CAPTURE_KEY = "is_front_camera_capture";
    private const string LAST_STICKER_PACKS_SYNC_KEY = "last_sticker_packs_sync";
    private const string HAS_SIGNED_ON_BEFORE_KEY = "has_signed_on_before";
    private const string HAS_STARTED_APP_BEFORE_KEY = "has_started_app_before";
    private const string CODE_REQUESTED_TIMESTAMP_KEY = "code_requested_timestamp";
    private const string LAST_INVITE_PAGE_SHOWN_KEY = "last_invite_page_shown";
    private const string LAST_INVITE_POPUP_SHOWN_KEY = "last_invite_popup_shown";
    private const string LAST_APP_DAILY_ACTIVITY_LOG_KEY = "last_app_daily_activity_log";
    private const string LAST_USER_DAILY_ACTIVITY_LOG_KEY = "last_user_daily_activity_log";
    private const string WAS_APP_IN_CALL_KEY = "was_app_in_call";
    private const string DID_APP_EXIST_WITH_UNREPORTED_CRASH_KEY = "did_app_exit_with_unreported_crash";
    private const string LAST_IMODNS_RESULT = "last_imodns_result";
    private const string HAS_ACCEPTED_GDPR = "has_accepted_gdpr";
    private string udid;
    private string cookie;
    private string cookieUid;
    private bool isCookieSignedOn;
    private string phoneNumber;
    private string countryCode;
    private bool showDevTools;
    private long lastAppDailyActivityLog;
    private long lastUserDailyActivityLog;
    private string userFullName;
    private string userAge;
    private bool showNotifications;
    private bool playSounds;
    private bool isChatsTabActive;
    private string notificationChannel;
    private string selectedStickersPack;
    private bool isFrontCameraCapture;
    private long lastStickerPacksSync;
    private bool hasSignedOnBefore;
    private bool hasStartedAppBefore;
    private long codeRequestedTimestamp;
    private long lastInvitePageShown;
    private long lastInvitePopupShown;
    private bool wasAppInCall;
    private bool didAppExitWithUnreportedCrash;
    private bool hasAcceptedGdpr;
    private IList<ImoDNSEndpoint> lastImodnsResult;

    private ApplicationSettings() => this.InitFromStorage();

    private void InitFromStorage()
    {
      JObject settings = IMO.ApplicationStorage.GetSettings();
      this.udid = settings.Value<string>((object) "udid");
      this.cookie = settings.Value<string>((object) "cookie");
      this.cookieUid = settings.Value<string>((object) "cookie_uid");
      this.isCookieSignedOn = settings.Value<bool>((object) "is_cookie_signed_on");
      this.lastAppDailyActivityLog = settings.Value<long>((object) "last_app_daily_activity_log");
      this.lastUserDailyActivityLog = settings.Value<long>((object) "last_user_daily_activity_log");
      this.userAge = settings.Value<string>((object) "user_age");
      this.userFullName = settings.Value<string>((object) "user_full_name");
      this.showNotifications = settings["show_notifications"] == null || settings.Value<bool>((object) "show_notifications");
      this.playSounds = settings["play_sounds"] == null || settings.Value<bool>((object) "play_sounds");
      this.isChatsTabActive = settings.Value<bool>((object) "is_chats_tab_open");
      this.notificationChannel = settings.Value<string>((object) "notification_channel");
      this.selectedStickersPack = settings.Value<string>((object) "selected_stickers_pack");
      this.isFrontCameraCapture = settings.Value<bool>((object) "is_front_camera_capture");
      this.lastStickerPacksSync = settings.Value<long>((object) "last_sticker_packs_sync");
      this.hasSignedOnBefore = settings.Value<bool>((object) "has_signed_on_before");
      this.hasStartedAppBefore = settings.Value<bool>((object) "has_started_app_before");
      this.codeRequestedTimestamp = settings.Value<long>((object) "code_requested_timestamp");
      this.lastInvitePageShown = settings.Value<long>((object) "last_invite_page_shown");
      this.lastInvitePopupShown = settings.Value<long>((object) "last_invite_popup_shown");
      this.wasAppInCall = settings.Value<bool>((object) "was_app_in_call");
      this.didAppExitWithUnreportedCrash = settings.Value<bool>((object) "did_app_exit_with_unreported_crash");
      this.countryCode = settings.Value<string>((object) "phone_cc");
      this.hasAcceptedGdpr = settings.Value<bool>((object) "has_accepted_gdpr");
      this.lastImodnsResult = (IList<ImoDNSEndpoint>) JsonConvert.DeserializeObject<List<ImoDNSEndpoint>>(settings.Value<string>((object) "last_imodns_result") ?? "[]");
      this.EnsureUdidGenerated();
      this.EnsureCookieGenerated();
      this.EnsureCountryCodeIsSet();
      this.showDevTools = settings.Value<bool>((object) "show_dev_tools");
      this.phoneNumber = settings.Value<string>((object) "phone_number");
    }

    private void EnsureCountryCodeIsSet()
    {
      if (this.countryCode != null)
        return;
      string cc;
      try
      {
        cc = typeof (AdvertisingManager).Assembly.GetType("Windows.System.UserProfile.GlobalizationPreferences").GetProperty("HomeGeographicRegion").GetValue((object) null, (object[]) null).ToString();
      }
      catch (Exception ex)
      {
        cc = RegionInfo.CurrentRegion.TwoLetterISORegionName;
      }
      if (CountryCodesHelper.GetCountryByCC(cc) == null)
      {
        ApplicationSettings.log.Error("Bad country code: " + cc, 143, nameof (EnsureCountryCodeIsSet));
        cc = "GB";
      }
      this.countryCode = cc;
      this.InternalStorageSet("phone_cc", cc);
    }

    public bool IsEuCountry() => Constants.EU_CCS.Contains(this.countryCode);

    private string GetOrCreateUDID()
    {
      return Convert.ToBase64String((byte[]) DeviceExtendedProperties.GetValue("DeviceUniqueId"));
    }

    public static ApplicationSettings Instance
    {
      get
      {
        if (ApplicationSettings.instance == null)
          ApplicationSettings.instance = new ApplicationSettings();
        return ApplicationSettings.instance;
      }
    }

    public string CookieUid
    {
      get => this.cookieUid;
      set
      {
        if (!(this.cookieUid != value))
          return;
        this.cookieUid = value;
        IMO.ApplicationStorage.SetSettingsProperty("cookie_uid", value);
        this.OnPropertyChanged(nameof (CookieUid));
      }
    }

    public bool IsCookieSignedOn
    {
      get => this.isCookieSignedOn;
      set
      {
        if (this.isCookieSignedOn == value)
          return;
        this.isCookieSignedOn = value;
        IMO.ApplicationStorage.SetSettingsProperty("is_cookie_signed_on", Convert.ToString(value));
        this.OnPropertyChanged(nameof (IsCookieSignedOn));
      }
    }

    public long LastAppDailyActivityLog
    {
      get => this.lastAppDailyActivityLog;
      set
      {
        this.lastAppDailyActivityLog = value;
        this.InternalStorageSet("last_app_daily_activity_log", Convert.ToString(value));
        this.OnPropertyChanged(nameof (LastAppDailyActivityLog));
      }
    }

    public long LastUserDailyActivityLog
    {
      get => this.lastUserDailyActivityLog;
      set
      {
        this.lastUserDailyActivityLog = value;
        this.InternalStorageSet("last_user_daily_activity_log", Convert.ToString(value));
        this.OnPropertyChanged(nameof (LastUserDailyActivityLog));
      }
    }

    public bool HasSignedOnBefore
    {
      get => this.hasSignedOnBefore;
      set
      {
        if (this.hasSignedOnBefore == value)
          return;
        this.hasSignedOnBefore = value;
        IMO.ApplicationStorage.SetSettingsProperty("has_signed_on_before", Convert.ToString(value));
        this.OnPropertyChanged(nameof (HasSignedOnBefore));
      }
    }

    public bool HasStartedAppBefore
    {
      get => this.hasStartedAppBefore;
      set
      {
        if (this.hasStartedAppBefore == value)
          return;
        this.hasStartedAppBefore = value;
        IMO.ApplicationStorage.SetSettingsProperty("has_started_app_before", Convert.ToString(value));
        this.OnPropertyChanged(nameof (HasStartedAppBefore));
      }
    }

    public bool IsFrontCameraCapture
    {
      get => this.isFrontCameraCapture;
      set
      {
        this.isFrontCameraCapture = value;
        this.InternalStorageSet("is_front_camera_capture", Convert.ToString(value));
        this.OnPropertyChanged(nameof (IsFrontCameraCapture));
      }
    }

    public long CodeRequestedTimestamp
    {
      get => this.codeRequestedTimestamp;
      set
      {
        this.codeRequestedTimestamp = value;
        this.InternalStorageSet("code_requested_timestamp", Convert.ToString(value));
        this.OnPropertyChanged(nameof (CodeRequestedTimestamp));
      }
    }

    public long LastInvitePageShown
    {
      get => this.lastInvitePageShown;
      set
      {
        this.lastInvitePageShown = value;
        this.InternalStorageSet("last_invite_page_shown", Convert.ToString(value));
        this.OnPropertyChanged(nameof (LastInvitePageShown));
      }
    }

    public long LastInvitePopupShown
    {
      get => this.lastInvitePopupShown;
      set
      {
        this.lastInvitePopupShown = value;
        this.InternalStorageSet("last_invite_popup_shown", Convert.ToString(value));
        this.OnPropertyChanged(nameof (LastInvitePopupShown));
      }
    }

    public bool ShowDevTools
    {
      get => false;
      set
      {
        this.showDevTools = value;
        this.InternalStorageSet("show_dev_tools", Convert.ToString(value));
        this.OnPropertyChanged(nameof (ShowDevTools));
      }
    }

    public string PhoneNumber
    {
      get => this.phoneNumber;
      set
      {
        if (!(this.phoneNumber != value))
          return;
        this.phoneNumber = value;
        if (this.phoneNumber != null)
          this.phoneNumber = this.phoneNumber.Trim();
        this.CodeRequestedTimestamp = 0L;
        this.InternalStorageSet("phone_number", this.phoneNumber);
        this.OnPropertyChanged(nameof (PhoneNumber));
      }
    }

    public string CountryCode
    {
      get => this.countryCode;
      set
      {
        this.countryCode = value;
        this.InternalStorageSet("phone_cc", value);
        this.OnPropertyChanged(nameof (CountryCode));
      }
    }

    public string UserFullName
    {
      get => this.userFullName;
      set
      {
        if (!(this.userFullName != value))
          return;
        this.userFullName = value;
        this.InternalStorageSet("user_full_name", value);
        this.OnPropertyChanged(nameof (UserFullName));
      }
    }

    public long LastStickerPacksSync
    {
      get => this.lastStickerPacksSync;
      set
      {
        this.lastStickerPacksSync = value;
        this.InternalStorageSet("last_sticker_packs_sync", Convert.ToString(value));
      }
    }

    public string UserAge
    {
      get => this.userAge;
      set
      {
        if (!(this.userAge != value))
          return;
        this.userAge = value;
        this.InternalStorageSet("user_age", value);
        this.OnPropertyChanged(nameof (UserAge));
      }
    }

    public string Udid
    {
      get => this.udid;
      set
      {
        this.udid = value;
        IMO.ApplicationStorage.SetSettingsProperty("udid", value);
        this.OnPropertyChanged(nameof (Udid));
      }
    }

    public string Cookie
    {
      get => this.cookie;
      set
      {
        this.cookie = value;
        IMO.ApplicationStorage.SetSettingsProperty("cookie", value);
        this.OnPropertyChanged(nameof (Cookie));
      }
    }

    public bool ShowNotifications
    {
      get => this.showNotifications;
      set
      {
        this.showNotifications = value;
        this.InternalStorageSet("show_notifications", Convert.ToString(value));
        this.OnPropertyChanged(nameof (ShowNotifications));
      }
    }

    public bool PlaySounds
    {
      get => this.playSounds;
      set
      {
        this.playSounds = value;
        this.InternalStorageSet("play_sounds", Convert.ToString(value));
        this.OnPropertyChanged(nameof (PlaySounds));
      }
    }

    public bool IsChatsTabActive
    {
      get => this.isChatsTabActive;
      set
      {
        if (this.isChatsTabActive == value)
          return;
        this.isChatsTabActive = value;
        this.InternalStorageSet("is_chats_tab_open", Convert.ToString(value));
        this.OnPropertyChanged(nameof (IsChatsTabActive));
      }
    }

    public string NotificationChannel
    {
      get => this.notificationChannel;
      set
      {
        if (!(this.notificationChannel != value))
          return;
        this.notificationChannel = value;
        this.InternalStorageSet("notification_channel", value);
        this.OnPropertyChanged(nameof (NotificationChannel));
      }
    }

    public string SelectedStickersPack
    {
      get => this.selectedStickersPack;
      set
      {
        if (!(this.selectedStickersPack != value))
          return;
        this.selectedStickersPack = value;
        this.InternalStorageSet("selected_stickers_pack", value);
        this.OnPropertyChanged("SelecteStickersPack");
      }
    }

    public bool WasAppInCall
    {
      get => this.wasAppInCall;
      set
      {
        if (this.wasAppInCall == value)
          return;
        this.wasAppInCall = value;
        this.InternalStorageSet("was_app_in_call", Convert.ToString(value));
        this.OnPropertyChanged(nameof (WasAppInCall));
      }
    }

    public bool DidAppExitWithUnreportedledCrash
    {
      get => this.didAppExitWithUnreportedCrash;
      set
      {
        if (this.didAppExitWithUnreportedCrash == value)
          return;
        this.didAppExitWithUnreportedCrash = value;
        this.InternalStorageSet("did_app_exit_with_unreported_crash", Convert.ToString(value));
        this.OnPropertyChanged(nameof (DidAppExitWithUnreportedledCrash));
      }
    }

    public bool HasAcceptedGdpr
    {
      get => this.hasAcceptedGdpr;
      set
      {
        if (this.hasAcceptedGdpr == value)
          return;
        this.hasAcceptedGdpr = value;
        this.InternalStorageSet("has_accepted_gdpr", Convert.ToString(value));
        this.OnPropertyChanged(nameof (HasAcceptedGdpr));
      }
    }

    public string StreamsInfo { get; set; }

    public IList<ImoDNSEndpoint> LastImoDNSResult
    {
      get => this.lastImodnsResult ?? (IList<ImoDNSEndpoint>) new List<ImoDNSEndpoint>();
      set
      {
        this.lastImodnsResult = value;
        IMO.ApplicationStorage.SetSettingsProperty("last_imodns_result", JsonConvert.SerializeObject((object) value));
        this.OnPropertyChanged(nameof (LastImoDNSResult));
      }
    }

    private void InternalStorageSet(string key, string value)
    {
      IMO.ApplicationStorage.SetSettingsProperty(key, value);
    }

    public void EnsureCookieGenerated()
    {
      if (this.cookie != null)
        return;
      this.Cookie = Utils.GetRandomString(32);
    }

    public void EnsureUdidGenerated()
    {
      if (this.udid != null)
        return;
      this.Udid = Utils.GetRandomString(32);
    }
  }
}
