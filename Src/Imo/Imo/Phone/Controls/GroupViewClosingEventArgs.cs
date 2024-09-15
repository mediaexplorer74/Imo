// Decompiled with JetBrains decompiler
// Type: Imo.Phone.Controls.GroupViewClosingEventArgs
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using System;
using System.Windows.Controls;


namespace Imo.Phone.Controls
{
  public class GroupViewClosingEventArgs : EventArgs
  {
    internal GroupViewClosingEventArgs(ItemsControl itemsControl, object selectedGroup)
    {
      this.ItemsControl = itemsControl;
      this.SelectedGroup = selectedGroup;
    }

    public ItemsControl ItemsControl { get; private set; }

    public object SelectedGroup { get; private set; }

    public bool Cancel { get; set; }
  }
}
