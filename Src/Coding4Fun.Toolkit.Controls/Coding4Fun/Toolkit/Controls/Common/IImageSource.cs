﻿// Decompiled with JetBrains decompiler
// Type: Coding4Fun.Toolkit.Controls.Common.IImageSource
// Assembly: Coding4Fun.Toolkit.Controls, Version=2.1.7.0, Culture=neutral, PublicKeyToken=c5fd7b72b1a17ce4
// MVID: 213D8A75-4B66-4004-95C2-D82C49ACB764
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\Coding4Fun.Toolkit.Controls.dll

using System.Windows.Media;


namespace Coding4Fun.Toolkit.Controls.Common
{
  public interface IImageSource
  {
    Stretch Stretch { get; set; }

    ImageSource ImageSource { get; set; }
  }
}
