// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.UI.Views.SettingsPageViewModel
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Helpers;
using ImoSilverlightApp.Storage;
using ImoSilverlightApp.Storage.Models;
using System.ComponentModel;
using System.Windows;


namespace ImoSilverlightApp.UI.Views
{
  internal class SettingsPageViewModel : ViewModelBase
  {
    private bool isUploading;

    public SettingsPageViewModel(FrameworkElement el)
      : base(el)
    {
    }

    protected override void OnLoaded(object sender, RoutedEventArgs e)
    {
      IMO.User.PropertyChanged += new PropertyChangedEventHandler(this.User_PropertyChanged);
    }

    protected override void OnUnloaded(object sender, RoutedEventArgs e)
    {
      IMO.User.PropertyChanged -= new PropertyChangedEventHandler(this.User_PropertyChanged);
    }

    private void User_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      if (!(e.PropertyName == "ProfilePhotoId"))
        return;
      this.OnPropertyChanged("ProfilePhotoUrl");
    }

    public string ProfilePhotoUrl
    {
      get => ImageUtils.GetPhotoUrlFromId(this.User.ProfilePhotoId, PictureSize.Medium);
    }

    public User User => IMO.User;

    public ApplicationSettings ApplicationSettings => IMO.ApplicationSettings;

    public ApplicationProperties ApplicationProperties => IMO.ApplicationProperties;

    public bool IsUploading
    {
      get => this.isUploading;
      set
      {
        if (this.isUploading == value)
          return;
        this.isUploading = value;
        this.OnPropertyChanged(nameof (IsUploading));
      }
    }
  }
}
