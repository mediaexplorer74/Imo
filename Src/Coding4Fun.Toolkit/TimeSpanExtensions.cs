// Decompiled with JetBrains decompiler
// Type: System.TimeSpanExtensions
// Assembly: Coding4Fun.Toolkit, Version=2.0.8.0, Culture=neutral, PublicKeyToken=c5fd7b72b1a17ce4
// MVID: 0559F6D8-2506-4233-9D45-53B2EC95C690
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\Coding4Fun.Toolkit.dll


namespace System
{
  public static class TimeSpanExtensions
  {
    public static TimeSpan CheckBound(this TimeSpan value, TimeSpan maximum)
    {
      return value.CheckBound(new TimeSpan(), maximum);
    }

    public static TimeSpan CheckBound(this TimeSpan value, TimeSpan minimum, TimeSpan maximum)
    {
      if (value < minimum)
        value = minimum;
      else if (value > maximum)
        value = maximum;
      return value;
    }
  }
}
