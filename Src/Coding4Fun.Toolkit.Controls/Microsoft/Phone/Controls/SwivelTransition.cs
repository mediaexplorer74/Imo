// Decompiled with JetBrains decompiler
// Type: Microsoft.Phone.Controls.SwivelTransition
// Assembly: Coding4Fun.Toolkit.Controls, Version=2.1.7.0, Culture=neutral, PublicKeyToken=c5fd7b72b1a17ce4
// MVID: 213D8A75-4B66-4004-95C2-D82C49ACB764
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\Coding4Fun.Toolkit.Controls.dll

using System.Windows;


namespace Microsoft.Phone.Controls
{
  internal class SwivelTransition : TransitionElement
  {
    public static readonly DependencyProperty ModeProperty = DependencyProperty.Register(nameof (Mode), typeof (SwivelTransitionMode), typeof (SwivelTransition), (PropertyMetadata) null);

    public SwivelTransitionMode Mode
    {
      get => (SwivelTransitionMode) this.GetValue(SwivelTransition.ModeProperty);
      set => this.SetValue(SwivelTransition.ModeProperty, (object) value);
    }

    public override ITransition GetTransition(UIElement element)
    {
      return Transitions.Swivel(element, this.Mode);
    }
  }
}
