// Decompiled with JetBrains decompiler
// Type: Coding4Fun.Toolkit.Controls.Primitives.TimeSpanWrapper
// Assembly: Coding4Fun.Toolkit.Controls, Version=2.1.7.0, Culture=neutral, PublicKeyToken=c5fd7b72b1a17ce4
// MVID: 213D8A75-4B66-4004-95C2-D82C49ACB764
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\Coding4Fun.Toolkit.Controls.dll

using System;
using System.Globalization;


namespace Coding4Fun.Toolkit.Controls.Primitives
{
  public class TimeSpanWrapper : ValueWrapper<TimeSpan>
  {
    public override ValueWrapper<TimeSpan> CreateNew(TimeSpan? value)
    {
      return (ValueWrapper<TimeSpan>) new TimeSpanWrapper(value.GetValueOrDefault(TimeSpan.FromMinutes(30.0)));
    }

    public TimeSpan TimeSpan => this.Value;

    public string HourNumber
    {
      get => this.TimeSpan.Hours.ToString((IFormatProvider) CultureInfo.InvariantCulture);
    }

    public string MinuteNumber
    {
      get => this.TimeSpan.Minutes.ToString((IFormatProvider) CultureInfo.InvariantCulture);
    }

    public string SecondNumber
    {
      get => this.TimeSpan.Seconds.ToString((IFormatProvider) CultureInfo.InvariantCulture);
    }

    public TimeSpanWrapper(TimeSpan timeSpan)
      : base(timeSpan)
    {
    }
  }
}
