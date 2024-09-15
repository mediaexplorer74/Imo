// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.UI.Views.MessageTemplates.MePhotoMessage
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Helpers;
using ImoSilverlightApp.Storage.Models;
using NLog;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;


namespace ImoSilverlightApp.UI.Views.MessageTemplates
{
  public sealed class MePhotoMessage : Grid
  {
    private static readonly Logger log = LogManager.GetLogger(typeof (BuddyPhotoMessage).Name);
    private bool isSavingImage;
    internal Grid selfIconGrid;
    internal Ellipse ellipse;
    private bool _contentLoaded;

    public MePhotoMessage()
    {
      this.InitializeComponent();
      this.selfIconGrid.DataContext = (object) this;
    }

    private void ImoImage_Tapped(object sender, GestureEventArgs e)
    {
      if (!(this.DataContext is PhotoMessage dataContext))
        return;
      IMO.NavigationManager.NavigateToPhotoPreview(dataContext.PhotoUrl, dataContext.Width, dataContext.Height, dataContext.Buid, dataContext.PhotoID);
    }

    private void Share_Click(object sender, RoutedEventArgs e)
    {
      if (!(this.DataContext is PhotoMessage dataContext))
        return;
      IMO.NavigationManager.NavigateToShareObjectToMembersPage(dataContext.PhotoID, "photo");
    }

    private async void Download_Click(object sender, RoutedEventArgs e)
    {
      PhotoMessage dataContext = this.DataContext as PhotoMessage;
      if (this.isSavingImage || dataContext == null)
        return;
      this.isSavingImage = true;
      DateTime dateTime;
      try
      {
        dateTime = Utils.UnixTimeStampToDateTime((double) dataContext.Timestamp / 1000.0);
      }
      catch (Exception ex)
      {
        MePhotoMessage.log.Error(ex, "Failed to convert Timestamp to DateTime: {0}" + (object) dataContext.Timestamp, 62, nameof (Download_Click));
        dateTime = DateTime.Now;
      }
      int num = 0;
      object obj;
      try
      {
        await IMO.ImageLoader.SaveImageAs(dataContext.PhotoUrl, dateTime);
      }
      catch (Exception ex)
      {
        obj = (object) ex;
        num = 1;
      }
      if (num == 1)
      {
        ImoMessageBoxResult messageBoxResult = await ImoMessageBox.Show("Failed to save image!");
        this.isSavingImage = false;
      }
      else
      {
        obj = (object) null;
        this.isSavingImage = false;
        IMO.MonitorLog.Log("media_messages", "save_photo");
        ImoMessageBoxResult messageBoxResult = await ImoMessageBox.Show("Image saved to library!");
      }
    }

    private void Delete_Click(object sender, RoutedEventArgs e)
    {
      IMO.IM.DeleteMessage(this.DataContext as Message);
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
      Application.LoadComponent((object) this, new Uri("/ImoSilverlightApp;component/UI/Views/MessageTemplates/MePhotoMessage.xaml", UriKind.Relative));
      this.selfIconGrid = (Grid) this.FindName("selfIconGrid");
      this.ellipse = (Ellipse) this.FindName("ellipse");
    }
  }
}
