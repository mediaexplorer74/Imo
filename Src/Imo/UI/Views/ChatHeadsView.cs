// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.UI.Views.ChatHeadsView
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;


namespace ImoSilverlightApp.UI.Views
{
  public class ChatHeadsView : UserControl
  {
    private bool _contentLoaded;

    public ChatHeadsView() => this.InitializeComponent();

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/ImoSilverlightApp;component/UI/Views/ChatHeadsView.xaml", UriKind.Relative));
    }
  }
}
