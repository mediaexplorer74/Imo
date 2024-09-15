// Decompiled with JetBrains decompiler
// Type: Coding4Fun.Toolkit.Controls.ValuePickerBase`1
// Assembly: Coding4Fun.Toolkit.Controls, Version=2.1.7.0, Culture=neutral, PublicKeyToken=c5fd7b72b1a17ce4
// MVID: 213D8A75-4B66-4004-95C2-D82C49ACB764
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\Coding4Fun.Toolkit.Controls.dll

using Coding4Fun.Toolkit.Controls.Common;
using Coding4Fun.Toolkit.Controls.Primitives;
using Microsoft.Phone.Controls;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;


namespace Coding4Fun.Toolkit.Controls
{
  [TemplatePart(Name = "ValueButton", Type = typeof (System.Windows.Controls.Primitives.ButtonBase))]
  public abstract class ValuePickerBase<T> : Control where T : struct
  {
    private const string ButtonPartName = "ValueButton";
    private System.Windows.Controls.Primitives.ButtonBase _valueButtonPart;
    private PhoneApplicationFrame _frame;
    private object _frameContentWhenOpened;
    private NavigationInTransition _savedNavigationInTransition;
    private NavigationOutTransition _savedNavigationOutTransition;
    private IValuePickerPage<T> _valuePickerPage;
    public static readonly DependencyProperty DialogTitleProperty = DependencyProperty.Register(nameof (DialogTitle), typeof (string), typeof (ValuePickerBase<T>), new PropertyMetadata((object) ""));
    public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof (Value), typeof (T?), typeof (ValuePickerBase<T>), new PropertyMetadata((object) null, new PropertyChangedCallback(ValuePickerBase<T>.OnValueChanged)));
    public static readonly DependencyProperty ValueStringProperty = DependencyProperty.Register(nameof (ValueString), typeof (string), typeof (ValuePickerBase<T>), (PropertyMetadata) null);
    public static readonly DependencyProperty ValueStringFormatProperty = DependencyProperty.Register(nameof (ValueStringFormat), typeof (string), typeof (ValuePickerBase<T>), new PropertyMetadata((object) null, new PropertyChangedCallback(ValuePickerBase<T>.OnValueStringFormatChanged)));
    public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(nameof (Header), typeof (object), typeof (ValuePickerBase<T>), (PropertyMetadata) null);
    public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register(nameof (HeaderTemplate), typeof (DataTemplate), typeof (ValuePickerBase<T>), (PropertyMetadata) null);
    public static readonly DependencyProperty PickerPageUriProperty = DependencyProperty.Register(nameof (PickerPageUri), typeof (Uri), typeof (ValuePickerBase<T>), (PropertyMetadata) null);

    public event RoutedPropertyChangedEventHandler<T> ValueChanged;

    public string DialogTitle
    {
      get => (string) this.GetValue(ValuePickerBase<T>.DialogTitleProperty);
      set => this.SetValue(ValuePickerBase<T>.DialogTitleProperty, (object) value);
    }

    public T? Value
    {
      get => (T?) this.GetValue(ValuePickerBase<T>.ValueProperty);
      set => this.SetValue(ValuePickerBase<T>.ValueProperty, (object) value);
    }

    private static void OnValueChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
    {
      ((ValuePickerBase<T>) o).OnValueChanged((T?) e.OldValue, (T?) e.NewValue);
    }

    private void OnValueChanged(T? oldValue, T? newValue)
    {
      this.UpdateValueString();
      if (!newValue.HasValue || !oldValue.HasValue)
        return;
      this.OnValueChanged(new RoutedPropertyChangedEventArgs<T>(oldValue.Value, newValue.Value));
    }

    protected virtual void OnValueChanged(RoutedPropertyChangedEventArgs<T> e)
    {
      RoutedPropertyChangedEventHandler<T> valueChanged = this.ValueChanged;
      if (valueChanged == null)
        return;
      valueChanged((object) this, e);
    }

    public string ValueString
    {
      get => (string) this.GetValue(ValuePickerBase<T>.ValueStringProperty);
      protected set => this.SetValue(ValuePickerBase<T>.ValueStringProperty, (object) value);
    }

    public string ValueStringFormat
    {
      get => (string) this.GetValue(ValuePickerBase<T>.ValueStringFormatProperty);
      set => this.SetValue(ValuePickerBase<T>.ValueStringFormatProperty, (object) value);
    }

    private static void OnValueStringFormatChanged(
      DependencyObject o,
      DependencyPropertyChangedEventArgs e)
    {
      ((ValuePickerBase<T>) o).OnValueStringFormatChanged();
    }

    private void OnValueStringFormatChanged() => this.UpdateValueString();

    public object Header
    {
      get => this.GetValue(ValuePickerBase<T>.HeaderProperty);
      set => this.SetValue(ValuePickerBase<T>.HeaderProperty, value);
    }

    public DataTemplate HeaderTemplate
    {
      get => (DataTemplate) this.GetValue(ValuePickerBase<T>.HeaderTemplateProperty);
      set => this.SetValue(ValuePickerBase<T>.HeaderTemplateProperty, (object) value);
    }

    public Uri PickerPageUri
    {
      get => (Uri) this.GetValue(ValuePickerBase<T>.PickerPageUriProperty);
      set => this.SetValue(ValuePickerBase<T>.PickerPageUriProperty, (object) value);
    }

    protected virtual string ValueStringFormatFallback => "{0}";

    public override void OnApplyTemplate()
    {
      if (this._valueButtonPart != null)
        this._valueButtonPart.Click -= new RoutedEventHandler(this.OnValueButtonClick);
      base.OnApplyTemplate();
      this._valueButtonPart = this.GetTemplateChild("ValueButton") as System.Windows.Controls.Primitives.ButtonBase;
      if (this._valueButtonPart == null)
        return;
      this._valueButtonPart.Click += new RoutedEventHandler(this.OnValueButtonClick);
    }

    private void OnValueButtonClick(object sender, RoutedEventArgs e) => this.OpenPicker();

    protected internal virtual void UpdateValueString()
    {
      this.ValueString = string.Format((IFormatProvider) CultureInfo.CurrentCulture, this.ValueStringFormat ?? this.ValueStringFormatFallback, (object) this.Value);
    }

    public void OpenPicker()
    {
      if ((Uri) null == this.PickerPageUri)
        throw new ArgumentException("PickerPageUri property must not be null.");
      if (this._frame != null)
        return;
      this._frame = ApplicationSpace.RootFrame as PhoneApplicationFrame;
      if (this._frame == null)
        return;
      this._frameContentWhenOpened = this._frame.Content;
      if (this._frameContentWhenOpened is UIElement contentWhenOpened)
      {
        this._savedNavigationInTransition = TransitionService.GetNavigationInTransition(contentWhenOpened);
        TransitionService.SetNavigationInTransition(contentWhenOpened, (NavigationInTransition) null);
        this._savedNavigationOutTransition = TransitionService.GetNavigationOutTransition(contentWhenOpened);
        TransitionService.SetNavigationOutTransition(contentWhenOpened, (NavigationOutTransition) null);
      }
      this._frame.Navigated += new NavigatedEventHandler(this.OnFrameNavigated);
      if (this._frame.GetType() == typeof (PhoneApplicationFrame))
        this._frame.NavigationStopped += new NavigationStoppedEventHandler(this.OnFrameNavigationStoppedOrFailed);
      this._frame.NavigationFailed += new NavigationFailedEventHandler(this.OnFrameNavigationStoppedOrFailed);
      this._frame.Navigate(this.PickerPageUri);
    }

    private void ClosePickerPage()
    {
      if (this._frame != null)
      {
        this._frame.Navigated -= new NavigatedEventHandler(this.OnFrameNavigated);
        this._frame.NavigationStopped -= new NavigationStoppedEventHandler(this.OnFrameNavigationStoppedOrFailed);
        this._frame.NavigationFailed -= new NavigationFailedEventHandler(this.OnFrameNavigationStoppedOrFailed);
        if (this._frameContentWhenOpened is UIElement contentWhenOpened)
        {
          TransitionService.SetNavigationInTransition(contentWhenOpened, this._savedNavigationInTransition);
          this._savedNavigationInTransition = (NavigationInTransition) null;
          TransitionService.SetNavigationOutTransition(contentWhenOpened, this._savedNavigationOutTransition);
          this._savedNavigationOutTransition = (NavigationOutTransition) null;
        }
        this._frame = (PhoneApplicationFrame) null;
        this._frameContentWhenOpened = (object) null;
      }
      if (this._valuePickerPage == null)
        return;
      T? nullable = this._valuePickerPage.Value;
      if (nullable.HasValue)
      {
        nullable = this._valuePickerPage.Value;
        this.Value = new T?(nullable.Value);
      }
      this._valuePickerPage = (IValuePickerPage<T>) null;
    }

    private void OnFrameNavigated(object sender, NavigationEventArgs e)
    {
      if (e.Content == this._frameContentWhenOpened)
      {
        this.ClosePickerPage();
      }
      else
      {
        if (this._valuePickerPage != null)
          return;
        this._valuePickerPage = e.Content as IValuePickerPage<T>;
        if (this._valuePickerPage == null)
          return;
        this.NavigateToNewPage(e.Content);
      }
    }

    protected virtual void NavigateToNewPage(object page)
    {
      if (!(page is IValuePickerPage<T> valuePickerPage))
        return;
      valuePickerPage.Value = new T?(this.Value.GetValueOrDefault());
      valuePickerPage.DialogTitle = this.DialogTitle;
      valuePickerPage.SetFlowDirection(this.FlowDirection);
    }

    private void OnFrameNavigationStoppedOrFailed(object sender, EventArgs e)
    {
      this.ClosePickerPage();
    }
  }
}
