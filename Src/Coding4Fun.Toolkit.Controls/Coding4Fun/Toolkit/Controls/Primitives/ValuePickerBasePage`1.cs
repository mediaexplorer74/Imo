// Decompiled with JetBrains decompiler
// Type: Coding4Fun.Toolkit.Controls.Primitives.ValuePickerBasePage`1
// Assembly: Coding4Fun.Toolkit.Controls, Version=2.1.7.0, Culture=neutral, PublicKeyToken=c5fd7b72b1a17ce4
// MVID: 213D8A75-4B66-4004-95C2-D82C49ACB764
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\Coding4Fun.Toolkit.Controls.dll

using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;


namespace Coding4Fun.Toolkit.Controls.Primitives
{
  public abstract class ValuePickerBasePage<T> : PhoneApplicationPage, IValuePickerPage<T> where T : struct
  {
    private const string VisibilityGroupName = "VisibilityStates";
    private const string OpenVisibilityStateName = "Open";
    private const string ClosedVisibilityStateName = "Closed";
    private static readonly string StateKeyValue = "ValuePickerPageBase_State_Value" + (object) typeof (T);
    private LoopingSelector _primarySelectorPart;
    private LoopingSelector _secondarySelectorPart;
    private LoopingSelector _tertiarySelectorPart;
    private Storyboard _closedStoryboard;
    private T? _value;
    public static readonly DependencyProperty DialogTitleProperty = DependencyProperty.Register(nameof (DialogTitle), typeof (string), typeof (ValuePickerBasePage<T>), new PropertyMetadata((object) ""));

    protected ValuePickerBasePage() => this.DataContext = (object) this;

    protected void InitializeValuePickerPage(
      LoopingSelector primarySelector,
      LoopingSelector secondarySelector,
      LoopingSelector tertiarySelector)
    {
      if (primarySelector == null)
        throw new ArgumentNullException(nameof (primarySelector));
      if (secondarySelector == null)
        throw new ArgumentNullException(nameof (secondarySelector));
      if (tertiarySelector == null)
        throw new ArgumentNullException(nameof (tertiarySelector));
      this._primarySelectorPart = primarySelector;
      this._secondarySelectorPart = secondarySelector;
      this._tertiarySelectorPart = tertiarySelector;
      this._primarySelectorPart.DataSource.SelectionChanged += new EventHandler<SelectionChangedEventArgs>(this.OnDataSourceSelectionChanged);
      this._secondarySelectorPart.DataSource.SelectionChanged += new EventHandler<SelectionChangedEventArgs>(this.OnDataSourceSelectionChanged);
      this._tertiarySelectorPart.DataSource.SelectionChanged += new EventHandler<SelectionChangedEventArgs>(this.OnDataSourceSelectionChanged);
      this._primarySelectorPart.IsExpandedChanged += new DependencyPropertyChangedEventHandler(this.OnSelectorIsExpandedChanged);
      this._secondarySelectorPart.IsExpandedChanged += new DependencyPropertyChangedEventHandler(this.OnSelectorIsExpandedChanged);
      this._tertiarySelectorPart.IsExpandedChanged += new DependencyPropertyChangedEventHandler(this.OnSelectorIsExpandedChanged);
      this._primarySelectorPart.Visibility = Visibility.Collapsed;
      this._secondarySelectorPart.Visibility = Visibility.Collapsed;
      this._tertiarySelectorPart.Visibility = Visibility.Collapsed;
      int num = 0;
      foreach (LoopingSelector element in this.GetSelectorsOrderedByCulturePattern())
      {
        Grid.SetColumn((FrameworkElement) element, num);
        element.Visibility = Visibility.Visible;
        ++num;
      }
      if (VisualTreeHelper.GetChild((DependencyObject) this, 0) is FrameworkElement child)
      {
        foreach (VisualStateGroup visualStateGroup in (IEnumerable) VisualStateManager.GetVisualStateGroups(child))
        {
          if ("VisibilityStates" == visualStateGroup.Name)
          {
            foreach (VisualState state in (IEnumerable) visualStateGroup.States)
            {
              if ("Closed" == state.Name && state.Storyboard != null)
              {
                this._closedStoryboard = state.Storyboard;
                this._closedStoryboard.Completed += new EventHandler(this.OnClosedStoryboardCompleted);
              }
            }
          }
        }
      }
      if (this.ApplicationBar != null)
      {
        foreach (object button in (IEnumerable) this.ApplicationBar.Buttons)
        {
          if (button is IApplicationBarIconButton applicationBarIconButton)
          {
            switch (applicationBarIconButton.Text)
            {
              case "DONE":
                applicationBarIconButton.Text = Coding4Fun.Toolkit.Controls.Properties.Resources.DoneText;
                applicationBarIconButton.Click += new EventHandler(this.OnDoneButtonClick);
                continue;
              case "CANCEL":
                applicationBarIconButton.Text = Coding4Fun.Toolkit.Controls.Properties.Resources.CancelText;
                applicationBarIconButton.Click += new EventHandler(this.OnCancelButtonClick);
                continue;
              default:
                continue;
            }
          }
        }
      }
      VisualStateManager.GoToState((Control) this, "Open", true);
    }

    private void OnDataSourceSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      DataSource<T> dataSource = (DataSource<T>) sender;
      this._primarySelectorPart.DataSource.SelectedItem = dataSource.SelectedItem;
      this._secondarySelectorPart.DataSource.SelectedItem = dataSource.SelectedItem;
      this._tertiarySelectorPart.DataSource.SelectedItem = dataSource.SelectedItem;
    }

    private void OnSelectorIsExpandedChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      if (!(bool) e.NewValue)
        return;
      this._primarySelectorPart.IsExpanded = sender == this._primarySelectorPart;
      this._secondarySelectorPart.IsExpanded = sender == this._secondarySelectorPart;
      this._tertiarySelectorPart.IsExpanded = sender == this._tertiarySelectorPart;
    }

    private void OnDoneButtonClick(object sender, EventArgs e)
    {
      this.Value = new T?(((ValueWrapper<T>) this._primarySelectorPart.DataSource.SelectedItem).Value);
      this.ClosePickerPage();
    }

    private void OnCancelButtonClick(object sender, EventArgs e)
    {
      this.Value = new T?();
      this.ClosePickerPage();
    }

    protected override void OnBackKeyPress(CancelEventArgs e)
    {
      if (e == null)
        throw new ArgumentNullException(nameof (e));
      e.Cancel = true;
      this.ClosePickerPage();
    }

    private void ClosePickerPage()
    {
      if (this._closedStoryboard != null)
        VisualStateManager.GoToState((Control) this, "Closed", true);
      else
        this.OnClosedStoryboardCompleted((object) null, (EventArgs) null);
    }

    private void OnClosedStoryboardCompleted(object sender, EventArgs e)
    {
      this.NavigationService.GoBack();
    }

    protected abstract IEnumerable<LoopingSelector> GetSelectorsOrderedByCulturePattern();

    protected static IEnumerable<LoopingSelector> GetSelectorsOrderedByCulturePattern(
      string pattern,
      char[] patternCharacters,
      LoopingSelector[] selectors)
    {
      if (pattern == null)
        throw new ArgumentNullException(nameof (pattern));
      if (patternCharacters == null)
        throw new ArgumentNullException(nameof (patternCharacters));
      if (selectors == null)
        throw new ArgumentNullException(nameof (selectors));
      if (patternCharacters.Length != selectors.Length)
        throw new ArgumentException("Arrays must contain the same number of elements.");
      List<Tuple<int, LoopingSelector>> source = new List<Tuple<int, LoopingSelector>>(patternCharacters.Length);
      for (int index = 0; index < patternCharacters.Length; ++index)
        source.Add(new Tuple<int, LoopingSelector>(pattern.IndexOf(patternCharacters[index]), selectors[index]));
      return source.Where<Tuple<int, LoopingSelector>>((Func<Tuple<int, LoopingSelector>, bool>) (p => -1 != p.Item1)).OrderBy<Tuple<int, LoopingSelector>, int>((Func<Tuple<int, LoopingSelector>, int>) (p => p.Item1)).Select<Tuple<int, LoopingSelector>, LoopingSelector>((Func<Tuple<int, LoopingSelector>, LoopingSelector>) (p => p.Item2)).Where<LoopingSelector>((Func<LoopingSelector, bool>) (s => s != null));
    }

    public virtual T? Value
    {
      get => this._value;
      set
      {
        this._value = value;
        this.SetDataSources();
      }
    }

    private void SetDataSources()
    {
      ValueWrapper<T> newWrapper = this.GetNewWrapper(this.Value);
      if (newWrapper == null || this._primarySelectorPart == null || this._secondarySelectorPart == null || this._tertiarySelectorPart == null)
        return;
      this._primarySelectorPart.DataSource.SelectedItem = (object) newWrapper;
      this._secondarySelectorPart.DataSource.SelectedItem = (object) newWrapper;
      this._tertiarySelectorPart.DataSource.SelectedItem = (object) newWrapper;
    }

    public string DialogTitle
    {
      get => (string) this.GetValue(ValuePickerBasePage<T>.DialogTitleProperty);
      set => this.SetValue(ValuePickerBasePage<T>.DialogTitleProperty, (object) value);
    }

    protected abstract ValueWrapper<T> GetNewWrapper(T? value);

    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
      if (e == null)
        throw new ArgumentNullException(nameof (e));
      base.OnNavigatedFrom(e);
      if (!("app://external/" == e.Uri.ToString()))
        return;
      this.State[ValuePickerBasePage<T>.StateKeyValue] = (object) this.Value;
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
      if (e == null)
        throw new ArgumentNullException(nameof (e));
      base.OnNavigatedTo(e);
      bool flag = false;
      if (this.State.ContainsKey(ValuePickerBasePage<T>.StateKeyValue))
      {
        this.Value = this.State[ValuePickerBasePage<T>.StateKeyValue] as T?;
        if (this.NavigationService.CanGoBack)
        {
          this.NavigationService.GoBack();
          flag = true;
        }
      }
      if (flag)
        return;
      this.InitDataSource();
      this.SetDataSources();
    }

    public abstract void InitDataSource();

    public abstract void SetFlowDirection(FlowDirection flowDirection);
  }
}
