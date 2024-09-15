// Decompiled with JetBrains decompiler
// Type: Coding4Fun.Toolkit.Controls.ButtonBaseHelper
// Assembly: Coding4Fun.Toolkit.Controls, Version=2.1.7.0, Culture=neutral, PublicKeyToken=c5fd7b72b1a17ce4
// MVID: 213D8A75-4B66-4004-95C2-D82C49ACB764
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\Coding4Fun.Toolkit.Controls.dll

using Coding4Fun.Toolkit.Controls.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;


namespace Coding4Fun.Toolkit.Controls
{
  internal static class ButtonBaseHelper
  {
    public static void UpdateImageSource(
      FrameworkElement content,
      Grid hostBody,
      ImageSource imageSource,
      Stretch stretch)
    {
      if (hostBody == null || content == null)
        return;
      Rectangle[] array = hostBody.Children.OfType<Rectangle>().ToArray<Rectangle>();
      for (int index = ((IEnumerable<Rectangle>) array).Count<Rectangle>() - 1; index >= 0; --index)
        hostBody.Children.Remove((UIElement) array[index]);
      if (imageSource == null)
        return;
      ImageBrush imageBrush1 = new ImageBrush();
      imageBrush1.ImageSource = imageSource;
      imageBrush1.Stretch = stretch;
      ImageBrush imageBrush2 = imageBrush1;
      Rectangle rectangle = new Rectangle();
      rectangle.OpacityMask = (Brush) imageBrush2;
      Rectangle target = rectangle;
      hostBody.Children.Add((UIElement) target);
      ButtonBaseHelper.ApplyForegroundToFillBinding(content, (FrameworkElement) target);
    }

    public static void ApplyTitleOffset(ContentControl contentTitle)
    {
      if (contentTitle == null)
        return;
      double bottom = -(contentTitle.FontSize / 8.0);
      double top = -(contentTitle.FontSize / 2.0) - bottom;
      contentTitle.Margin = new Thickness(0.0, top, 0.0, bottom);
    }

    public static void ApplyForegroundToFillBinding(ContentControl control)
    {
      if (control == null || !(control.Content is FrameworkElement content))
        return;
      if (TypeExtensions.IsTypeOf(content, typeof (Shape)))
      {
        Shape target = content as Shape;
        ButtonBaseHelper.ResetVerifyAndApplyForegroundToFillBinding((FrameworkElement) control, target);
      }
      else
      {
        foreach (Shape target in content.GetLogicalChildrenByType<Shape>(false))
        {
          target.GetHashCode();
          ButtonBaseHelper.ResetVerifyAndApplyForegroundToFillBinding((FrameworkElement) control, target);
        }
      }
    }

    internal static void ResetVerifyAndApplyForegroundToFillBinding(
      FrameworkElement source,
      Shape target)
    {
      if (target == null)
        return;
      bool flag = target.GetBindingExpression(Shape.FillProperty) != null;
      if (!(target.Fill == null | flag))
        return;
      target.Fill = (Brush) null;
      ButtonBaseHelper.ApplyBinding(source, (FrameworkElement) target, "Foreground", Shape.FillProperty);
    }

    internal static void ApplyForegroundToFillBinding(
      FrameworkElement source,
      FrameworkElement target)
    {
      ButtonBaseHelper.ApplyBinding(source, target, "Foreground", Shape.FillProperty);
    }

    public static void ApplyBinding(
      FrameworkElement source,
      FrameworkElement target,
      string propertyPath,
      DependencyProperty property,
      IValueConverter converter = null,
      object converterParameter = null)
    {
      if (source == null || target == null)
        return;
      target.SetBinding(property, new Binding()
      {
        Source = (object) source,
        Path = new PropertyPath(propertyPath, new object[0]),
        Converter = converter,
        ConverterParameter = converterParameter
      });
    }

    public static Path CreateXamlCheck(FrameworkElement control)
    {
      if (XamlReader.Load("<Path \r\n\t\t\t\t\txmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"\r\n\t\t\t\t\tStretch=\"Uniform\" \r\n\t\t\t\t\tData=\"F1M227.2217,408.499L226.4427,407.651C226.2357,407.427,226.2467,407.075,226.4737,406.865L228.7357,404.764C228.8387,404.668,228.9737,404.615,229.1147,404.615C229.2707,404.615,229.4147,404.679,229.5207,404.792L235.7317,411.479L246.4147,397.734C246.5207,397.601,246.6827,397.522,246.8547,397.522C246.9797,397.522,247.0987,397.563,247.1967,397.639L249.6357,399.533C249.7507,399.624,249.8257,399.756,249.8447,399.906C249.8627,400.052,249.8237,400.198,249.7357,400.313L236.0087,417.963z\"\r\n\t\t\t\t\t/>") is Path target)
        ButtonBaseHelper.ApplyBinding(control, (FrameworkElement) target, "ButtonHeight", FrameworkElement.HeightProperty, (IValueConverter) new NumberMultiplierConverter(), (object) 0.25);
      return target;
    }

    public static Path CreateXamlCancel(FrameworkElement control)
    {
      if (XamlReader.Load("<Path \r\n\t\t\t\t\txmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"\r\n\t\t\t\t\tStretch=\"Uniform\" \r\n\t\t\t\t\tData=\"M15.047,0 L17.709,2.663 L11.5166,8.85499 L17.71,15.048 L15.049,17.709 L8.8553,11.5161 L2.662,17.709 L0,15.049 L6.19351,8.85467 L0.002036,2.66401 L2.66304,0.002015 L8.85463,6.19319 z\"\r\n\t\t\t\t\t/>") is Path target)
        ButtonBaseHelper.ApplyBinding(control, (FrameworkElement) target, "ButtonHeight", FrameworkElement.HeightProperty, (IValueConverter) new NumberMultiplierConverter(), (object) 0.25);
      return target;
    }
  }
}
