// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.Helpers.VisualUtils
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using NLog;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;


namespace ImoSilverlightApp.Helpers
{
  internal class VisualUtils
  {
    private static readonly Logger log = LogManager.GetLogger(typeof (VisualUtils).Name);

    public static T FindParent<T>(DependencyObject child) where T : DependencyObject
    {
      DependencyObject parent = VisualTreeHelper.GetParent(child);
      if (parent == null)
        return default (T);
      return parent is T obj ? obj : VisualUtils.FindParent<T>(parent);
    }

    public static IEnumerable<T> GetChildrenOfType<T>(DependencyObject depObj) where T : DependencyObject
    {
      if (depObj != null)
      {
        for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); ++i)
        {
          DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
          if (child is T obj1)
            yield return obj1;
          if (child != null)
          {
            foreach (T obj2 in VisualUtils.GetChildrenOfType<T>(child))
              yield return obj2;
          }
          child = (DependencyObject) null;
        }
      }
    }

    public static T GetViewModelOf<T>(object element) where T : class
    {
      if (element is FrameworkElement)
      {
        if ((element as FrameworkElement).DataContext is T)
          return (element as FrameworkElement).DataContext as T;
        VisualUtils.log.Error(string.Format("Unable to convert {0} to {1}", (object) (element as FrameworkElement).DataContext.GetType(), (object) typeof (T)), 65, nameof (GetViewModelOf));
        return default (T);
      }
      VisualUtils.log.Error("element is not a FrameworkElement", 72, nameof (GetViewModelOf));
      return default (T);
    }
  }
}
