// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.Connection.ImoDNSProvider
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Helpers;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;


namespace ImoSilverlightApp.Connection
{
  internal abstract class ImoDNSProvider : BaseManager
  {
    public abstract Task<JObject> GetIps();

    public abstract bool CanGetIps();

    protected JObject GetParameterJson()
    {
      return new JObject()
      {
        ["uid"] = (JToken) (string.IsNullOrEmpty(IMO.User.Uid) ? IMO.ApplicationSettings.CookieUid : IMO.User.Uid),
        ["sim_iso"] = (JToken) Utils.GetPhoneCountryCode(),
        ["mcc_mnc"] = (JToken) Utils.GetCarrierName(),
        ["ua"] = (JToken) UserAgentHelper.GetUA(),
        ["udid"] = (JToken) IMO.ApplicationSettings.Udid
      };
    }
  }
}
