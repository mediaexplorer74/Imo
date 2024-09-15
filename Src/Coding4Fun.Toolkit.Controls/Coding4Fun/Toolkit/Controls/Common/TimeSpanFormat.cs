// Decompiled with JetBrains decompiler
// Type: Coding4Fun.Toolkit.Controls.Common.TimeSpanFormat
// Assembly: Coding4Fun.Toolkit.Controls, Version=2.1.7.0, Culture=neutral, PublicKeyToken=c5fd7b72b1a17ce4
// MVID: 213D8A75-4B66-4004-95C2-D82C49ACB764
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\Coding4Fun.Toolkit.Controls.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;


namespace Coding4Fun.Toolkit.Controls.Common
{
  public class TimeSpanFormat
  {
    public static string Format(TimeSpan time, string format)
    {
      StringBuilder stringBuilder1 = new StringBuilder(format);
      List<Match> list1 = Regex.Matches(format, "{0:[^}]*}").Cast<Match>().ToList<Match>();
      list1.Reverse();
      foreach (Match match1 in list1)
      {
        StringBuilder stringBuilder2 = new StringBuilder(match1.Value);
        List<Match> list2 = Regex.Matches(match1.Value, "[d]{1,2}|[h]{1,2}|[m]{1,2}|[s]{1,2}|[f]{1,7}|[F]{1,7}").Cast<Match>().ToList<Match>();
        list2.Reverse();
        foreach (Match match2 in list2)
        {
          switch (match2.Value[0])
          {
            case 'F':
              stringBuilder2.Replace(match2.Value, ((double) time.Milliseconds / 1000.0).ToString(match2.Value.Replace("F", "0")), match2.Index, match2.Length);
              continue;
            case 'd':
              stringBuilder2.Replace(match2.Value, time.Days.ToString("00"), match2.Index, match2.Length);
              continue;
            case 'f':
              stringBuilder2.Replace(match2.Value, (time.Milliseconds / 1000).ToString(match2.Value.Replace("f", "0")), match2.Index, match2.Length);
              continue;
            case 'h':
              stringBuilder2.Replace(match2.Value, time.Hours.ToString("00"), match2.Index, match2.Length);
              continue;
            case 'm':
              stringBuilder2.Replace(match2.Value, time.Minutes.ToString("00"), match2.Index, match2.Length);
              continue;
            case 's':
              stringBuilder2.Replace(match2.Value, time.Seconds.ToString("00"), match2.Index, match2.Length);
              continue;
            default:
              continue;
          }
        }
        stringBuilder2.Remove(0, 3);
        stringBuilder2.Remove(stringBuilder2.Length - 1, 1);
        stringBuilder1.Replace(match1.Value, stringBuilder2.ToString(), match1.Index, match1.Length);
      }
      return stringBuilder1.ToString();
    }
  }
}
