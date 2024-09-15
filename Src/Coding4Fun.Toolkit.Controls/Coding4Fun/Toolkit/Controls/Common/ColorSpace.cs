// Decompiled with JetBrains decompiler
// Type: Coding4Fun.Toolkit.Controls.Common.ColorSpace
// Assembly: Coding4Fun.Toolkit.Controls, Version=2.1.7.0, Culture=neutral, PublicKeyToken=c5fd7b72b1a17ce4
// MVID: 213D8A75-4B66-4004-95C2-D82C49ACB764
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\Coding4Fun.Toolkit.Controls.dll

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace Coding4Fun.Toolkit.Controls.Common
{
  public class ColorSpace
  {
    private const byte MinValue = 0;
    private const byte MaxValue = 255;
    private const byte DefaultAlphaValue = 255;
    private static readonly Color[] ColorGradients = new Color[6]
    {
      Color.FromArgb(byte.MaxValue, byte.MaxValue, (byte) 0, (byte) 0),
      Color.FromArgb(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte) 0),
      Color.FromArgb(byte.MaxValue, (byte) 0, byte.MaxValue, (byte) 0),
      Color.FromArgb(byte.MaxValue, (byte) 0, byte.MaxValue, byte.MaxValue),
      Color.FromArgb(byte.MaxValue, (byte) 0, (byte) 0, byte.MaxValue),
      Color.FromArgb(byte.MaxValue, byte.MaxValue, (byte) 0, byte.MaxValue)
    };
    private static readonly Color[] BlackAndWhiteGradients = new Color[6]
    {
      Color.FromArgb(byte.MaxValue, (byte) 76, (byte) 76, (byte) 76),
      Color.FromArgb(byte.MaxValue, (byte) 225, (byte) 225, (byte) 225),
      Color.FromArgb(byte.MaxValue, (byte) 149, (byte) 149, (byte) 149),
      Color.FromArgb(byte.MaxValue, (byte) 178, (byte) 178, (byte) 178),
      Color.FromArgb(byte.MaxValue, (byte) 29, (byte) 29, (byte) 29),
      Color.FromArgb(byte.MaxValue, (byte) 105, (byte) 105, (byte) 105)
    };

    public static LinearGradientBrush GetColorGradientBrush(Orientation orientation)
    {
      return ColorSpace.CreateGradientBrush(orientation, ColorSpace.ColorGradients);
    }

    public static LinearGradientBrush GetBlackAndWhiteGradientBrush(Orientation orientation)
    {
      return ColorSpace.CreateGradientBrush(orientation, ColorSpace.BlackAndWhiteGradients);
    }

    private static LinearGradientBrush CreateGradientBrush(
      Orientation orientation,
      params Color[] colors)
    {
      LinearGradientBrush gradientBrush = new LinearGradientBrush();
      float num = 1f / (float) colors.Length;
      for (int index = 0; index < colors.Length; ++index)
        gradientBrush.GradientStops.Add(new GradientStop()
        {
          Offset = (double) num * (double) index,
          Color = colors[index]
        });
      gradientBrush.GradientStops.Add(new GradientStop()
      {
        Offset = (double) num * (double) colors.Length,
        Color = colors[0]
      });
      if (orientation == Orientation.Vertical)
      {
        gradientBrush.StartPoint = new Point(0.0, 1.0);
        gradientBrush.EndPoint = new Point();
      }
      else
        gradientBrush.EndPoint = new Point(1.0, 0.0);
      return gradientBrush;
    }

    public static Color GetColorFromHueValue(float position)
    {
      position /= 360f;
      position *= (float) (ColorSpace.ColorGradients.Length * (int) byte.MaxValue);
      byte num1 = (byte) ((double) position % (double) byte.MaxValue);
      byte num2 = (byte) ((uint) byte.MaxValue - (uint) num1);
      switch ((int) position / (int) byte.MaxValue)
      {
        case 0:
          return Color.FromArgb(byte.MaxValue, byte.MaxValue, num1, (byte) 0);
        case 1:
          return Color.FromArgb(byte.MaxValue, num2, byte.MaxValue, (byte) 0);
        case 2:
          return Color.FromArgb(byte.MaxValue, (byte) 0, byte.MaxValue, num1);
        case 3:
          return Color.FromArgb(byte.MaxValue, (byte) 0, num2, byte.MaxValue);
        case 4:
          return Color.FromArgb(byte.MaxValue, num1, (byte) 0, byte.MaxValue);
        case 5:
          return Color.FromArgb(byte.MaxValue, byte.MaxValue, (byte) 0, num2);
        default:
          return Colors.Black;
      }
    }

    public static string GetHexCode(Color c)
    {
      byte num = c.R;
      string str1 = num.ToString("X2");
      num = c.G;
      string str2 = num.ToString("X2");
      num = c.B;
      string str3 = num.ToString("X2");
      return string.Format("#{0}{1}{2}", (object) str1, (object) str2, (object) str3);
    }

    public static Color ConvertHsvToRgb(float hue, float saturation, float value)
    {
      hue /= 360f;
      if ((double) saturation > 0.0)
      {
        if ((double) hue >= 1.0)
          hue = 0.0f;
        hue = 6f * hue;
        int num1 = (int) Math.Floor((double) hue);
        byte num2 = (byte) Math.Round((double) byte.MaxValue * (double) value * (1.0 - (double) saturation));
        byte num3 = (byte) Math.Round((double) byte.MaxValue * (double) value * (1.0 - (double) saturation * ((double) hue - (double) num1)));
        byte num4 = (byte) Math.Round((double) byte.MaxValue * (double) value * (1.0 - (double) saturation * (1.0 - ((double) hue - (double) num1))));
        byte num5 = (byte) Math.Round((double) byte.MaxValue * (double) value);
        switch (num1)
        {
          case 0:
            return Color.FromArgb(byte.MaxValue, num5, num4, num2);
          case 1:
            return Color.FromArgb(byte.MaxValue, num3, num5, num2);
          case 2:
            return Color.FromArgb(byte.MaxValue, num2, num5, num4);
          case 3:
            return Color.FromArgb(byte.MaxValue, num2, num3, num5);
          case 4:
            return Color.FromArgb(byte.MaxValue, num4, num2, num5);
          case 5:
            return Color.FromArgb(byte.MaxValue, num5, num2, num3);
          default:
            return Color.FromArgb((byte) 0, (byte) 0, (byte) 0, (byte) 0);
        }
      }
      else
      {
        byte num = (byte) ((double) value * (double) byte.MaxValue);
        return Color.FromArgb(byte.MaxValue, num, num, num);
      }
    }
  }
}
