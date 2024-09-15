// Decompiled with JetBrains decompiler
// Type: Coding4Fun.Toolkit.Controls.Tile
// Assembly: Coding4Fun.Toolkit.Controls, Version=2.1.7.0, Culture=neutral, PublicKeyToken=c5fd7b72b1a17ce4
// MVID: 213D8A75-4B66-4004-95C2-D82C49ACB764
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\Coding4Fun.Toolkit.Controls.dll

using System.Windows;


namespace Coding4Fun.Toolkit.Controls
{
  public class Tile : ButtonBase
  {
    public static readonly DependencyProperty TextWrappingProperty = DependencyProperty.Register(nameof (TextWrapping), typeof (TextWrapping), typeof (Tile), new PropertyMetadata((object) TextWrapping.NoWrap));

    public Tile() => this.DefaultStyleKey = (object) typeof (Tile);

    public TextWrapping TextWrapping
    {
      get => (TextWrapping) this.GetValue(Tile.TextWrappingProperty);
      set => this.SetValue(Tile.TextWrappingProperty, (object) value);
    }
  }
}
