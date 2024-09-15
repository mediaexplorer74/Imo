// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.UI.Views.IconButton
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;


namespace ImoSilverlightApp.UI.Views
{
  public class IconButton : UserControl
  {
    public static readonly DependencyProperty iconNameProperty = DependencyProperty.Register(nameof (IconName), typeof (string), typeof (IconButton), new PropertyMetadata((object) null, new PropertyChangedCallback(IconButton.IconNameChangedHandler)));
    public static readonly DependencyProperty iconSizeProperty = DependencyProperty.Register(nameof (IconSize), typeof (double), typeof (IconButton), new PropertyMetadata((object) 0.0));
    internal UserControl iconButtonRoot;
    internal Grid grid;
    internal Path path;
    private bool _contentLoaded;

    public event EventHandler Click;

    public IconButton()
    {
      this.InitializeComponent();
      this.Loaded += new RoutedEventHandler(this.IconButton_Loaded);
    }

    private void IconButton_Loaded(object sender, RoutedEventArgs e)
    {
    }

    public string IconName
    {
      get => (string) this.GetValue(IconButton.iconNameProperty);
      set => this.SetValue(IconButton.iconNameProperty, (object) value);
    }

    private static void IconNameChangedHandler(
      DependencyObject dependencyObject,
      DependencyPropertyChangedEventArgs args)
    {
      Path path = ((IconButton) dependencyObject).path;
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

    public double IconSize
    {
      get => (double) this.GetValue(IconButton.iconSizeProperty);
      set => this.SetValue(IconButton.iconSizeProperty, (object) value);
    }

    private void Grid_Tapped(object sender, GestureEventArgs e)
    {
      if (!this.IsEnabled)
        return;
      e.Handled = true;
      this.OnClick();
    }

    private void OnClick()
    {
      EventHandler click = this.Click;
      if (click == null)
        return;
      click((object) this, EventArgs.Empty);
    }

    private void Grid_PointerReleased(object sender, MouseEventArgs e)
    {
      this.grid.Background = (Brush) new SolidColorBrush(Colors.Transparent);
    }

    private void Grid_PointerPressed(object sender, MouseButtonEventArgs e)
    {
      if (this.path.Fill == null)
        return;
      this.grid.Background = (Brush) new SolidColorBrush(((SolidColorBrush) this.path.Fill).Color with
      {
        A = (byte) 24
      });
    }

    private void Grid_PointerExited(object sender, MouseEventArgs e)
    {
      this.grid.Background = (Brush) new SolidColorBrush(Colors.Transparent);
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/ImoSilverlightApp;component/UI/Views/IconButton.xaml", UriKind.Relative));
      this.iconButtonRoot = (UserControl) this.FindName("iconButtonRoot");
      this.grid = (Grid) this.FindName("grid");
      this.path = (Path) this.FindName("path");
    }
  }
}
