// Decompiled with JetBrains decompiler
// Type: System.Windows.Controls.TemplatedVisualTreeExtensions
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using System.Collections.Generic;
using System.Linq;


namespace System.Windows.Controls
{
  public static class TemplatedVisualTreeExtensions
  {
    internal static T GetFirstLogicalChildByType<T>(
      this FrameworkElement parent,
      bool applyTemplates)
      where T : FrameworkElement
    {
      Queue<FrameworkElement> frameworkElementQueue = new Queue<FrameworkElement>();
      frameworkElementQueue.Enqueue(parent);
      while (frameworkElementQueue.Count > 0)
      {
        FrameworkElement parent1 = frameworkElementQueue.Dequeue();
        Control control = parent1 as Control;
        if (applyTemplates && control != null)
          control.ApplyTemplate();
        if (parent1 is T && parent1 != parent)
          return (T) parent1;
        foreach (FrameworkElement frameworkElement in VisualTreeExtensions.GetVisualChildren(parent1).OfType<FrameworkElement>())
          frameworkElementQueue.Enqueue(frameworkElement);
      }
      return default (T);
    }

    internal static IEnumerable<T> GetLogicalChildrenByType<T>(
      this FrameworkElement parent,
      bool applyTemplates)
      where T : FrameworkElement
    {
      if (applyTemplates && parent is Control)
        ((Control) parent).ApplyTemplate();
      Queue<FrameworkElement> queue = new Queue<FrameworkElement>(VisualTreeExtensions.GetVisualChildren(parent).OfType<FrameworkElement>());
      while (queue.Count > 0)
      {
        FrameworkElement element = queue.Dequeue();
        if (applyTemplates && element is Control)
          ((Control) element).ApplyTemplate();
        if (element is T obj)
          yield return obj;
        foreach (FrameworkElement frameworkElement in VisualTreeExtensions.GetVisualChildren(element).OfType<FrameworkElement>())
          queue.Enqueue(frameworkElement);
        element = (FrameworkElement) null;
      }
    }
  }
}
