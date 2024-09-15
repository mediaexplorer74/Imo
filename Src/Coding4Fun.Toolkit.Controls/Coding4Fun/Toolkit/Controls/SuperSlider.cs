﻿// Decompiled with JetBrains decompiler
// Type: Coding4Fun.Toolkit.Controls.SuperSlider
// Assembly: Coding4Fun.Toolkit.Controls, Version=2.1.7.0, Culture=neutral, PublicKeyToken=c5fd7b72b1a17ce4
// MVID: 213D8A75-4B66-4004-95C2-D82C49ACB764
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\Coding4Fun.Toolkit.Controls.dll

using Coding4Fun.Toolkit.Controls.Binding;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;


namespace Coding4Fun.Toolkit.Controls
{
  [TemplateVisualState(Name = "Disabled", GroupName = "CommonStates")]
  [TemplateVisualState(Name = "Normal", GroupName = "CommonStates")]
  public class SuperSlider : Control
  {
    private bool _isLayoutInit;
    protected Rectangle BackgroundRectangle;
    private const string BackgroundRectangleName = "BackgroundRectangle";
    protected Rectangle ProgressRectangle;
    private const string ProgressRectangleName = "ProgressRectangle";
    private MovementMonitor _monitor;
    private const string BodyName = "Body";
    private const string BarBodyName = "BarBody";
    public static readonly DependencyProperty BarHeightProperty = DependencyProperty.Register(nameof (BarHeight), typeof (double), typeof (SuperSlider), new PropertyMetadata((object) 24.0, new PropertyChangedCallback(SuperSlider.OnLayoutChanged)));
    public static readonly DependencyProperty BarWidthProperty = DependencyProperty.Register(nameof (BarWidth), typeof (double), typeof (SuperSlider), new PropertyMetadata((object) 24.0, new PropertyChangedCallback(SuperSlider.OnLayoutChanged)));
    public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(nameof (Title), typeof (string), typeof (SuperSlider), new PropertyMetadata((object) ""));
    public static readonly DependencyProperty ThumbProperty = DependencyProperty.Register(nameof (Thumb), typeof (object), typeof (SuperSlider), new PropertyMetadata(new PropertyChangedCallback(SuperSlider.OnLayoutChanged)));
    public static readonly DependencyProperty BackgroundSizeProperty = DependencyProperty.Register(nameof (BackgroundSize), typeof (double), typeof (SuperSlider), new PropertyMetadata((object) double.NaN, new PropertyChangedCallback(SuperSlider.OnLayoutChanged)));
    public static readonly DependencyProperty ProgressSizeProperty = DependencyProperty.Register(nameof (ProgressSize), typeof (double), typeof (SuperSlider), new PropertyMetadata((object) double.NaN, new PropertyChangedCallback(SuperSlider.OnLayoutChanged)));
    public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof (Value), typeof (double), typeof (SuperSlider), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(SuperSlider.OnValueChanged)));
    public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(nameof (Minimum), typeof (double), typeof (SuperSlider), new PropertyMetadata((object) 0.0));
    public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(nameof (Maximum), typeof (double), typeof (SuperSlider), new PropertyMetadata((object) 10.0));
    public static readonly DependencyProperty StepProperty = DependencyProperty.Register(nameof (StepFrequency), typeof (double), typeof (SuperSlider), new PropertyMetadata((object) 0.0));
    public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(nameof (Orientation), typeof (Orientation), typeof (SuperSlider), new PropertyMetadata((object) Orientation.Horizontal, new PropertyChangedCallback(SuperSlider.OnLayoutChanged)));

    public event EventHandler<PropertyChangedEventArgs<double>> ValueChanged;

    public SuperSlider()
    {
      this.DefaultStyleKey = (object) typeof (SuperSlider);
      PreventScrollBinding.SetIsEnabled((DependencyObject) this, true);
      this.IsEnabledChanged += new DependencyPropertyChangedEventHandler(this.SuperSlider_IsEnabledChanged);
      this.Loaded += new RoutedEventHandler(this.SuperSlider_Loaded);
      this.SizeChanged += new SizeChangedEventHandler(this.SuperSlider_SizeChanged);
    }

    private void SuperSlider_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      this.IsEnabledVisualStateUpdate();
    }

    private void SuperSlider_Loaded(object sender, RoutedEventArgs e)
    {
      this._isLayoutInit = true;
      this.AdjustAndUpdateLayout();
      this.IsEnabledVisualStateUpdate();
    }

    private void SuperSlider_SizeChanged(object sender, SizeChangedEventArgs e)
    {
      this.AdjustAndUpdateLayout();
    }

    public override void OnApplyTemplate()
    {
      base.OnApplyTemplate();
      this.BackgroundRectangle = this.GetTemplateChild("BackgroundRectangle") as Rectangle;
      this.ProgressRectangle = this.GetTemplateChild("ProgressRectangle") as Rectangle;
      if (this.GetTemplateChild("Body") is Grid templateChild)
      {
        this._monitor = new MovementMonitor();
        this._monitor.Movement += new EventHandler<MovementMonitorEventArgs>(this._monitor_Movement);
        this._monitor.MonitorControl((Panel) templateChild);
      }
      this.AdjustLayout();
    }

    public double BarHeight
    {
      get => (double) this.GetValue(SuperSlider.BarHeightProperty);
      set => this.SetValue(SuperSlider.BarHeightProperty, (object) value);
    }

    public double BarWidth
    {
      get => (double) this.GetValue(SuperSlider.BarWidthProperty);
      set => this.SetValue(SuperSlider.BarWidthProperty, (object) value);
    }

    public string Title
    {
      get => (string) this.GetValue(SuperSlider.TitleProperty);
      set => this.SetValue(SuperSlider.TitleProperty, (object) value);
    }

    public object Thumb
    {
      get => this.GetValue(SuperSlider.ThumbProperty);
      set => this.SetValue(SuperSlider.ThumbProperty, value);
    }

    public double BackgroundSize
    {
      get => (double) this.GetValue(SuperSlider.BackgroundSizeProperty);
      set => this.SetValue(SuperSlider.BackgroundSizeProperty, (object) value);
    }

    public double ProgressSize
    {
      get => (double) this.GetValue(SuperSlider.ProgressSizeProperty);
      set => this.SetValue(SuperSlider.ProgressSizeProperty, (object) value);
    }

    public double Value
    {
      get => (double) this.GetValue(SuperSlider.ValueProperty);
      set => this.SetValue(SuperSlider.ValueProperty, (object) value);
    }

    public double Minimum
    {
      get => (double) this.GetValue(SuperSlider.MinimumProperty);
      set => this.SetValue(SuperSlider.MinimumProperty, (object) value);
    }

    public double Maximum
    {
      get => (double) this.GetValue(SuperSlider.MaximumProperty);
      set => this.SetValue(SuperSlider.MaximumProperty, (object) value);
    }

    public double StepFrequency
    {
      get => (double) this.GetValue(SuperSlider.StepProperty);
      set => this.SetValue(SuperSlider.StepProperty, (object) value);
    }

    public Orientation Orientation
    {
      get => (Orientation) this.GetValue(SuperSlider.OrientationProperty);
      set => this.SetValue(SuperSlider.OrientationProperty, (object) value);
    }

    private void _monitor_Movement(object sender, MovementMonitorEventArgs e)
    {
      this.UpdateSampleBasedOnManipulation(e.X, e.Y);
    }

    private static void OnValueChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
    {
      if (!(o is SuperSlider superSlider) || e.NewValue == e.OldValue)
        return;
      superSlider.SyncValueAndPosition((double) e.NewValue, (double) e.OldValue);
    }

    private static void OnLayoutChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
    {
      if (!(o is SuperSlider superSlider) || e.NewValue == e.OldValue)
        return;
      superSlider.AdjustAndUpdateLayout();
    }

    private void UpdateSampleBasedOnManipulation(double x, double y)
    {
      double controlMax = this.GetControlMax();
      double num = (this.IsVertical() ? controlMax - y : x).CheckBound(controlMax);
      double minimum = this.Minimum;
      if (!controlMax.AlmostEquals(0.0))
        minimum += (this.Maximum - this.Minimum) * (num / controlMax);
      this.SyncValueAndPosition(minimum, this.Value);
    }

    private double GetControlMax() => !this.IsVertical() ? this.ActualWidth : this.ActualHeight;

    private void SyncValueAndPosition(double newValue, double oldValue)
    {
      if (!this._isLayoutInit)
        return;
      this._isLayoutInit = true;
      if (this.StepFrequency > 0.0)
      {
        double num1 = newValue % this.StepFrequency;
        double num2 = Math.Floor(newValue - num1);
        newValue = num1 < this.StepFrequency / 2.0 ? num2 : num2 + this.StepFrequency;
      }
      newValue = newValue.CheckBound(this.Minimum, this.Maximum);
      if (oldValue.AlmostEquals(newValue))
        return;
      this.Value = newValue;
      this.UpdateUserInterface();
      if (this.ValueChanged == null)
        return;
      this.ValueChanged((object) this, new PropertyChangedEventArgs<double>(oldValue, this.Value));
    }

    private void UpdateUserInterface()
    {
      double controlMax = this.GetControlMax();
      double offset = (this.Value - this.Minimum) / (this.Maximum - this.Minimum) * controlMax;
      bool isVert = this.IsVertical();
      SuperSlider.SetSizeBasedOnOrientation((FrameworkElement) this.ProgressRectangle, isVert, offset);
      if (!(this.Thumb is FrameworkElement thumb))
        return;
      double num1 = isVert ? thumb.ActualHeight : thumb.ActualWidth;
      double num2 = (offset - num1 / 2.0).CheckBound(controlMax - num1);
      thumb.Margin = isVert ? new Thickness(0.0, 0.0, 0.0, num2) : new Thickness(num2, 0.0, 0.0, 0.0);
    }

    private void AdjustAndUpdateLayout()
    {
      try
      {
        this.AdjustLayout();
        this.UpdateUserInterface();
      }
      catch
      {
      }
    }

    private void AdjustLayout()
    {
      if (this.ProgressRectangle == null || this.BackgroundRectangle == null)
        return;
      bool isVert = this.IsVertical();
      if (this.GetTemplateChild("BarBody") is Grid templateChild)
      {
        if (isVert)
        {
          templateChild.Width = this.BarWidth;
          templateChild.Height = double.NaN;
        }
        else
        {
          templateChild.Width = double.NaN;
          templateChild.Height = this.BarHeight;
        }
      }
      SuperSlider.SetAlignment((FrameworkElement) this.ProgressRectangle, isVert);
      this.ProgressRectangle.Width = double.NaN;
      this.ProgressRectangle.Height = double.NaN;
      this.BackgroundRectangle.Width = double.NaN;
      this.BackgroundRectangle.Height = double.NaN;
      if (this.ProgressSize > 0.0)
        SuperSlider.SetSizeBasedOnOrientation((FrameworkElement) this.ProgressRectangle, !isVert, this.ProgressSize);
      if (this.BackgroundSize > 0.0)
        SuperSlider.SetSizeBasedOnOrientation((FrameworkElement) this.BackgroundRectangle, !isVert, this.BackgroundSize);
      if (this.Thumb == null)
        return;
      SuperSlider.SetAlignment(this.Thumb as FrameworkElement, isVert);
    }

    private void IsEnabledVisualStateUpdate()
    {
      VisualStateManager.GoToState((Control) this, this.IsEnabled ? "Normal" : "Disabled", true);
    }

    private static void SetSizeBasedOnOrientation(
      FrameworkElement control,
      bool isVert,
      double offset)
    {
      if (control == null)
        return;
      if (isVert)
        control.Height = offset;
      else
        control.Width = offset;
    }

    private bool IsVertical() => this.Orientation == Orientation.Vertical;

    private static void SetAlignment(FrameworkElement control, bool isVert)
    {
      if (control == null)
        return;
      control.HorizontalAlignment = isVert ? HorizontalAlignment.Stretch : HorizontalAlignment.Left;
      control.VerticalAlignment = isVert ? VerticalAlignment.Bottom : VerticalAlignment.Stretch;
    }
  }
}
