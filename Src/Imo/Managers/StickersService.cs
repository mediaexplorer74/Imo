// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.Managers.StickersService
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;


namespace ImoSilverlightApp.Managers
{
  internal class StickersService : BaseManager
  {
    public void GetPack(Action<JToken> callback)
    {
      BaseManager.Send("stickers", "get_packs", new Dictionary<string, object>()
      {
        {
          "uid",
          (object) IMO.User.Uid
        }
      }, callback);
    }

    public void GetPackStickers(string packId, Action<JToken> callback)
    {
      BaseManager.Send("stickers", "get_pack_stickers", new Dictionary<string, object>()
      {
        {
          "uid",
          (object) IMO.User.Uid
        },
        {
          "pack_id",
          (object) packId
        }
      }, callback);
    }
  }
}
