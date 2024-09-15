// Decompiled with JetBrains decompiler
// Type: Imo.Phone.Controls.PhysicsConstants
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using System;
using System.Windows;
using System.Windows.Media.Animation;


namespace Imo.Phone.Controls
{
  internal static class PhysicsConstants
  {
    internal static double GetStopTime(Point initialVelocity)
    {
      double num = Math.Min(Math.Sqrt(initialVelocity.X * initialVelocity.X + initialVelocity.Y * initialVelocity.Y), MotionParameters.MaximumSpeed);
      return MotionParameters.ParkingSpeed >= num ? 0.0 : Math.Log(MotionParameters.ParkingSpeed / num) / Math.Log(MotionParameters.Friction);
    }

    internal static Point GetStopPoint(Point initialVelocity)
    {
      double num1 = Math.Sqrt(initialVelocity.X * initialVelocity.X + initialVelocity.Y * initialVelocity.Y);
      Point initialVelocity1 = initialVelocity;
      if (num1 > MotionParameters.MaximumSpeed && num1 > 0.0)
      {
        initialVelocity1.X *= MotionParameters.MaximumSpeed / num1;
        initialVelocity1.Y *= MotionParameters.MaximumSpeed / num1;
      }
      double num2 = (Math.Pow(MotionParameters.Friction, PhysicsConstants.GetStopTime(initialVelocity1)) - 1.0) / Math.Log(MotionParameters.Friction);
      return new Point(initialVelocity1.X * num2, initialVelocity1.Y * num2);
    }

    internal static IEasingFunction GetEasingFunction(double stopTime)
    {
      ExponentialEase easingFunction = new ExponentialEase();
      easingFunction.Exponent = stopTime * Math.Log(MotionParameters.Friction);
      easingFunction.EasingMode = EasingMode.EaseIn;
      return (IEasingFunction) easingFunction;
    }
  }
}
