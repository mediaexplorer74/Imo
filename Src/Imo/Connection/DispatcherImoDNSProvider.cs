// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.Connection.DispatcherImoDNSProvider
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Threading.Tasks;


namespace ImoSilverlightApp.Connection
{
  internal class DispatcherImoDNSProvider : ImoDNSProvider
  {
    private static readonly Logger log = LogManager.GetLogger(typeof (DispatcherImoDNSProvider).Name);

    public override Task<JObject> GetIps()
    {
      DispatcherImoDNSProvider.log.Info("Getting ips via dispatcher");
      JObject data = new JObject();
      data["data"] = (JToken) this.GetParameterJson();
      TaskCompletionSource<JObject> taskSource = new TaskCompletionSource<JObject>();
      BaseManager.Send("imo_dns", "get_ips", data, (Action<JToken>) (response =>
      {
        JObject result;
        try
        {
          result = response.ToObject<JObject>();
        }
        catch (Exception ex)
        {
          taskSource.SetException(ex);
          return;
        }
        taskSource.SetResult(result);
      }));
      return taskSource.Task;
    }

    public override bool CanGetIps() => IMO.Network.IsConnected;
  }
}
