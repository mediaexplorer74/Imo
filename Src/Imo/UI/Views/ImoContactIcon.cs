// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.UI.Views.ImoContactIcon
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Storage.Models;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace ImoSilverlightApp.UI.Views
{
  public class ImoContactIcon : UserControl
  {
    public static readonly DependencyProperty ObservedDataContextProperty = DependencyProperty.Register("ObservedDataContext", typeof (object), typeof (ImoContactIcon), new PropertyMetadata((object) null, new PropertyChangedCallback(ImoContactIcon.OnObservedDataContextChanged)));
    private static readonly BitmapImage onlineImage = new BitmapImage(new Uri("/Assets/Icons/status_online_sm.png", UriKind.Relative));
    private static readonly BitmapImage awayImage = new BitmapImage(new Uri("/Assets/Icons/status_away_sm.png", UriKind.Relative));
    private static readonly BitmapImage offlineImage = new BitmapImage(new Uri("/Assets/Icons/status_offline_sm.png", UriKind.Relative));
    private Contact contact;
    public static readonly DependencyProperty showPrimitiveProperty = DependencyProperty.Register(nameof (ShowPrimitive), typeof (bool), typeof (ImoContactIcon), new PropertyMetadata((object) false, (PropertyChangedCallback) ((s, e) => ((ImoContactIcon) s).UpdatePrimitive())));
    public static readonly DependencyProperty navigateToConversationProperty = DependencyProperty.Register(nameof (NavigateToConversation), typeof (bool), typeof (ImoContactIcon), new PropertyMetadata((object) false));
    internal UserControl imoContactIconRoot;
    internal Grid clippedGrid;
    internal Ellipse ellipse;
    internal ImageBrush ellipseBrush;
    internal Image primitive;
    private bool _contentLoaded;

    private static void OnObservedDataContextChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      ((ImoContactIcon) d).OnDataContextChanged();
    }

    public ImoContactIcon()
    {
      this.InitializeComponent();
      this.Loaded += new RoutedEventHandler(this.ImoContactIcon_Loaded);
      this.Unloaded += new RoutedEventHandler(this.ImoContactIcon_Unloaded);
      BindingOperations.SetBinding((DependencyObject) this, ImoContactIcon.ObservedDataContextProperty, (BindingBase) new Binding());
    }

    private void OnDataContextChanged()
    {
      if (this.contact == this.DataContext)
        return;
      if (this.contact != null)
        this.contact.PropertyChanged -= new PropertyChangedEventHandler(this.Contact_PropertyChanged);
      this.contact = (Contact) this.DataContext;
      if (this.contact != null)
        this.contact.PropertyChanged += new PropertyChangedEventHandler(this.Contact_PropertyChanged);
      this.ReloadIcon();
      this.UpdatePrimitive();
    }

    private void ImoContactIcon_Loaded(object sender, RoutedEventArgs e)
    {
      if (this.contact == this.DataContext)
        return;
      if (this.contact != null)
        this.contact.PropertyChanged -= new PropertyChangedEventHandler(this.Contact_PropertyChanged);
      this.contact = (Contact) this.DataContext;
      if (this.contact != null)
        this.contact.PropertyChanged += new PropertyChangedEventHandler(this.Contact_PropertyChanged);
      this.ReloadIcon();
      this.UpdatePrimitive();
    }

    private void ImoContactIcon_Unloaded(object sender, RoutedEventArgs e)
    {
      if (this.contact == null)
        return;
      this.contact.PropertyChanged -= new PropertyChangedEventHandler(this.Contact_PropertyChanged);
      this.contact = (Contact) null;
    }

    private async void ReloadIcon()
    {
      this.ellipseBrush.ImageSource = (ImageSource) null;
      if (this.contact == null || this.contact.IconUrl == null)
        return;
      string currentUrl = this.contact.IconUrl;
      BitmapImage bitmapImage = await IMO.ImageLoader.LoadImage(this.contact.IconUrl);
      if (this.contact != null && currentUrl == this.contact.IconUrl)
        this.ellipseBrush.ImageSource = (ImageSource) bitmapImage;
      currentUrl = (string) null;
    }

    private void UpdatePrimitive()
    {
      if (this.contact == null || !this.ShowPrimitive)
        this.primitive.Source = (ImageSource) null;
      else if (this.contact.Primitive == Primitive.Available)
        this.primitive.Source = (ImageSource) ImoContactIcon.onlineImage;
      else if (this.contact.Primitive == Primitive.Away)
      {
        this.primitive.Source = (ImageSource) ImoContactIcon.awayImage;
      }
      else
      {
        if (this.contact.Primitive != Primitive.Offline)
          return;
        this.primitive.Source = (ImageSource) ImoContactIcon.offlineImage;
      }
    }

    private void Contact_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      if (e.PropertyName == "IconUrl")
      {
        this.ReloadIcon();
      }
      else
      {
        if (!(e.PropertyName == "Primitive"))
          return;
        this.UpdatePrimitive();
      }
    }

    public bool ShowPrimitive
    {
      get => (bool) this.GetValue(ImoContactIcon.showPrimitiveProperty);
      set => this.SetValue(ImoContactIcon.showPrimitiveProperty, (object) value);
    }

    public bool NavigateToConversation
    {
      get => (bool) this.GetValue(ImoContactIcon.navigateToConversationProperty);
      set => this.SetValue(ImoContactIcon.navigateToConversationProperty, (object) value);
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/ImoSilverlightApp;component/UI/Views/ImoContactIcon.xaml", UriKind.Relative));
      this.imoContactIconRoot = (UserControl) this.FindName("imoContactIconRoot");
      this.clippedGrid = (Grid) this.FindName("clippedGrid");
      this.ellipse = (Ellipse) this.FindName("ellipse");
      this.ellipseBrush = (ImageBrush) this.FindName("ellipseBrush");
      this.primitive = (Image) this.FindName("primitive");
    }
  }
}
