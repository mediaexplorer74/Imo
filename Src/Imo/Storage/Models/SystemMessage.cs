// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.Storage.Models.SystemMessage
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll


namespace ImoSilverlightApp.Storage.Models
{
  internal class SystemMessage : Message
  {
    public SystemMessage(MessageOrigin origin)
      : base(origin)
    {
    }

    public bool IsMissedVideoCall { get; private set; }

    public bool IsMissedCall { get; set; }

    public bool IsJoinedGroup { get; set; }

    public bool IsLeftGroup { get; set; }

    public bool IsKickedFromGroup { get; set; }

    public bool IsJustJoined { get; set; }

    public bool IsDeletedPhoto { get; set; }

    public string CallBackIcon { get; private set; }

    protected override void Init()
    {
      this.messageType = MessageType.System;
      this.IsMissedVideoCall = !"audio_chat".Equals(this.imdata.Value<string>((object) "chat_type"));
      if (this.IsJoinedGroup || this.IsJustJoined || this.IsLeftGroup || this.IsKickedFromGroup)
        this.msg = this.AuthorAlias + " " + this.msg;
      if (this.IsMissedCall)
      {
        if (this.msg.EndsWith("."))
          this.msg = this.msg.Substring(0, this.msg.Length - 1);
        this.CallBackIcon = this.IsMissedVideoCall ? "VideoCallIcon" : "AudioCallIcon";
      }
      if (!this.IsDeletedPhoto)
        return;
      this.msg = this.AuthorAlias + " uploaded a photo that was deleted";
    }
  }
}
