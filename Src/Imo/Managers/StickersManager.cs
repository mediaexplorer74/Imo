// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.Managers.StickersManager
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Helpers;
using ImoSilverlightApp.Storage.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;


namespace ImoSilverlightApp.Managers
{
  internal class StickersManager : BaseManager
  {
    private IDictionary<string, StickerPack> stickerPacks;

    public event EventHandler<EventArg<StickerPack>> StickerPackAddedToLibrary;

    public event EventHandler<EventArg<StickerPack>> StickerPackRemovedFromLibrary;

    public event EventHandler StickersSynced;

    public StickersManager()
    {
      this.stickerPacks = (IDictionary<string, StickerPack>) new Dictionary<string, StickerPack>();
      IMO.AccountManager.SignedOn += new EventHandler<EventArg<SignOnData>>(this.AccountManager_SignedOn);
      IMO.Dispatcher.Resetted += new EventHandler(this.Dispatcher_Resetted);
      this.InitFromStorage();
    }

    private void Dispatcher_Resetted(object sender, EventArgs e)
    {
      foreach (StickerPack stickerPack in (IEnumerable<StickerPack>) this.stickerPacks.Values)
        stickerPack.InvalidatePendingRequests();
    }

    private void InitFromStorage()
    {
      foreach (StickerPack stickerPack in this.stickerPacks.Values.ToList<StickerPack>())
        this.RemoveStickerPack(stickerPack);
      foreach (KeyValuePair<string, JToken> stickerPack in IMO.ApplicationStorage.GetStickerPacks())
        this.AddStickerPack(JsonConvert.DeserializeObject<StickerPack>(stickerPack.Value.ToString()), true);
    }

    private void AccountManager_SignedOn(object sender, EventArg<SignOnData> e) => this.SyncPacks();

    public IList<StickerPack> GetLibraryStickerPacks()
    {
      return (IList<StickerPack>) this.stickerPacks.Values.ToList<StickerPack>();
    }

    private void SyncPacks()
    {
      if ((DateTime.Now - Utils.TimestampToDateTime(IMO.ApplicationSettings.LastStickerPacksSync)).TotalDays <= 1.0)
        return;
      IMO.StickersService.GetPack((Action<JToken>) (result =>
      {
        if (!(result is JArray))
          return;
        IMO.ApplicationSettings.LastStickerPacksSync = Utils.GetTimestamp();
        HashSet<string> stringSet = new HashSet<string>();
        foreach (JToken jtoken in (IEnumerable<JToken>) result)
        {
          string key = jtoken.Value<string>((object) "pack_id");
          stringSet.Add(key);
          if (this.stickerPacks.ContainsKey(key))
            this.stickerPacks[key].UpdateFromJObject(jtoken.ToObject<JObject>());
          else
            this.AddStickerPack(JsonConvert.DeserializeObject<StickerPack>(jtoken.ToString()));
        }
        foreach (StickerPack stickerPack in this.stickerPacks.Values.ToList<StickerPack>())
        {
          if (!stringSet.Contains(stickerPack.PackId))
            this.RemoveStickerPack(stickerPack);
        }
        this.OnStickersSynced();
      }));
    }

    internal StickerPack GetStickersPack(string packId)
    {
      StickerPack stickersPack = (StickerPack) null;
      this.stickerPacks.TryGetValue(packId, out stickersPack);
      return stickersPack;
    }

    private void AddStickerPack(StickerPack stickerPack, bool dontAddToStorage = false)
    {
      this.stickerPacks[stickerPack.PackId] = stickerPack;
      stickerPack.PropertyChanged += new PropertyChangedEventHandler(this.StickerPack_PropertyChanged);
      if (stickerPack.IsInLibrary)
        this.OnStickerPackAddedToLibrary(stickerPack);
      if (dontAddToStorage)
        return;
      IMO.ApplicationStorage.AddStickerPack(stickerPack);
    }

    private void RemoveStickerPack(StickerPack stickerPack)
    {
      this.stickerPacks.Remove(stickerPack.PackId);
      stickerPack.PropertyChanged -= new PropertyChangedEventHandler(this.StickerPack_PropertyChanged);
      if (!stickerPack.IsInLibrary)
        return;
      this.OnStickerPackRemovedFromLibrary(stickerPack);
    }

    private void OnStickerPackAddedToLibrary(StickerPack stickerPack)
    {
      EventHandler<EventArg<StickerPack>> packAddedToLibrary = this.StickerPackAddedToLibrary;
      if (packAddedToLibrary == null)
        return;
      packAddedToLibrary((object) this, new EventArg<StickerPack>(stickerPack));
    }

    private void OnStickersSynced()
    {
      EventHandler stickersSynced = this.StickersSynced;
      if (stickersSynced == null)
        return;
      stickersSynced((object) this, EventArgs.Empty);
    }

    private void OnStickerPackRemovedFromLibrary(StickerPack stickerPack)
    {
      EventHandler<EventArg<StickerPack>> removedFromLibrary = this.StickerPackRemovedFromLibrary;
      if (removedFromLibrary == null)
        return;
      removedFromLibrary((object) this, new EventArg<StickerPack>(stickerPack));
    }

    private void StickerPack_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      if (!(e.PropertyName == "IsInLibrary"))
        return;
      StickerPack stickerPack = (StickerPack) sender;
      if (stickerPack.IsInLibrary)
        this.OnStickerPackAddedToLibrary(stickerPack);
      else
        this.OnStickerPackRemovedFromLibrary(stickerPack);
    }
  }
}
