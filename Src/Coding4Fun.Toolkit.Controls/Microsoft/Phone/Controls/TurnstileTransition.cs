// Decompiled with JetBrains decompiler
// Type: Microsoft.Phone.Controls.TurnstileTransition
// Assembly: Coding4Fun.Toolkit.Controls, Version=2.1.7.0, Culture=neutral, PublicKeyToken=c5fd7b72b1a17ce4
// MVID: 213D8A75-4B66-4004-95C2-D82C49ACB764
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\Coding4Fun.Toolkit.Controls.dll

using System.Windows;


namespace Microsoft.Phone.Controls
{
  internal class TurnstileTransition : TransitionElement
  {
    public static readonly DependencyProperty ModeProperty = DependencyProperty.Register(nameof (Mode), typeof (TurnstileTransitionMode), typeof (TurnstileTransition), (PropertyMetadata) null);

    public TurnstileTransitionMode Mode
    {
      get => (TurnstileTransitionMode) this.GetValue(TurnstileTransition.ModeProperty);
      set => this.SetValue(TurnstileTransition.ModeProperty, (object) value);
    }

    public override ITransition GetTransition(UIElement element)
    {
      return Transitions.Turnstile(element, this.Mode);
    }
  }
}
