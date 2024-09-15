// Decompiled with JetBrains decompiler
// Type: Coding4Fun.Toolkit.Controls.Common.ControlHelper
// Assembly: Coding4Fun.Toolkit.Controls, Version=2.1.7.0, Culture=neutral, PublicKeyToken=c5fd7b72b1a17ce4
// MVID: 213D8A75-4B66-4004-95C2-D82C49ACB764
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\Coding4Fun.Toolkit.Controls.dll

using System;
using System.Windows;
using System.Windows.Media.Animation;


namespace Coding4Fun.Toolkit.Controls.Common
{
  public class ControlHelper
  {
    internal static int MagicSpacingNumber = 12;

    [Obsolete("Made into extension, Moved to Coding4Fun.Toolkit.dll now, Namespace is System, ")]
    public static double CheckBound(double value, double max) => value.CheckBound(max);

    [Obsolete("Made into extension, Moved to Coding4Fun.Toolkit.dll now, Namespace is System, ")]
    public static double CheckBound(double value, double min, double max)
    {
      return value.CheckBound(min, max);
    }

    public static void CreateDoubleAnimations(
      Storyboard sb,
      DependencyObject target,
      string propertyPath,
      double fromValue = 0.0,
      double toValue = 0.0,
      int speed = 500)
    {
      DoubleAnimation doubleAnimation = new DoubleAnimation();
      doubleAnimation.To = new double?(toValue);
      doubleAnimation.From = new double?(fromValue);
      doubleAnimation.Duration = new Duration(TimeSpan.FromMilliseconds((double) speed));
      DoubleAnimation element = doubleAnimation;
      Storyboard.SetTarget((Timeline) element, target);
      Storyboard.SetTargetProperty((Timeline) element, new PropertyPath(propertyPath, new object[0]));
      sb.Children.Add((Timeline) element);
    }
  }
}
