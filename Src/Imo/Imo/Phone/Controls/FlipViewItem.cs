// Decompiled with JetBrains decompiler
// Type: Imo.Phone.Controls.FlipViewItem
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace Imo.Phone.Controls
{
  public class FlipViewItem : ContentControl
  {
    public static readonly DependencyProperty IsSelectedProperty = FlipView.IsSelectedProperty;

    public FlipViewItem() => this.DefaultStyleKey = (object) typeof (FlipViewItem);

    public bool IsSelected
    {
      get => (bool) this.GetValue(FlipViewItem.IsSelectedProperty);
      set => this.SetValue(FlipViewItem.IsSelectedProperty, (object) value);
    }

    internal FlipView ParentFlipView { get; set; }

    internal object Item { get; set; }

    internal void OnIsSelectedChanged(bool newValue)
    {
      if (this.ParentFlipView == null)
        return;
      this.ParentFlipView.NotifyItemSelected(this, newValue);
    }

    protected override void OnManipulationStarted(ManipulationStartedEventArgs e)
    {
      base.OnManipulationStarted(e);
      if (this.ParentFlipView != null)
        this.ParentFlipView.OnManipulationStarted((object) this, e);
      e.Handled = true;
    }

    protected override void OnManipulationDelta(ManipulationDeltaEventArgs e)
    {
      base.OnManipulationDelta(e);
      if (this.ParentFlipView != null)
        this.ParentFlipView.OnManipulationDelta((object) this, e);
      e.Handled = true;
    }

    protected override void OnManipulationCompleted(ManipulationCompletedEventArgs e)
    {
      base.OnManipulationCompleted(e);
      if (this.ParentFlipView != null)
        this.ParentFlipView.OnManipulationCompleted((object) this, e);
      e.Handled = true;
    }
  }
}
