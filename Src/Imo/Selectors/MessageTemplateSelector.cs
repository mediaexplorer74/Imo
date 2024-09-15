// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.Selectors.MessageTemplateSelector
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Storage.Models;
using System.Windows;


namespace ImoSilverlightApp.Selectors
{
  public class MessageTemplateSelector : DataTemplateSelector
  {
    public DataTemplate MsgMeTemplate { get; set; }

    public DataTemplate MsgBuddyTemplate { get; set; }

    public DataTemplate PhotoMeTemplate { get; set; }

    public DataTemplate PhotoBuddyTemplate { get; set; }

    public DataTemplate VideoMeTemplate { get; set; }

    public DataTemplate VideoBuddyTemplate { get; set; }

    public DataTemplate AudioMeTemplate { get; set; }

    public DataTemplate AudioBuddyTemplate { get; set; }

    public DataTemplate StickerMeTemplate { get; set; }

    public DataTemplate StickerBuddyTemplate { get; set; }

    public DataTemplate SystemTemplate { get; set; }

    public override DataTemplate SelectTemplate(object item, DependencyObject container)
    {
      if (!(item is Message message))
        return this.MsgMeTemplate;
      if (message.MessageType == MessageType.System)
        return this.SystemTemplate;
      if (message.IsMyMessage)
      {
        switch (message)
        {
          case PhotoMessage _:
            return this.PhotoMeTemplate;
          case StickerMessage _:
            return this.StickerMeTemplate;
          case VideoMessage _:
            return this.VideoMeTemplate;
          case AudioMessage _:
            return this.AudioMeTemplate;
          default:
            return this.MsgMeTemplate;
        }
      }
      else
      {
        switch (message)
        {
          case PhotoMessage _:
            return this.PhotoBuddyTemplate;
          case StickerMessage _:
            return this.StickerBuddyTemplate;
          case VideoMessage _:
            return this.VideoBuddyTemplate;
          case AudioMessage _:
            return this.AudioBuddyTemplate;
          default:
            return this.MsgBuddyTemplate;
        }
      }
    }
  }
}
