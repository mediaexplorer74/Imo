// Decompiled with JetBrains decompiler
// Type: Coding4Fun.Toolkit.Controls.MovementMonitor
// Assembly: Coding4Fun.Toolkit.Controls, Version=2.1.7.0, Culture=neutral, PublicKeyToken=c5fd7b72b1a17ce4
// MVID: 213D8A75-4B66-4004-95C2-D82C49ACB764
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\Coding4Fun.Toolkit.Controls.dll

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;


namespace Coding4Fun.Toolkit.Controls
{
  public class MovementMonitor
  {
    protected Rectangle Monitor;
    private double _xOffsetStartValue;
    private double _yOffsetStartValue;

    public event EventHandler<MovementMonitorEventArgs> Movement;

    public void MonitorControl(Panel panel)
    {
      Rectangle rectangle = new Rectangle();
      rectangle.Fill = (Brush) new SolidColorBrush(Color.FromArgb((byte) 0, (byte) 0, (byte) 0, (byte) 0));
      this.Monitor = rectangle;
      this.Monitor.SetValue(Grid.RowSpanProperty, (object) 2147483646);
      this.Monitor.SetValue(Grid.ColumnSpanProperty, (object) 2147483646);
      this.Monitor.ManipulationStarted += new EventHandler<ManipulationStartedEventArgs>(this.MonitorManipulationStarted);
      this.Monitor.ManipulationDelta += new EventHandler<ManipulationDeltaEventArgs>(this.MonitorManipulationDelta);
      panel.Children.Add((UIElement) this.Monitor);
    }

    private void MonitorManipulationDelta(object sender, ManipulationDeltaEventArgs e)
    {
      if (this.Movement != null)
        this.Movement((object) this, new MovementMonitorEventArgs()
        {
          X = this._xOffsetStartValue + e.CumulativeManipulation.Translation.X,
          Y = this._yOffsetStartValue + e.CumulativeManipulation.Translation.Y
        });
      e.Handled = true;
    }

    private void MonitorManipulationStarted(object sender, ManipulationStartedEventArgs e)
    {
      this._xOffsetStartValue = e.ManipulationOrigin.X;
      this._yOffsetStartValue = e.ManipulationOrigin.Y;
      if (this.Movement != null)
        this.Movement((object) this, new MovementMonitorEventArgs()
        {
          X = this._xOffsetStartValue,
          Y = this._yOffsetStartValue
        });
      e.Handled = true;
    }
  }
}
