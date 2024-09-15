// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.UI.Views.BusyIndicator
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;


namespace ImoSilverlightApp.UI.Views
{
  public class BusyIndicator : UserControl
  {
    public static readonly DependencyProperty textProperty = DependencyProperty.Register(nameof (Text), typeof (string), typeof (BusyIndicator), new PropertyMetadata((object) string.Empty));
    internal UserControl busyIndicatorRoot;
    private bool _contentLoaded;

    public BusyIndicator()
    {
      this.InitializeComponent();
      this.Loaded += new RoutedEventHandler(this.BusyIndicator_Loaded);
    }

    private void BusyIndicator_Loaded(object sender, RoutedEventArgs e)
    {
    }

    public string Text
    {
      get => this.GetValue(BusyIndicator.textProperty).ToString();
      set => this.SetValue(BusyIndicator.textProperty, (object) value);
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/ImoSilverlightApp;component/UI/Views/BusyIndicator.xaml", UriKind.Relative));
      this.busyIndicatorRoot = (UserControl) this.FindName("busyIndicatorRoot");
    }
  }
}
