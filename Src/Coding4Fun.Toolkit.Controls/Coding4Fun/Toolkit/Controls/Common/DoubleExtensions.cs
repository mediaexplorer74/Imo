// Decompiled with JetBrains decompiler
// Type: Coding4Fun.Toolkit.Controls.Common.DoubleExtensions
// Assembly: Coding4Fun.Toolkit.Controls, Version=2.1.7.0, Culture=neutral, PublicKeyToken=c5fd7b72b1a17ce4
// MVID: 213D8A75-4B66-4004-95C2-D82C49ACB764
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\Coding4Fun.Toolkit.Controls.dll

using System;


namespace Coding4Fun.Toolkit.Controls.Common
{
  [Obsolete("Moved to Coding4Fun.Toolkit.dll now, Namespace is System, ")]
  public static class DoubleExtensions
  {
    public static double CheckBound(this double value, double maximum)
    {
      return System.DoubleExtensions.CheckBound(value, maximum);
    }

    public static double CheckBound(this double value, double minimum, double maximum)
    {
      return System.DoubleExtensions.CheckBound(value, minimum, maximum);
    }

    public static bool AlmostEquals(this double a, double b, double precision = 4.94065645841247E-324)
    {
      return System.DoubleExtensions.AlmostEquals(a, b, precision);
    }
  }
}
