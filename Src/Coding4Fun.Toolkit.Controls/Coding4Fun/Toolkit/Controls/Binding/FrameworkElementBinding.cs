// Decompiled with JetBrains decompiler
// Type: Coding4Fun.Toolkit.Controls.Binding.FrameworkElementBinding
// Assembly: Coding4Fun.Toolkit.Controls, Version=2.1.7.0, Culture=neutral, PublicKeyToken=c5fd7b72b1a17ce4
// MVID: 213D8A75-4B66-4004-95C2-D82C49ACB764
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\Coding4Fun.Toolkit.Controls.dll

using System.Windows;
using System.Windows.Media;


namespace Coding4Fun.Toolkit.Controls.Binding
{
  public class FrameworkElementBinding
  {
    public static readonly DependencyProperty ClipToBoundsProperty = DependencyProperty.RegisterAttached("ClipToBounds", typeof (bool), typeof (FrameworkElementBinding), new PropertyMetadata((object) false, new PropertyChangedCallback(FrameworkElementBinding.OnClipToBoundsPropertyChanged)));

    public static bool GetClipToBounds(DependencyObject obj)
    {
      return (bool) obj.GetValue(FrameworkElementBinding.ClipToBoundsProperty);
    }

    public static void SetClipToBounds(DependencyObject obj, bool value)
    {
      obj.SetValue(FrameworkElementBinding.ClipToBoundsProperty, (object) value);
    }

    private static void OnClipToBoundsPropertyChanged(
      DependencyObject obj,
      DependencyPropertyChangedEventArgs e)
    {
      if (e.NewValue == e.OldValue)
        return;
      FrameworkElementBinding.HandleClipToBoundsEventAppend((object) obj, (bool) e.NewValue);
    }

    private static void HandleClipToBoundsEventAppend(object sender, bool value)
    {
      if (!(sender is FrameworkElement element))
        return;
      FrameworkElementBinding.SetClippingBound(element, value);
      if (value)
      {
        element.Loaded += new RoutedEventHandler(FrameworkElementBinding.ClipToBoundsPropertyChanged);
        element.SizeChanged += new SizeChangedEventHandler(FrameworkElementBinding.ClipToBoundsPropertyChanged);
      }
      else
      {
        element.Loaded -= new RoutedEventHandler(FrameworkElementBinding.ClipToBoundsPropertyChanged);
        element.SizeChanged -= new SizeChangedEventHandler(FrameworkElementBinding.ClipToBoundsPropertyChanged);
      }
    }

    private static void ClipToBoundsPropertyChanged(object sender, RoutedEventArgs e)
    {
      if (!(sender is FrameworkElement element))
        return;
      FrameworkElementBinding.SetClippingBound(element, FrameworkElementBinding.GetClipToBounds((DependencyObject) element));
    }

    private static void SetClippingBound(FrameworkElement element, bool setClippingBound)
    {
      if (setClippingBound)
        element.Clip = (Geometry) new RectangleGeometry()
        {
          Rect = new Rect(0.0, 0.0, element.ActualWidth, element.ActualHeight)
        };
      else
        element.Clip = (Geometry) null;
    }
  }
}
