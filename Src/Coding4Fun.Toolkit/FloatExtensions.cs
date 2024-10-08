﻿// Decompiled with JetBrains decompiler
// Type: System.FloatExtensions
// Assembly: Coding4Fun.Toolkit, Version=2.0.8.0, Culture=neutral, PublicKeyToken=c5fd7b72b1a17ce4
// MVID: 0559F6D8-2506-4233-9D45-53B2EC95C690
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\Coding4Fun.Toolkit.dll


namespace System
{
  public static class FloatExtensions
  {
    public static double CheckBound(this float value, float maximum)
    {
      return value.CheckBound(0.0f, maximum);
    }

    public static double CheckBound(this float value, float minimum, float maximum)
    {
      if ((double) value <= (double) minimum)
        value = minimum;
      else if ((double) value >= (double) maximum)
        value = maximum;
      return (double) value;
    }

    public static bool AlmostEquals(this float a, float b, double precision = 1.4012984643248171E-45)
    {
      return (double) Math.Abs(a - b) <= precision;
    }
  }
}
