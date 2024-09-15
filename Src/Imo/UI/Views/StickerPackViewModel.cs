// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.UI.Views.StickerPackViewModel
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Storage.Models;


namespace ImoSilverlightApp.UI.Views
{
  internal class StickerPackViewModel : StickerItemViewModel
  {
    private StickerPack stickerPack;

    public StickerPackViewModel(StickerPack stickerPack) => this.stickerPack = stickerPack;

    public StickerPack StickerPack => this.stickerPack;

    public override bool Equals(object obj)
    {
      return obj is StickerPackViewModel && this.stickerPack == (obj as StickerPackViewModel).StickerPack;
    }

    public override int GetHashCode() => this.stickerPack.GetHashCode();
  }
}
