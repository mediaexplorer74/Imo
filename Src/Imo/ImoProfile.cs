// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.ImoProfile
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;


namespace ImoSilverlightApp
{
  internal class ImoProfile : BaseManager
  {
    public void GetMyProfile(Action<JToken> callback)
    {
      BaseManager.Send("imoprofile", "get_my_profile", new Dictionary<string, object>()
      {
        {
          "ssid",
          (object) IMO.Dispatcher.GetSSID()
        },
        {
          "uid",
          (object) IMO.User.Uid
        }
      }, callback);
    }
  }
}
