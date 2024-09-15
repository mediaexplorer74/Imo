// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.Storage.Models.PhotoStream
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Helpers;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.ObjectModel;


namespace ImoSilverlightApp.Storage.Models
{
  public class PhotoStream : ModelBase
  {
    private string buid;
    private string streamId;
    private ObservableCollection<Photo> photos;
    private bool isSynced;

    public event EventHandler<EventArgs> PhotosSynced;

    public PhotoStream(string buid)
    {
      this.buid = buid;
      this.streamId = Utils.GenerateStreamId(IMO.User.Uid, buid);
      this.photos = new ObservableCollection<Photo>();
    }

    internal void InvalidateIsSynced() => this.isSynced = false;

    public void SetPhotosFromResponse(JArray jsonStream)
    {
      this.photos.Clear();
      foreach (JObject json in jsonStream)
        this.photos.Add(new Photo(this.buid, json));
      this.OnPhotosSynced();
    }

    public string StreamId => this.streamId;

    public ObservableCollection<Photo> Photos => this.photos;

    public bool IsSynced => this.isSynced;

    public void OnPhotosSynced()
    {
      EventHandler<EventArgs> photosSynced = this.PhotosSynced;
      if (photosSynced == null)
        return;
      photosSynced((object) this, EventArgs.Empty);
    }

    public void SyncPhotos()
    {
      if (this.isSynced)
        return;
      IMO.Pixel.GetObjects(this.buid, (Action<JToken>) (result =>
      {
        this.isSynced = true;
        JArray jsonStream = result.Value<JArray>((object) "objects");
        if (jsonStream == null)
          return;
        IMO.PhotoStreamsManager.GetOrCreatePhotoStream(this.buid).SetPhotosFromResponse(jsonStream);
      }));
    }
  }
}
