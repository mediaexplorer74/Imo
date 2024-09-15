// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.UI.Views.MessageTemplates.MeVideoMessage
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Helpers;
using ImoSilverlightApp.Storage.Models;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;


namespace ImoSilverlightApp.UI.Views.MessageTemplates
{
  public sealed class MeVideoMessage : Grid
  {
    internal Grid selfIconGrid;
    internal Ellipse ellipse;
    private bool _contentLoaded;

    public MeVideoMessage()
    {
      this.InitializeComponent();
      this.selfIconGrid.DataContext = (object) this;
    }

    public User User => IMO.User;

    public string IconUrl => ImageUtils.GetPhotoUrlFromId(this.User.ProfilePhotoId);

    public MessageMergeState MergeType
    {
      get
      {
        return !(this.DataContext is Message dataContext) ? MessageMergeState.NONE : dataContext.MergeType;
      }
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/ImoSilverlightApp;component/UI/Views/MessageTemplates/MeVideoMessage.xaml", UriKind.Relative));
      this.selfIconGrid = (Grid) this.FindName("selfIconGrid");
      this.ellipse = (Ellipse) this.FindName("ellipse");
    }
  }
}
