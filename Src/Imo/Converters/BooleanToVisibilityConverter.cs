﻿// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.Converters.BooleanToVisibilityConverter
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using System.Windows;


namespace ImoSilverlightApp.Converters
{
  public class BooleanToVisibilityConverter : BooleanConverterBase<Visibility>
  {
    public BooleanToVisibilityConverter()
      : base(Visibility.Visible, Visibility.Collapsed)
    {
    }
  }
}