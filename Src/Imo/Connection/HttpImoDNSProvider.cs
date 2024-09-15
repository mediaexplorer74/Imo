// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.Connection.HttpImoDNSProvider
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Helpers;
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;


namespace ImoSilverlightApp.Connection
{
  internal class HttpImoDNSProvider : ImoDNSProvider
  {
    private static readonly Logger log = LogManager.GetLogger(typeof (HttpImoDNSProvider).Name);
    private Func<string> getUrlFunc;

    public HttpImoDNSProvider(string url) => this.getUrlFunc = (Func<string>) (() => url);

    public HttpImoDNSProvider(Func<string> getUrlFunc) => this.getUrlFunc = getUrlFunc;

    public string Url => this.getUrlFunc();

    public override async Task<JObject> GetIps()
    {
      string uriString = this.getUrlFunc();
      HttpImoDNSProvider.log.Info("Getting ips via url: " + uriString);
      JObject parameterJson = this.GetParameterJson();
      HttpWebRequest http = (HttpWebRequest) WebRequest.Create(new Uri(uriString));
      http.Accept = "application/json";
      http.ContentType = "application/json";
      http.Method = "POST";
      string parsedContent = parameterJson.ToString();
      using (Stream requestStream = await http.GetRequestStreamAsync())
      {
        byte[] bytes = Encoding.UTF8.GetBytes(parsedContent);
        await requestStream.WriteAsync(bytes, 0, bytes.Length);
      }
      WebResponse responseAsync = await http.GetResponseAsync();
      JObject ips;
      using (Stream responseStream = responseAsync.GetResponseStream())
      {
        byte[] bytes = Utils.ReadStreamFully(responseStream, (int) responseAsync.ContentLength);
        ips = JObject.Parse(Encoding.UTF8.GetString(bytes, 0, bytes.Length));
      }
      return ips;
    }

    public override bool CanGetIps() => true;
  }
}
