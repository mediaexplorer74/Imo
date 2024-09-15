// Decompiled with JetBrains decompiler
// Type: Microsoft.Phone.Controls.TransitionService
// Assembly: Coding4Fun.Toolkit.Controls, Version=2.1.7.0, Culture=neutral, PublicKeyToken=c5fd7b72b1a17ce4
// MVID: 213D8A75-4B66-4004-95C2-D82C49ACB764
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\Coding4Fun.Toolkit.Controls.dll

using System;
using System.Windows;


namespace Microsoft.Phone.Controls
{
  internal static class TransitionService
  {
    public static readonly DependencyProperty NavigationInTransitionProperty = DependencyProperty.RegisterAttached("NavigationInTransition", typeof (NavigationInTransition), typeof (TransitionService), (PropertyMetadata) null);
    public static readonly DependencyProperty NavigationOutTransitionProperty = DependencyProperty.RegisterAttached("NavigationOutTransition", typeof (NavigationOutTransition), typeof (TransitionService), (PropertyMetadata) null);

    public static NavigationInTransition GetNavigationInTransition(UIElement element)
    {
      return element != null ? (NavigationInTransition) element.GetValue(TransitionService.NavigationInTransitionProperty) : throw new ArgumentNullException(nameof (element));
    }

    public static NavigationOutTransition GetNavigationOutTransition(UIElement element)
    {
      return element != null ? (NavigationOutTransition) element.GetValue(TransitionService.NavigationOutTransitionProperty) : throw new ArgumentNullException(nameof (element));
    }

    public static void SetNavigationInTransition(UIElement element, NavigationInTransition value)
    {
      if (element == null)
        throw new ArgumentNullException(nameof (element));
      element.SetValue(TransitionService.NavigationInTransitionProperty, (object) value);
    }

    public static void SetNavigationOutTransition(UIElement element, NavigationOutTransition value)
    {
      if (element == null)
        throw new ArgumentNullException(nameof (element));
      element.SetValue(TransitionService.NavigationOutTransitionProperty, (object) value);
    }
  }
}
