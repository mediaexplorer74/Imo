// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.UI.Views.PhotoStreamViewerPageViewModel
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Storage.Models;
using System.Windows;


namespace ImoSilverlightApp.UI.Views
{
  internal class PhotoStreamViewerPageViewModel : ViewModelBase
  {
    private PhotoStream photoStream;
    private string buid;

    public PhotoStreamViewerPageViewModel(
      FrameworkElement element,
      string buid,
      PhotoStream photoStream)
      : base(element)
    {
      this.photoStream = photoStream;
      this.buid = buid;
    }

    public PhotoStream PhotoStream => this.photoStream;

    public string Buid => this.buid;
  }
}
