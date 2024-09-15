// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.UI.Views.Arc
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;


namespace ImoSilverlightApp.UI.Views
{
  public class Arc : UserControl
  {
    public static readonly DependencyProperty CenterProperty = DependencyProperty.Register(nameof (Center), typeof (Point), typeof (Arc), new PropertyMetadata((object) new Point(0.0, 0.0)));
    public static readonly DependencyProperty StartAngleProperty = DependencyProperty.Register(nameof (StartAngle), typeof (double), typeof (Arc), new PropertyMetadata((object) 0.0));
    public static readonly DependencyProperty EndAngleProperty = DependencyProperty.Register(nameof (EndAngle), typeof (double), typeof (Arc), new PropertyMetadata((object) (Math.PI / 2.0), new PropertyChangedCallback(Arc.EndAngleProperty_Changed)));
    public static readonly DependencyProperty RadiusProperty = DependencyProperty.Register(nameof (Radius), typeof (double), typeof (Arc), new PropertyMetadata((object) 10.0));
    public static readonly DependencyProperty SmallAngleProperty = DependencyProperty.Register(nameof (SmallAngle), typeof (bool), typeof (Arc), new PropertyMetadata((object) false));
    internal Grid LayoutRoot;
    internal Path path;
    private bool _contentLoaded;

    public Arc()
    {
      this.InitializeComponent();
      this.Loaded += new RoutedEventHandler(this.Arc_Loaded);
    }

    public Point Center
    {
      get => (Point) this.GetValue(Arc.CenterProperty);
      set => this.SetValue(Arc.CenterProperty, (object) value);
    }

    public double StartAngle
    {
      get => (double) this.GetValue(Arc.StartAngleProperty);
      set => this.SetValue(Arc.StartAngleProperty, (object) value);
    }

    public double EndAngle
    {
      get => (double) this.GetValue(Arc.EndAngleProperty);
      set => this.SetValue(Arc.EndAngleProperty, (object) value);
    }

    private static void EndAngleProperty_Changed(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      ((Arc) d).UpdateUI();
    }

    public double Radius
    {
      get => (double) this.GetValue(Arc.RadiusProperty);
      set => this.SetValue(Arc.RadiusProperty, (object) value);
    }

    public bool SmallAngle
    {
      get => (bool) this.GetValue(Arc.SmallAngleProperty);
      set => this.SetValue(Arc.SmallAngleProperty, (object) value);
    }

    private void Arc_Loaded(object sender, RoutedEventArgs e) => this.UpdateUI();

    private void UpdateUI()
    {
      double num1 = this.StartAngle < 0.0 ? this.StartAngle + 2.0 * Math.PI : this.StartAngle;
      double num2 = this.EndAngle < 0.0 ? this.EndAngle + 2.0 * Math.PI : this.EndAngle;
      if (num2 < num1)
        num2 += 2.0 * Math.PI;
      SweepDirection sweepDirection = SweepDirection.Clockwise;
      bool flag;
      if (this.SmallAngle)
      {
        flag = false;
        sweepDirection = num2 - num1 <= Math.PI ? SweepDirection.Clockwise : SweepDirection.Counterclockwise;
      }
      else
        flag = Math.Abs(num2 - num1) > Math.PI;
      double num3 = this.Radius - this.path.StrokeThickness / 2.0;
      Point point1;
      ref Point local1 = ref point1;
      double x1 = this.Center.X + Math.Cos(num1) * num3;
      Point center = this.Center;
      double y1 = center.Y + Math.Sin(num1) * num3;
      local1 = new Point(x1, y1);
      Point point2;
      ref Point local2 = ref point2;
      center = this.Center;
      double x2 = center.X + Math.Cos(num2) * num3;
      center = this.Center;
      double y2 = center.Y + Math.Sin(num2) * num3;
      local2 = new Point(x2, y2);
      PathSegmentCollection segmentCollection = new PathSegmentCollection();
      segmentCollection.Add((PathSegment) new ArcSegment()
      {
        Point = point2,
        Size = new Size(num3, num3),
        RotationAngle = 0.0,
        IsLargeArc = flag,
        SweepDirection = sweepDirection
      });
      PathFigureCollection figureCollection = new PathFigureCollection();
      PathFigure pathFigure = new PathFigure()
      {
        StartPoint = point1,
        Segments = segmentCollection,
        IsClosed = true
      };
      pathFigure.IsClosed = false;
      figureCollection.Add(pathFigure);
      Path path = this.path;
      PathGeometry pathGeometry = new PathGeometry();
      pathGeometry.Figures = figureCollection;
      pathGeometry.FillRule = FillRule.EvenOdd;
      pathGeometry.Transform = (Transform) null;
      path.Data = (Geometry) pathGeometry;
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/ImoSilverlightApp;component/UI/Views/Arc.xaml", UriKind.Relative));
      this.LayoutRoot = (Grid) this.FindName("LayoutRoot");
      this.path = (Path) this.FindName("path");
    }
  }
}
