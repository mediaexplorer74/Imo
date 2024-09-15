// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.UI.Views.IconCircle
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;


namespace ImoSilverlightApp.UI.Views
{
  public class IconCircle : UserControl
  {
    public static readonly DependencyProperty iconNameProperty = DependencyProperty.Register(nameof (IconName), typeof (string), typeof (IconCircle), new PropertyMetadata((object) null, new PropertyChangedCallback(IconCircle.IconNameChanged)));
    internal UserControl iconCircleRoot;
    internal Border container;
    internal Icon icon;
    private bool _contentLoaded;

    public IconCircle()
    {
      this.InitializeComponent();
      this.Loaded += new RoutedEventHandler(this.IconCircle_Loaded);
    }

    private void IconCircle_Loaded(object sender, RoutedEventArgs e) => this.HandleSizeChanged();

    private static void IconNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      IconCircle iconCircle = (IconCircle) d;
      iconCircle.icon.IconName = iconCircle.IconName;
    }

    public string IconName
    {
      get => (string) this.GetValue(IconCircle.iconNameProperty);
      set => this.SetValue(IconCircle.iconNameProperty, (object) value);
    }

    private void root_SizeChanged(object sender, SizeChangedEventArgs e)
    {
      this.HandleSizeChanged();
    }

    private void HandleSizeChanged()
    {
      this.container.CornerRadius = new CornerRadius(this.iconCircleRoot.Height * 0.5);
      this.icon.Margin = new Thickness(this.iconCircleRoot.Height * 0.25);
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/ImoSilverlightApp;component/UI/Views/IconCircle.xaml", UriKind.Relative));
      this.iconCircleRoot = (UserControl) this.FindName("iconCircleRoot");
      this.container = (Border) this.FindName("container");
      this.icon = (Icon) this.FindName("icon");
    }
  }
}
