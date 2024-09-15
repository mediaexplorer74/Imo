// Decompiled with JetBrains decompiler
// Type: Coding4Fun.Toolkit.Controls.Primitives.LoopingSelector
// Assembly: Coding4Fun.Toolkit.Controls, Version=2.1.7.0, Culture=neutral, PublicKeyToken=c5fd7b72b1a17ce4
// MVID: 213D8A75-4B66-4004-95C2-D82C49ACB764
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\Coding4Fun.Toolkit.Controls.dll

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;


namespace Coding4Fun.Toolkit.Controls.Primitives
{
  [TemplatePart(Name = "ItemsPanel", Type = typeof (Panel))]
  [TemplatePart(Name = "CenteringTransform", Type = typeof (TranslateTransform))]
  [TemplatePart(Name = "PanningTransform", Type = typeof (TranslateTransform))]
  public class LoopingSelector : Control
  {
    private const string ItemsPanelName = "ItemsPanel";
    private const string CenteringTransformName = "CenteringTransform";
    private const string PanningTransformName = "PanningTransform";
    private const double DragSensitivity = 12.0;
    private static readonly Duration _selectDuration = new Duration(TimeSpan.FromMilliseconds(500.0));
    private readonly IEasingFunction _selectEase;
    private static readonly Duration _panDuration = new Duration(TimeSpan.FromMilliseconds(100.0));
    private readonly IEasingFunction _panEase;
    private DoubleAnimation _panelAnimation;
    private Storyboard _panelStoryboard;
    private Panel _itemsPanel;
    private TranslateTransform _panningTransform;
    private TranslateTransform _centeringTransform;
    private bool _isSelecting;
    private LoopingSelectorItem _selectedItem;
    private Queue<LoopingSelectorItem> _temporaryItemsPool;
    private double _minimumPanelScroll;
    private double _maximumPanelScroll;
    private int _additionalItemsCount;
    private bool _isAnimating;
    private double _dragTarget;
    private bool _isAllowedToDragVertically;
    private bool _isAllowedToDragHorizontally;
    private bool _isDragging;
    private LoopingSelector.State _state;
    public static readonly DependencyProperty DataSourceProperty = DependencyProperty.Register(nameof (DataSource), typeof (ILoopingSelectorDataSource), typeof (LoopingSelector), new PropertyMetadata((object) null, new PropertyChangedCallback(LoopingSelector.OnDataModelChanged)));
    public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register(nameof (ItemTemplate), typeof (DataTemplate), typeof (LoopingSelector), new PropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(nameof (Orientation), typeof (Orientation), typeof (LoopingSelector), new PropertyMetadata((object) Orientation.Vertical));
    public static readonly DependencyProperty FrictionProperty = DependencyProperty.Register(nameof (Friction), typeof (double), typeof (LoopingSelector), new PropertyMetadata((object) 0.2));
    public static readonly DependencyProperty MaximumSpeedProperty = DependencyProperty.Register(nameof (MaximumSpeed), typeof (double), typeof (LoopingSelector), new PropertyMetadata((object) 4000.0));
    public static readonly DependencyProperty ParkingSpeedProperty = DependencyProperty.Register(nameof (ParkingSpeed), typeof (double), typeof (LoopingSelector), new PropertyMetadata((object) 80.0));
    public static readonly DependencyProperty ItemPaddingProperty = DependencyProperty.Register(nameof (ItemPadding), typeof (Thickness), typeof (LoopingSelector), new PropertyMetadata((object) new Thickness(6.0)));
    public static readonly DependencyProperty IsExpandedProperty = DependencyProperty.Register(nameof (IsExpanded), typeof (bool), typeof (LoopingSelector), new PropertyMetadata((object) false, new PropertyChangedCallback(LoopingSelector.OnIsExpandedChanged)));

    public ILoopingSelectorDataSource DataSource
    {
      get => (ILoopingSelectorDataSource) this.GetValue(LoopingSelector.DataSourceProperty);
      set
      {
        if (this.DataSource != null)
          this.DataSource.SelectionChanged -= new EventHandler<SelectionChangedEventArgs>(this.value_SelectionChanged);
        this.SetValue(LoopingSelector.DataSourceProperty, (object) value);
        if (value == null)
          return;
        value.SelectionChanged += new EventHandler<SelectionChangedEventArgs>(this.value_SelectionChanged);
      }
    }

    private void value_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (!this.IsReady || this._isSelecting || e.AddedItems.Count != 1)
        return;
      object addedItem = e.AddedItems[0];
      foreach (LoopingSelectorItem child in (PresentationFrameworkCollection<UIElement>) this._itemsPanel.Children)
      {
        if (child.DataContext == addedItem)
        {
          this.SelectAndSnapTo(child);
          return;
        }
      }
      this.UpdateData();
    }

    private static void OnDataModelChanged(
      DependencyObject obj,
      DependencyPropertyChangedEventArgs e)
    {
      ((LoopingSelector) obj).UpdateData();
    }

    public DataTemplate ItemTemplate
    {
      get => (DataTemplate) this.GetValue(LoopingSelector.ItemTemplateProperty);
      set => this.SetValue(LoopingSelector.ItemTemplateProperty, (object) value);
    }

    public Orientation Orientation
    {
      get => (Orientation) this.GetValue(LoopingSelector.OrientationProperty);
      set => this.SetValue(LoopingSelector.OrientationProperty, (object) value);
    }

    public double Friction
    {
      get => (double) this.GetValue(LoopingSelector.FrictionProperty);
      set => this.SetValue(LoopingSelector.FrictionProperty, (object) value);
    }

    public double MaximumSpeed
    {
      get => (double) this.GetValue(LoopingSelector.MaximumSpeedProperty);
      set => this.SetValue(LoopingSelector.MaximumSpeedProperty, (object) value);
    }

    public double ParkingSpeed
    {
      get => (double) this.GetValue(LoopingSelector.ParkingSpeedProperty);
      set => this.SetValue(LoopingSelector.ParkingSpeedProperty, (object) value);
    }

    public Size ItemSize { get; set; }

    public Thickness ItemPadding
    {
      get => (Thickness) this.GetValue(LoopingSelector.ItemPaddingProperty);
      set => this.SetValue(LoopingSelector.ItemPaddingProperty, (object) value);
    }

    public LoopingSelector()
    {
      ExponentialEase exponentialEase = new ExponentialEase();
      exponentialEase.EasingMode = EasingMode.EaseInOut;
      this._selectEase = (IEasingFunction) exponentialEase;
      this._panEase = (IEasingFunction) new ExponentialEase();
      this._minimumPanelScroll = -3.4028234663852886E+38;
      this._maximumPanelScroll = 3.4028234663852886E+38;
      this._isAllowedToDragVertically = true;
      this._isAllowedToDragHorizontally = true;
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this._isAllowedToDragVertically = this.Orientation == Orientation.Vertical;
      this._isAllowedToDragHorizontally = this.Orientation == Orientation.Horizontal;
      this.DefaultStyleKey = (object) typeof (LoopingSelector);
      this.CreateEventHandlers();
    }

    public bool IsExpanded
    {
      get => (bool) this.GetValue(LoopingSelector.IsExpandedProperty);
      set => this.SetValue(LoopingSelector.IsExpandedProperty, (object) value);
    }

    public event DependencyPropertyChangedEventHandler IsExpandedChanged;

    private static void OnIsExpandedChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      LoopingSelector sender1 = (LoopingSelector) sender;
      sender1.UpdateItemState();
      if (!sender1.IsExpanded)
        sender1.SelectAndSnapToClosest();
      if (sender1._state == LoopingSelector.State.Normal || sender1._state == LoopingSelector.State.Expanded)
        sender1._state = sender1.IsExpanded ? LoopingSelector.State.Expanded : LoopingSelector.State.Normal;
      DependencyPropertyChangedEventHandler isExpandedChanged = sender1.IsExpandedChanged;
      if (isExpandedChanged == null)
        return;
      isExpandedChanged((object) sender1, e);
    }

    public override void OnApplyTemplate()
    {
      base.OnApplyTemplate();
      if (!(this.GetTemplateChild("ItemsPanel") is Panel panel))
        panel = (Panel) new Canvas();
      this._itemsPanel = panel;
      if (!(this.GetTemplateChild("CenteringTransform") is TranslateTransform translateTransform1))
        translateTransform1 = new TranslateTransform();
      this._centeringTransform = translateTransform1;
      if (!(this.GetTemplateChild("PanningTransform") is TranslateTransform translateTransform2))
        translateTransform2 = new TranslateTransform();
      this._panningTransform = translateTransform2;
      this.CreateVisuals();
    }

    private void LoopingSelector_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      if (!this._isAnimating)
        return;
      if (this.Orientation == Orientation.Vertical)
      {
        double y = this._panningTransform.Y;
        this.StopAnimation();
        this._panningTransform.Y = y;
      }
      else
      {
        double x = this._panningTransform.X;
        this.StopAnimation();
        this._panningTransform.X = x;
      }
      this._isAnimating = false;
      this._state = LoopingSelector.State.Dragging;
    }

    private void LoopingSelector_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
      if (this._selectedItem == sender || this._state != LoopingSelector.State.Dragging || this._isAnimating)
        return;
      this.SelectAndSnapToClosest();
      this._state = LoopingSelector.State.Expanded;
    }

    private void OnTap(object sender, GestureEventArgs e)
    {
      if (this._panningTransform == null)
        return;
      this.SelectAndSnapToClosest();
      e.Handled = true;
    }

    private void OnManipulationStarted(object sender, ManipulationStartedEventArgs e)
    {
      this._isAllowedToDragVertically = true;
      this._isAllowedToDragHorizontally = true;
      this._isDragging = false;
    }

    private void OnManipulationDelta(object sender, ManipulationDeltaEventArgs e)
    {
      if (this.Orientation == Orientation.Vertical)
      {
        if (this._isDragging)
        {
          this.AnimatePanel(LoopingSelector._panDuration, this._panEase, this._dragTarget += e.DeltaManipulation.Translation.Y);
          e.Handled = true;
        }
        else if (Math.Abs(e.CumulativeManipulation.Translation.X) > 12.0)
        {
          this._isAllowedToDragVertically = false;
        }
        else
        {
          if (!this._isAllowedToDragVertically || Math.Abs(e.CumulativeManipulation.Translation.Y) <= 12.0)
            return;
          this._isDragging = true;
          this._state = LoopingSelector.State.Dragging;
          e.Handled = true;
          this._selectedItem = (LoopingSelectorItem) null;
          if (!this.IsExpanded)
            this.IsExpanded = true;
          this._dragTarget = this._panningTransform.Y;
          this.UpdateItemState();
        }
      }
      else if (this._isDragging)
      {
        this.AnimatePanel(LoopingSelector._panDuration, this._panEase, this._dragTarget += e.DeltaManipulation.Translation.X);
        e.Handled = true;
      }
      else if (Math.Abs(e.CumulativeManipulation.Translation.Y) > 12.0)
      {
        this._isAllowedToDragHorizontally = false;
      }
      else
      {
        if (!this._isAllowedToDragHorizontally || Math.Abs(e.CumulativeManipulation.Translation.X) <= 12.0)
          return;
        this._isDragging = true;
        this._state = LoopingSelector.State.Dragging;
        e.Handled = true;
        this._selectedItem = (LoopingSelectorItem) null;
        if (!this.IsExpanded)
          this.IsExpanded = true;
        this._dragTarget = this._panningTransform.X;
        this.UpdateItemState();
      }
    }

    private void OnManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
    {
      if (!this._isDragging)
        return;
      if (e.IsInertial)
      {
        this._state = LoopingSelector.State.Flicking;
        this._selectedItem = (LoopingSelectorItem) null;
        if (!this.IsExpanded)
          this.IsExpanded = true;
        Point initialVelocity = this.Orientation != Orientation.Vertical ? new Point(e.FinalVelocities.LinearVelocity.X, 0.0) : new Point(0.0, e.FinalVelocities.LinearVelocity.Y);
        double stopTime = PhysicsConstants.GetStopTime(initialVelocity, this.Friction, this.MaximumSpeed, this.ParkingSpeed);
        Point stopPoint = PhysicsConstants.GetStopPoint(initialVelocity, this.Friction, this.MaximumSpeed, this.ParkingSpeed);
        IEasingFunction easingFunction = PhysicsConstants.GetEasingFunction(stopTime, this.Friction);
        double to = this.Orientation != Orientation.Vertical ? this._panningTransform.X + stopPoint.X : this._panningTransform.Y + stopPoint.Y;
        this.AnimatePanel(new Duration(TimeSpan.FromSeconds(stopTime)), easingFunction, to);
        e.Handled = true;
        this._selectedItem = (LoopingSelectorItem) null;
        this.UpdateItemState();
      }
      if (this._state == LoopingSelector.State.Dragging)
        this.SelectAndSnapToClosest();
      this._state = LoopingSelector.State.Expanded;
    }

    private void OnSizeChanged(object sender, SizeChangedEventArgs e)
    {
      if (this.Orientation == Orientation.Vertical)
        this._centeringTransform.Y = Math.Round(e.NewSize.Height / 2.0);
      else
        this._centeringTransform.X = Math.Round(e.NewSize.Width / 2.0);
      RectangleGeometry rectangleGeometry = new RectangleGeometry();
      Size newSize = e.NewSize;
      double width = newSize.Width;
      newSize = e.NewSize;
      double height = newSize.Height;
      rectangleGeometry.Rect = new Rect(0.0, 0.0, width, height);
      this.Clip = (Geometry) rectangleGeometry;
      this.UpdateData();
    }

    private void OnWrapperClick(object sender, EventArgs e)
    {
      if (this._state == LoopingSelector.State.Normal)
      {
        this._state = LoopingSelector.State.Expanded;
        this.IsExpanded = true;
      }
      else
      {
        if (this._state != LoopingSelector.State.Expanded)
          return;
        if (!this._isAnimating && sender == this._selectedItem)
        {
          this._state = LoopingSelector.State.Normal;
          this.IsExpanded = false;
        }
        else
        {
          if (sender == this._selectedItem || this._isAnimating)
            return;
          this.SelectAndSnapTo((LoopingSelectorItem) sender);
        }
      }
    }

    private void SelectAndSnapTo(LoopingSelectorItem item)
    {
      if (item == null)
        return;
      if (this._selectedItem != null)
        this._selectedItem.SetState(this.IsExpanded ? LoopingSelectorItem.State.Expanded : LoopingSelectorItem.State.Normal, true);
      if (this._selectedItem != item)
      {
        this._selectedItem = item;
        this.Dispatcher.BeginInvoke((Action) (() =>
        {
          this._isSelecting = true;
          this.DataSource.SelectedItem = item.DataContext;
          this._isSelecting = false;
        }));
      }
      this._selectedItem.SetState(LoopingSelectorItem.State.Selected, true);
      TranslateTransform transform = item.Transform;
      if (transform == null)
        return;
      if (this.Orientation == Orientation.Vertical)
      {
        double to = -transform.Y - Math.Round(item.ActualHeight / 2.0);
        if (this._panningTransform.Y == to)
          return;
        this.AnimatePanel(LoopingSelector._selectDuration, this._selectEase, to);
      }
      else
      {
        double to = -transform.X - Math.Round(item.ActualWidth / 2.0);
        if (this._panningTransform.X == to)
          return;
        this.AnimatePanel(LoopingSelector._selectDuration, this._selectEase, to);
      }
    }

    private void UpdateData()
    {
      if (!this.IsReady)
        return;
      this._temporaryItemsPool = new Queue<LoopingSelectorItem>(this._itemsPanel.Children.Count);
      foreach (LoopingSelectorItem child in (PresentationFrameworkCollection<UIElement>) this._itemsPanel.Children)
      {
        if (child.GetState() == LoopingSelectorItem.State.Selected)
          child.SetState(LoopingSelectorItem.State.Normal, false);
        this._temporaryItemsPool.Enqueue(child);
        child.Remove();
      }
      this._itemsPanel.Children.Clear();
      this.StopAnimation();
      if (this.Orientation == Orientation.Vertical)
        this._panningTransform.Y = 0.0;
      else
        this._panningTransform.X = 0.0;
      this._minimumPanelScroll = -3.4028234663852886E+38;
      this._maximumPanelScroll = 3.4028234663852886E+38;
      this.Balance();
    }

    private void AnimatePanel(Duration duration, IEasingFunction ease, double to)
    {
      double num1 = Math.Max(this._minimumPanelScroll, Math.Min(this._maximumPanelScroll, to));
      if (to != num1)
      {
        double num2;
        double num3;
        if (this.Orientation == Orientation.Vertical)
        {
          num2 = Math.Abs(this._panningTransform.Y - to);
          num3 = Math.Abs(this._panningTransform.Y - num1);
        }
        else
        {
          num2 = Math.Abs(this._panningTransform.X - to);
          num3 = Math.Abs(this._panningTransform.X - num1);
        }
        double num4 = num3 / num2;
        duration = new Duration(TimeSpan.FromMilliseconds((double) duration.TimeSpan.Milliseconds * num4));
        to = num1;
      }
      double num5 = this.Orientation != Orientation.Vertical ? this._panningTransform.X : this._panningTransform.Y;
      this.StopAnimation();
      CompositionTarget.Rendering += new EventHandler(this.AnimationPerFrameCallback);
      this._panelAnimation.Duration = duration;
      this._panelAnimation.EasingFunction = ease;
      this._panelAnimation.From = new double?(num5);
      this._panelAnimation.To = new double?(to);
      this._panelStoryboard.Begin();
      this._panelStoryboard.SeekAlignedToLastTick(TimeSpan.Zero);
      this._isAnimating = true;
    }

    private void StopAnimation()
    {
      this._panelStoryboard.Stop();
      CompositionTarget.Rendering -= new EventHandler(this.AnimationPerFrameCallback);
    }

    private void Brake(double newStoppingPoint)
    {
      if (!this._panelAnimation.To.HasValue || !this._panelAnimation.From.HasValue)
        return;
      double num1 = this._panelAnimation.To.Value - this._panelAnimation.From.Value;
      double num2 = newStoppingPoint;
      this.AnimatePanel(new Duration(TimeSpan.FromMilliseconds((double) this._panelAnimation.Duration.TimeSpan.Milliseconds * ((this.Orientation != Orientation.Vertical ? num2 - this._panningTransform.X : num2 - this._panningTransform.Y) / num1))), this._panelAnimation.EasingFunction, newStoppingPoint);
    }

    private bool IsReady
    {
      get
      {
        return (this.Orientation == Orientation.Vertical ? (this.ActualHeight > 0.0 ? 1 : 0) : (this.ActualWidth > 0.0 ? 1 : 0)) != 0 && this.DataSource != null && this._itemsPanel != null;
      }
    }

    private void Balance()
    {
      if (!this.IsReady)
        return;
      double actualItemWidth = this.ActualItemWidth;
      double actualItemHeight = this.ActualItemHeight;
      this._additionalItemsCount = this.Orientation != Orientation.Vertical ? (int) Math.Round(this.ActualWidth * 1.5 / actualItemWidth) : (int) Math.Round(this.ActualHeight * 1.5 / actualItemHeight);
      int num = -1;
      LoopingSelectorItem loopingSelectorItem1;
      if (this._itemsPanel.Children.Count == 0)
      {
        num = 0;
        this._selectedItem = loopingSelectorItem1 = this.CreateAndAddItem(this._itemsPanel, this.DataSource.SelectedItem);
        if (this.Orientation == Orientation.Vertical)
        {
          loopingSelectorItem1.Transform.Y = -actualItemHeight / 2.0;
          loopingSelectorItem1.Transform.X = (this.ActualWidth - actualItemWidth) / 2.0;
        }
        else
        {
          loopingSelectorItem1.Transform.X = -actualItemWidth / 2.0;
          loopingSelectorItem1.Transform.Y = (this.ActualHeight - actualItemHeight) / 2.0;
        }
        loopingSelectorItem1.SetState(LoopingSelectorItem.State.Selected, false);
      }
      else
        loopingSelectorItem1 = (LoopingSelectorItem) this._itemsPanel.Children[this.GetClosestItem()];
      int count1;
      LoopingSelectorItem before = LoopingSelector.GetFirstItem(loopingSelectorItem1, out count1);
      int count2;
      LoopingSelectorItem after = LoopingSelector.GetLastItem(loopingSelectorItem1, out count2);
      if (count1 < count2 || count1 < this._additionalItemsCount)
      {
        for (; count1 < this._additionalItemsCount; ++count1)
        {
          object previous = this.DataSource.GetPrevious(before.DataContext);
          if (previous == null)
          {
            this._maximumPanelScroll = this.Orientation != Orientation.Vertical ? -before.Transform.X - actualItemWidth / 2.0 : -before.Transform.Y - actualItemHeight / 2.0;
            if (this._panelAnimation.To.HasValue && this._isAnimating && this._panelAnimation.To.Value > this._maximumPanelScroll)
            {
              this.Brake(this._maximumPanelScroll);
              break;
            }
            break;
          }
          LoopingSelectorItem loopingSelectorItem2;
          if (count2 > this._additionalItemsCount)
          {
            loopingSelectorItem2 = after;
            after = after.Previous;
            loopingSelectorItem2.Remove();
            loopingSelectorItem2.Content = loopingSelectorItem2.DataContext = previous;
          }
          else
          {
            loopingSelectorItem2 = this.CreateAndAddItem(this._itemsPanel, previous);
            if (this.Orientation == Orientation.Vertical)
              loopingSelectorItem2.Transform.X = (this.ActualWidth - actualItemWidth) / 2.0;
            else
              loopingSelectorItem2.Transform.Y = (this.ActualHeight - actualItemHeight) / 2.0;
          }
          if (this.Orientation == Orientation.Vertical)
            loopingSelectorItem2.Transform.Y = before.Transform.Y - actualItemHeight;
          else
            loopingSelectorItem2.Transform.X = before.Transform.X - actualItemWidth;
          loopingSelectorItem2.InsertBefore(before);
          before = loopingSelectorItem2;
        }
      }
      if (count2 < count1 || count2 < this._additionalItemsCount)
      {
        for (; count2 < this._additionalItemsCount; ++count2)
        {
          object next = this.DataSource.GetNext(after.DataContext);
          if (next == null)
          {
            this._minimumPanelScroll = this.Orientation != Orientation.Vertical ? -after.Transform.X - actualItemWidth / 2.0 : -after.Transform.Y - actualItemHeight / 2.0;
            if (this._panelAnimation.To.HasValue && this._isAnimating && this._panelAnimation.To.Value < this._minimumPanelScroll)
            {
              this.Brake(this._minimumPanelScroll);
              break;
            }
            break;
          }
          LoopingSelectorItem loopingSelectorItem3;
          if (count1 > this._additionalItemsCount)
          {
            loopingSelectorItem3 = before;
            before = before.Next;
            loopingSelectorItem3.Remove();
            loopingSelectorItem3.Content = loopingSelectorItem3.DataContext = next;
          }
          else
          {
            loopingSelectorItem3 = this.CreateAndAddItem(this._itemsPanel, next);
            if (this.Orientation == Orientation.Vertical)
              loopingSelectorItem3.Transform.X = (this.ActualWidth - actualItemWidth) / 2.0;
            else
              loopingSelectorItem3.Transform.Y = (this.ActualHeight - this.ActualItemHeight) / 2.0;
          }
          if (this.Orientation == Orientation.Vertical)
            loopingSelectorItem3.Transform.Y = after.Transform.Y + actualItemHeight;
          else
            loopingSelectorItem3.Transform.X = after.Transform.X + actualItemWidth;
          loopingSelectorItem3.InsertAfter(after);
          after = loopingSelectorItem3;
        }
      }
      this._temporaryItemsPool = (Queue<LoopingSelectorItem>) null;
    }

    private static LoopingSelectorItem GetFirstItem(LoopingSelectorItem item, out int count)
    {
      count = 0;
      for (; item.Previous != null; item = item.Previous)
        ++count;
      return item;
    }

    private static LoopingSelectorItem GetLastItem(LoopingSelectorItem item, out int count)
    {
      count = 0;
      for (; item.Next != null; item = item.Next)
        ++count;
      return item;
    }

    private void AnimationPerFrameCallback(object sender, EventArgs e) => this.Balance();

    private int GetClosestItem()
    {
      if (!this.IsReady)
        return -1;
      int count = this._itemsPanel.Children.Count;
      double y = this._panningTransform.Y;
      double x = this._panningTransform.X;
      double num1 = this.ActualItemHeight / 2.0;
      double num2 = this.ActualItemWidth / 2.0;
      int closestItem = -1;
      double num3 = double.MaxValue;
      for (int index = 0; index < count; ++index)
      {
        LoopingSelectorItem child = (LoopingSelectorItem) this._itemsPanel.Children[index];
        double num4;
        if (this.Orientation == Orientation.Vertical)
        {
          num4 = Math.Abs(child.Transform.Y + num1 + y);
          if (num4 <= num1)
          {
            closestItem = index;
            break;
          }
        }
        else
        {
          num4 = Math.Abs(child.Transform.X + num2 + x);
          if (num4 <= num2)
          {
            closestItem = index;
            break;
          }
        }
        if (num3 > num4)
        {
          num3 = num4;
          closestItem = index;
        }
      }
      return closestItem;
    }

    private void PanelStoryboardCompleted(object sender, EventArgs e)
    {
      CompositionTarget.Rendering -= new EventHandler(this.AnimationPerFrameCallback);
      this._isAnimating = false;
      if (this._state == LoopingSelector.State.Dragging)
        return;
      this.SelectAndSnapToClosest();
    }

    private void SelectAndSnapToClosest()
    {
      if (!this.IsReady)
        return;
      int closestItem = this.GetClosestItem();
      if (closestItem == -1)
        return;
      this.SelectAndSnapTo((LoopingSelectorItem) this._itemsPanel.Children[closestItem]);
    }

    private void UpdateItemState()
    {
      if (!this.IsReady)
        return;
      bool isExpanded = this.IsExpanded;
      foreach (LoopingSelectorItem child in (PresentationFrameworkCollection<UIElement>) this._itemsPanel.Children)
      {
        if (child == this._selectedItem)
          child.SetState(LoopingSelectorItem.State.Selected, true);
        else
          child.SetState(isExpanded ? LoopingSelectorItem.State.Expanded : LoopingSelectorItem.State.Normal, true);
      }
    }

    private double ActualItemWidth
    {
      get
      {
        Thickness padding = this.Padding;
        double left = padding.Left;
        padding = this.Padding;
        double right = padding.Right;
        return left + right + this.ItemSize.Width;
      }
    }

    private double ActualItemHeight
    {
      get
      {
        Thickness padding = this.Padding;
        double top = padding.Top;
        padding = this.Padding;
        double bottom = padding.Bottom;
        return top + bottom + this.ItemSize.Height;
      }
    }

    private void CreateVisuals()
    {
      this._panelAnimation = new DoubleAnimation();
      Storyboard.SetTarget((Timeline) this._panelAnimation, (DependencyObject) this._panningTransform);
      Storyboard.SetTargetProperty((Timeline) this._panelAnimation, new PropertyPath(this.Orientation == Orientation.Vertical ? "Y" : "X", new object[0]));
      this._panelStoryboard = new Storyboard();
      this._panelStoryboard.Children.Add((Timeline) this._panelAnimation);
      this._panelStoryboard.Completed += new EventHandler(this.PanelStoryboardCompleted);
    }

    private void CreateEventHandlers()
    {
      this.SizeChanged += new SizeChangedEventHandler(this.OnSizeChanged);
      this.ManipulationStarted += new EventHandler<ManipulationStartedEventArgs>(this.OnManipulationStarted);
      this.ManipulationCompleted += new EventHandler<ManipulationCompletedEventArgs>(this.OnManipulationCompleted);
      this.ManipulationDelta += new EventHandler<ManipulationDeltaEventArgs>(this.OnManipulationDelta);
      this.Tap += new EventHandler<GestureEventArgs>(this.OnTap);
      this.AddHandler(UIElement.MouseLeftButtonDownEvent, (Delegate) new MouseButtonEventHandler(this.LoopingSelector_MouseLeftButtonDown), true);
      this.AddHandler(UIElement.MouseLeftButtonUpEvent, (Delegate) new MouseButtonEventHandler(this.LoopingSelector_MouseLeftButtonUp), true);
    }

    private LoopingSelectorItem CreateAndAddItem(Panel parent, object content)
    {
      int num = this._temporaryItemsPool == null ? 0 : (this._temporaryItemsPool.Count > 0 ? 1 : 0);
      LoopingSelectorItem andAddItem = num != 0 ? this._temporaryItemsPool.Dequeue() : new LoopingSelectorItem();
      if (num == 0)
      {
        andAddItem.ContentTemplate = this.ItemTemplate;
        LoopingSelectorItem loopingSelectorItem1 = andAddItem;
        Size itemSize = this.ItemSize;
        double width = itemSize.Width;
        loopingSelectorItem1.Width = width;
        LoopingSelectorItem loopingSelectorItem2 = andAddItem;
        itemSize = this.ItemSize;
        double height = itemSize.Height;
        loopingSelectorItem2.Height = height;
        andAddItem.Padding = this.ItemPadding;
        andAddItem.Click += new EventHandler<EventArgs>(this.OnWrapperClick);
      }
      andAddItem.DataContext = andAddItem.Content = content;
      parent.Children.Add((UIElement) andAddItem);
      if (num == 0)
        andAddItem.ApplyTemplate();
      return andAddItem;
    }

    private enum State
    {
      Normal,
      Expanded,
      Dragging,
      Snapping,
      Flicking,
    }
  }
}
