// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.UI.Views.ImoAudioAmpsElement
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;


namespace ImoSilverlightApp.UI.Views
{
  public class ImoAudioAmpsElement : Canvas
  {
    private int[] amps;
    private int currentPosMs;

    public int[] Amps
    {
      get => this.amps;
      set
      {
        this.amps = value;
        this.InvalidateAmps();
      }
    }

    public int CurrentPosMs
    {
      get => this.currentPosMs;
      set
      {
        this.currentPosMs = value;
        this.InvalidatePosition();
      }
    }

    private void InvalidatePosition()
    {
      int num = (int) (Math.Ceiling((double) this.CurrentPosMs / 100.0) + 0.001);
      Brush brush1 = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 2, (byte) 81, (byte) 127));
      Brush brush2 = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 193, (byte) 193, (byte) 193));
      for (int index = 0; index < this.Children.Count; ++index)
      {
        bool flag = index < num;
        if (this.Children[index] is Rectangle child)
        {
          Brush brush3 = flag ? brush1 : brush2;
          child.Fill = brush3;
        }
      }
    }

    private void InvalidateAmps()
    {
      this.Children.Clear();
      if (this.Amps == null || this.Amps.Length == 0)
        return;
      int num1 = ((IEnumerable<int>) this.Amps).Max();
      int num2 = (int) (Math.Ceiling((double) this.CurrentPosMs / 100.0) + 0.001);
      double num3 = Math.Max(3.0, this.RenderSize.Width / (double) this.Amps.Length) - 1.0;
      double height = this.RenderSize.Height;
      double length = 0.0;
      Brush brush1 = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 2, (byte) 121, (byte) 194));
      Brush brush2 = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 158, (byte) 158, (byte) 158));
      for (int index = 0; index < this.Amps.Length && length < this.RenderSize.Width; ++index)
      {
        bool flag = index < num2;
        double num4 = Math.Max((double) this.Amps[index] * height / (double) num1, 1.0);
        Rectangle element = new Rectangle();
        element.Width = num3;
        element.Height = num4;
        element.Fill = flag ? brush1 : brush2;
        this.Children.Add((UIElement) element);
        Canvas.SetLeft((UIElement) element, length);
        Canvas.SetTop((UIElement) element, (height - num4) / 2.0);
        length += num3 + 1.0;
      }
    }
  }
}
