// Decompiled with JetBrains decompiler
// Type: Coding4Fun.Toolkit.Controls.RectangularButton
// Assembly: Coding4Fun.Toolkit.Controls, Version=2.1.7.0, Culture=neutral, PublicKeyToken=c5fd7b72b1a17ce4
// MVID: 213D8A75-4B66-4004-95C2-D82C49ACB764
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\Coding4Fun.Toolkit.Controls.dll

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace Coding4Fun.Toolkit.Controls
{
  public class RectangularButton : ButtonBase, IAppBarButton
  {
    public static readonly DependencyProperty PressedBrushProperty = DependencyProperty.Register(nameof (PressedBrush), typeof (Brush), typeof (RectangularButton), new PropertyMetadata((object) null));
    public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(nameof (Orientation), typeof (Orientation), typeof (RectangularButton), new PropertyMetadata((object) Orientation.Vertical));
    public static readonly DependencyProperty ButtonWidthProperty = DependencyProperty.Register(nameof (ButtonWidth), typeof (double), typeof (RectangularButton), new PropertyMetadata((object) double.NaN));
    public static readonly DependencyProperty ButtonHeightProperty = DependencyProperty.Register(nameof (ButtonHeight), typeof (double), typeof (RectangularButton), new PropertyMetadata((object) double.NaN));

    public RectangularButton() => this.DefaultStyleKey = (object) typeof (RectangularButton);

    private void ApplyingTemplate()
    {
    }

    private bool IsContentEmpty(object content) => content == null;

    protected override void OnContentChanged(object oldContent, object newContent)
    {
      base.OnContentChanged(oldContent, newContent);
      if (oldContent == newContent)
        return;
      this.AppendCheck(this.Content);
      this.Dispatcher.BeginInvoke((Action) (() => ButtonBaseHelper.ApplyForegroundToFillBinding(this.GetTemplateChild("ContentBody") as ContentControl)));
    }

    private void AppendCheck(object content)
    {
      if (!this.IsContentEmpty(content))
        return;
      this.Content = (object) ButtonBaseHelper.CreateXamlCheck((FrameworkElement) this);
    }

    public override void OnApplyTemplate()
    {
      this.ApplyingTemplate();
      this.AppendCheck(this.Content);
      ButtonBaseHelper.ApplyForegroundToFillBinding(this.GetTemplateChild("ContentBody") as ContentControl);
      ButtonBaseHelper.ApplyTitleOffset(this.GetTemplateChild("ContentTitle") as ContentControl);
      base.OnApplyTemplate();
    }

    public Brush PressedBrush
    {
      get => (Brush) this.GetValue(RectangularButton.PressedBrushProperty);
      set => this.SetValue(RectangularButton.PressedBrushProperty, (object) value);
    }

    public Orientation Orientation
    {
      get => (Orientation) this.GetValue(RectangularButton.OrientationProperty);
      set => this.SetValue(RectangularButton.OrientationProperty, (object) value);
    }

    public double ButtonWidth
    {
      get => (double) this.GetValue(RectangularButton.ButtonWidthProperty);
      set => this.SetValue(RectangularButton.ButtonWidthProperty, (object) value);
    }

    public double ButtonHeight
    {
      get => (double) this.GetValue(RectangularButton.ButtonHeightProperty);
      set => this.SetValue(RectangularButton.ButtonHeightProperty, (object) value);
    }
  }
}
