// Decompiled with JetBrains decompiler
// Type: System.Windows.Controls.VisualTreeExtensions
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;


namespace System.Windows.Controls
{
  internal static class VisualTreeExtensions
  {
    internal static IEnumerable<DependencyObject> GetVisualChildren(this DependencyObject parent)
    {
      int childCount = VisualTreeHelper.GetChildrenCount(parent);
      for (int counter = 0; counter < childCount; ++counter)
        yield return VisualTreeHelper.GetChild(parent, counter);
    }

    internal static IEnumerable<FrameworkElement> GetLogicalChildrenBreadthFirst(
      this FrameworkElement parent)
    {
      Queue<FrameworkElement> queue = new Queue<FrameworkElement>(VisualTreeExtensions.GetVisualChildren(parent).OfType<FrameworkElement>());
      while (queue.Count > 0)
      {
        FrameworkElement element = queue.Dequeue();
        yield return element;
        foreach (FrameworkElement frameworkElement in VisualTreeExtensions.GetVisualChildren(element).OfType<FrameworkElement>())
          queue.Enqueue(frameworkElement);
        element = (FrameworkElement) null;
      }
    }

    internal static IEnumerable<FrameworkElement> GetVisualAncestors(this FrameworkElement node)
    {
      for (FrameworkElement parent = VisualTreeExtensions.GetVisualParent(node); parent != null; parent = VisualTreeExtensions.GetVisualParent(parent))
        yield return parent;
    }

    internal static FrameworkElement GetVisualParent(this FrameworkElement node)
    {
      return VisualTreeHelper.GetParent((DependencyObject) node) as FrameworkElement;
    }

    internal static T GetParentByType<T>(this DependencyObject element) where T : FrameworkElement
    {
      T obj = default (T);
      for (DependencyObject parent = VisualTreeHelper.GetParent(element); parent != null; parent = VisualTreeHelper.GetParent(parent))
      {
        if (parent is T parentByType)
          return parentByType;
      }
      return default (T);
    }
  }
}
