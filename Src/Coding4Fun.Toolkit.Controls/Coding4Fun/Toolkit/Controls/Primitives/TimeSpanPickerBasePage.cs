// Decompiled with JetBrains decompiler
// Type: Coding4Fun.Toolkit.Controls.Primitives.TimeSpanPickerBasePage
// Assembly: Coding4Fun.Toolkit.Controls, Version=2.1.7.0, Culture=neutral, PublicKeyToken=c5fd7b72b1a17ce4
// MVID: 213D8A75-4B66-4004-95C2-D82C49ACB764
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\Coding4Fun.Toolkit.Controls.dll

using System;


namespace Coding4Fun.Toolkit.Controls.Primitives
{
  public abstract class TimeSpanPickerBasePage : 
    ValuePickerBasePage<TimeSpan>,
    ITimeSpanPickerPage<TimeSpan>,
    IValuePickerPage<TimeSpan>
  {
    protected override ValueWrapper<TimeSpan> GetNewWrapper(TimeSpan? value)
    {
      return (ValueWrapper<TimeSpan>) new TimeSpanWrapper(value.GetValueOrDefault(TimeSpan.FromMinutes(30.0)));
    }

    public TimeSpan Maximum { get; set; }

    public TimeSpan Minimum { get; set; }

    public TimeSpan StepFrequency { get; set; }

    public override TimeSpan? Value
    {
      set
      {
        if (!value.HasValue)
          return;
        base.Value = new TimeSpan?(value.Value.CheckBound(this.Minimum, this.Maximum));
      }
    }
  }
}
