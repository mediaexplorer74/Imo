// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.UI.Views.BuddyProfilePage
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Storage.Models;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;


namespace ImoSilverlightApp.UI.Views
{
  public class BuddyProfilePage : ImoPage
  {
    private bool _contentLoaded;

    public BuddyProfilePage() => this.InitializeComponent();

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
      base.OnNavigatedTo(e);
      string buid = "";
      if (!this.NavigationContext.QueryString.TryGetValue("buid", out buid))
        return;
      this.DataContext = (object) IMO.ContactsManager.GetOrCreateContact(buid);
    }

    private async void DeleteContact_Tapped(object sender, GestureEventArgs e)
    {
      if (!(this.DataContext is Contact dataContext))
        return;
      if (!await IMO.ContactsManager.DeleteContact(dataContext, true))
        return;
      IMO.NavigationManager.NavigateToHome();
    }

    private void ViewAlbum_Tapped(object sender, GestureEventArgs e)
    {
      if (!(this.DataContext is Contact dataContext))
        return;
      IMO.NavigationManager.NavigateToGallery(dataContext.Buid);
    }

    private void ToggleSwitch_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
      if (!(this.DataContext is Contact dataContext))
        return;
      if (dataContext.IsFavorite)
        IMO.ContactsManager.AddContactToFavorites(dataContext);
      else
        IMO.ContactsManager.RemoveContactFromFavorites(dataContext);
    }

    private void Image_Tap(object sender, GestureEventArgs e)
    {
      IMO.NavigationManager.NavigateToPhotoPreview((this.DataContext as Contact).PhotoUrl, 0, 0);
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/ImoSilverlightApp;component/UI/Views/BuddyProfilePage.xaml", UriKind.Relative));
    }
  }
}
