﻿// Decompiled with JetBrains decompiler
// Type: Coding4Fun.Toolkit.Controls.HourTimeSpanDataSource
// Assembly: Coding4Fun.Toolkit.Controls, Version=2.1.7.0, Culture=neutral, PublicKeyToken=c5fd7b72b1a17ce4
// MVID: 213D8A75-4B66-4004-95C2-D82C49ACB764
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\Coding4Fun.Toolkit.Controls.dll

using System;


namespace Coding4Fun.Toolkit.Controls
{
  public class HourTimeSpanDataSource : TimeSpanDataSource
  {
    public HourTimeSpanDataSource()
      : base(23, 1)
    {
    }

    public HourTimeSpanDataSource(int max, int step)
      : base(max, step)
    {
    }

    protected override TimeSpan? GetRelativeTo(TimeSpan relativeDate, int delta)
    {
      return new TimeSpan?(new TimeSpan(this.ComputeRelativeTo(relativeDate.Hours, delta), relativeDate.Minutes, relativeDate.Seconds));
    }
  }
}