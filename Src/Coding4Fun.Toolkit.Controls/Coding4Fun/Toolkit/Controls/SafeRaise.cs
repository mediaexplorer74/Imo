// Decompiled with JetBrains decompiler
// Type: Coding4Fun.Toolkit.Controls.SafeRaise
// Assembly: Coding4Fun.Toolkit.Controls, Version=2.1.7.0, Culture=neutral, PublicKeyToken=c5fd7b72b1a17ce4
// MVID: 213D8A75-4B66-4004-95C2-D82C49ACB764
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\Coding4Fun.Toolkit.Controls.dll

using System;


namespace Coding4Fun.Toolkit.Controls
{
  internal static class SafeRaise
  {
    public static void Raise(EventHandler eventToRaise, object sender)
    {
      if (eventToRaise == null)
        return;
      eventToRaise(sender, EventArgs.Empty);
    }

    public static void Raise(EventHandler<EventArgs> eventToRaise, object sender)
    {
      SafeRaise.Raise<EventArgs>(eventToRaise, sender, EventArgs.Empty);
    }

    public static void Raise<T>(EventHandler<T> eventToRaise, object sender, T args) where T : EventArgs
    {
      if (eventToRaise == null)
        return;
      eventToRaise(sender, args);
    }

    public static void Raise<T>(
      EventHandler<T> eventToRaise,
      object sender,
      SafeRaise.GetEventArgs<T> getEventArgs)
      where T : EventArgs
    {
      if (eventToRaise == null)
        return;
      eventToRaise(sender, getEventArgs());
    }

    public delegate T GetEventArgs<T>() where T : EventArgs;
  }
}
