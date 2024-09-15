// Decompiled with JetBrains decompiler
// Type: Coding4Fun.Toolkit.Controls.RoundButton
// Assembly: Coding4Fun.Toolkit.Controls, Version=2.1.7.0, Culture=neutral, PublicKeyToken=c5fd7b72b1a17ce4
// MVID: 213D8A75-4B66-4004-95C2-D82C49ACB764
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\Coding4Fun.Toolkit.Controls.dll

using Coding4Fun.Toolkit.Controls.Common;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace Coding4Fun.Toolkit.Controls
{
  public class RoundButton : ButtonBase, IImageSource, IAppBarButton
  {
    private Grid _hostContainer;
    private FrameworkElement _contentBody;
    public static readonly DependencyProperty StretchProperty = DependencyProperty.Register(nameof (Stretch), typeof (Stretch), typeof (RoundButton), new PropertyMetadata((object) Stretch.None, new PropertyChangedCallback(RoundButton.OnUpdate)));
    public static readonly DependencyProperty ImageSourceProperty = DependencyProperty.Register(nameof (ImageSource), typeof (ImageSource), typeof (RoundButton), new PropertyMetadata((object) null, new PropertyChangedCallback(RoundButton.OnUpdate)));
    public static readonly DependencyProperty PressedBrushProperty = DependencyProperty.Register(nameof (PressedBrush), typeof (Brush), typeof (RoundButton), new PropertyMetadata((object) null));
    public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(nameof (Orientation), typeof (Orientation), typeof (RoundButton), new PropertyMetadata((object) Orientation.Vertical));
    public static readonly DependencyProperty ButtonWidthProperty = DependencyProperty.Register(nameof (ButtonWidth), typeof (double), typeof (RoundButton), new PropertyMetadata((object) double.NaN));
    public static readonly DependencyProperty ButtonHeightProperty = DependencyProperty.Register(nameof (ButtonHeight), typeof (double), typeof (RoundButton), new PropertyMetadata((object) double.NaN));

    private bool IsContentEmpty(object content) => content == null && this.ImageSource == null;

    private void ApplyingTemplate()
    {
      this._hostContainer = this.GetTemplateChild("ContentHost") as Grid;
      this._contentBody = this.GetTemplateChild("ContentBody") as FrameworkElement;
      ButtonBaseHelper.UpdateImageSource(this._contentBody, this._hostContainer, this.ImageSource, this.Stretch);
    }

    public Stretch Stretch
    {
      get => (Stretch) this.GetValue(RoundButton.StretchProperty);
      set => this.SetValue(RoundButton.StretchProperty, (object) value);
    }

    public ImageSource ImageSource
    {
      get => (ImageSource) this.GetValue(RoundButton.ImageSourceProperty);
      set => this.SetValue(RoundButton.ImageSourceProperty, (object) value);
    }

    private static void OnUpdate(DependencyObject o, DependencyPropertyChangedEventArgs e)
    {
      if (!(o is RoundButton roundButton))
        return;
      roundButton.UpdateImageSource();
    }

    private void UpdateImageSource()
    {
      ButtonBaseHelper.UpdateImageSource(this._contentBody, this._hostContainer, this.ImageSource, this.Stretch);
    }

    public RoundButton() => this.DefaultStyleKey = (object) typeof (RoundButton);

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
      get => (Brush) this.GetValue(RoundButton.PressedBrushProperty);
      set => this.SetValue(RoundButton.PressedBrushProperty, (object) value);
    }

    public Orientation Orientation
    {
      get => (Orientation) this.GetValue(RoundButton.OrientationProperty);
      set => this.SetValue(RoundButton.OrientationProperty, (object) value);
    }

    public double ButtonWidth
    {
      get => (double) this.GetValue(RoundButton.ButtonWidthProperty);
      set => this.SetValue(RoundButton.ButtonWidthProperty, (object) value);
    }

    public double ButtonHeight
    {
      get => (double) this.GetValue(RoundButton.ButtonHeightProperty);
      set => this.SetValue(RoundButton.ButtonHeightProperty, (object) value);
    }
  }
}
