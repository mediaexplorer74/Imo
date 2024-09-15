// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.UI.Views.Icon
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;


namespace ImoSilverlightApp.UI.Views
{
  public class Icon : UserControl
  {
    public static readonly DependencyProperty iconNameProperty = DependencyProperty.Register(nameof (IconName), typeof (string), typeof (Icon), new PropertyMetadata((object) null, new PropertyChangedCallback(Icon.IconNameChangedHandler)));
    internal UserControl iconRoot;
    internal Path path;
    private bool _contentLoaded;

    public event EventHandler Click;

    public Icon()
    {
      this.InitializeComponent();
      this.Loaded += new RoutedEventHandler(this.Icon_Loaded);
    }

    private void Icon_Loaded(object sender, RoutedEventArgs e)
    {
    }

    private static void IconNameChangedHandler(
      DependencyObject dependencyObject,
      DependencyPropertyChangedEventArgs args)
    {
      Path path = ((Icon) dependencyObject).path;
      if (args.NewValue != null)
      {
        string str = (string) Application.Current.Resources[(object) ((string) args.NewValue + "Data")];
        if (str[0] != 'F')
          str = "F1" + str;
        path.Data = (Geometry) XamlReader.Load("<Geometry xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'>" + str + "</Geometry>");
      }
      else
        path.Data = (Geometry) null;
    }

    public string IconName
    {
      get => (string) this.GetValue(Icon.iconNameProperty);
      set => this.SetValue(Icon.iconNameProperty, (object) value);
    }

    private void OnClick()
    {
      EventHandler click = this.Click;
      if (click == null)
        return;
      click((object) this, EventArgs.Empty);
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/ImoSilverlightApp;component/UI/Views/Icon.xaml", UriKind.Relative));
      this.iconRoot = (UserControl) this.FindName("iconRoot");
      this.path = (Path) this.FindName("path");
    }
  }
}
