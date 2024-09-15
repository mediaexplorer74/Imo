// Decompiled with JetBrains decompiler
// Type: Imo.Phone.Controls.FlipView
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using Microsoft.Phone.Controls.Primitives;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;


namespace Imo.Phone.Controls
{
  [TemplatePart(Name = "ScrollingHost", Type = typeof (ScrollViewer))]
  [TemplatePart(Name = "ItemsPresenter", Type = typeof (ItemsPresenter))]
  [StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof (FlipViewItem))]
  public class FlipView : TemplatedItemsControl<FlipViewItem>, ISupportInitialize
  {
    private const string ElementScrollingHostName = "ScrollingHost";
    private const string ElementItemsPresenterName = "ItemsPresenter";
    private const double MaxDraggingSquishDistance = 125.0;
    private static readonly Duration ZeroDuration = (Duration) TimeSpan.Zero;
    private static readonly Duration DefaultDuration = (Duration) TimeSpan.FromSeconds(0.4);
    private static readonly Duration UnsquishDuration = (Duration) TimeSpan.FromSeconds(0.3);
    private static readonly IEasingFunction DefaultEase = (IEasingFunction) new ExponentialEase()
    {
      Exponent = 5.0
    };
    private static readonly IEasingFunction UnsquishEase = FlipView.DefaultEase;
    private FlipView.InitializingData _initializingData;
    private bool _updatingSelection;
    private Orientation _orientation = Orientation.Horizontal;
    private Size _itemsHostSize = new Size(double.NaN, double.NaN);
    private List<FlipViewItem> _realizedItems = new List<FlipViewItem>();
    private bool _loaded;
    private bool _animating;
    private bool _hasDragDelta;
    private bool _dragging;
    private FlipView.DragLock _dragLock;
    private WeakReference _gestureSource;
    private Point _gestureOrigin;
    private ManipulationStartedEventArgs _gestureStartedEventArgs;
    private FlipView.Animator _animator;
    private int? _deferredSelectedIndex;
    private bool _suppressAnimation;
    private double? _offsetWhenDragStarted;
    private bool _squishing;
    private bool _supressHandleManipulation;
    public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register(nameof (SelectedIndex), typeof (int), typeof (FlipView), new PropertyMetadata((object) -1, (PropertyChangedCallback) ((d, e) => ((FlipView) d).OnSelectedIndexChanged((int) e.OldValue, (int) e.NewValue))));
    public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(nameof (SelectedItem), typeof (object), typeof (FlipView), new PropertyMetadata((PropertyChangedCallback) ((d, e) => ((FlipView) d).OnSelectedItemChanged(e.OldValue, e.NewValue))));
    public static readonly DependencyProperty UseTouchAnimationsForAllNavigationProperty = DependencyProperty.Register(nameof (UseTouchAnimationsForAllNavigation), typeof (bool), typeof (FlipView), new PropertyMetadata((object) true));
    public static readonly DependencyProperty UpdateSelectionModeProperty = DependencyProperty.Register(nameof (UpdateSelectionMode), typeof (UpdateSelectionMode), typeof (FlipView), (PropertyMetadata) null);
    internal static readonly DependencyProperty IsSelectedProperty = DependencyProperty.RegisterAttached("IsSelected", typeof (bool), typeof (FlipView), new PropertyMetadata(new PropertyChangedCallback(FlipView.OnIsSelectedChanged)));

    public FlipView()
    {
      this.DefaultStyleKey = (object) typeof (FlipView);
      this.Loaded += new RoutedEventHandler(this.OnLoaded);
      this.Unloaded += new RoutedEventHandler(this.OnUnloaded);
      this.SizeChanged += new SizeChangedEventHandler(this.OnSizeChanged);
    }

    public int SelectedIndex
    {
      get => (int) this.GetValue(FlipView.SelectedIndexProperty);
      set => this.SetValue(FlipView.SelectedIndexProperty, (object) value);
    }

    private void OnSelectedIndexChanged(int oldIndex, int newIndex)
    {
      if (this._updatingSelection || this.IsInit)
        return;
      if (newIndex >= -1 && newIndex < this.Items.Count)
      {
        this.UpdateSelection(oldIndex, newIndex, this.SelectedItem, this.Items[newIndex]);
      }
      else
      {
        this.SelectedIndex = oldIndex;
        throw new ArgumentOutOfRangeException("SelectedIndex");
      }
    }

    public object SelectedItem
    {
      get => this.GetValue(FlipView.SelectedItemProperty);
      set => this.SetValue(FlipView.SelectedItemProperty, value);
    }

    private void OnSelectedItemChanged(object oldValue, object newValue)
    {
      if (this._updatingSelection || this.IsInit)
        return;
      int newSelectedIndex = this.Items.IndexOf(newValue);
      if (newSelectedIndex != -1 || newValue == null && this.Items.Count == 0)
        this.UpdateSelection(this.SelectedIndex, newSelectedIndex, oldValue, newValue);
      else
        this.SelectedItem = oldValue;
    }

    public bool UseTouchAnimationsForAllNavigation
    {
      get => (bool) this.GetValue(FlipView.UseTouchAnimationsForAllNavigationProperty);
      set => this.SetValue(FlipView.UseTouchAnimationsForAllNavigationProperty, (object) value);
    }

    public UpdateSelectionMode UpdateSelectionMode
    {
      get => (UpdateSelectionMode) this.GetValue(FlipView.UpdateSelectionModeProperty);
      set => this.SetValue(FlipView.UpdateSelectionModeProperty, (object) value);
    }

    private static void OnIsSelectedChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      if (!(d is FlipViewItem flipViewItem))
        return;
      flipViewItem.OnIsSelectedChanged((bool) e.NewValue);
    }

    private Panel ItemsHost { get; set; }

    private Size ItemsHostSize
    {
      get => this._itemsHostSize;
      set
      {
        if (!(this._itemsHostSize != value))
          return;
        this._itemsHostSize = value;
        this.UpdateItemsSize();
      }
    }

    private double ItemSize
    {
      get
      {
        return this.Orientation != Orientation.Horizontal ? this.ItemsHostSize.Height : this.ItemsHostSize.Width;
      }
    }

    private ScrollViewer ElementScrollingHost { get; set; }

    private ItemsPresenter ElementItemsPresenter { get; set; }

    private Orientation Orientation
    {
      get => this._orientation;
      set
      {
        if (this._orientation == value)
          return;
        this._orientation = value;
        this.UpdateItemsSize();
      }
    }

    private bool IsInit => this._initializingData != null;

    private bool ShouldHandleManipulation
    {
      get
      {
        return this.Items.Count > 1 && this.ElementItemsPresenter != null && !this._supressHandleManipulation;
      }
    }

    private int EffectiveSelectedIndex
    {
      get => this._deferredSelectedIndex.GetValueOrDefault(this.SelectedIndex);
    }

    private double ScrollOffset
    {
      get
      {
        if (this.ElementScrollingHost == null)
          return 0.0;
        return this.Orientation != Orientation.Horizontal ? this.ElementScrollingHost.VerticalOffset : this.ElementScrollingHost.HorizontalOffset;
      }
    }

    private double TransformOffset => this._animator == null ? 0.0 : this._animator.CurrentOffset;

    public event SelectionChangedEventHandler SelectionChanged;

    public override void OnApplyTemplate()
    {
      this._animator = (FlipView.Animator) null;
      this.ItemsHost = (Panel) null;
      this.ItemsHostSize = new Size(double.NaN, double.NaN);
      if (this.ElementItemsPresenter != null)
        this.LayoutUpdated -= new EventHandler(this.OnLayoutUpdated);
      base.OnApplyTemplate();
      this.ElementScrollingHost = this.GetTemplateChild("ScrollingHost") as ScrollViewer;
      this.ElementItemsPresenter = this.GetTemplateChild("ItemsPresenter") as ItemsPresenter;
      if (this.ElementItemsPresenter == null)
        return;
      this.InitializeItemsHost();
      if (this.ItemsHost != null)
        return;
      this.LayoutUpdated += new EventHandler(this.OnLayoutUpdated);
    }

    protected override Size MeasureOverride(Size availableSize)
    {
      if (double.IsNaN(this._itemsHostSize.Width) && double.IsNaN(this._itemsHostSize.Height))
      {
        this._itemsHostSize = availableSize;
        this.UpdateItemsSize();
      }
      return base.MeasureOverride(availableSize);
    }

    protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
    {
      base.OnItemsChanged(e);
      int selectedIndex = this.SelectedIndex;
      int num = selectedIndex;
      object selectedItem = this.SelectedItem;
      object newSelectedItem = selectedItem;
      switch (e.Action)
      {
        case NotifyCollectionChangedAction.Add:
          for (int index = 0; index < e.NewItems.Count; ++index)
          {
            if (e.NewItems[index] is FlipViewItem newItem && newItem.IsSelected)
            {
              num = e.NewStartingIndex + index;
              newSelectedItem = (object) newItem;
            }
          }
          if (num == selectedIndex && e.NewStartingIndex <= selectedIndex && !this.IsInit)
          {
            num = selectedIndex + e.NewItems.Count;
            break;
          }
          break;
        case NotifyCollectionChangedAction.Remove:
          if (e.OldItems.Contains(selectedItem))
          {
            if (e.OldStartingIndex <= this.Items.Count - 1)
            {
              num = e.OldStartingIndex;
              newSelectedItem = this.Items[num];
              break;
            }
            if (this.Items.Count > 0)
            {
              num = this.Items.Count - 1;
              newSelectedItem = this.Items[num];
              break;
            }
            num = -1;
            newSelectedItem = (object) null;
            break;
          }
          if (e.OldStartingIndex + e.OldItems.Count <= selectedIndex)
          {
            num -= e.OldItems.Count;
            break;
          }
          break;
        case NotifyCollectionChangedAction.Replace:
          if (e.OldItems.Contains(selectedItem))
          {
            FlipViewItem container = this.GetContainer(selectedIndex);
            if (container != null)
            {
              container.IsSelected = true;
              break;
            }
            break;
          }
          break;
        case NotifyCollectionChangedAction.Reset:
          if (this.Items.Count > 0)
          {
            num = 0;
            newSelectedItem = this.Items[0];
            if (this.ItemTemplate == null)
            {
              for (int index = 0; index < this.Items.Count; ++index)
              {
                FlipViewItem container = this.GetContainer(index);
                if (container != null && container.IsSelected)
                {
                  num = index;
                  newSelectedItem = this.Items[index];
                }
              }
              break;
            }
            break;
          }
          num = -1;
          newSelectedItem = (object) null;
          break;
        default:
          throw new InvalidOperationException();
      }
      if (num < 0 && this.Items.Count > 0)
      {
        num = 0;
        newSelectedItem = this.Items[0];
      }
      this._suppressAnimation = true;
      this.UpdateSelection(selectedIndex, num, selectedItem, newSelectedItem);
      this._suppressAnimation = false;
      if (this._animating)
      {
        this._deferredSelectedIndex = new int?();
        this.CompleteAnimateTo();
      }
      if (this._gestureStartedEventArgs == null)
        return;
      this._supressHandleManipulation = true;
      this._gestureStartedEventArgs.Complete();
      this._supressHandleManipulation = false;
      this.GoTo(0.0);
    }

    protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
    {
      base.PrepareContainerForItemOverride(element, item);
      FlipViewItem container = (FlipViewItem) element;
      container.ParentFlipView = this;
      if (element != item)
        container.Item = item;
      int num = this.ItemContainerGenerator.IndexFromContainer(element);
      if (num != -1)
        container.IsSelected = this.SelectedIndex == num;
      this._realizedItems.Add(container);
      this.UpdateItemSize(container, this.Orientation == Orientation.Horizontal);
    }

    protected override void ClearContainerForItemOverride(DependencyObject element, object item)
    {
      base.ClearContainerForItemOverride(element, item);
      FlipViewItem flipViewItem = (FlipViewItem) element;
      if (!flipViewItem.Equals(item))
        flipViewItem.ClearValue(ContentControl.ContentProperty);
      flipViewItem.Item = (object) null;
      this._realizedItems.Remove(flipViewItem);
    }

    internal void NotifyItemSelected(FlipViewItem container, bool isSelected)
    {
      if (this._updatingSelection)
        return;
      int newSelectedIndex = this.ItemContainerGenerator.IndexFromContainer((DependencyObject) container);
      if (newSelectedIndex < 0 || newSelectedIndex >= this.Items.Count)
        return;
      object newSelectedItem = container.Item ?? (object) container;
      if (isSelected)
      {
        this.UpdateSelection(this.SelectedIndex, newSelectedIndex, this.SelectedItem, newSelectedItem);
      }
      else
      {
        if (this.SelectedIndex != newSelectedIndex)
          return;
        this.UpdateSelection(this.SelectedIndex, -1, this.SelectedItem, (object) null);
      }
    }

    private void InvokeSelectionChanged(List<object> unselectedItems, List<object> selectedItems)
    {
      this.OnSelectionChanged(new SelectionChangedEventArgs((IList) unselectedItems, (IList) selectedItems));
    }

    private void OnSelectionChanged(SelectionChangedEventArgs e)
    {
      if (this.SelectionChanged == null)
        return;
      this.SelectionChanged((object) this, e);
    }

    private FlipViewItem GetContainer(int index)
    {
      if (index < 0 || this.Items.Count <= index)
        return (FlipViewItem) null;
      return this.Items[index] is FlipViewItem flipViewItem ? flipViewItem : this.ItemContainerGenerator.ContainerFromIndex(index) as FlipViewItem;
    }

    private void SetItemIsSelected(object item, bool value)
    {
      if (!(item is FlipViewItem flipViewItem1))
        flipViewItem1 = this.ItemContainerGenerator.ContainerFromItem(item) as FlipViewItem;
      FlipViewItem flipViewItem2 = flipViewItem1;
      if (flipViewItem2 == null)
        return;
      flipViewItem2.IsSelected = value;
    }

    private void UpdateSelection(
      int oldSelectedIndex,
      int newSelectedIndex,
      object oldSelectedItem,
      object newSelectedItem)
    {
      int num1 = newSelectedIndex - oldSelectedIndex;
      bool flag1 = !InternalUtils.AreValuesEqual(oldSelectedItem, newSelectedItem);
      if (num1 == 0 && !flag1)
        return;
      int num2;
      if (((!this._loaded || !this.UseTouchAnimationsForAllNavigation || this.Items.Count <= 1 || oldSelectedIndex == -1 ? 0 : (Math.Abs(num1) == 1 ? 1 : 0)) & (flag1 ? 1 : 0)) != 0)
      {
        if (this._animating && this._deferredSelectedIndex.HasValue)
        {
          int? deferredSelectedIndex = this._deferredSelectedIndex;
          int num3 = newSelectedIndex;
          if ((deferredSelectedIndex.GetValueOrDefault() == num3 ? (deferredSelectedIndex.HasValue ? 1 : 0) : 0) != 0)
            goto label_6;
        }
        num2 = !this._suppressAnimation ? 1 : 0;
        goto label_7;
      }
label_6:
      num2 = 0;
label_7:
      bool flag2 = num2 != 0;
      try
      {
        this._updatingSelection = true;
        if (newSelectedIndex < 0 && this.Items.Count > 0)
        {
          newSelectedIndex = 0;
          newSelectedItem = this.Items[newSelectedIndex];
        }
        this.SelectedIndex = newSelectedIndex;
        this.SelectedItem = newSelectedItem;
        if (flag1)
        {
          List<object> unselectedItems = new List<object>();
          List<object> selectedItems = new List<object>();
          if (oldSelectedItem != null)
          {
            this.SetItemIsSelected(oldSelectedItem, false);
            unselectedItems.Add(oldSelectedItem);
          }
          if (newSelectedItem != null)
          {
            this.SetItemIsSelected(newSelectedItem, true);
            selectedItems.Add(newSelectedItem);
          }
          this.InvokeSelectionChanged(unselectedItems, selectedItems);
        }
      }
      finally
      {
        this._updatingSelection = false;
      }
      if (flag2)
      {
        this.AnimateTo(this.SelectedIndex, false);
      }
      else
      {
        if (this._hasDragDelta)
          return;
        if (this._animating)
        {
          if (this.UseTouchAnimationsForAllNavigation)
            return;
          this._deferredSelectedIndex = new int?();
          this.CompleteAnimateTo();
        }
        else
          this.ScrollSelectionIntoView();
      }
    }

    private void UpdateItemsHostSize()
    {
      this.ItemsHostSize = new Size(this.ItemsHost.ActualWidth, this.ItemsHost.ActualHeight);
    }

    private void UpdateItemsSize()
    {
      bool horizontal = this.Orientation == Orientation.Horizontal;
      foreach (FlipViewItem realizedItem in this._realizedItems)
        this.UpdateItemSize(realizedItem, horizontal);
    }

    private void UpdateItemSize(FlipViewItem container, bool horizontal)
    {
      if (horizontal)
      {
        container.Width = this._itemsHostSize.Width;
        container.ClearValue(FrameworkElement.HeightProperty);
      }
      else
      {
        container.ClearValue(FrameworkElement.WidthProperty);
        container.Height = this._itemsHostSize.Height;
      }
    }

    private void InitializeItemsHost()
    {
      this.ItemsHost = TemplatedVisualTreeExtensions.GetFirstLogicalChildByType<Panel>(this.ElementItemsPresenter, false);
      if (this.ItemsHost == null)
        return;
      this.UpdateItemsHostSize();
      this.ItemsHost.SizeChanged += new SizeChangedEventHandler(this.OnItemsHostSizeChanged);
      this.Orientation = this.ItemsHost is VirtualizingStackPanel itemsHost ? itemsHost.Orientation : throw new InvalidOperationException("FlipView_NotAllowedItemsPanel");
      this.ScrollSelectionIntoView();
    }

    private void OnItemsHostSizeChanged(object sender, SizeChangedEventArgs e)
    {
      this.UpdateItemsHostSize();
    }

    private void OnLayoutUpdated(object sender, EventArgs e)
    {
      this.InitializeItemsHost();
      if (this.ItemsHost == null)
        return;
      this.LayoutUpdated -= new EventHandler(this.OnLayoutUpdated);
    }

    private void ScrollSelectionIntoView()
    {
      int effectiveSelectedIndex = this.EffectiveSelectedIndex;
      if (this.ItemsHost == null || this.ElementScrollingHost == null || !this._loaded || effectiveSelectedIndex < 0 || (double) effectiveSelectedIndex == this.ScrollOffset)
        return;
      this.ElementScrollingHost.UpdateLayout();
      if (this.Orientation == Orientation.Horizontal)
        this.ElementScrollingHost.ScrollToHorizontalOffset((double) effectiveSelectedIndex);
      else
        this.ElementScrollingHost.ScrollToVerticalOffset((double) effectiveSelectedIndex);
      this.ElementScrollingHost.UpdateLayout();
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
      this._loaded = true;
      this.ScrollSelectionIntoView();
    }

    private void OnUnloaded(object sender, RoutedEventArgs e) => this._loaded = false;

    private void OnSizeChanged(object sender, SizeChangedEventArgs e)
    {
      this.ScrollSelectionIntoView();
    }

    internal void OnManipulationStarted(object sender, ManipulationStartedEventArgs e)
    {
      this._gestureSource = new WeakReference((object) e.ManipulationContainer);
      this._gestureOrigin = e.ManipulationOrigin;
      this._gestureStartedEventArgs = e;
      this._dragLock = FlipView.DragLock.Unset;
      this._dragging = false;
    }

    internal void OnManipulationDelta(object sender, ManipulationDeltaEventArgs e)
    {
      if (!this._dragging)
        this.ReleaseMouseCaptureAtGestureOrigin();
      this._dragging = true;
      if (this._dragLock == FlipView.DragLock.Unset)
      {
        Point translation = e.CumulativeManipulation.Translation;
        double x = translation.X;
        translation = e.CumulativeManipulation.Translation;
        double y = translation.Y;
        double num = FlipView.AngleFromVector(x, y) % 180.0;
        this._dragLock = num <= 45.0 || num >= 135.0 ? FlipView.DragLock.Horizontal : FlipView.DragLock.Vertical;
      }
      e.Handled = true;
      if (!this.HasDragDelta(e.DeltaManipulation))
        return;
      this.Drag(e);
    }

    internal void OnManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
    {
      this._hasDragDelta = this.HasDragDelta(e.TotalManipulation);
      System.Windows.Input.ManipulationDelta totalManipulation = (System.Windows.Input.ManipulationDelta) null;
      this._gestureStartedEventArgs = (ManipulationStartedEventArgs) null;
      this._dragLock = FlipView.DragLock.Unset;
      this._dragging = false;
      if (this._hasDragDelta)
      {
        if (e.IsInertial)
        {
          double angle = FlipView.AngleFromVector(e.FinalVelocities.LinearVelocity.X, e.FinalVelocities.LinearVelocity.Y);
          if (this.Orientation == Orientation.Vertical)
          {
            angle -= 90.0;
            if (angle < 0.0)
              angle += 360.0;
          }
          if (angle <= 45.0 || angle >= 315.0)
            angle = 0.0;
          else if (angle >= 135.0 && angle <= 225.0)
            angle = 180.0;
          this.ReleaseMouseCaptureAtGestureOrigin();
          this.Flick(angle);
          if (angle == 0.0 || angle == 180.0)
            e.Handled = true;
        }
        else
        {
          totalManipulation = e.TotalManipulation;
          e.Handled = true;
        }
      }
      this.GesturesComplete(totalManipulation);
    }

    private void Drag(ManipulationDeltaEventArgs e)
    {
      this._hasDragDelta = this.HasDragDelta(e.CumulativeManipulation);
      if (!this.ShouldHandleManipulation)
        return;
      if (this._animating)
      {
        this._animating = false;
        double scrollOffset = this.ScrollOffset;
        double transformOffset = this.TransformOffset;
        this.ScrollSelectionIntoView();
        if (!this._offsetWhenDragStarted.HasValue)
          this._offsetWhenDragStarted = new double?((this.ScrollOffset - scrollOffset) * this.ItemSize + transformOffset);
      }
      double targetOffset = this.Orientation == Orientation.Horizontal ? e.CumulativeManipulation.Translation.X : e.CumulativeManipulation.Translation.Y;
      if (this._offsetWhenDragStarted.HasValue)
        targetOffset += this._offsetWhenDragStarted.Value;
      this._squishing = false;
      if (this.EffectiveSelectedIndex <= 0)
      {
        if (targetOffset > 125.0)
          targetOffset = 125.0;
        if (targetOffset > 0.0)
          this._squishing = true;
      }
      else if (this.EffectiveSelectedIndex >= this.Items.Count - 1)
      {
        if (targetOffset < -125.0)
          targetOffset = -125.0;
        if (targetOffset < 0.0)
          this._squishing = true;
      }
      this.GoTo(targetOffset);
    }

    private void Flick(double angle)
    {
      if (!this.ShouldHandleManipulation)
        return;
      int num = (int) angle;
      switch (num)
      {
        case 0:
        case 180:
          this.AnimateTo(this.EffectiveSelectedIndex + (num == 180 ? 1 : -1));
          break;
      }
    }

    private void GesturesComplete(System.Windows.Input.ManipulationDelta totalManipulation)
    {
      if (this.ShouldHandleManipulation && this._hasDragDelta)
      {
        if (totalManipulation != null)
          this.AnimateTo((int) Math.Round(this.ScrollOffset - this.TransformOffset / this.ItemSize));
        else if (!this._animating)
          this.AnimateTo(this.EffectiveSelectedIndex);
      }
      this._hasDragDelta = false;
      this._offsetWhenDragStarted = new double?();
      this._squishing = false;
    }

    private void ReleaseMouseCaptureAtGestureOrigin()
    {
      if (this._gestureSource == null)
        return;
      if (!(this._gestureSource.Target is FrameworkElement target))
        return;
      try
      {
        foreach (UIElement inHostCoordinate in VisualTreeHelper.FindElementsInHostCoordinates(target.TransformToVisual((UIElement) null).Transform(this._gestureOrigin), Application.Current.RootVisual))
          inHostCoordinate.ReleaseMouseCapture();
      }
      catch (ArgumentException ex)
      {
      }
    }

    private bool HasDragDelta(System.Windows.Input.ManipulationDelta manipulation)
    {
      Point translation;
      if (this._dragLock == FlipView.DragLock.Horizontal)
      {
        translation = manipulation.Translation;
        if (translation.X != 0.0 && this.Orientation == Orientation.Horizontal)
          return true;
      }
      if (this._dragLock == FlipView.DragLock.Vertical)
      {
        translation = manipulation.Translation;
        if (translation.Y != 0.0)
          return this.Orientation == Orientation.Vertical;
      }
      return false;
    }

    private void AnimateTo(int index, bool changeIndex = true)
    {
      if (this._suppressAnimation)
        return;
      double? nullable1 = new double?();
      double? nullable2 = new double?();
      if (this._animating && this.ValidateIndex(index))
      {
        nullable1 = new double?(this.ScrollOffset);
        nullable2 = new double?(this.TransformOffset);
        this._deferredSelectedIndex = new int?();
        this.CompleteAnimateTo();
      }
      if (changeIndex && !this.ValidateIndex(index))
        return;
      if (changeIndex && this.UpdateSelectionMode == UpdateSelectionMode.BeforeTransition)
      {
        changeIndex = false;
        this._suppressAnimation = true;
        this.SelectedIndex = index;
        this._suppressAnimation = false;
      }
      this._animating = true;
      if (changeIndex)
        this._deferredSelectedIndex = new int?(index);
      if (nullable1.HasValue && nullable2.HasValue)
        this.GoTo((this.ScrollOffset - nullable1.Value) * this.ItemSize + nullable2.Value);
      Duration duration;
      IEasingFunction easingFunction;
      if (this._squishing)
      {
        duration = FlipView.UnsquishDuration;
        easingFunction = FlipView.UnsquishEase;
      }
      else
      {
        duration = FlipView.DefaultDuration;
        easingFunction = FlipView.DefaultEase;
      }
      this.GoTo((this.ScrollOffset - (double) this.EffectiveSelectedIndex) * this.ItemSize, duration, easingFunction, new Action(this.CompleteAnimateTo));
    }

    private void CompleteAnimateTo()
    {
      int? deferredSelectedIndex = this._deferredSelectedIndex;
      this._animating = false;
      this._deferredSelectedIndex = new int?();
      if (deferredSelectedIndex.HasValue)
      {
        this._suppressAnimation = true;
        this.SelectedIndex = deferredSelectedIndex.Value;
        this._suppressAnimation = false;
      }
      this.ScrollSelectionIntoView();
      this.GoTo(0.0);
    }

    private bool ValidateIndex(int index) => index >= 0 && index <= this.Items.Count - 1;

    private void GoTo(double targetOffset)
    {
      this.GoTo(targetOffset, FlipView.ZeroDuration, (IEasingFunction) null, (Action) null);
    }

    private void GoTo(
      double targetOffset,
      Duration duration,
      IEasingFunction easingFunction,
      Action completionAction)
    {
      if (!FlipView.Animator.TryEnsureAnimator((FrameworkElement) this.ElementItemsPresenter, this.Orientation, ref this._animator))
        return;
      this._animator.GoTo(targetOffset, duration, easingFunction, completionAction);
    }

    private static double AngleFromVector(double x, double y)
    {
      double num = Math.Atan2(y, x);
      if (num < 0.0)
        num = 2.0 * Math.PI + num;
      return num * 360.0 / (2.0 * Math.PI);
    }

    void ISupportInitialize.BeginInit()
    {
      this._initializingData = new FlipView.InitializingData()
      {
        InitialItem = this.SelectedItem,
        InitialIndex = this.SelectedIndex
      };
    }

    void ISupportInitialize.EndInit()
    {
      if (this._initializingData == null)
        throw new InvalidOperationException();
      int selectedIndex = this.SelectedIndex;
      object selectedItem = this.SelectedItem;
      if (this._initializingData.InitialIndex != selectedIndex)
      {
        this.SelectedIndex = this._initializingData.InitialIndex;
        this._initializingData = (FlipView.InitializingData) null;
        this.SelectedIndex = selectedIndex;
      }
      else if (this._initializingData.InitialItem != selectedItem)
      {
        this.SelectedItem = this._initializingData.InitialItem;
        this._initializingData = (FlipView.InitializingData) null;
        this.SelectedItem = selectedItem;
      }
      this._initializingData = (FlipView.InitializingData) null;
    }

    private class Animator
    {
      private static readonly PropertyPath TranslateXPropertyPath = new PropertyPath((object) CompositeTransform.TranslateXProperty);
      private static readonly PropertyPath TranslateYPropertyPath = new PropertyPath((object) CompositeTransform.TranslateYProperty);
      private readonly Storyboard _sbRunning = new Storyboard();
      private readonly DoubleAnimation _daRunning = new DoubleAnimation();
      private readonly Orientation _orientation;
      private CompositeTransform _transform;
      private Action _oneTimeAction;

      public Animator(CompositeTransform compositeTransform, Orientation orientation)
      {
        this._transform = compositeTransform;
        this._orientation = orientation;
        this._sbRunning.Completed += new EventHandler(this.OnCompleted);
        this._sbRunning.Children.Add((Timeline) this._daRunning);
        Storyboard.SetTarget((Timeline) this._daRunning, (DependencyObject) this._transform);
        Storyboard.SetTargetProperty((Timeline) this._daRunning, this._orientation == Orientation.Horizontal ? FlipView.Animator.TranslateXPropertyPath : FlipView.Animator.TranslateYPropertyPath);
      }

      public double CurrentOffset
      {
        get
        {
          return this._orientation != Orientation.Horizontal ? this._transform.TranslateY : this._transform.TranslateX;
        }
      }

      public Orientation Orientation => this._orientation;

      public void GoTo(double targetOffset, Duration duration)
      {
        this.GoTo(targetOffset, duration, (IEasingFunction) null, (Action) null);
      }

      public void GoTo(
        double targetOffset,
        Duration duration,
        IEasingFunction easingFunction,
        Action completionAction)
      {
        this._daRunning.To = new double?(targetOffset);
        this._daRunning.Duration = duration;
        this._daRunning.EasingFunction = easingFunction;
        this._sbRunning.Begin();
        this._sbRunning.SeekAlignedToLastTick(TimeSpan.Zero);
        this._oneTimeAction = completionAction;
      }

      private void OnCompleted(object sender, EventArgs e)
      {
        Action oneTimeAction = this._oneTimeAction;
        if (oneTimeAction == null || this._sbRunning.GetCurrentState() == ClockState.Active)
          return;
        this._oneTimeAction = (Action) null;
        oneTimeAction();
      }

      public static bool TryEnsureAnimator(
        FrameworkElement targetElement,
        Orientation orientation,
        ref FlipView.Animator animator)
      {
        if (animator == null || animator.Orientation != orientation)
        {
          CompositeTransform compositeTransform = FlipView.Animator.GetCompositeTransform((UIElement) targetElement);
          if (compositeTransform != null)
          {
            animator = new FlipView.Animator(compositeTransform, orientation);
          }
          else
          {
            animator = (FlipView.Animator) null;
            return false;
          }
        }
        return true;
      }

      public static CompositeTransform GetCompositeTransform(UIElement container)
      {
        if (container == null)
          return (CompositeTransform) null;
        if (!(container.RenderTransform is CompositeTransform compositeTransform))
        {
          compositeTransform = new CompositeTransform();
          container.RenderTransform = (Transform) compositeTransform;
        }
        return compositeTransform;
      }
    }

    private class InitializingData
    {
      public int InitialIndex;
      public object InitialItem;
    }

    private enum DragLock
    {
      Unset,
      Free,
      Vertical,
      Horizontal,
    }
  }
}
