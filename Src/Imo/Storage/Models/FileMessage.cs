// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.Storage.Models.FileMessage
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Helpers;
using NLog;


namespace ImoSilverlightApp.Storage.Models
{
  internal class FileMessage : Message
  {
    private static readonly Logger log = LogManager.GetLogger(typeof (FileMessage).Name);
    private string fileName;
    private string fileUrl;
    private string localPath = "";
    private bool isDownloaded;

    public FileMessage(MessageOrigin origin)
      : base(origin)
    {
    }

    protected override void Init()
    {
      this.msg = "sent a file";
      string url = this.imdata.Value<string>((object) "url");
      if (url != null)
      {
        this.fileName = Utils.GetFileNameFromUrl(url);
        this.fileUrl = url;
      }
      else
        FileMessage.log.Error("File uploaded message has null url", 35, nameof (Init));
    }

    public string FileName => this.fileName;

    public string FileUrl => this.fileUrl;

    public string LocalPath
    {
      get => this.localPath;
      set => this.localPath = value;
    }

    public bool IsDownloaded
    {
      get => this.isDownloaded;
      set
      {
        if (this.isDownloaded == value)
          return;
        this.isDownloaded = value;
        this.OnPropertyChanged(nameof (IsDownloaded));
      }
    }
  }
}
