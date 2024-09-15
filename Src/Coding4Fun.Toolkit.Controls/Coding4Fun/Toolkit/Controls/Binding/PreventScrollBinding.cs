// Decompiled with JetBrains decompiler
// Type: Coding4Fun.Toolkit.Controls.Binding.PreventScrollBinding
// Assembly: Coding4Fun.Toolkit.Controls, Version=2.1.7.0, Culture=neutral, PublicKeyToken=c5fd7b72b1a17ce4
// MVID: 213D8A75-4B66-4004-95C2-D82C49ACB764
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\Coding4Fun.Toolkit.Controls.dll

using Coding4Fun.Toolkit.Controls.Common;
using Microsoft.Phone.Controls;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;


namespace Coding4Fun.Toolkit.Controls.Binding
{
  public class PreventScrollBinding
  {
    private static FrameworkElement _internalPanningControl;
    private static readonly DependencyProperty IsScrollSuspendedProperty = DependencyProperty.RegisterAttached("IsScrollSuspended", typeof (bool), typeof (PreventScrollBinding), new PropertyMetadata((object) false));
    private static readonly DependencyProperty LastTouchPointProperty = DependencyProperty.RegisterAttached("LastTouchPoint", typeof (Point), typeof (PreventScrollBinding), new PropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty IsEnabled = DependencyProperty.RegisterAttached(nameof (IsEnabled), typeof (bool), typeof (PreventScrollBinding), new PropertyMetadata((object) false, new PropertyChangedCallback(PreventScrollBinding.IsEnabledDependencyPropertyChangedCallback)));

    public static bool GetIsEnabled(DependencyObject obj)
    {
      return (bool) obj.GetValue(PreventScrollBinding.IsEnabled);
    }

    public static void SetIsEnabled(DependencyObject obj, bool value)
    {
      obj.SetValue(PreventScrollBinding.IsEnabled, (object) value);
    }

    private static void IsEnabledDependencyPropertyChangedCallback(
      DependencyObject dobj,
      DependencyPropertyChangedEventArgs ea)
    {
      if (!(dobj is FrameworkElement frameworkElement))
        return;
      frameworkElement.UseOptimizedManipulationRouting = false;
      frameworkElement.Unloaded += new RoutedEventHandler(PreventScrollBinding.BlockingElementUnloaded);
      frameworkElement.ManipulationStarted += new EventHandler<ManipulationStartedEventArgs>(PreventScrollBinding.SuspendScroll);
      frameworkElement.MouseLeftButtonDown += new MouseButtonEventHandler(PreventScrollBinding.SuspendScroll);
    }

    private static void BlockingElementUnloaded(object sender, RoutedEventArgs e)
    {
      if (!(sender is FrameworkElement frameworkElement))
        return;
      frameworkElement.Unloaded -= new RoutedEventHandler(PreventScrollBinding.BlockingElementUnloaded);
      frameworkElement.ManipulationStarted -= new EventHandler<ManipulationStartedEventArgs>(PreventScrollBinding.SuspendScroll);
      frameworkElement.MouseLeftButtonDown -= new MouseButtonEventHandler(PreventScrollBinding.SuspendScroll);
    }

    private static void SuspendScroll(object sender, RoutedEventArgs e)
    {
      FrameworkElement blockingElement = sender as FrameworkElement;
      if (PreventScrollBinding._internalPanningControl == null)
        PreventScrollBinding._internalPanningControl = PreventScrollBinding.FindAncestor((DependencyObject) blockingElement, (Func<DependencyObject, bool>) (p => p is Pivot || p is Panorama)) as FrameworkElement;
      if (PreventScrollBinding._internalPanningControl != null && (bool) PreventScrollBinding._internalPanningControl.GetValue(PreventScrollBinding.IsScrollSuspendedProperty) || PreventScrollBinding.FindAncestor(e.OriginalSource as DependencyObject, (Func<DependencyObject, bool>) (dobj => dobj == blockingElement)) != blockingElement)
        return;
      if (PreventScrollBinding._internalPanningControl != null)
        PreventScrollBinding._internalPanningControl.SetValue(PreventScrollBinding.IsScrollSuspendedProperty, (object) true);
      Touch.FrameReported += new TouchFrameEventHandler(PreventScrollBinding.TouchFrameReported);
      if (blockingElement != null)
        blockingElement.IsHitTestVisible = true;
      if (PreventScrollBinding._internalPanningControl == null)
        return;
      PreventScrollBinding._internalPanningControl.IsHitTestVisible = false;
    }

    private static void TouchFrameReported(object sender, TouchFrameEventArgs e)
    {
      if (PreventScrollBinding._internalPanningControl == null)
        return;
      TouchPoint touchPoint = PreventScrollBinding._internalPanningControl.GetValue(PreventScrollBinding.LastTouchPointProperty) as TouchPoint;
      int num = (bool) PreventScrollBinding._internalPanningControl.GetValue(PreventScrollBinding.IsScrollSuspendedProperty) ? 1 : 0;
      TouchPointCollection touchPoints = e.GetTouchPoints((UIElement) ApplicationSpace.RootFrame);
      if (touchPoint == null || touchPoint != touchPoints.Last<TouchPoint>())
        touchPoint = touchPoints.Last<TouchPoint>();
      if (num == 0 || touchPoint == null || touchPoint.Action != TouchAction.Up)
        return;
      Touch.FrameReported -= new TouchFrameEventHandler(PreventScrollBinding.TouchFrameReported);
      PreventScrollBinding._internalPanningControl.IsHitTestVisible = true;
      PreventScrollBinding._internalPanningControl.SetValue(PreventScrollBinding.IsScrollSuspendedProperty, (object) false);
    }

    public static DependencyObject FindAncestor(
      DependencyObject dependencyObject,
      Func<DependencyObject, bool> predicate)
    {
      if (predicate(dependencyObject))
        return dependencyObject;
      DependencyObject dependencyObject1 = (DependencyObject) null;
      if (dependencyObject is FrameworkElement reference)
        dependencyObject1 = reference.Parent ?? VisualTreeHelper.GetParent((DependencyObject) reference);
      return dependencyObject1 == null ? (DependencyObject) null : PreventScrollBinding.FindAncestor(dependencyObject1, predicate);
    }
  }
}
