// Decompiled with JetBrains decompiler
// Type: Coding4Fun.Toolkit.Controls.TimeSpanPickerPage
// Assembly: Coding4Fun.Toolkit.Controls, Version=2.1.7.0, Culture=neutral, PublicKeyToken=c5fd7b72b1a17ce4
// MVID: 213D8A75-4B66-4004-95C2-D82C49ACB764
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\Coding4Fun.Toolkit.Controls.dll

using Coding4Fun.Toolkit.Controls.Primitives;
using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;


namespace Coding4Fun.Toolkit.Controls
{
  public class TimeSpanPickerPage : TimeSpanPickerBasePage
  {
    internal VisualStateGroup VisibilityStates;
    internal VisualState Open;
    internal VisualState Closed;
    internal PlaneProjection PlaneProjection;
    internal Rectangle SystemTrayPlaceholder;
    internal TextBlock HeaderTitle;
    internal LoopingSelector PrimarySelector;
    internal LoopingSelector SecondarySelector;
    internal LoopingSelector TertiarySelector;
    private bool _contentLoaded;

    public TimeSpanPickerPage() => this.InitializeComponent();

    public override void InitDataSource()
    {
      int seconds = this.StepFrequency.Seconds;
      this.TertiarySelector.DataSource = (ILoopingSelectorDataSource) new SecondTimeSpanDataSource(this.Maximum >= TimeSpan.FromMinutes(1.0) ? 60 : Math.Min(this.Maximum.Seconds + seconds, 60), seconds);
      TimeSpan timeSpan;
      int num1;
      if (!(this.StepFrequency > TimeSpan.FromMinutes(1.0)))
      {
        num1 = 1;
      }
      else
      {
        timeSpan = this.StepFrequency;
        num1 = timeSpan.Minutes;
      }
      int step1 = num1;
      int max1;
      if (!(this.Maximum >= TimeSpan.FromHours(1.0)))
      {
        timeSpan = this.Maximum;
        max1 = Math.Min(timeSpan.Minutes + step1, 60);
      }
      else
        max1 = 60;
      this.SecondarySelector.DataSource = (ILoopingSelectorDataSource) new MinuteTimeSpanDataSource(max1, step1);
      int num2;
      if (!(this.StepFrequency > TimeSpan.FromHours(1.0)))
      {
        num2 = 1;
      }
      else
      {
        timeSpan = this.StepFrequency;
        num2 = timeSpan.Hours;
      }
      int step2 = num2;
      int max2;
      if (!(this.Maximum >= TimeSpan.FromHours(24.0)))
      {
        timeSpan = this.Maximum;
        max2 = timeSpan.Hours + step2;
      }
      else
        max2 = 24;
      this.PrimarySelector.DataSource = (ILoopingSelectorDataSource) new HourTimeSpanDataSource(max2, step2);
      this.InitializeValuePickerPage(this.PrimarySelector, this.SecondarySelector, this.TertiarySelector);
    }

    protected override IEnumerable<LoopingSelector> GetSelectorsOrderedByCulturePattern()
    {
      return ValuePickerBasePage<TimeSpan>.GetSelectorsOrderedByCulturePattern(CultureInfo.CurrentCulture.DateTimeFormat.LongTimePattern.ToUpperInvariant(), new char[3]
      {
        'H',
        'M',
        'S'
      }, new LoopingSelector[3]
      {
        this.PrimarySelector,
        this.SecondarySelector,
        this.TertiarySelector
      }).Where<LoopingSelector>((Func<LoopingSelector, bool>) (s => !s.DataSource.IsEmpty));
    }

    protected override void OnOrientationChanged(OrientationChangedEventArgs e)
    {
      if (e == null)
        throw new ArgumentNullException(nameof (e));
      base.OnOrientationChanged(e);
      this.SystemTrayPlaceholder.Visibility = (PageOrientation.Portrait & e.Orientation) != PageOrientation.None ? Visibility.Visible : Visibility.Collapsed;
    }

    public override void SetFlowDirection(FlowDirection flowDirection)
    {
      this.HeaderTitle.FlowDirection = flowDirection;
      this.PrimarySelector.FlowDirection = flowDirection;
      this.SecondarySelector.FlowDirection = flowDirection;
      this.TertiarySelector.FlowDirection = flowDirection;
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Coding4Fun.Toolkit.Controls;component/ValuePicker/TimeSpanPicker/TimespanPickerPage.xaml", UriKind.Relative));
      this.VisibilityStates = (VisualStateGroup) this.FindName("VisibilityStates");
      this.Open = (VisualState) this.FindName("Open");
      this.Closed = (VisualState) this.FindName("Closed");
      this.PlaneProjection = (PlaneProjection) this.FindName("PlaneProjection");
      this.SystemTrayPlaceholder = (Rectangle) this.FindName("SystemTrayPlaceholder");
      this.HeaderTitle = (TextBlock) this.FindName("HeaderTitle");
      this.PrimarySelector = (LoopingSelector) this.FindName("PrimarySelector");
      this.SecondarySelector = (LoopingSelector) this.FindName("SecondarySelector");
      this.TertiarySelector = (LoopingSelector) this.FindName("TertiarySelector");
    }
  }
}
