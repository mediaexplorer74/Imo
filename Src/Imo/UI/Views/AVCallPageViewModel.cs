// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.UI.Views.AVCallPageViewModel
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.AV;
using ImoSilverlightApp.Storage.Models;
using System;
using System.Collections.Generic;
using System.Windows;


namespace ImoSilverlightApp.UI.Views
{
  internal class AVCallPageViewModel : ViewModelBase
  {
    private CallController callController;

    public AVCallPageViewModel(FrameworkElement element, CallController callController)
      : base(element)
    {
      this.callController = callController;
      if (!this.Contact.IsGroup)
        return;
      IMO.AVManager.GetGroupCallMembers(this.Contact.Buid, (Action<List<Contact>>) (t => { }));
    }

    public Contact Contact => this.callController.Contact;

    public CallController CallController => this.callController;

    public GroupCallController GroupCallController => this.callController as GroupCallController;

    public bool IsGroupCall => this.Contact.IsGroup;
  }
}
