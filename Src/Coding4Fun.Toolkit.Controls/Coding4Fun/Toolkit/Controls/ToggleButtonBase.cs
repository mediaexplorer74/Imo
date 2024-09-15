// Decompiled with JetBrains decompiler
// Type: Coding4Fun.Toolkit.Controls.ToggleButtonBase
// Assembly: Coding4Fun.Toolkit.Controls, Version=2.1.7.0, Culture=neutral, PublicKeyToken=c5fd7b72b1a17ce4
// MVID: 213D8A75-4B66-4004-95C2-D82C49ACB764
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\Coding4Fun.Toolkit.Controls.dll

using Coding4Fun.Toolkit.Controls.Common;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace Coding4Fun.Toolkit.Controls
{
  public abstract class ToggleButtonBase : CheckBox, IImageSource, IButtonBase, IAppBarButton
  {
    public static readonly DependencyProperty StretchProperty = DependencyProperty.Register(nameof (Stretch), typeof (Stretch), typeof (ToggleButtonBase), new PropertyMetadata((object) Stretch.None, new PropertyChangedCallback(ToggleButtonBase.OnUpdate)));
    public static readonly DependencyProperty ImageSourceProperty = DependencyProperty.Register(nameof (ImageSource), typeof (ImageSource), typeof (ToggleButtonBase), new PropertyMetadata((object) null, new PropertyChangedCallback(ToggleButtonBase.OnUpdate)));
    public static readonly DependencyProperty LabelProperty = DependencyProperty.Register(nameof (Label), typeof (object), typeof (ToggleButtonBase), new PropertyMetadata((object) string.Empty));
    public static readonly DependencyProperty CheckedBrushProperty = DependencyProperty.Register(nameof (CheckedBrush), typeof (Brush), typeof (ToggleButtonBase), new PropertyMetadata((object) null));
    public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(nameof (Orientation), typeof (Orientation), typeof (ToggleButtonBase), new PropertyMetadata((object) Orientation.Vertical));
    public static readonly DependencyProperty ButtonWidthProperty = DependencyProperty.Register(nameof (ButtonWidth), typeof (double), typeof (ToggleButtonBase), new PropertyMetadata((object) double.NaN));
    public static readonly DependencyProperty ButtonHeightProperty = DependencyProperty.Register(nameof (ButtonHeight), typeof (double), typeof (ToggleButtonBase), new PropertyMetadata((object) double.NaN));

    private bool IsContentEmpty(object content) => content == null && this.ImageSource == null;

    private void ApplyingTemplate() => this.UpdateImageSource();

    public Stretch Stretch
    {
      get => (Stretch) this.GetValue(ToggleButtonBase.StretchProperty);
      set => this.SetValue(ToggleButtonBase.StretchProperty, (object) value);
    }

    public ImageSource ImageSource
    {
      get => (ImageSource) this.GetValue(ToggleButtonBase.ImageSourceProperty);
      set => this.SetValue(ToggleButtonBase.ImageSourceProperty, (object) value);
    }

    private static void OnUpdate(DependencyObject o, DependencyPropertyChangedEventArgs e)
    {
      if (!(o is ToggleButtonBase toggleButtonBase))
        return;
      toggleButtonBase.UpdateImageSource();
    }

    private void UpdateImageSource()
    {
      Grid templateChild1 = this.GetTemplateChild("ContentHost") as Grid;
      Grid templateChild2 = this.GetTemplateChild("DisabledContentHost") as Grid;
      FrameworkElement templateChild3 = this.GetTemplateChild("EnabledContent") as FrameworkElement;
      FrameworkElement templateChild4 = this.GetTemplateChild("DisabledContent") as FrameworkElement;
      Grid hostBody = templateChild1;
      ImageSource imageSource = this.ImageSource;
      int stretch = (int) this.Stretch;
      ButtonBaseHelper.UpdateImageSource(templateChild3, hostBody, imageSource, (Stretch) stretch);
      ButtonBaseHelper.UpdateImageSource(templateChild4, templateChild2, this.ImageSource, this.Stretch);
    }

    protected ToggleButtonBase()
    {
      this.IsEnabledChanged += new DependencyPropertyChangedEventHandler(this.IsEnabledStateChanged);
    }

    private void IsEnabledStateChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      this.IsEnabledStateChanged();
    }

    private void IsEnabledStateChanged()
    {
      ContentControl templateChild1 = this.GetTemplateChild("ContentBody") as ContentControl;
      Grid templateChild2 = this.GetTemplateChild("EnabledHolder") as Grid;
      Grid templateChild3 = this.GetTemplateChild("DisabledHolder") as Grid;
      if (templateChild1 != null && templateChild3 != null && templateChild2 != null)
      {
        if (!this.IsEnabled)
          templateChild2.Children.Remove((UIElement) templateChild1);
        else
          templateChild3.Children.Remove((UIElement) templateChild1);
        if (this.IsEnabled)
        {
          if (!templateChild2.Children.Contains((UIElement) templateChild1))
            templateChild2.Children.Insert(0, (UIElement) templateChild1);
        }
        else if (!templateChild3.Children.Contains((UIElement) templateChild1))
          templateChild3.Children.Insert(0, (UIElement) templateChild1);
      }
      this.UpdateLayout();
      ButtonBaseHelper.ApplyForegroundToFillBinding(templateChild1);
    }

    protected override void OnContentChanged(object oldContent, object newContent)
    {
      base.OnContentChanged(oldContent, newContent);
      if (oldContent == newContent)
        return;
      this.AppendCheck(this.Content);
      this.IsEnabledStateChanged();
    }

    private void AppendCheck(object content)
    {
      if (!this.IsContentEmpty(content))
        return;
      this.Content = (object) ButtonBaseHelper.CreateXamlCheck((FrameworkElement) this);
    }

    public override void OnApplyTemplate()
    {
      base.OnApplyTemplate();
      this.ApplyingTemplate();
      this.AppendCheck(this.Content);
      this.IsEnabledStateChanged();
      ButtonBaseHelper.ApplyTitleOffset(this.GetTemplateChild("ContentTitle") as ContentControl);
    }

    public object Label
    {
      get => this.GetValue(ToggleButtonBase.LabelProperty);
      set => this.SetValue(ToggleButtonBase.LabelProperty, value);
    }

    public Brush CheckedBrush
    {
      get => (Brush) this.GetValue(ToggleButtonBase.CheckedBrushProperty);
      set => this.SetValue(ToggleButtonBase.CheckedBrushProperty, (object) value);
    }

    public Orientation Orientation
    {
      get => (Orientation) this.GetValue(ToggleButtonBase.OrientationProperty);
      set => this.SetValue(ToggleButtonBase.OrientationProperty, (object) value);
    }

    public double ButtonWidth
    {
      get => (double) this.GetValue(ToggleButtonBase.ButtonWidthProperty);
      set => this.SetValue(ToggleButtonBase.ButtonWidthProperty, (object) value);
    }

    public double ButtonHeight
    {
      get => (double) this.GetValue(ToggleButtonBase.ButtonHeightProperty);
      set => this.SetValue(ToggleButtonBase.ButtonHeightProperty, (object) value);
    }
  }
}
