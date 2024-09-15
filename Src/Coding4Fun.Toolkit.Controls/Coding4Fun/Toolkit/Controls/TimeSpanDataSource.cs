// Decompiled with JetBrains decompiler
// Type: Coding4Fun.Toolkit.Controls.TimeSpanDataSource
// Assembly: Coding4Fun.Toolkit.Controls, Version=2.1.7.0, Culture=neutral, PublicKeyToken=c5fd7b72b1a17ce4
// MVID: 213D8A75-4B66-4004-95C2-D82C49ACB764
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\Coding4Fun.Toolkit.Controls.dll

using System;


namespace Coding4Fun.Toolkit.Controls
{
  public abstract class TimeSpanDataSource : DataSource<TimeSpan>
  {
    protected int Max;
    protected int Step;

    protected TimeSpanDataSource(int max, int step)
    {
      this.Max = max;
      this.Step = step;
    }

    public override bool IsEmpty => this.Max - 1 == 0 || this.Step == 0;

    protected int ComputeRelativeTo(int value, int delta)
    {
      int max = this.Max;
      return max <= 0 ? value : (max + value + delta * this.Step) % max + (max <= value ? max : 0);
    }
  }
}
