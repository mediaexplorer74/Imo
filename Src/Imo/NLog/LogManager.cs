// Decompiled with JetBrains decompiler
// Type: NLog.LogManager
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using Windows.Foundation.Diagnostics;


namespace NLog
{
  internal class LogManager
  {
    private static FileLoggingSession loggingSession;
    private static LoggingChannel loggingChannel;

    private static FileLoggingSession LoggingSession
    {
      get
      {
        if (LogManager.loggingSession == null)
          LogManager.loggingSession = new FileLoggingSession("MainLogSession");
        return LogManager.loggingSession;
      }
    }

    private static LoggingChannel LoggingChannel
    {
      get
      {
        if (LogManager.loggingChannel == null)
        {
          LogManager.loggingChannel = new LoggingChannel("AppLogChannel");
          LogManager.LoggingSession.AddLoggingChannel((ILoggingChannel) LogManager.loggingChannel);
        }
        return LogManager.loggingChannel;
      }
    }

    internal static Logger GetLogger(string name) => new Logger(name);

    public static void HandleDeactivated()
    {
    }

    public static void LogMessage(string m, LoggingLevel loggingLevel = 1)
    {
    }
  }
}
