// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.UI.Views.StickersPickerViewModel
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Storage.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;


namespace ImoSilverlightApp.UI.Views
{
  internal class StickersPickerViewModel : ViewModelBase
  {
    private ObservableCollection<StickerItemViewModel> stickerItems;
    private StickerItemViewModel selectedStickerItem;
    private StickersPickerView view;
    private bool disableStoreSelection;

    public StickersPickerViewModel(StickersPickerView view)
      : base((FrameworkElement) view)
    {
      this.view = view;
      this.stickerItems = new ObservableCollection<StickerItemViewModel>();
    }

    protected override void OnLoaded(object sender, RoutedEventArgs e)
    {
      this.stickerItems.Clear();
      this.stickerItems.Add((StickerItemViewModel) DrawingCanvasViewModel.Instance);
      foreach (StickerPack libraryStickerPack in (IEnumerable<StickerPack>) IMO.StickersManager.GetLibraryStickerPacks())
      {
        this.disableStoreSelection = true;
        this.stickerItems.Add((StickerItemViewModel) new StickerPackViewModel(libraryStickerPack));
        this.disableStoreSelection = false;
      }
      if (this.stickerItems.Count > 0)
        this.HandleStickersSynced();
      IMO.StickersManager.StickerPackAddedToLibrary += new EventHandler<EventArg<StickerPack>>(this.StickersManager_StickerPackAddedToLibrary);
      IMO.StickersManager.StickerPackRemovedFromLibrary += new EventHandler<EventArg<StickerPack>>(this.StickersManager_StickerPackRemovedFromLibrary);
      IMO.StickersManager.StickersSynced += new EventHandler(this.StickersManager_StickersSynced);
    }

    protected override void OnUnloaded(object sender, RoutedEventArgs e)
    {
      IMO.StickersManager.StickerPackAddedToLibrary -= new EventHandler<EventArg<StickerPack>>(this.StickersManager_StickerPackAddedToLibrary);
      IMO.StickersManager.StickerPackRemovedFromLibrary -= new EventHandler<EventArg<StickerPack>>(this.StickersManager_StickerPackRemovedFromLibrary);
      IMO.StickersManager.StickersSynced -= new EventHandler(this.StickersManager_StickersSynced);
    }

    private void StickersManager_StickersSynced(object sender, EventArgs e)
    {
      this.HandleStickersSynced();
    }

    private void HandleStickersSynced()
    {
      if (this.stickerItems.Count <= 1)
        return;
      if (IMO.ApplicationSettings.SelectedStickersPack == null)
        IMO.ApplicationSettings.SelectedStickersPack = (this.stickerItems[1] as StickerPackViewModel).StickerPack.PackId;
      StickerPack stickerPack = IMO.StickersManager.GetStickersPack(IMO.ApplicationSettings.SelectedStickersPack);
      if (stickerPack == null)
      {
        stickerPack = (this.stickerItems[1] as StickerPackViewModel).StickerPack;
        IMO.ApplicationSettings.SelectedStickersPack = stickerPack.PackId;
      }
      this.SelectedStickerItem = this.stickerItems.Where<StickerItemViewModel>((Func<StickerItemViewModel, bool>) (x => x is StickerPackViewModel && (x as StickerPackViewModel).StickerPack == stickerPack)).First<StickerItemViewModel>();
    }

    private void StickersManager_StickerPackAddedToLibrary(object sender, EventArg<StickerPack> e)
    {
      this.stickerItems.Add((StickerItemViewModel) new StickerPackViewModel(e.Arg));
    }

    private void StickersManager_StickerPackRemovedFromLibrary(
      object sender,
      EventArg<StickerPack> e)
    {
      this.stickerItems.Remove((StickerItemViewModel) new StickerPackViewModel(e.Arg));
    }

    public ObservableCollection<StickerItemViewModel> StickerItems => this.stickerItems;

    public StickerItemViewModel SelectedStickerItem
    {
      get => this.selectedStickerItem;
      set
      {
        if (this.selectedStickerItem == value)
          return;
        if (this.selectedStickerItem != null)
          this.selectedStickerItem.IsSelected = false;
        this.selectedStickerItem = value;
        if (this.selectedStickerItem != null)
        {
          if (this.selectedStickerItem is StickerPackViewModel && this.selectedStickerItem is StickerPackViewModel selectedStickerItem)
          {
            if (!this.disableStoreSelection)
              IMO.ApplicationSettings.SelectedStickersPack = selectedStickerItem.StickerPack.PackId;
            selectedStickerItem.StickerPack.SyncStickers();
          }
          this.selectedStickerItem.IsSelected = true;
          this.view.ScrollSelectedStickerPackIntoView();
        }
        this.OnPropertyChanged(nameof (SelectedStickerItem));
      }
    }
  }
}
