// Decompiled with JetBrains decompiler
// Type: Coding4Fun.Toolkit.Controls.Common.ApplicationSpace
// Assembly: Coding4Fun.Toolkit.Controls, Version=2.1.7.0, Culture=neutral, PublicKeyToken=c5fd7b72b1a17ce4
// MVID: 213D8A75-4B66-4004-95C2-D82C49ACB764
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\Coding4Fun.Toolkit.Controls.dll

using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;


namespace Coding4Fun.Toolkit.Controls.Common
{
  public static class ApplicationSpace
  {
    public static int ScaleFactor() => Application.Current.Host.Content.ScaleFactor;

    public static Frame RootFrame => Application.Current.RootVisual as Frame;

    public static bool IsDesignMode => DesignerProperties.IsInDesignTool;

    public static Dispatcher CurrentDispatcher => Deployment.Current.Dispatcher;
  }
}
