// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.Storage.Models.StickerPack
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;


namespace ImoSilverlightApp.Storage.Models
{
  internal class StickerPack : ModelBase
  {
    [JsonProperty(PropertyName = "pack_id")]
    private string packId;
    [JsonProperty(PropertyName = "name")]
    private string name;
    [JsonProperty(PropertyName = "num_stickers")]
    private int stickersCount;
    private ObservableCollection<Sticker> stickers;
    private bool isInLibrary;
    private bool isSyncing;

    public ObservableCollection<Sticker> Stickers
    {
      get
      {
        if (this.stickers == null)
          this.stickers = new ObservableCollection<Sticker>();
        return this.stickers;
      }
    }

    public string PackId => this.packId;

    public bool IsInLibrary
    {
      get => true;
      set
      {
        if (this.isInLibrary == value)
          return;
        this.isInLibrary = value;
        this.OnPropertyChanged(nameof (IsInLibrary));
      }
    }

    public bool IsSynced => this.Stickers.Count > 0;

    public bool IsSyncing
    {
      get => this.isSyncing;
      set
      {
        if (this.IsSyncing == value)
          return;
        this.isSyncing = value;
        this.OnPropertyChanged(nameof (IsSyncing));
      }
    }

    public void InvalidatePendingRequests() => this.IsSyncing = false;

    internal void UpdateFromJObject(JObject jObj)
    {
      JsonConvert.PopulateObject(jObj.ToString(), (object) this);
      this.UpdateStorageIfNeeded();
    }

    public void UpdateStorageIfNeeded()
    {
      if (!this.IsInLibrary)
        return;
      IMO.ApplicationStorage.AddStickerPack(this);
    }

    public string ImageUrl => "s/stickers/v1/packs/" + this.packId + "/thumbnail/2x";

    public void SyncStickers()
    {
      if (this.IsSynced || this.IsSyncing)
        return;
      this.IsSyncing = true;
      IMO.StickersService.GetPackStickers(this.packId, (Action<JToken>) (result =>
      {
        this.IsSyncing = false;
        if (!(result is JArray))
          return;
        this.Stickers.Clear();
        foreach (JToken jtoken in (IEnumerable<JToken>) result)
          this.Stickers.Add(Sticker.GetSticker(jtoken.ToObject<JObject>()));
        this.UpdateStorageIfNeeded();
      }));
    }
  }
}
