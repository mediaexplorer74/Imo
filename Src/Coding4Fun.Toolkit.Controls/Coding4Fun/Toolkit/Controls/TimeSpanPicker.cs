// Decompiled with JetBrains decompiler
// Type: Coding4Fun.Toolkit.Controls.TimeSpanPicker
// Assembly: Coding4Fun.Toolkit.Controls, Version=2.1.7.0, Culture=neutral, PublicKeyToken=c5fd7b72b1a17ce4
// MVID: 213D8A75-4B66-4004-95C2-D82C49ACB764
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\Coding4Fun.Toolkit.Controls.dll

using Coding4Fun.Toolkit.Controls.Common;
using Coding4Fun.Toolkit.Controls.Primitives;
using System;
using System.Globalization;
using System.Windows;


namespace Coding4Fun.Toolkit.Controls
{
  public class TimeSpanPicker : ValuePickerBase<TimeSpan>
  {
    public static readonly DependencyProperty MaxProperty = DependencyProperty.Register(nameof (Maximum), typeof (TimeSpan), typeof (ValuePickerBase<TimeSpan>), new PropertyMetadata((object) TimeSpan.FromHours(24.0), new PropertyChangedCallback(TimeSpanPicker.OnLimitsChanged)));
    public static readonly DependencyProperty MinProperty = DependencyProperty.Register(nameof (Minimum), typeof (TimeSpan), typeof (ValuePickerBase<TimeSpan>), new PropertyMetadata((object) TimeSpan.Zero, new PropertyChangedCallback(TimeSpanPicker.OnLimitsChanged)));
    public static readonly DependencyProperty StepProperty = DependencyProperty.Register(nameof (StepFrequency), typeof (TimeSpan), typeof (ValuePickerBase<TimeSpan>), new PropertyMetadata((object) TimeSpan.FromSeconds(1.0)));

    public TimeSpanPicker()
    {
      this.DefaultStyleKey = (object) typeof (TimeSpanPicker);
      this.Value = new TimeSpan?(TimeSpan.FromMinutes(30.0));
      this.DialogTitle = ValuePickerResources.TimeSpanPickerTitle;
    }

    protected internal override void UpdateValueString()
    {
      if (this.Value.HasValue)
      {
        TimeSpan time = this.Value.Value;
        if (!string.IsNullOrEmpty(this.ValueStringFormat))
        {
          this.ValueString = TimeSpanFormat.Format(time, this.ValueStringFormat);
          return;
        }
      }
      this.ValueString = string.Format((IFormatProvider) CultureInfo.CurrentCulture, this.ValueStringFormat ?? this.ValueStringFormatFallback, (object) this.Value);
    }

    public TimeSpan Maximum
    {
      get => (TimeSpan) this.GetValue(TimeSpanPicker.MaxProperty);
      set => this.SetValue(TimeSpanPicker.MaxProperty, (object) value);
    }

    public TimeSpan Minimum
    {
      get => (TimeSpan) this.GetValue(TimeSpanPicker.MinProperty);
      set => this.SetValue(TimeSpanPicker.MinProperty, (object) value);
    }

    private static void OnLimitsChanged(
      DependencyObject sender,
      DependencyPropertyChangedEventArgs e)
    {
      if (!(sender is TimeSpanPicker timeSpanPicker))
        return;
      timeSpanPicker.ValidateBounds();
    }

    private void ValidateBounds()
    {
      if (this.Minimum < TimeSpan.Zero)
        this.Minimum = TimeSpan.Zero;
      if (this.Maximum > TimeSpan.MaxValue)
        this.Maximum = TimeSpan.MaxValue;
      if (this.Maximum < this.Minimum)
        this.Maximum = this.Minimum;
      if (this.Value.HasValue)
        this.Value = new TimeSpan?(System.TimeSpanExtensions.CheckBound(this.Value.Value, this.Minimum, this.Maximum));
      else
        this.Value = new TimeSpan?(this.Minimum);
    }

    public TimeSpan StepFrequency
    {
      get => (TimeSpan) this.GetValue(TimeSpanPicker.StepProperty);
      set => this.SetValue(TimeSpanPicker.StepProperty, (object) value);
    }

    protected override void NavigateToNewPage(object page)
    {
      if (page is ITimeSpanPickerPage<TimeSpan> timeSpanPickerPage)
      {
        timeSpanPickerPage.Minimum = this.Minimum;
        timeSpanPickerPage.Maximum = this.Maximum;
        timeSpanPickerPage.StepFrequency = this.StepFrequency;
      }
      base.NavigateToNewPage(page);
    }
  }
}
