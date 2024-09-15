// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.UI.Views.StickersPickerView
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using Imo.Phone.Controls;
using ImoSilverlightApp.Helpers;
using ImoSilverlightApp.Storage.Models;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace ImoSilverlightApp.UI.Views
{
  public class StickersPickerView : UserControl
  {
    internal ListBox stickerPacskItemsControl;
    internal FlipView flipView;
    private bool _contentLoaded;

    public event EventHandler<EventArg<Sticker>> StickerPicked;

    public StickersPickerView()
    {
      this.InitializeComponent();
      this.DataContext = (object) (this.ViewModel = new StickersPickerViewModel(this));
      this.Loaded += new RoutedEventHandler(this.StickersPickerView_Loaded);
    }

    private void StickersPickerView_Loaded(object sender, RoutedEventArgs e)
    {
    }

    private StickersPickerViewModel ViewModel { get; set; }

    private void StickerPack_Tapped(object sender, GestureEventArgs e)
    {
      StickerItemViewModel viewModelOf = VisualUtils.GetViewModelOf<StickerItemViewModel>(sender);
      if (viewModelOf == null)
        return;
      this.ViewModel.SelectedStickerItem = viewModelOf;
    }

    private void Sticker_Tapped(object sender, GestureEventArgs e)
    {
      Sticker viewModelOf = VisualUtils.GetViewModelOf<Sticker>(sender);
      if (viewModelOf == null)
        return;
      this.OnStickerPicked(viewModelOf);
    }

    private void OnStickerPicked(Sticker sticker)
    {
      EventHandler<EventArg<Sticker>> stickerPicked = this.StickerPicked;
      if (stickerPicked == null)
        return;
      stickerPicked((object) this, new EventArg<Sticker>(sticker));
    }

    internal void ScrollSelectedStickerPackIntoView()
    {
      StickerItemViewModel selectedStickerItem = this.ViewModel.SelectedStickerItem;
      if (selectedStickerItem == null)
        return;
      foreach (object obj in (PresentationFrameworkCollection<object>) this.stickerPacskItemsControl.Items)
      {
        StickerItemViewModel stickerItemViewModel = obj as StickerItemViewModel;
        if (stickerItemViewModel == selectedStickerItem)
        {
          this.stickerPacskItemsControl.UpdateLayout();
          this.stickerPacskItemsControl.ScrollIntoView((object) stickerItemViewModel);
          break;
        }
      }
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/ImoSilverlightApp;component/UI/Views/StickersPickerView.xaml", UriKind.Relative));
      this.stickerPacskItemsControl = (ListBox) this.FindName("stickerPacskItemsControl");
      this.flipView = (FlipView) this.FindName("flipView");
    }
  }
}
