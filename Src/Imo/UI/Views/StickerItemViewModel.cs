// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.UI.Views.StickerItemViewModel
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Storage.Models;


namespace ImoSilverlightApp.UI.Views
{
  internal abstract class StickerItemViewModel : ModelBase
  {
    private bool isSelected;

    public bool IsSelected
    {
      get => this.isSelected;
      set
      {
        if (this.isSelected == value)
          return;
        this.isSelected = value;
        this.OnPropertyChanged(nameof (IsSelected));
      }
    }
  }
}
