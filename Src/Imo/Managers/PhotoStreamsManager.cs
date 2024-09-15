// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.Managers.PhotoStreamsManager
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Storage.Models;
using System;
using System.Collections.Generic;


namespace ImoSilverlightApp.Managers
{
  internal class PhotoStreamsManager
  {
    private Dictionary<string, PhotoStream> photoStreams;

    public PhotoStreamsManager()
    {
      this.photoStreams = new Dictionary<string, PhotoStream>();
      IMO.Dispatcher.Resetted += new EventHandler(this.Dispatcher_Resetted);
    }

    private void Dispatcher_Resetted(object sender, EventArgs e)
    {
      foreach (PhotoStream photoStream in this.photoStreams.Values)
        photoStream.InvalidateIsSynced();
    }

    public PhotoStream GetOrCreatePhotoStream(string buid)
    {
      if (!this.photoStreams.ContainsKey(buid))
        this.photoStreams.Add(buid, new PhotoStream(buid));
      return this.photoStreams[buid];
    }
  }
}
