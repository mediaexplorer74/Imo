// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.Helpers.IdentIconHelper
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Media;


namespace ImoSilverlightApp.Helpers
{
  internal class IdentIconHelper
  {
    private static readonly Color[] identiconColors = new Color[12]
    {
      IdentIconHelper.ColorConverter.ConvertFromString("#6CD5FA"),
      IdentIconHelper.ColorConverter.ConvertFromString("#5A98FA"),
      IdentIconHelper.ColorConverter.ConvertFromString("#4032E6"),
      IdentIconHelper.ColorConverter.ConvertFromString("#8E39EF"),
      IdentIconHelper.ColorConverter.ConvertFromString("#C73E87"),
      IdentIconHelper.ColorConverter.ConvertFromString("#DB5758"),
      IdentIconHelper.ColorConverter.ConvertFromString("#E5734C"),
      IdentIconHelper.ColorConverter.ConvertFromString("#EC9F4A"),
      IdentIconHelper.ColorConverter.ConvertFromString("#F1B64D"),
      IdentIconHelper.ColorConverter.ConvertFromString("#FDE775"),
      IdentIconHelper.ColorConverter.ConvertFromString("#EFE86F"),
      IdentIconHelper.ColorConverter.ConvertFromString("#B5D267")
    };
    private static readonly Regex NAME_PATTERN = new Regex("[-.'@\\s]");
    private static readonly Regex UPPER_PATTERN = new Regex("[\\p{Lu}]+");
    private static readonly Regex LOWER_PATTERN = new Regex("[^\\p{Lu}]+");

    public static Color GetBuddyColor(string buid)
    {
      int index1 = 0;
      if (buid != null)
      {
        int num = Math.Min(15, buid.Length);
        for (int index2 = 0; index2 < num; ++index2)
          index1 += (int) buid[index2];
        index1 = (index1 + 2) % IdentIconHelper.identiconColors.Length;
      }
      return IdentIconHelper.identiconColors[index1];
    }

    public static string GetBuddyInitials(string alias)
    {
      string input1 = "";
      if (alias != null)
      {
        foreach (string input2 in IdentIconHelper.NAME_PATTERN.Split(alias))
        {
          if (input2.Length > 0)
          {
            input1 += input2[0].ToString();
            if (!IdentIconHelper.UPPER_PATTERN.Match(input2).Success)
            {
              foreach (string str in IdentIconHelper.LOWER_PATTERN.Split(input2.Substring(1)))
              {
                if (str.Length > 0)
                  input1 += str[0].ToString();
              }
            }
          }
        }
        if (IdentIconHelper.LOWER_PATTERN.Match(input1).Success)
          input1 = input1.ToUpper();
        if (input1.Length > 5)
          input1 = input1.Substring(0, 2) + input1.Substring(input1.Length - 3, 3);
      }
      return input1;
    }

    private class ColorConverter
    {
      public static Color ConvertFromString(string hexColor)
      {
        byte maxValue = byte.MaxValue;
        if (hexColor.Length > 7)
        {
          maxValue = byte.Parse(hexColor.Substring(1, 2), NumberStyles.HexNumber);
          hexColor = hexColor.Substring(3);
        }
        byte r = byte.Parse(hexColor.Substring(1, 2), NumberStyles.HexNumber);
        byte g = byte.Parse(hexColor.Substring(3, 2), NumberStyles.HexNumber);
        byte b = byte.Parse(hexColor.Substring(5, 2), NumberStyles.HexNumber);
        return Color.FromArgb(maxValue, r, g, b);
      }
    }
  }
}
