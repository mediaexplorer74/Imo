// Decompiled with JetBrains decompiler
// Type: Coding4Fun.Toolkit.Controls.Common.TimeSpanExtensions
// Assembly: Coding4Fun.Toolkit.Controls, Version=2.1.7.0, Culture=neutral, PublicKeyToken=c5fd7b72b1a17ce4
// MVID: 213D8A75-4B66-4004-95C2-D82C49ACB764
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\Coding4Fun.Toolkit.Controls.dll

using System;


namespace Coding4Fun.Toolkit.Controls.Common
{
  [Obsolete("Moved to Coding4Fun.Toolkit.dll now, Namespace is System, ")]
  public static class TimeSpanExtensions
  {
    public static TimeSpan CheckBound(this TimeSpan value, TimeSpan maximum)
    {
      return System.TimeSpanExtensions.CheckBound(value, maximum);
    }

    public static TimeSpan CheckBound(this TimeSpan value, TimeSpan minimum, TimeSpan maximum)
    {
      return System.TimeSpanExtensions.CheckBound(value, minimum, maximum);
    }
  }
}
