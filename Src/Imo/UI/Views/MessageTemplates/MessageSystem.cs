// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.UI.Views.MessageTemplates.MessageSystem
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Storage.Models;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace ImoSilverlightApp.UI.Views.MessageTemplates
{
  public sealed class MessageSystem : StackPanel
  {
    private bool _contentLoaded;

    public MessageSystem() => this.InitializeComponent();

    private void Callback_Tap(object sender, GestureEventArgs e)
    {
      SystemMessage dataContext = this.DataContext as SystemMessage;
      if (dataContext.IsMissedVideoCall)
        IMO.AVManager.StartVideoCall(dataContext.Buid, "callback_button");
      else
        IMO.AVManager.StartAudioCall(dataContext.Buid, "callback_button");
    }

    private void Copy_Click(object sender, RoutedEventArgs e)
    {
      Clipboard.SetText((this.DataContext as Message).Msg);
    }

    private void Delete_Click(object sender, RoutedEventArgs e)
    {
      IMO.IM.DeleteMessage(this.DataContext as Message);
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/ImoSilverlightApp;component/UI/Views/MessageTemplates/MessageSystem.xaml", UriKind.Relative));
    }
  }
}
