// Decompiled with JetBrains decompiler
// Type: Coding4Fun.Toolkit.Controls.ButtonBase
// Assembly: Coding4Fun.Toolkit.Controls, Version=2.1.7.0, Culture=neutral, PublicKeyToken=c5fd7b72b1a17ce4
// MVID: 213D8A75-4B66-4004-95C2-D82C49ACB764
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\Coding4Fun.Toolkit.Controls.dll

using System.Windows;
using System.Windows.Controls;


namespace Coding4Fun.Toolkit.Controls
{
  public abstract class ButtonBase : Button, IButtonBase
  {
    public static readonly DependencyProperty LabelProperty = DependencyProperty.Register(nameof (Label), typeof (object), typeof (ButtonBase), new PropertyMetadata((object) string.Empty));

    public override void OnApplyTemplate() => base.OnApplyTemplate();

    public object Label
    {
      get => this.GetValue(ButtonBase.LabelProperty);
      set => this.SetValue(ButtonBase.LabelProperty, value);
    }
  }
}
