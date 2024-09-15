// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.UI.Views.GalleryPageViewModel
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Storage.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;


namespace ImoSilverlightApp.UI.Views
{
  internal class GalleryPageViewModel : ViewModelBase
  {
    public PhotoStream PhotoStream { get; private set; }

    public string Buid { get; private set; }

    public Contact Contact { get; private set; }

    public ObservableCollection<GalleryRowViewModel> PhotoRows { get; private set; }

    public GalleryPageViewModel(string buid, FrameworkElement el)
      : base(el)
    {
      this.PhotoStream = IMO.PhotoStreamsManager.GetOrCreatePhotoStream(buid);
      this.Buid = buid;
      this.Contact = IMO.ContactsManager.GetOrCreateContact(buid);
      this.PhotoRows = new ObservableCollection<GalleryRowViewModel>();
      this.SyncLoadedParts();
      this.PhotoStream.PhotosSynced += new EventHandler<EventArgs>(this.PhotoStream_PhotosSynced);
    }

    public override void Dispose()
    {
      base.Dispose();
      this.PhotoStream.PhotosSynced -= new EventHandler<EventArgs>(this.PhotoStream_PhotosSynced);
    }

    private void PhotoStream_PhotosSynced(object sender, EventArgs e)
    {
      this.PhotoRows.Clear();
      this.SyncLoadedParts();
    }

    public void SyncLoadedParts()
    {
      List<Photo> photoList = new List<Photo>();
      foreach (Photo photo in (Collection<Photo>) this.PhotoStream.Photos)
      {
        if (photoList.Count == 3)
        {
          this.PhotoRows.Add(new GalleryRowViewModel()
          {
            Photos = (IList<Photo>) photoList
          });
          photoList = new List<Photo>();
        }
        photoList.Add(photo);
      }
      if (photoList.Count <= 0)
        return;
      this.PhotoRows.Add(new GalleryRowViewModel()
      {
        Photos = (IList<Photo>) photoList
      });
    }
  }
}
