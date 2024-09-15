// Decompiled with JetBrains decompiler
// Type: NLog.Logger
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp;
using ImoSilverlightApp.Helpers;
using System;
using System.Runtime.CompilerServices;
using Windows.Foundation.Diagnostics;


namespace NLog
{
  internal class Logger
  {
    private string name;

    public Logger(string name) => this.name = name;

    internal void Info(string v) => LogManager.LogMessage(this.name + ": " + v);

    internal void Warn(string v) => LogManager.LogMessage(this.name + ": " + v, (LoggingLevel) 2);

    internal void Error(string message, [CallerLineNumber] int lineNumber = -1, [CallerMemberName] string caller = "unknown")
    {
      this.InternalError(message, (Exception) null, (ImoLogData) null, caller, lineNumber);
    }

    internal void Debug(string v) => LogManager.LogMessage(this.name + ": " + v, (LoggingLevel) 0);

    internal void Error(string message, ImoLogData logData, [CallerLineNumber] int lineNumber = -1, [CallerMemberName] string caller = "unknown")
    {
      this.InternalError(message, (Exception) null, logData, caller, lineNumber);
    }

    internal void Error(Exception ex, string message, [CallerLineNumber] int lineNumber = -1, [CallerMemberName] string caller = "unknown")
    {
      this.InternalError(message, ex, (ImoLogData) null, caller, lineNumber);
    }

    internal void Error(Exception ex, [CallerLineNumber] int lineNumber = -1, [CallerMemberName] string caller = "unknown")
    {
      this.InternalError(ex.Message, ex, (ImoLogData) null, caller, lineNumber);
    }

    internal void AppCrash(Exception ex, [CallerLineNumber] int lineNumber = -1, [CallerMemberName] string caller = "unknown")
    {
      Exception ex1 = ex;
      ImoLogData logData = new ImoLogData();
      logData.IsAppCrash = true;
      string caller1 = caller;
      int lineNumber1 = lineNumber;
      this.InternalError("Unhandled exception", ex1, logData, caller1, lineNumber1);
      LogManager.HandleDeactivated();
    }

    internal void Error(
      Exception ex,
      string message,
      ImoLogData imoLogData,
      [CallerLineNumber] int lineNumber = -1,
      [CallerMemberName] string caller = "unknown")
    {
      this.InternalError(message, ex, (ImoLogData) null, caller, lineNumber);
    }

    private void InternalError(
      string message,
      Exception ex,
      ImoLogData logData,
      string caller,
      int lineNumber)
    {
      string m = this.name + "." + caller + "-" + (object) lineNumber + ": " + message;
      if (ex != null)
        m = m + "\n\n" + ex.ToString();
      LogManager.LogMessage(m, (LoggingLevel) 3);
      IMO.ErrorReporter.LogToBackend(message, ex, logData, this.name, caller, lineNumber);
    }

    internal void Warn(Exception ex, string v) => this.Warn(ex.Message + ":" + v);
  }
}
