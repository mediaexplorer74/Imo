// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.Storage.Models.StickerMessage
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll


namespace ImoSilverlightApp.Storage.Models
{
  internal class StickerMessage : Message
  {
    public Sticker Sticker { get; private set; }

    public string StickerUrl { get; private set; }

    protected override void Init()
    {
      this.msg = "sent a sticker";
      this.Sticker = Sticker.GetSticker(this.imdata);
      this.StickerUrl = this.Sticker.Url;
    }

    public StickerMessage(MessageOrigin origin)
      : base(origin)
    {
    }
  }
}
