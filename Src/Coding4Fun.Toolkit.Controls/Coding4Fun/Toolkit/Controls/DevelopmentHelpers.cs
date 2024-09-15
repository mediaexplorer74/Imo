// Decompiled with JetBrains decompiler
// Type: Coding4Fun.Toolkit.Controls.DevelopmentHelpers
// Assembly: Coding4Fun.Toolkit.Controls, Version=2.1.7.0, Culture=neutral, PublicKeyToken=c5fd7b72b1a17ce4
// MVID: 213D8A75-4B66-4004-95C2-D82C49ACB764
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\Coding4Fun.Toolkit.Controls.dll

using Coding4Fun.Toolkit.Controls.Common;
using System;


namespace Coding4Fun.Toolkit.Controls
{
  public static class DevelopmentHelpers
  {
    [Obsolete("Moved to Coding4Fun.Toolkit.Controls.Common.ApplicationSpace")]
    public static bool IsDesignMode => ApplicationSpace.IsDesignMode;

    [Obsolete("Moved to Coding4Fun.Toolkit.dll now, Namespace is System")]
    public static bool IsTypeOf(this object target, Type type)
    {
      return TypeExtensions.IsTypeOf(target, type);
    }

    [Obsolete("Moved to Coding4Fun.Toolkit.dll now, Namespace is System")]
    public static bool IsTypeOf(this object target, object referenceObject)
    {
      return TypeExtensions.IsTypeOf(target, referenceObject);
    }
  }
}
