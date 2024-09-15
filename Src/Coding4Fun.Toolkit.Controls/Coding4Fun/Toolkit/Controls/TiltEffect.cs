// Decompiled with JetBrains decompiler
// Type: Coding4Fun.Toolkit.Controls.TiltEffect
// Assembly: Coding4Fun.Toolkit.Controls, Version=2.1.7.0, Culture=neutral, PublicKeyToken=c5fd7b72b1a17ce4
// MVID: 213D8A75-4B66-4004-95C2-D82C49ACB764
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\Coding4Fun.Toolkit.Controls.dll

using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;


namespace Coding4Fun.Toolkit.Controls
{
  public class TiltEffect : DependencyObject
  {
    private static readonly Dictionary<DependencyObject, CacheMode> OriginalCacheMode = new Dictionary<DependencyObject, CacheMode>();
    private const double MaxAngle = 0.3;
    private const double MaxDepression = 25.0;
    private static bool _hasStarted;
    private static readonly TimeSpan TiltReturnAnimationDelay = TimeSpan.FromMilliseconds(200.0);
    private static readonly TimeSpan TiltReturnAnimationDuration = TimeSpan.FromMilliseconds(100.0);
    private static FrameworkElement _currentTiltElement;
    private static Storyboard _tiltReturnStoryboard;
    private static DoubleAnimation _tiltReturnXAnimation;
    private static DoubleAnimation _tiltReturnYAnimation;
    private static DoubleAnimation _tiltReturnZAnimation;
    private static Point _currentTiltElementCenter;
    private static bool _wasPauseAnimation;
    public static readonly DependencyProperty IsTiltEnabledProperty = DependencyProperty.RegisterAttached("IsTiltEnabled", typeof (bool), typeof (TiltEffect), new PropertyMetadata((object) false, new PropertyChangedCallback(TiltEffect.OnIsTiltEnabledChanged)));
    public static readonly DependencyProperty SuppressTiltProperty = DependencyProperty.RegisterAttached("SuppressTilt", typeof (bool), typeof (TiltEffect), (PropertyMetadata) null);

    public static bool UseLogarithmicEase { get; set; }

    public static List<Type> TiltableItems { get; private set; }

    private TiltEffect()
    {
    }

    static TiltEffect()
    {
      TiltEffect.TiltableItems = new List<Type>()
      {
        typeof (ButtonBase),
        typeof (ListBoxItem)
      };
    }

    public static bool GetIsTiltEnabled(DependencyObject source)
    {
      return (bool) source.GetValue(TiltEffect.IsTiltEnabledProperty);
    }

    public static void SetIsTiltEnabled(DependencyObject source, bool value)
    {
      source.SetValue(TiltEffect.IsTiltEnabledProperty, (object) value);
    }

    public static bool GetSuppressTilt(DependencyObject source)
    {
      return (bool) source.GetValue(TiltEffect.SuppressTiltProperty);
    }

    public static void SetSuppressTilt(DependencyObject source, bool value)
    {
      source.SetValue(TiltEffect.SuppressTiltProperty, (object) value);
    }

    private static void OnIsTiltEnabledChanged(
      DependencyObject target,
      DependencyPropertyChangedEventArgs args)
    {
      if (!(target is FrameworkElement frameworkElement))
        return;
      if (frameworkElement.GetVisualParent() == null)
        frameworkElement.Loaded += new RoutedEventHandler(TiltEffect.element_Loaded);
      else
        TiltEffect.StateManagementForIsTiltEnabled((bool) args.NewValue, frameworkElement);
    }

    private static void element_Loaded(object sender, RoutedEventArgs e)
    {
      FrameworkElement element = sender as FrameworkElement;
      TiltEffect.StateManagementForIsTiltEnabled(TiltEffect.GetIsTiltEnabled(sender as DependencyObject), element);
      if (element == null)
        return;
      element.Loaded -= new RoutedEventHandler(TiltEffect.element_Loaded);
    }

    private static void StateManagementForIsTiltEnabled(bool isEnabled, FrameworkElement element)
    {
      if (isEnabled)
        TiltEffect.AddToValidControls((UIElement) element);
      else
        TiltEffect.RemoveFromValidControls(element);
    }

    private static void AddToValidControls(UIElement element)
    {
      bool flag = false;
      if ((!(element.ReadLocalValue(TiltEffect.SuppressTiltProperty) is bool) ? 0 : ((bool) element.ReadLocalValue(TiltEffect.SuppressTiltProperty) ? 1 : 0)) == 0 && TiltEffect.TiltableItems.Any<Type>((Func<Type, bool>) (t => TypeExtensions.IsTypeOf(element, t))))
      {
        flag = true;
        element.ManipulationStarted += new EventHandler<ManipulationStartedEventArgs>(TiltEffect.InitiateTiltEffect);
      }
      if (flag)
        return;
      foreach (DependencyObject visualChild in element.GetVisualChildren())
        TiltEffect.AddToValidControls((UIElement) (visualChild as FrameworkElement));
    }

    private static void RemoveFromValidControls(FrameworkElement element)
    {
      foreach (UIElement uiElement in element.GetVisualChildren().OfType<FrameworkElement>())
        uiElement.ManipulationStarted -= new EventHandler<ManipulationStartedEventArgs>(TiltEffect.InitiateTiltEffect);
    }

    private static void InitiateTiltEffect(object sender, ManipulationStartedEventArgs e)
    {
      FrameworkElement source = sender as FrameworkElement;
      if (TiltEffect._hasStarted)
        return;
      TiltEffect.TryStartTiltEffect(source, e);
      TiltEffect._hasStarted = true;
    }

    private static void TiltEffectDelta(object sender, ManipulationDeltaEventArgs e)
    {
      TiltEffect.ContinueTiltEffect(sender as FrameworkElement, e);
    }

    private static void TiltEffectCompleted(object sender, ManipulationCompletedEventArgs e)
    {
      TiltEffect.EndTiltEffect();
    }

    private static void EndTiltEffect()
    {
      TiltEffect._hasStarted = false;
      TiltEffect.EndTiltEffect(TiltEffect._currentTiltElement);
    }

    private static void TryStartTiltEffect(FrameworkElement source, ManipulationStartedEventArgs e)
    {
      FrameworkElement child = VisualTreeHelper.GetChild((DependencyObject) source, 0) as FrameworkElement;
      FrameworkElement manipulationContainer = e.ManipulationContainer as FrameworkElement;
      if (child == null || manipulationContainer == null)
        return;
      Point touchPoint = manipulationContainer.TransformToVisual((UIElement) child).Transform(e.ManipulationOrigin);
      Point centerPoint = new Point(child.ActualWidth / 2.0, child.ActualHeight / 2.0);
      Point centerToCenterDelta = TiltEffect.GetCenterToCenterDelta(child, source);
      TiltEffect.BeginTiltEffect(child, touchPoint, centerPoint, centerToCenterDelta);
    }

    private static Point GetCenterToCenterDelta(
      FrameworkElement element,
      FrameworkElement container)
    {
      Point point1 = new Point(element.ActualWidth / 2.0, element.ActualHeight / 2.0);
      Point point2 = !(container is PhoneApplicationFrame applicationFrame) ? new Point(container.ActualWidth / 2.0, container.ActualHeight / 2.0) : ((applicationFrame.Orientation & PageOrientation.Landscape) == PageOrientation.Landscape ? new Point(container.ActualHeight / 2.0, container.ActualWidth / 2.0) : new Point(container.ActualWidth / 2.0, container.ActualHeight / 2.0));
      Point point3 = element.TransformToVisual((UIElement) container).Transform(point1);
      return new Point(point2.X - point3.X, point2.Y - point3.Y);
    }

    private static void BeginTiltEffect(
      FrameworkElement element,
      Point touchPoint,
      Point centerPoint,
      Point centerDelta)
    {
      if (TiltEffect._tiltReturnStoryboard != null)
        TiltEffect.StopTiltReturnStoryboardAndCleanup();
      if (!TiltEffect.PrepareControlForTilt(element, centerDelta))
        return;
      TiltEffect._currentTiltElement = element;
      TiltEffect._currentTiltElementCenter = centerPoint;
      TiltEffect.PrepareTiltReturnStoryboard(element);
      TiltEffect.ApplyTiltEffect(TiltEffect._currentTiltElement, touchPoint, TiltEffect._currentTiltElementCenter);
    }

    private static bool PrepareControlForTilt(FrameworkElement element, Point centerDelta)
    {
      if (element.Projection != null || element.RenderTransform != null && element.RenderTransform.GetType() != typeof (MatrixTransform))
        return false;
      TiltEffect.OriginalCacheMode[(DependencyObject) element] = element.CacheMode;
      element.CacheMode = (CacheMode) new BitmapCache();
      TranslateTransform translateTransform = new TranslateTransform()
      {
        X = centerDelta.X,
        Y = centerDelta.Y
      };
      element.RenderTransform = (Transform) translateTransform;
      PlaneProjection planeProjection = new PlaneProjection()
      {
        GlobalOffsetX = -1.0 * centerDelta.X,
        GlobalOffsetY = -1.0 * centerDelta.Y
      };
      element.Projection = (Projection) planeProjection;
      element.ManipulationDelta += new EventHandler<ManipulationDeltaEventArgs>(TiltEffect.TiltEffectDelta);
      element.ManipulationCompleted += new EventHandler<ManipulationCompletedEventArgs>(TiltEffect.TiltEffectCompleted);
      return true;
    }

    private static void RevertPrepareControlForTilt(FrameworkElement element)
    {
      element.ManipulationDelta -= new EventHandler<ManipulationDeltaEventArgs>(TiltEffect.TiltEffectDelta);
      element.ManipulationCompleted -= new EventHandler<ManipulationCompletedEventArgs>(TiltEffect.TiltEffectCompleted);
      element.Projection = (Projection) null;
      element.RenderTransform = (Transform) null;
      CacheMode cacheMode;
      if (TiltEffect.OriginalCacheMode.TryGetValue((DependencyObject) element, out cacheMode))
      {
        element.CacheMode = cacheMode;
        TiltEffect.OriginalCacheMode.Remove((DependencyObject) element);
      }
      else
        element.CacheMode = (CacheMode) null;
    }

    private static void PrepareTiltReturnStoryboard(FrameworkElement element)
    {
      if (TiltEffect._tiltReturnStoryboard == null)
      {
        TiltEffect._tiltReturnStoryboard = new Storyboard();
        TiltEffect._tiltReturnStoryboard.Completed += new EventHandler(TiltEffect.TiltReturnStoryboard_Completed);
        TiltEffect._tiltReturnXAnimation = TiltEffect.MakeDoubleAnimation(PlaneProjection.RotationXProperty);
        TiltEffect._tiltReturnYAnimation = TiltEffect.MakeDoubleAnimation(PlaneProjection.RotationYProperty);
        TiltEffect._tiltReturnZAnimation = TiltEffect.MakeDoubleAnimation(PlaneProjection.GlobalOffsetZProperty);
        TiltEffect._tiltReturnStoryboard.Children.Add((Timeline) TiltEffect._tiltReturnXAnimation);
        TiltEffect._tiltReturnStoryboard.Children.Add((Timeline) TiltEffect._tiltReturnYAnimation);
        TiltEffect._tiltReturnStoryboard.Children.Add((Timeline) TiltEffect._tiltReturnZAnimation);
      }
      Storyboard.SetTarget((Timeline) TiltEffect._tiltReturnXAnimation, (DependencyObject) element.Projection);
      Storyboard.SetTarget((Timeline) TiltEffect._tiltReturnYAnimation, (DependencyObject) element.Projection);
      Storyboard.SetTarget((Timeline) TiltEffect._tiltReturnZAnimation, (DependencyObject) element.Projection);
    }

    private static DoubleAnimation MakeDoubleAnimation(DependencyProperty property)
    {
      DoubleAnimation element = new DoubleAnimation();
      Storyboard.SetTargetProperty((Timeline) element, new PropertyPath((object) property));
      element.BeginTime = new TimeSpan?(TiltEffect.TiltReturnAnimationDelay);
      element.To = new double?(0.0);
      element.Duration = (Duration) TiltEffect.TiltReturnAnimationDuration;
      return element;
    }

    private static void ContinueTiltEffect(FrameworkElement element, ManipulationDeltaEventArgs e)
    {
      if (!(e.ManipulationContainer is FrameworkElement manipulationContainer) || element == null)
        return;
      Point point = manipulationContainer.TransformToVisual((UIElement) element).Transform(e.ManipulationOrigin);
      if (!new Rect(0.0, 0.0, TiltEffect._currentTiltElement.ActualWidth, TiltEffect._currentTiltElement.ActualHeight).Contains(point))
        TiltEffect.PauseTiltEffect();
      else
        TiltEffect.ApplyTiltEffect(TiltEffect._currentTiltElement, point, TiltEffect._currentTiltElementCenter);
    }

    private static void EndTiltEffect(FrameworkElement element)
    {
      if (element != null)
      {
        element.ManipulationCompleted -= new EventHandler<ManipulationCompletedEventArgs>(TiltEffect.TiltEffectCompleted);
        element.ManipulationDelta -= new EventHandler<ManipulationDeltaEventArgs>(TiltEffect.TiltEffectDelta);
      }
      if (TiltEffect._tiltReturnStoryboard != null)
      {
        TiltEffect._wasPauseAnimation = false;
        if (TiltEffect._tiltReturnStoryboard.GetCurrentState() == ClockState.Active)
          return;
        TiltEffect._tiltReturnStoryboard.Begin();
      }
      else
        TiltEffect.StopTiltReturnStoryboardAndCleanup();
    }

    private static void TiltReturnStoryboard_Completed(object sender, EventArgs e)
    {
      if (TiltEffect._wasPauseAnimation)
        TiltEffect.ResetTiltEffect(TiltEffect._currentTiltElement);
      else
        TiltEffect.StopTiltReturnStoryboardAndCleanup();
    }

    private static void ResetTiltEffect(FrameworkElement element)
    {
      if (!(element.Projection is PlaneProjection projection))
        return;
      projection.RotationY = 0.0;
      projection.RotationX = 0.0;
      projection.GlobalOffsetZ = 0.0;
    }

    private static void StopTiltReturnStoryboardAndCleanup()
    {
      if (TiltEffect._tiltReturnStoryboard != null)
        TiltEffect._tiltReturnStoryboard.Stop();
      if (TiltEffect._currentTiltElement == null)
        return;
      TiltEffect.RevertPrepareControlForTilt(TiltEffect._currentTiltElement);
      TiltEffect._currentTiltElement = (FrameworkElement) null;
    }

    private static void PauseTiltEffect()
    {
      if (TiltEffect._tiltReturnStoryboard == null || TiltEffect._wasPauseAnimation)
        return;
      TiltEffect._tiltReturnStoryboard.Stop();
      TiltEffect._wasPauseAnimation = true;
      TiltEffect._tiltReturnStoryboard.Begin();
    }

    private static void ResetTiltReturnStoryboard()
    {
      TiltEffect._tiltReturnStoryboard.Stop();
      TiltEffect._wasPauseAnimation = false;
    }

    private static void ApplyTiltEffect(
      FrameworkElement element,
      Point touchPoint,
      Point centerPoint)
    {
      TiltEffect.ResetTiltReturnStoryboard();
      Point point = new Point(Math.Min(Math.Max(touchPoint.X / (centerPoint.X * 2.0), 0.0), 1.0), Math.Min(Math.Max(touchPoint.Y / (centerPoint.Y * 2.0), 0.0), 1.0));
      if (double.IsNaN(point.X) || double.IsNaN(point.Y))
        return;
      double num1 = Math.Abs(point.X - 0.5);
      double num2 = Math.Abs(point.Y - 0.5);
      double num3 = (double) -Math.Sign(point.X - 0.5);
      double num4 = (double) Math.Sign(point.Y - 0.5);
      double num5 = num1 + num2;
      double num6 = num1 + num2 > 0.0 ? num1 / (num1 + num2) : 0.0;
      double num7 = num5 * 0.3 * 180.0 / Math.PI;
      double num8 = (1.0 - num5) * 25.0;
      if (!(element.Projection is PlaneProjection projection))
        return;
      projection.RotationY = num7 * num6 * num3;
      projection.RotationX = num7 * (1.0 - num6) * num4;
      projection.GlobalOffsetZ = -num8;
    }
  }
}
