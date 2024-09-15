// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.UI.Views.IconCircleStrokedButton
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;


namespace ImoSilverlightApp.UI.Views
{
  public class IconCircleStrokedButton : UserControl
  {
    private Brush originalEllipseFill;
    public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.Register(nameof (StrokeThickness), typeof (int), typeof (IconCircleStrokedButton), new PropertyMetadata((object) 2));
    public static readonly DependencyProperty StrokeProperty = DependencyProperty.Register(nameof (Stroke), typeof (Brush), typeof (IconCircleStrokedButton), new PropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty IconNameProperty = DependencyProperty.Register(nameof (IconName), typeof (string), typeof (IconCircleStrokedButton), new PropertyMetadata((object) string.Empty));
    internal UserControl rootIconButton;
    internal Grid layoutRoot;
    internal Ellipse ellipse;
    private bool _contentLoaded;

    public int StrokeThickness
    {
      get => (int) this.GetValue(IconCircleStrokedButton.StrokeThicknessProperty);
      set => this.SetValue(IconCircleStrokedButton.StrokeThicknessProperty, (object) value);
    }

    public Brush Stroke
    {
      get => (Brush) this.GetValue(IconCircleStrokedButton.StrokeProperty);
      set => this.SetValue(IconCircleStrokedButton.StrokeProperty, (object) value);
    }

    public string IconName
    {
      get => (string) this.GetValue(IconCircleStrokedButton.IconNameProperty);
      set => this.SetValue(IconCircleStrokedButton.IconNameProperty, (object) value);
    }

    public IconCircleStrokedButton() => this.InitializeComponent();

    private void layoutRoot_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      if (this.Foreground == null)
        return;
      Color color = ((SolidColorBrush) this.Foreground).Color with
      {
        A = 170
      };
      this.originalEllipseFill = this.ellipse.Fill;
      this.ellipse.Fill = (Brush) new SolidColorBrush(color);
    }

    private void layoutRoot_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
      if (this.originalEllipseFill == null)
        return;
      this.ellipse.Fill = this.originalEllipseFill;
      this.originalEllipseFill = (Brush) null;
    }

    private void layoutRoot_MouseLeave(object sender, MouseEventArgs e)
    {
      if (this.originalEllipseFill == null)
        return;
      this.ellipse.Fill = this.originalEllipseFill;
      this.originalEllipseFill = (Brush) null;
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/ImoSilverlightApp;component/UI/Views/IconCircleStrokedButton.xaml", UriKind.Relative));
      this.rootIconButton = (UserControl) this.FindName("rootIconButton");
      this.layoutRoot = (Grid) this.FindName("layoutRoot");
      this.ellipse = (Ellipse) this.FindName("ellipse");
    }
  }
}
