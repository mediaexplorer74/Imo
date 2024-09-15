// Decompiled with JetBrains decompiler
// Type: Coding4Fun.Toolkit.Controls.ValueChangedEventArgs`1
// Assembly: Coding4Fun.Toolkit.Controls, Version=2.1.7.0, Culture=neutral, PublicKeyToken=c5fd7b72b1a17ce4
// MVID: 213D8A75-4B66-4004-95C2-D82C49ACB764
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\Coding4Fun.Toolkit.Controls.dll

using System;


namespace Coding4Fun.Toolkit.Controls
{
  public class ValueChangedEventArgs<T> : EventArgs where T : struct
  {
    public ValueChangedEventArgs(T? oldValue, T? newValue)
    {
      this.OldValue = oldValue;
      this.NewValue = newValue;
    }

    public T? OldValue { get; private set; }

    public T? NewValue { get; private set; }
  }
}
