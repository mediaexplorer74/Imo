﻿// Decompiled with JetBrains decompiler
// Type: Microsoft.Phone.Controls.RollTransition
// Assembly: Coding4Fun.Toolkit.Controls, Version=2.1.7.0, Culture=neutral, PublicKeyToken=c5fd7b72b1a17ce4
// MVID: 213D8A75-4B66-4004-95C2-D82C49ACB764
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\Coding4Fun.Toolkit.Controls.dll

using System.Windows;


namespace Microsoft.Phone.Controls
{
  internal class RollTransition : TransitionElement
  {
    public override ITransition GetTransition(UIElement element) => Transitions.Roll(element);
  }
}