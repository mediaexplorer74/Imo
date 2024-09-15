// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.Helpers.RegularActionsUtils
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using NLog;
using System;
using System.Windows.Threading;


namespace ImoSilverlightApp.Helpers
{
  internal class RegularActionsUtils
  {
    private static readonly Logger log = LogManager.GetLogger(typeof (RegularActionsUtils).Name);
    private const int ENSURE_IMAGES_CACHE_LIMIT_INTERVAL_DAYS = 3;
    private static bool isSendingAppDailyActiveLog = false;
    private static bool isSendingUserDailyActiveLog = false;
    private static DateTime lastIsNewDayCheck = DateTime.MinValue;

    internal static void ScheduleRegularIntervalAction()
    {
      DispatcherTimer dispatcherTimer = new DispatcherTimer();
      Action action = (Action) (() =>
      {
        RegularActionsUtils.LogAppActivity();
        RegularActionsUtils.LogUserActivity();
        RegularActionsUtils.MaybeUpdateTimestampsUI();
      });
      dispatcherTimer.Tick += (EventHandler) ((s, e) => action());
      action();
      dispatcherTimer.Interval = TimeSpan.FromMilliseconds(60000.0);
      dispatcherTimer.Start();
    }

    private static void EnsureImagesCacheLimit()
    {
    }

    private static void MaybeUpdateTimestampsUI()
    {
      if (!RegularActionsUtils.IsNewDay())
        return;
      RegularActionsUtils.log.Info("New day: updating messages timestmap in UI");
      IMO.ConversationsManager.UpdateNewDayMessagesTimestampUI();
      IMO.ContactsManager.UpdateNewDayActivityTimestampsUI();
    }

    internal static bool IsNewDay()
    {
      if (!(RegularActionsUtils.lastIsNewDayCheck != DateTime.Now.Date))
        return false;
      DateTime lastIsNewDayCheck = RegularActionsUtils.lastIsNewDayCheck;
      RegularActionsUtils.lastIsNewDayCheck = DateTime.Now.Date;
      DateTime minValue = DateTime.MinValue;
      return lastIsNewDayCheck != minValue;
    }

    internal static void LogAppActivity()
    {
      if ((DateTime.Now - Utils.TimestampToDateTime(IMO.ApplicationSettings.LastAppDailyActivityLog)).TotalDays <= 1.0 || RegularActionsUtils.isSendingAppDailyActiveLog)
        return;
      RegularActionsUtils.isSendingAppDailyActiveLog = true;
      IMO.MonitorLog.Log("daily_active_apps", "active", (Action) (() =>
      {
        RegularActionsUtils.isSendingAppDailyActiveLog = false;
        IMO.ApplicationSettings.LastAppDailyActivityLog = Utils.GetTimestamp();
      }));
    }

    internal static void LogUserActivity()
    {
      if (!IMO.ApplicationProperties.IsSignedOn || (DateTime.Now - Utils.TimestampToDateTime(IMO.ApplicationSettings.LastUserDailyActivityLog)).TotalDays <= 1.0 || RegularActionsUtils.isSendingUserDailyActiveLog)
        return;
      RegularActionsUtils.isSendingUserDailyActiveLog = true;
      IMO.MonitorLog.Log("daily_active_users", "active", (Action) (() =>
      {
        RegularActionsUtils.isSendingUserDailyActiveLog = false;
        IMO.ApplicationSettings.LastUserDailyActivityLog = Utils.GetTimestamp();
      }));
    }

    internal static void InvalidatePendingRequests()
    {
      RegularActionsUtils.isSendingAppDailyActiveLog = false;
      RegularActionsUtils.isSendingUserDailyActiveLog = false;
    }
  }
}
