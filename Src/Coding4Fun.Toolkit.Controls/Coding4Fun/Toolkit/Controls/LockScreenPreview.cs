// Decompiled with JetBrains decompiler
// Type: Coding4Fun.Toolkit.Controls.LockScreenPreview
// Assembly: Coding4Fun.Toolkit.Controls, Version=2.1.7.0, Culture=neutral, PublicKeyToken=c5fd7b72b1a17ce4
// MVID: 213D8A75-4B66-4004-95C2-D82C49ACB764
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\Coding4Fun.Toolkit.Controls.dll

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace Coding4Fun.Toolkit.Controls
{
  [TemplatePart(Name = "TimeText", Type = typeof (TextBlock))]
  [TemplatePart(Name = "DayText", Type = typeof (TextBlock))]
  [TemplatePart(Name = "DateText", Type = typeof (TextBlock))]
  public class LockScreenPreview : ContentControl
  {
    public const string TimeText = "TimeText";
    public const string DayText = "DayText";
    public const string DateText = "DateText";
    public const string LockScreenImage = "LockScreenImage";
    public static readonly DependencyProperty LockScreenImageSourceProperty = DependencyProperty.Register(nameof (LockScreenImageSource), typeof (ImageSource), typeof (LockScreenPreview), new PropertyMetadata((object) null));
    public static readonly DependencyProperty TextLine1Property = DependencyProperty.Register(nameof (TextLine1), typeof (string), typeof (LockScreenPreview), new PropertyMetadata((object) null));
    public static readonly DependencyProperty TextLine2Property = DependencyProperty.Register(nameof (TextLine2), typeof (string), typeof (LockScreenPreview), new PropertyMetadata((object) null));
    public static readonly DependencyProperty TextLine3Property = DependencyProperty.Register(nameof (TextLine3), typeof (string), typeof (LockScreenPreview), new PropertyMetadata((object) null));
    public static readonly DependencyProperty NotificationIconSourceProperty = DependencyProperty.Register(nameof (NotificationIconSource), typeof (ImageSource), typeof (LockScreenPreview), new PropertyMetadata((object) null));
    public static readonly DependencyProperty ShowNotificationCountProperty = DependencyProperty.Register(nameof (ShowNotificationCount), typeof (bool), typeof (LockScreenPreview), new PropertyMetadata((object) true));
    public static readonly DependencyProperty Support720Property = DependencyProperty.Register(nameof (Support720), typeof (bool), typeof (LockScreenPreview), new PropertyMetadata((object) false));

    public ImageSource LockScreenImageSource
    {
      get => (ImageSource) this.GetValue(LockScreenPreview.LockScreenImageSourceProperty);
      set => this.SetValue(LockScreenPreview.LockScreenImageSourceProperty, (object) value);
    }

    public string TextLine1
    {
      get => (string) this.GetValue(LockScreenPreview.TextLine1Property);
      set => this.SetValue(LockScreenPreview.TextLine1Property, (object) value);
    }

    public string TextLine2
    {
      get => (string) this.GetValue(LockScreenPreview.TextLine2Property);
      set => this.SetValue(LockScreenPreview.TextLine2Property, (object) value);
    }

    public string TextLine3
    {
      get => (string) this.GetValue(LockScreenPreview.TextLine3Property);
      set => this.SetValue(LockScreenPreview.TextLine3Property, (object) value);
    }

    public ImageSource NotificationIconSource
    {
      get => (ImageSource) this.GetValue(LockScreenPreview.NotificationIconSourceProperty);
      set => this.SetValue(LockScreenPreview.NotificationIconSourceProperty, (object) value);
    }

    public bool ShowNotificationCount
    {
      get => (bool) this.GetValue(LockScreenPreview.ShowNotificationCountProperty);
      set => this.SetValue(LockScreenPreview.ShowNotificationCountProperty, (object) value);
    }

    public bool Support720
    {
      get => (bool) this.GetValue(LockScreenPreview.Support720Property);
      set => this.SetValue(LockScreenPreview.Support720Property, (object) value);
    }

    public LockScreenPreview() => this.DefaultStyleKey = (object) typeof (LockScreenPreview);

    public override void OnApplyTemplate()
    {
      base.OnApplyTemplate();
      DateTime now = DateTime.Now;
      CultureInfo currentUiCulture = CultureInfo.CurrentUICulture;
      if (this.GetTemplateChild("DateText") is TextBlock templateChild1)
        templateChild1.Text = now.ToString(currentUiCulture.DateTimeFormat.MonthDayPattern);
      if (this.GetTemplateChild("DayText") is TextBlock templateChild2)
      {
        string str = now.DayOfWeek.ToString();
        templateChild2.Text = str;
      }
      if (!(this.GetTemplateChild("TimeText") is TextBlock templateChild3))
        return;
      templateChild3.Text = now.ToString(currentUiCulture.DateTimeFormat.ShortTimePattern);
      if (!string.IsNullOrEmpty(currentUiCulture.DateTimeFormat.AMDesignator))
        templateChild3.Text = templateChild3.Text.Replace(currentUiCulture.DateTimeFormat.AMDesignator, string.Empty);
      if (string.IsNullOrEmpty(currentUiCulture.DateTimeFormat.PMDesignator))
        return;
      templateChild3.Text = templateChild3.Text.Replace(currentUiCulture.DateTimeFormat.PMDesignator, string.Empty);
    }
  }
}
