// Decompiled with JetBrains decompiler
// Type: Imo.Phone.Controls.GroupViewOpenedEventArgs
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using System;
using System.Windows.Controls;


namespace Imo.Phone.Controls
{
  public class GroupViewOpenedEventArgs : EventArgs
  {
    internal GroupViewOpenedEventArgs(ItemsControl itemsControl)
    {
      this.ItemsControl = itemsControl;
    }

    public ItemsControl ItemsControl { get; private set; }
  }
}
