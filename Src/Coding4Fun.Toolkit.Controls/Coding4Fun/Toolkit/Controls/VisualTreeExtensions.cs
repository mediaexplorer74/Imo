// Decompiled with JetBrains decompiler
// Type: Coding4Fun.Toolkit.Controls.VisualTreeExtensions
// Assembly: Coding4Fun.Toolkit.Controls, Version=2.1.7.0, Culture=neutral, PublicKeyToken=c5fd7b72b1a17ce4
// MVID: 213D8A75-4B66-4004-95C2-D82C49ACB764
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\Coding4Fun.Toolkit.Controls.dll

using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;


namespace Coding4Fun.Toolkit.Controls
{
  public static class VisualTreeExtensions
  {
    public static IEnumerable<DependencyObject> GetVisualChildren(this DependencyObject parent)
    {
      int childCount = VisualTreeHelper.GetChildrenCount(parent);
      for (int counter = 0; counter < childCount; ++counter)
        yield return VisualTreeHelper.GetChild(parent, counter);
    }

    public static IEnumerable<FrameworkElement> GetLogicalChildrenBreadthFirst(
      this FrameworkElement parent)
    {
      Queue<FrameworkElement> queue = new Queue<FrameworkElement>(parent.GetVisualChildren().OfType<FrameworkElement>());
      while (queue.Count > 0)
      {
        FrameworkElement element = queue.Dequeue();
        yield return element;
        foreach (FrameworkElement frameworkElement in element.GetVisualChildren().OfType<FrameworkElement>())
          queue.Enqueue(frameworkElement);
        element = (FrameworkElement) null;
      }
    }

    public static IEnumerable<FrameworkElement> GetVisualAncestors(this FrameworkElement node)
    {
      for (FrameworkElement parent = node.GetVisualParent(); parent != null; parent = parent.GetVisualParent())
        yield return parent;
    }

    public static FrameworkElement GetVisualParent(this FrameworkElement node)
    {
      return VisualTreeHelper.GetParent((DependencyObject) node) as FrameworkElement;
    }
  }
}
