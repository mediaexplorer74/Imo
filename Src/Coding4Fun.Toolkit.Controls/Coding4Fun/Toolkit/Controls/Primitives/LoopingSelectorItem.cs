// Decompiled with JetBrains decompiler
// Type: Coding4Fun.Toolkit.Controls.Primitives.LoopingSelectorItem
// Assembly: Coding4Fun.Toolkit.Controls, Version=2.1.7.0, Culture=neutral, PublicKeyToken=c5fd7b72b1a17ce4
// MVID: 213D8A75-4B66-4004-95C2-D82C49ACB764
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\Coding4Fun.Toolkit.Controls.dll

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;


namespace Coding4Fun.Toolkit.Controls.Primitives
{
  [TemplatePart(Name = "Transform", Type = typeof (TranslateTransform))]
  [TemplateVisualState(GroupName = "Common", Name = "Normal")]
  [TemplateVisualState(GroupName = "Common", Name = "Expanded")]
  [TemplateVisualState(GroupName = "Common", Name = "Selected")]
  public class LoopingSelectorItem : ContentControl
  {
    private const string TransformPartName = "Transform";
    private const string CommonGroupName = "Common";
    private const string NormalStateName = "Normal";
    private const string ExpandedStateName = "Expanded";
    private const string SelectedStateName = "Selected";
    private bool _shouldClick;
    private LoopingSelectorItem.State _state;

    public LoopingSelectorItem()
    {
      this.DefaultStyleKey = (object) typeof (LoopingSelectorItem);
      this.MouseLeftButtonDown += new MouseButtonEventHandler(this.LoopingSelectorItem_MouseLeftButtonDown);
      this.MouseLeftButtonUp += new MouseButtonEventHandler(this.LoopingSelectorItem_MouseLeftButtonUp);
      this.LostMouseCapture += new MouseEventHandler(this.LoopingSelectorItem_LostMouseCapture);
      this.Tap += new EventHandler<GestureEventArgs>(this.LoopingSelectorItem_Tap);
    }

    internal void SetState(LoopingSelectorItem.State newState, bool useTransitions)
    {
      if (this._state == newState)
        return;
      this._state = newState;
      switch (this._state)
      {
        case LoopingSelectorItem.State.Normal:
          VisualStateManager.GoToState((Control) this, "Normal", useTransitions);
          break;
        case LoopingSelectorItem.State.Expanded:
          VisualStateManager.GoToState((Control) this, "Expanded", useTransitions);
          break;
        case LoopingSelectorItem.State.Selected:
          VisualStateManager.GoToState((Control) this, "Selected", useTransitions);
          break;
      }
    }

    internal LoopingSelectorItem.State GetState() => this._state;

    private void LoopingSelectorItem_Tap(object sender, GestureEventArgs e) => e.Handled = true;

    private void LoopingSelectorItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      this.CaptureMouse();
      this._shouldClick = true;
    }

    private void LoopingSelectorItem_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
      this.ReleaseMouseCapture();
      if (!this._shouldClick)
        return;
      this._shouldClick = false;
      SafeRaise.Raise(this.Click, (object) this);
    }

    private void LoopingSelectorItem_LostMouseCapture(object sender, MouseEventArgs e)
    {
      this._shouldClick = false;
    }

    public event EventHandler<EventArgs> Click;

    public override void OnApplyTemplate()
    {
      base.OnApplyTemplate();
      if (!(this.GetTemplateChild("Transform") is TranslateTransform translateTransform))
        translateTransform = new TranslateTransform();
      this.Transform = translateTransform;
    }

    internal LoopingSelectorItem Previous { get; private set; }

    internal LoopingSelectorItem Next { get; private set; }

    internal void Remove()
    {
      if (this.Previous != null)
        this.Previous.Next = this.Next;
      if (this.Next != null)
        this.Next.Previous = this.Previous;
      this.Next = this.Previous = (LoopingSelectorItem) null;
    }

    internal void InsertAfter(LoopingSelectorItem after)
    {
      this.Next = after.Next;
      this.Previous = after;
      if (after.Next != null)
        after.Next.Previous = this;
      after.Next = this;
    }

    internal void InsertBefore(LoopingSelectorItem before)
    {
      this.Next = before;
      this.Previous = before.Previous;
      if (before.Previous != null)
        before.Previous.Next = this;
      before.Previous = this;
    }

    internal TranslateTransform Transform { get; private set; }

    internal enum State
    {
      Normal,
      Expanded,
      Selected,
    }
  }
}
