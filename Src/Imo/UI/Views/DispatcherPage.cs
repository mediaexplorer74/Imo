// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.UI.Views.DispatcherPage
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using Microsoft.Phone.Controls;
using System;
using System.Diagnostics;
using System.Windows;


namespace ImoSilverlightApp.UI.Views
{
  public class DispatcherPage : PhoneApplicationPage
  {
    internal Pivot pivot;
    private bool _contentLoaded;

    public DispatcherPage()
    {
      this.InitializeComponent();
      this.DataContext = (object) new DispatcherViewModel((FrameworkElement) this);
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/ImoSilverlightApp;component/UI/Views/DispatcherPage.xaml", UriKind.Relative));
      this.pivot = (Pivot) this.FindName("pivot");
    }
  }
}
