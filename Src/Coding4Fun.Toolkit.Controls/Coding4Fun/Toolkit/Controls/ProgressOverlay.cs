// Decompiled with JetBrains decompiler
// Type: Coding4Fun.Toolkit.Controls.ProgressOverlay
// Assembly: Coding4Fun.Toolkit.Controls, Version=2.1.7.0, Culture=neutral, PublicKeyToken=c5fd7b72b1a17ce4
// MVID: 213D8A75-4B66-4004-95C2-D82C49ACB764
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\Coding4Fun.Toolkit.Controls.dll

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;


namespace Coding4Fun.Toolkit.Controls
{
  public class ProgressOverlay : ContentControl
  {
    private Storyboard _fadeIn;
    private Storyboard _fadeOut;
    private Grid _layoutGrid;
    private const string FadeInName = "FadeInStoryboard";
    private const string FadeOutName = "FadeOutStoryboard";
    private const string LayoutGridName = "LayoutGrid";
    public static readonly DependencyProperty ProgressControlProperty = DependencyProperty.Register(nameof (ProgressControl), typeof (object), typeof (ProgressOverlay), new PropertyMetadata((PropertyChangedCallback) null));

    public ProgressOverlay() => this.DefaultStyleKey = (object) typeof (ProgressOverlay);

    public object ProgressControl
    {
      get => this.GetValue(ProgressOverlay.ProgressControlProperty);
      set => this.SetValue(ProgressOverlay.ProgressControlProperty, value);
    }

    public override void OnApplyTemplate()
    {
      base.OnApplyTemplate();
      this._fadeIn = this.GetTemplateChild("FadeInStoryboard") as Storyboard;
      this._fadeOut = this.GetTemplateChild("FadeOutStoryboard") as Storyboard;
      this._layoutGrid = this.GetTemplateChild("LayoutGrid") as Grid;
      if (this._fadeOut == null)
        return;
      this._fadeOut.Completed += new EventHandler(this.FadeOutCompleted);
    }

    private void FadeOutCompleted(object sender, EventArgs e)
    {
      this._layoutGrid.Opacity = 1.0;
      this.Visibility = Visibility.Collapsed;
    }

    public void Show()
    {
      if (this._fadeIn == null)
        this.ApplyTemplate();
      this.Visibility = Visibility.Visible;
      if (this._fadeOut != null)
        this._fadeOut.Stop();
      if (this._fadeIn == null)
        return;
      this._fadeIn.Begin();
    }

    public void Hide()
    {
      if (this._fadeOut == null)
        this.ApplyTemplate();
      if (this._fadeIn != null)
        this._fadeIn.Stop();
      if (this._fadeOut == null)
        return;
      this._fadeOut.Begin();
    }
  }
}
