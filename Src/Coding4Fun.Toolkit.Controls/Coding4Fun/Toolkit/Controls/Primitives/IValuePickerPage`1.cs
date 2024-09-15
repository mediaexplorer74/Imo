// Decompiled with JetBrains decompiler
// Type: Coding4Fun.Toolkit.Controls.Primitives.IValuePickerPage`1
// Assembly: Coding4Fun.Toolkit.Controls, Version=2.1.7.0, Culture=neutral, PublicKeyToken=c5fd7b72b1a17ce4
// MVID: 213D8A75-4B66-4004-95C2-D82C49ACB764
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\Coding4Fun.Toolkit.Controls.dll

using System.Windows;


namespace Coding4Fun.Toolkit.Controls.Primitives
{
  public interface IValuePickerPage<T> where T : struct
  {
    void InitDataSource();

    T? Value { get; set; }

    string DialogTitle { get; set; }

    void SetFlowDirection(FlowDirection flowDirection);
  }
}
