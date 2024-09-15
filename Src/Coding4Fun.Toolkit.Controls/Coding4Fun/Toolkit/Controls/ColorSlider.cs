// Decompiled with JetBrains decompiler
// Type: Coding4Fun.Toolkit.Controls.ColorSlider
// Assembly: Coding4Fun.Toolkit.Controls, Version=2.1.7.0, Culture=neutral, PublicKeyToken=c5fd7b72b1a17ce4
// MVID: 213D8A75-4B66-4004-95C2-D82C49ACB764
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\Coding4Fun.Toolkit.Controls.dll

using Coding4Fun.Toolkit.Controls.Common;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;


namespace Coding4Fun.Toolkit.Controls
{
  public class ColorSlider : ColorBaseControl
  {
    private const double HueSelectorSize = 24.0;
    private bool _fromSliderChange;
    protected Grid Body;
    private const string BodyName = "Body";
    protected Rectangle SelectedColor;
    private const string SelectedColorName = "SelectedColor";
    protected SuperSlider Slider;
    private const string SliderName = "Slider";
    public static readonly DependencyProperty ThumbProperty = DependencyProperty.Register(nameof (Thumb), typeof (object), typeof (ColorSlider), new PropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty IsColorVisibleProperty = DependencyProperty.Register(nameof (IsColorVisible), typeof (bool), typeof (ColorSlider), new PropertyMetadata((object) true, new PropertyChangedCallback(ColorSlider.OnIsColorVisibleChanged)));
    public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(nameof (Orientation), typeof (Orientation), typeof (ColorSlider), new PropertyMetadata((object) Orientation.Vertical, new PropertyChangedCallback(ColorSlider.OnOrientationPropertyChanged)));

    public ColorSlider()
    {
      this.DefaultStyleKey = (object) typeof (ColorSlider);
      this.IsEnabledChanged += new DependencyPropertyChangedEventHandler(this.SuperSlider_IsEnabledChanged);
    }

    public override void OnApplyTemplate()
    {
      base.OnApplyTemplate();
      this.Body = this.GetTemplateChild("Body") as Grid;
      this.Slider = this.GetTemplateChild("Slider") as SuperSlider;
      if (this.Thumb == null)
        this.Thumb = (object) new ColorSliderThumb();
      this.SelectedColor = this.GetTemplateChild("SelectedColor") as Rectangle;
      this.SizeChanged += new SizeChangedEventHandler(this.UserControl_SizeChanged);
      if (this.Slider != null)
      {
        this.Slider.ValueChanged += new EventHandler<PropertyChangedEventArgs<double>>(this.Slider_ValueChanged);
        Color color = this.Color;
        if (color.A == (byte) 0)
        {
          color = this.Color;
          if (color.R == (byte) 0)
          {
            color = this.Color;
            if (color.G == (byte) 0)
            {
              color = this.Color;
              if (color.B == (byte) 0)
              {
                this.Color = Color.FromArgb(byte.MaxValue, (byte) 6, byte.MaxValue, (byte) 0);
                goto label_9;
              }
            }
          }
        }
        this.UpdateLayoutBasedOnColor();
      }
label_9:
      this.IsEnabledVisualStateUpdate();
    }

    private void Slider_ValueChanged(object sender, PropertyChangedEventArgs<double> e)
    {
      this.SetColorFromSlider(e.NewValue);
    }

    private void SetColorFromSlider(double value)
    {
      this._fromSliderChange = true;
      this.ColorChanging(ColorSpace.GetColorFromHueValue((float) ((int) value % 360)));
      this._fromSliderChange = false;
    }

    private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
    {
      this.AdjustLayoutBasedOnOrientation();
    }

    private void SuperSlider_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      this.IsEnabledVisualStateUpdate();
    }

    public object Thumb
    {
      get => this.GetValue(ColorSlider.ThumbProperty);
      set => this.SetValue(ColorSlider.ThumbProperty, value);
    }

    public bool IsColorVisible
    {
      get => (bool) this.GetValue(ColorSlider.IsColorVisibleProperty);
      set => this.SetValue(ColorSlider.IsColorVisibleProperty, (object) value);
    }

    public Orientation Orientation
    {
      get => (Orientation) this.GetValue(ColorSlider.OrientationProperty);
      set => this.SetValue(ColorSlider.OrientationProperty, (object) value);
    }

    private static void OnIsColorVisibleChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      if (!(d is ColorSlider colorSlider))
        return;
      colorSlider.AdjustLayoutBasedOnOrientation();
    }

    private static void OnOrientationPropertyChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      if (!(d is ColorSlider colorSlider))
        return;
      colorSlider.AdjustLayoutBasedOnOrientation();
    }

    private void AdjustLayoutBasedOnOrientation()
    {
      if (this.Body == null || this.Slider == null || this.SelectedColor == null)
        return;
      bool flag = this.Orientation == Orientation.Vertical;
      this.IsEnabledVisualStateUpdate();
      this.Body.RowDefinitions.Clear();
      this.Body.ColumnDefinitions.Clear();
      if (flag)
      {
        this.Body.RowDefinitions.Add(new RowDefinition());
        this.Body.RowDefinitions.Add(new RowDefinition());
      }
      else
      {
        this.Body.ColumnDefinitions.Add(new ColumnDefinition());
        this.Body.ColumnDefinitions.Add(new ColumnDefinition());
      }
      FrameworkElement thumb = (FrameworkElement) this.Slider.Thumb;
      if (thumb != null)
      {
        thumb.Height = flag ? 24.0 : double.NaN;
        thumb.Width = flag ? double.NaN : 24.0;
      }
      this.SelectedColor.SetValue(Grid.RowProperty, (object) (flag ? 1 : 0));
      this.SelectedColor.SetValue(Grid.ColumnProperty, (object) (flag ? 0 : 1));
      double actualWidth = this.Slider.ActualWidth;
      double actualHeight = this.Slider.ActualHeight;
      this.SelectedColor.Height = this.SelectedColor.Width = flag ? actualWidth : actualHeight;
      if (flag)
      {
        this.Body.RowDefinitions[0].Height = new GridLength(1.0, GridUnitType.Star);
        this.Body.RowDefinitions[1].Height = new GridLength(1.0, GridUnitType.Auto);
      }
      else
      {
        this.Body.ColumnDefinitions[0].Width = new GridLength(1.0, GridUnitType.Star);
        this.Body.ColumnDefinitions[1].Width = new GridLength(1.0, GridUnitType.Auto);
      }
      this.SelectedColor.Visibility = this.IsColorVisible ? Visibility.Visible : Visibility.Collapsed;
    }

    protected internal override void UpdateLayoutBasedOnColor()
    {
      if (this._fromSliderChange)
        return;
      base.UpdateLayoutBasedOnColor();
      if (this.Slider == null)
        return;
      this.Slider.Value = (double) this.Color.GetHue();
    }

    private void IsEnabledVisualStateUpdate()
    {
      VisualStateManager.GoToState((Control) this, this.IsEnabled ? "Normal" : "Disabled", true);
      this.Slider.Background = this.IsEnabled ? (Brush) ColorSpace.GetColorGradientBrush(this.Orientation) : (Brush) ColorSpace.GetBlackAndWhiteGradientBrush(this.Orientation);
    }
  }
}
