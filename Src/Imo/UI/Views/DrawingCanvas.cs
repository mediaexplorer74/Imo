// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.UI.Views.DrawingCanvas
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Helpers;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Windows.Storage;


namespace ImoSilverlightApp.UI.Views
{
  public class DrawingCanvas : UserControl
  {
    private Point prevPoint = new Point(-1.0, -1.0);
    private double strokeThickness = 6.0;
    private PathFigure pathFigure;
    private PathGeometry pathGeometry;
    private bool isDrawing;
    private SolidColorBrush redBrush = new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 215, (byte) 56, (byte) 56));
    private SolidColorBrush yellowBrush = new SolidColorBrush(Color.FromArgb(byte.MaxValue, byte.MaxValue, (byte) 222, (byte) 0));
    private SolidColorBrush greenBrush = new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 41, (byte) 223, (byte) 54));
    private SolidColorBrush blueBrush = new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 41, (byte) 80, (byte) 223));
    private SolidColorBrush blackBrush = new SolidColorBrush(Colors.Black);
    private SolidColorBrush selectedBrush;
    internal Grid LayoutRoot;
    internal Canvas canvas;
    internal Ellipse redEllipse;
    internal Ellipse yellowEllipse;
    internal Ellipse greenEllipse;
    internal Ellipse blueEllipse;
    internal Ellipse blackEllipse;
    private bool _contentLoaded;

    public DrawingCanvas()
    {
      this.InitializeComponent();
      this.redEllipse.Visibility = Visibility.Visible;
      this.selectedBrush = this.redBrush;
    }

    private void Canvas_MouseMove(object sender, MouseEventArgs e)
    {
      if (this.pathFigure == null || !this.isDrawing)
        return;
      Point position = e.GetPosition((UIElement) this.canvas);
      if (this.prevPoint.X <= -0.5 || this.Distance(position, this.prevPoint) <= 2.0)
        return;
      this.pathFigure.Segments.Add((PathSegment) new LineSegment()
      {
        Point = position
      });
      this.prevPoint = position;
    }

    private double Distance(Point p1, Point p2)
    {
      double x1 = p1.X;
      double y1 = p1.Y;
      double x2 = p2.X;
      double y2 = p2.Y;
      double num = x1;
      return Math.Sqrt(Math.Pow(x2 - num, 2.0) + Math.Pow(y2 - y1, 2.0));
    }

    private void canvas_ManipulationStarted(object sender, ManipulationStartedEventArgs e)
    {
      e.Handled = true;
    }

    private void canvas_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
    {
      e.Handled = true;
    }

    private void canvas_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
    {
      e.Handled = true;
    }

    public async void SendToConverastion()
    {
      if (this.canvas.Children.Count <= 0)
        return;
      WriteableBitmap bitmap = new WriteableBitmap((UIElement) this.canvas, (Transform) null);
      MemoryStream targetStream = new MemoryStream();
      bitmap.SaveJpeg((Stream) targetStream, bitmap.PixelWidth, bitmap.PixelHeight, 0, 100);
      targetStream.Seek(0L, SeekOrigin.Begin);
      string filePath = System.IO.Path.Combine(ApplicationData.Current.LocalCacheFolder.Path, "tmp.jpg");
      try
      {
        await FSUtils.WriteFileAsync(filePath, targetStream.ToArray());
        string str = await IMO.ConversationsManager.CurrentOpenConversation.SendPhoto(filePath);
        IMO.MonitorLog.Log("touch_drawing", "sent_drawing");
      }
      catch (Exception ex)
      {
        ImoMessageBoxResult messageBoxResult = await ImoMessageBox.Show("Failed to send photo - phone memory is full!");
      }
      filePath = (string) null;
    }

    private void ButtonSend_Click(object sender, EventArgs e) => this.SendToConverastion();

    private void canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      this.isDrawing = true;
      Point position = e.GetPosition((UIElement) this.canvas);
      System.Windows.Shapes.Path path = new System.Windows.Shapes.Path();
      path.Stroke = (Brush) this.selectedBrush;
      path.StrokeThickness = this.strokeThickness;
      path.StrokeEndLineCap = PenLineCap.Round;
      path.StrokeStartLineCap = PenLineCap.Round;
      this.pathFigure = new PathFigure();
      this.pathFigure.StartPoint = position;
      this.pathFigure.Segments.Add((PathSegment) new LineSegment()
      {
        Point = position
      });
      this.pathGeometry = new PathGeometry();
      this.pathGeometry.Figures.Add(this.pathFigure);
      path.Data = (Geometry) this.pathGeometry;
      this.canvas.Children.Add((UIElement) path);
      this.prevPoint = position;
    }

    private void ButtonClear_Click(object sender, EventArgs e) => this.canvas.Children.Clear();

    private void DisableAllColorButtons()
    {
      this.redEllipse.Visibility = this.yellowEllipse.Visibility = this.greenEllipse.Visibility = this.blueEllipse.Visibility = this.blackEllipse.Visibility = Visibility.Collapsed;
    }

    private void RedButton_Tap(object sender, GestureEventArgs e)
    {
      this.DisableAllColorButtons();
      this.redEllipse.Visibility = Visibility.Visible;
      this.selectedBrush = this.redBrush;
    }

    private void YellowButton_Tap(object sender, GestureEventArgs e)
    {
      this.DisableAllColorButtons();
      this.yellowEllipse.Visibility = Visibility.Visible;
      this.selectedBrush = this.yellowBrush;
    }

    private void GreenButton_Tap(object sender, GestureEventArgs e)
    {
      this.DisableAllColorButtons();
      this.greenEllipse.Visibility = Visibility.Visible;
      this.selectedBrush = this.greenBrush;
    }

    private void BlueButton_Tap(object sender, GestureEventArgs e)
    {
      this.DisableAllColorButtons();
      this.blueEllipse.Visibility = Visibility.Visible;
      this.selectedBrush = this.blueBrush;
    }

    private void BlackButton_Tap(object sender, GestureEventArgs e)
    {
      this.DisableAllColorButtons();
      this.blackEllipse.Visibility = Visibility.Visible;
      this.selectedBrush = this.blackBrush;
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/ImoSilverlightApp;component/UI/Views/DrawingCanvas.xaml", UriKind.Relative));
      this.LayoutRoot = (Grid) this.FindName("LayoutRoot");
      this.canvas = (Canvas) this.FindName("canvas");
      this.redEllipse = (Ellipse) this.FindName("redEllipse");
      this.yellowEllipse = (Ellipse) this.FindName("yellowEllipse");
      this.greenEllipse = (Ellipse) this.FindName("greenEllipse");
      this.blueEllipse = (Ellipse) this.FindName("blueEllipse");
      this.blackEllipse = (Ellipse) this.FindName("blackEllipse");
    }
  }
}
