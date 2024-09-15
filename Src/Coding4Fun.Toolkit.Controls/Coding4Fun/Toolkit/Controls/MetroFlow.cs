// Decompiled with JetBrains decompiler
// Type: Coding4Fun.Toolkit.Controls.MetroFlow
// Assembly: Coding4Fun.Toolkit.Controls, Version=2.1.7.0, Culture=neutral, PublicKeyToken=c5fd7b72b1a17ce4
// MVID: 213D8A75-4B66-4004-95C2-D82C49ACB764
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\Coding4Fun.Toolkit.Controls.dll

using Coding4Fun.Toolkit.Controls.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;


namespace Coding4Fun.Toolkit.Controls
{
  public class MetroFlow : ItemsControl
  {
    private GridLength _minimizedGridLength = new GridLength(48.0);
    private readonly GridLength _maximizedGridLength = new GridLength(1.0, GridUnitType.Star);
    private Storyboard _animationBoard;
    private Grid _layoutGrid;
    private int _minimizingColumnIndex;
    private const string LayoutRootName = "LayoutRoot";
    public static readonly DependencyProperty AnimationDurationProperty = DependencyProperty.Register(nameof (AnimationDuration), typeof (TimeSpan), typeof (MetroFlow), new PropertyMetadata((object) TimeSpan.FromMilliseconds(100.0)));
    public static readonly DependencyProperty SelectedColumnIndexProperty = DependencyProperty.Register(nameof (SelectedColumnIndex), typeof (int), typeof (MetroFlow), new PropertyMetadata((object) 0, new PropertyChangedCallback(MetroFlow.SelectedColumnIndexChanged)));
    public static readonly DependencyProperty ExpandingWidthProperty = DependencyProperty.Register(nameof (ExpandingWidth), typeof (double), typeof (MetroFlow), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(MetroFlow.ColumnGrowWidthChanged)));
    public static readonly DependencyProperty CollapsingWidthProperty = DependencyProperty.Register(nameof (CollapsingWidth), typeof (double), typeof (MetroFlow), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(MetroFlow.ColumnShrinkWidthChanged)));

    public event EventHandler<SelectionChangedEventArgs> SelectionChanged;

    public event EventHandler<SelectionTapEventArgs> SelectionTap;

    public MetroFlow() => this.DefaultStyleKey = (object) typeof (MetroFlow);

    protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
    {
      base.OnItemsChanged(e);
      if (this._layoutGrid == null)
        return;
      if (this.SelectedColumnIndex >= this.Items.Count)
        this.SelectedColumnIndex = this.Items.Count - 1;
      else if (this.Items.Count > 0 && this.SelectedColumnIndex < 0)
        this.SelectedColumnIndex = 0;
      this.ControlLoaded();
    }

    protected override bool IsItemItsOwnContainerOverride(object item) => item is MetroFlowData;

    public override void OnApplyTemplate()
    {
      base.OnApplyTemplate();
      this._layoutGrid = this.GetTemplateChild("LayoutRoot") as Grid;
      if (this._layoutGrid == null || ApplicationSpace.IsDesignMode && this.Items.Count <= 0)
        return;
      this.ControlLoaded();
    }

    public TimeSpan AnimationDuration
    {
      get => (TimeSpan) this.GetValue(MetroFlow.AnimationDurationProperty);
      set => this.SetValue(MetroFlow.AnimationDurationProperty, (object) value);
    }

    public int SelectedColumnIndex
    {
      get => (int) this.GetValue(MetroFlow.SelectedColumnIndexProperty);
      set => this.SetValue(MetroFlow.SelectedColumnIndexProperty, (object) value);
    }

    private static void SelectedColumnIndexChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      if (!(d is MetroFlow metroFlow) || e.NewValue == e.OldValue)
        return;
      metroFlow.SelectionIndexChanged((int) e.OldValue);
    }

    private void SelectionIndexChanged(int oldIndex)
    {
      this._minimizingColumnIndex = oldIndex;
      this.VerifyMinimizingColumnIndex();
      if (this.SelectionChanged != null)
      {
        MetroFlowData metroFlowData = this.Items.Count <= 0 || this.SelectedColumnIndex < 0 ? (MetroFlowData) null : (MetroFlowData) this.Items[this.SelectedColumnIndex];
        this.SelectionChanged((object) this, new SelectionChangedEventArgs((IList) new List<MetroFlowData>()
        {
          this.Items.Count <= 0 || this._minimizingColumnIndex < 0 ? (MetroFlowData) null : (MetroFlowData) this.Items[this._minimizingColumnIndex]
        }, (IList) new List<MetroFlowData>()
        {
          metroFlowData
        }));
      }
      this.CreateSb(this._layoutGrid, oldIndex);
    }

    public double ExpandingWidth
    {
      get => (double) this.GetValue(MetroFlow.ExpandingWidthProperty);
      set => this.SetValue(MetroFlow.ExpandingWidthProperty, (object) value);
    }

    public double CollapsingWidth
    {
      get => (double) this.GetValue(MetroFlow.CollapsingWidthProperty);
      set => this.SetValue(MetroFlow.CollapsingWidthProperty, (object) value);
    }

    private static void ColumnGrowWidthChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      if (!(d is MetroFlow metroFlow))
        return;
      Grid layoutGrid = metroFlow._layoutGrid;
      if (layoutGrid.ColumnDefinitions.Count <= 1)
        return;
      MetroFlow.ChangeColumnWidth(layoutGrid.ColumnDefinitions[metroFlow.SelectedColumnIndex], (double) e.NewValue);
    }

    private static void ColumnShrinkWidthChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      if (!(d is MetroFlow metroFlow))
        return;
      metroFlow.VerifyMinimizingColumnIndex();
      Grid layoutGrid = metroFlow._layoutGrid;
      if (layoutGrid.ColumnDefinitions.Count <= 1)
        return;
      MetroFlow.ChangeColumnWidth(layoutGrid.ColumnDefinitions[metroFlow._minimizingColumnIndex], (double) e.NewValue);
    }

    private void VerifyMinimizingColumnIndex()
    {
      if (this._minimizingColumnIndex < this.Items.Count)
        return;
      this._minimizingColumnIndex = this.Items.Count - 1;
      if (this.SelectedColumnIndex == this._minimizingColumnIndex)
        --this._minimizingColumnIndex;
      if (this._minimizingColumnIndex >= 0)
        return;
      this._minimizingColumnIndex = 0;
    }

    private static void ChangeColumnWidth(ColumnDefinition target, double value)
    {
      if (target == null)
        return;
      target.Width = new GridLength(value);
    }

    private void ControlLoaded()
    {
      Grid layoutGrid = this._layoutGrid;
      if (this._layoutGrid == null || this.Items == null)
        return;
      layoutGrid.ColumnDefinitions.Clear();
      layoutGrid.Children.Clear();
      int num = 0;
      foreach (MetroFlowData metroFlowData in (PresentationFrameworkCollection<object>) this.Items)
      {
        bool flag = num == this.SelectedColumnIndex;
        ColumnDefinition columnDefinition = new ColumnDefinition()
        {
          Width = !flag ? this._minimizedGridLength : new GridLength(1.0, GridUnitType.Star)
        };
        layoutGrid.ColumnDefinitions.Add(columnDefinition);
        MetroFlowItem metroFlowItem = new MetroFlowItem()
        {
          ItemIndex = num + 1,
          ItemIndexOpacity = !flag ? 1.0 : 0.0,
          ItemIndexVisibility = !flag ? Visibility.Visible : Visibility.Collapsed,
          ImageSource = (ImageSource) new BitmapImage(metroFlowData.ImageUri),
          ImageOpacity = flag ? 1.0 : 0.0,
          ImageVisibility = flag ? Visibility.Visible : Visibility.Collapsed,
          Title = metroFlowData.Title,
          TitleOpacity = flag ? 1.0 : 0.0,
          TitleVisibility = flag ? Visibility.Visible : Visibility.Collapsed
        };
        metroFlowItem.SetValue(Grid.ColumnProperty, (object) num);
        metroFlowItem.Tap += new EventHandler<GestureEventArgs>(this.ItemTap);
        layoutGrid.Children.Add((UIElement) metroFlowItem);
        ++num;
      }
    }

    private void ItemTap(object sender, GestureEventArgs e)
    {
      if (!(sender is MetroFlowItem element))
        return;
      int selectedColumnIndex1 = this.SelectedColumnIndex;
      this.SelectedColumnIndex = MetroFlow.GetColumnIndex((DependencyObject) element);
      int selectedColumnIndex2 = this.SelectedColumnIndex;
      if (selectedColumnIndex1 != selectedColumnIndex2 || this.SelectionTap == null)
        return;
      this.SelectionTap((object) this, new SelectionTapEventArgs()
      {
        Index = this.SelectedColumnIndex,
        Data = (MetroFlowData) this.Items[this.SelectedColumnIndex]
      });
    }

    private void HandleStoppingAnimation(int targetIndex)
    {
      if (this._animationBoard == null || this._animationBoard.GetCurrentState() != ClockState.Active)
        return;
      this._animationBoard.Stop();
      this.AnimationCompleted(targetIndex);
    }

    private void CreateSb(Grid target, int oldIndex)
    {
      if (target == null || target.ColumnDefinitions.Count < this.SelectedColumnIndex)
        return;
      this.HandleStoppingAnimation(oldIndex);
      Storyboard sb = new Storyboard();
      MetroFlowItem metroFlowItem1 = MetroFlow.GetMetroFlowItem((Panel) target, this.SelectedColumnIndex);
      MetroFlowItem metroFlowItem2 = MetroFlow.GetMetroFlowItem((Panel) target, oldIndex);
      if (metroFlowItem1 != null)
      {
        metroFlowItem1.ImageVisibility = Visibility.Visible;
        metroFlowItem1.TitleVisibility = Visibility.Visible;
        this.CreateDoubleAnimations(sb, (DependencyObject) metroFlowItem1, "ImageOpacity", 1.0, metroFlowItem1.ImageOpacity);
        this.CreateDoubleAnimations(sb, (DependencyObject) metroFlowItem1, "TitleOpacity", 1.0, metroFlowItem1.TitleOpacity);
        this.CreateDoubleAnimations(sb, (DependencyObject) metroFlowItem1, "ItemIndexOpacity", fromValue: metroFlowItem1.ItemIndexOpacity);
      }
      if (metroFlowItem2 != null)
      {
        metroFlowItem2.ItemIndexVisibility = Visibility.Visible;
        this.CreateDoubleAnimations(sb, (DependencyObject) metroFlowItem2, "ImageOpacity", fromValue: metroFlowItem2.ImageOpacity);
        this.CreateDoubleAnimations(sb, (DependencyObject) metroFlowItem2, "TitleOpacity", fromValue: metroFlowItem2.TitleOpacity);
        this.CreateDoubleAnimations(sb, (DependencyObject) metroFlowItem2, "ItemIndexOpacity", 1.0, metroFlowItem2.ItemIndexOpacity);
      }
      DoubleAnimation doubleAnimations1 = this.CreateDoubleAnimations(sb, (DependencyObject) this, "CollapsingWidth", this._minimizedGridLength.Value);
      DoubleAnimation doubleAnimations2 = this.CreateDoubleAnimations(sb, (DependencyObject) this, "ExpandingWidth", fromValue: this._minimizedGridLength.Value);
      sb.Completed += (EventHandler) ((sbSender, sbEventArgs) => this.AnimationCompleted());
      if (metroFlowItem2 != null)
      {
        double actualWidth = metroFlowItem2.ActualWidth;
        doubleAnimations2.To = new double?(actualWidth);
        doubleAnimations1.From = new double?(actualWidth);
      }
      this.UpdateLayout();
      this._animationBoard = sb;
      this._animationBoard.Begin();
    }

    private DoubleAnimation CreateDoubleAnimations(
      Storyboard sb,
      DependencyObject target,
      string propertyPath,
      double toValue = 0.0,
      double fromValue = 0.0)
    {
      DoubleAnimation doubleAnimation = new DoubleAnimation();
      doubleAnimation.To = new double?(toValue);
      doubleAnimation.From = new double?(fromValue);
      doubleAnimation.Duration = (Duration) this.AnimationDuration;
      DoubleAnimation element = doubleAnimation;
      Storyboard.SetTarget((Timeline) element, target);
      Storyboard.SetTargetProperty((Timeline) element, new PropertyPath(propertyPath, new object[0]));
      sb.Children.Add((Timeline) element);
      return element;
    }

    private static MetroFlowItem GetMetroFlowItem(Panel target, int index)
    {
      return target.Children.Where<UIElement>((Func<UIElement, bool>) (item => MetroFlow.GetColumnIndex((DependencyObject) item) == index)).SingleOrDefault<UIElement>() as MetroFlowItem;
    }

    private static int GetColumnIndex(DependencyObject element)
    {
      return (int) element.GetValue(Grid.ColumnProperty);
    }

    private void AnimationCompleted() => this.AnimationCompleted(this.SelectedColumnIndex);

    private void AnimationCompleted(int column)
    {
      for (int index = 0; index < this._layoutGrid.ColumnDefinitions.Count; ++index)
        this._layoutGrid.ColumnDefinitions[index].Width = index != column ? this._minimizedGridLength : this._maximizedGridLength;
      foreach (MetroFlowItem element in this._layoutGrid.Children.Select<UIElement, MetroFlowItem>((Func<UIElement, MetroFlowItem>) (t => t as MetroFlowItem)))
        MetroFlow.SetMetroFlowControlItemProperties(element, MetroFlow.GetColumnIndex((DependencyObject) element) == column);
      this.UpdateLayout();
    }

    private static void SetMetroFlowControlItemProperties(MetroFlowItem item, bool isLarge)
    {
      if (item == null)
        return;
      item.ImageVisibility = item.TitleVisibility = isLarge ? Visibility.Visible : Visibility.Collapsed;
      MetroFlowItem metroFlowItem1 = item;
      MetroFlowItem metroFlowItem2 = item;
      int num1 = isLarge ? 1 : 0;
      double num2;
      double num3 = num2 = (double) num1;
      metroFlowItem2.ImageOpacity = num2;
      double num4 = num3;
      metroFlowItem1.TitleOpacity = num4;
      item.ItemIndexVisibility = isLarge ? Visibility.Collapsed : Visibility.Visible;
      item.ItemIndexOpacity = isLarge ? 0.0 : 1.0;
    }
  }
}
