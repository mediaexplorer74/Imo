// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.UI.Views.ImoSelfIcon
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Helpers;
using ImoSilverlightApp.Storage.Models;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;


namespace ImoSilverlightApp.UI.Views
{
  public class ImoSelfIcon : UserControl, INotifyPropertyChanged
  {
    private PictureSize imageSize;
    public static readonly DependencyProperty showLoadingProperty = DependencyProperty.Register(nameof (ShowLoading), typeof (bool), typeof (ImoSelfIcon), new PropertyMetadata((object) false));
    internal UserControl selfIconRoot;
    internal Ellipse ellipse;
    internal ImageBrush ellipseBrush;
    private bool _contentLoaded;

    public ImoSelfIcon()
    {
      this.InitializeComponent();
      this.DataContext = (object) this;
      this.Loaded += new RoutedEventHandler(this.ImoSelfIcon_Loaded);
      this.Unloaded += new RoutedEventHandler(this.ImoSelfIcon_Unloaded);
    }

    private void ImoSelfIcon_Loaded(object sender, RoutedEventArgs e)
    {
      IMO.User.PropertyChanged += new PropertyChangedEventHandler(this.User_PropertyChanged);
      this.ReloadIcon();
    }

    private async void ReloadIcon()
    {
      if (IMO.User.ProfilePhotoUrl == null)
        return;
      this.ellipseBrush.ImageSource = (ImageSource) await IMO.ImageLoader.LoadImage(IMO.User.ProfilePhotoUrl);
    }

    private void ImoSelfIcon_Unloaded(object sender, RoutedEventArgs e)
    {
      IMO.User.PropertyChanged -= new PropertyChangedEventHandler(this.User_PropertyChanged);
    }

    public bool ShowLoading
    {
      get => (bool) this.GetValue(ImoSelfIcon.showLoadingProperty);
      set => this.SetValue(ImoSelfIcon.showLoadingProperty, (object) value);
    }

    private void User_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      if (!(e.PropertyName == "ProfilePhotoId"))
        return;
      this.ReloadIcon();
    }

    public User User => IMO.User;

    public PictureSize ImageSize
    {
      get => this.imageSize;
      set
      {
        if (this.imageSize == value)
          return;
        this.imageSize = value;
        this.OnPropertyChanged("PhotoUrl");
      }
    }

    public string PhotoUrl
    {
      get => ImageUtils.GetPhotoUrlFromId(this.User.ProfilePhotoId, this.imageSize);
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string name)
    {
      PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
      if (propertyChanged == null)
        return;
      propertyChanged((object) this, new PropertyChangedEventArgs(name));
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/ImoSilverlightApp;component/UI/Views/ImoSelfIcon.xaml", UriKind.Relative));
      this.selfIconRoot = (UserControl) this.FindName("selfIconRoot");
      this.ellipse = (Ellipse) this.FindName("ellipse");
      this.ellipseBrush = (ImageBrush) this.FindName("ellipseBrush");
    }
  }
}
