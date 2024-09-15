// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.Managers.ErrorReporter
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Helpers;
using ImoSilverlightApp.Storage.Models;
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using Windows.Foundation;
using Windows.Web.Http;


namespace ImoSilverlightApp.Managers
{
  internal class ErrorReporter
  {
    private static readonly Logger log = LogManager.GetLogger(typeof (ErrorReporter).Name);
    private static ErrorReporter instance;

    private ErrorReporter()
    {
    }

    public static ErrorReporter Instance
    {
      get
      {
        if (ErrorReporter.instance == null)
          ErrorReporter.instance = new ErrorReporter();
        return ErrorReporter.instance;
      }
    }

    public void LogToBackend(
      string message,
      Exception exception,
      ImoLogData logData,
      string type,
      string method,
      int lineNumber)
    {
      try
      {
        if (exception != null)
        {
          string stackTrace = this.GetInnerException(exception).StackTrace;
          Utils.Assert(stackTrace != null, "exception report, innerStackTrace is null");
          string imoFrame = this.GetImoFrame(stackTrace);
          Utils.Assert(imoFrame != null, "exception report, frame is null");
          string path = string.Format("{1}{0}{2}{0}{3}", (object) "\\", (object) "imoWindowsPhone", (object) IMO.ApplicationProperties.Version, (object) imoFrame);
          this.InternalErrorLogToBackend(message, path, exception.ToString(), logData);
        }
        else
        {
          string path = string.Format("{1}{0}{2}{0}{3}{0}{4}{0}{5}", (object) "\\", (object) "imoWindowsPhone", (object) IMO.ApplicationProperties.Version, (object) type, (object) method, (object) lineNumber);
          this.InternalErrorLogToBackend(message, path, "no-stack-trace", logData);
        }
      }
      catch (Exception ex1)
      {
        try
        {
          this.InternalErrorLogToBackend("Error while reporting error", "ErrorReporter\\LogToBackend", ex1.ToString(), new ImoLogData()
          {
            AdditionalStackTrace = exception.ToString()
          });
        }
        catch (Exception ex2)
        {
          ErrorReporter.log.Warn("Error logging error to backend: " + ex2.Message);
        }
      }
    }

    public void SendAVDump(byte[] crashDump)
    {
      this.SendErrorsToBackend(new JObject()
      {
        {
          "ssid",
          (JToken) IMO.Dispatcher.GetSSID()
        },
        {
          "udid",
          (JToken) ApplicationSettings.Instance.Udid
        },
        {
          "user_agent",
          (JToken) UserAgentHelper.GetUA()
        },
        {
          "class_name",
          (JToken) "macaw"
        },
        {
          "stack_trace",
          (JToken) Convert.ToBase64String(crashDump)
        }
      });
    }

    private string GetImoFrame(string stackTrace)
    {
      string[] strArray = stackTrace.Split(new string[1]
      {
        Environment.NewLine
      }, StringSplitOptions.RemoveEmptyEntries);
      string imoFrame = (string) null;
      for (int index = 0; index < strArray.Length; ++index)
      {
        string str = strArray[index];
        if (str.Contains("ImoSilverlightApp"))
        {
          imoFrame = str;
          break;
        }
      }
      if (imoFrame == null)
        imoFrame = strArray[0];
      string str1 = "   at ImoSilverlightApp.";
      if (imoFrame.StartsWith(str1))
        imoFrame = imoFrame.Substring(str1.Length);
      return imoFrame;
    }

    private Exception GetInnerException(Exception exception)
    {
      Exception exception1 = exception;
      while (exception1 is TargetInvocationException)
        exception1 = exception1.InnerException;
      return exception1 ?? exception;
    }

    private void InternalErrorLogToBackend(
      string message,
      string path,
      string stackTrace,
      ImoLogData logData = null)
    {
      JObject data = new JObject();
      data.Add("service", (JToken) "WindowsPhone");
      data.Add("shard", (JToken) 0);
      data.Add("levelname", (JToken) "ERROR");
      data.Add("pathname", (JToken) path);
      data.Add("lineno", (JToken) -1);
      data.Add("levelno", (JToken) 40);
      string additionalInfo = this.GetAdditionalInfo(logData);
      string str = string.Format("{0} : {1} {2}{2}{3}{2}{2}{4}", (object) path, (object) message, (object) Environment.NewLine, (object) stackTrace, (object) additionalInfo);
      data.Add(nameof (message), (JToken) str);
      data.Add("host", (JToken) UserAgentHelper.GetUA());
      data.Add("uid", (JToken) IMO.ApplicationSettings.CookieUid);
      data.Add("udid", (JToken) IMO.ApplicationSettings.Udid);
      data.Add("queue_size", IMO.Dispatcher.GetOutgoingQueueSize());
      JObject nextOutgoingMessage = IMO.Dispatcher.GetNextOutgoingMessage();
      if (nextOutgoingMessage != null)
        data.Add("first_message", (JToken) nextOutgoingMessage.ToString());
      bool isBlocking = logData != null && logData.IsAppCrash;
      this.SendErrorsToBackend(data, isBlocking);
    }

    private string GetAdditionalInfo(ImoLogData logData)
    {
      string additionalInfo = "User Agent\n===============\n" + UserAgentHelper.GetUA() + "\n\n";
      if (logData != null)
        additionalInfo = additionalInfo + "Additional Stack Trace\n===============\n" + logData.AdditionalStackTrace + "\n\n";
      return additionalInfo;
    }

    private void InternalLogExceptionToBackend(
      string message,
      string type,
      string method,
      string stackTrace)
    {
      this.SendErrorsToBackend(new JObject()
      {
        {
          "ssid",
          (JToken) IMO.Dispatcher.GetSSID()
        },
        {
          "user_agent",
          (JToken) UserAgentHelper.GetUA()
        },
        {
          "stack_trace",
          (JToken) stackTrace
        },
        {
          "class_name",
          (JToken) type
        },
        {
          "function_name",
          (JToken) method
        }
      });
    }

    private JObject CreateServerMessage(JObject innerData)
    {
      try
      {
        bool flag = innerData["stack_trace"] != null;
        return new JObject()
        {
          {
            "ack",
            (JToken) 0
          },
          {
            "ssid",
            (JToken) IMO.Dispatcher.GetSSID()
          },
          {
            "messages",
            (JToken) new JArray()
            {
              (JToken) new JObject()
              {
                {
                  "seq",
                  (JToken) 0
                },
                {
                  "from",
                  (JToken) new JObject()
                  {
                    {
                      "system",
                      (JToken) "client"
                    },
                    {
                      "ssid",
                      (JToken) IMO.Dispatcher.GetSSID()
                    },
                    {
                      "udid",
                      (JToken) IMO.ApplicationSettings.Udid
                    },
                    {
                      "uid",
                      (JToken) (IMO.ApplicationSettings.CookieUid ?? "Not registered account")
                    }
                  }
                },
                {
                  "to",
                  (JToken) new JObject()
                  {
                    {
                      "system",
                      (JToken) (flag ? "exceptions" : "errormonitor")
                    }
                  }
                },
                {
                  "data",
                  (JToken) new JObject()
                  {
                    {
                      "method",
                      (JToken) (flag ? "add_exception" : "log_error_public")
                    },
                    {
                      "data",
                      (JToken) innerData
                    }
                  }
                }
              }
            }
          }
        };
      }
      catch (Exception ex)
      {
        ErrorReporter.log.Warn("Error while creating server message for error: " + ex.Message);
        return (JObject) null;
      }
    }

    private void SendErrorsToBackend(JObject data, bool isBlocking = false)
    {
      try
      {
        JObject serverMessage = this.CreateServerMessage(data);
        if (serverMessage == null)
          return;
        this.SendError(serverMessage, isBlocking);
      }
      catch (Exception ex)
      {
        ErrorReporter.log.Warn(ex, "Exception sending errors to backend");
      }
    }

    private async void SendError(JObject jObj, bool isBlocking)
    {
      IHttpContent ihttpContent = (IHttpContent) new HttpFormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>) new KeyValuePair<string, string>[2]
      {
        new KeyValuePair<string, string>("method", "rest_rpc"),
        new KeyValuePair<string, string>("data", jObj.ToString())
      });
      HttpClient httpClient = new HttpClient();
      HttpResponseMessage response = (HttpResponseMessage) null;
      if (isBlocking)
      {
        try
        {
          IAsyncOperationWithProgress<HttpResponseMessage, HttpProgress> operationWithProgress = httpClient.PostAsync(new Uri(IMO.Network.ConnectionData.RestRpcPostUrl), ihttpContent);
          Thread.Sleep(2000);
          response = operationWithProgress.GetResults();
        }
        catch (Exception ex)
        {
          ErrorReporter.log.Warn("Error while posting https error: " + response?.ReasonPhrase ?? ex.Message);
          response = (HttpResponseMessage) null;
        }
      }
      else
      {
        try
        {
          response = await httpClient.PostAsync(new Uri(IMO.Network.ConnectionData.RestRpcPostUrl), ihttpContent);
        }
        catch (Exception ex)
        {
          ErrorReporter.log.Warn("Error while posting https error: " + response?.ReasonPhrase ?? ex.Message);
          response = (HttpResponseMessage) null;
        }
      }
      if (response == null || response.IsSuccessStatusCode)
        return;
      ErrorReporter.log.Warn("Error while posting https error: " + response.ReasonPhrase);
    }
  }
}
