// Decompiled with JetBrains decompiler
// Type: Microsoft.Phone.Controls.ITransition
// Assembly: Coding4Fun.Toolkit.Controls, Version=2.1.7.0, Culture=neutral, PublicKeyToken=c5fd7b72b1a17ce4
// MVID: 213D8A75-4B66-4004-95C2-D82C49ACB764
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\Coding4Fun.Toolkit.Controls.dll

using System;
using System.Windows.Media.Animation;


namespace Microsoft.Phone.Controls
{
  internal interface ITransition
  {
    event EventHandler Completed;

    ClockState GetCurrentState();

    TimeSpan GetCurrentTime();

    void Pause();

    void Resume();

    void Seek(TimeSpan offset);

    void SeekAlignedToLastTick(TimeSpan offset);

    void SkipToFill();

    void Begin();

    void Stop();
  }
}
