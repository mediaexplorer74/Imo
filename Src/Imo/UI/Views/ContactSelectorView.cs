// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.UI.Views.ContactSelectorView
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using Imo.Phone.Controls;
using ImoSilverlightApp.Helpers;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace ImoSilverlightApp.UI.Views
{
  public class ContactSelectorView : UserControl
  {
    public static readonly DependencyProperty hasGroupsProperty = DependencyProperty.Register(nameof (HasGroups), typeof (bool), typeof (ContactSelectorView), new PropertyMetadata((object) false));
    public static readonly DependencyProperty isPhonebookOnlyProperty = DependencyProperty.Register(nameof (IsPhonebookOnly), typeof (bool), typeof (ContactSelectorView), new PropertyMetadata((object) false));
    private double oldExtentHeight;
    internal ScrollViewer scrollViewer;
    internal ImoTextBox searchTextBox;
    internal LongListSelector searchResultList;
    private bool _contentLoaded;

    public ContactSelectorView()
    {
      this.InitializeComponent();
      this.DataContext = (object) (this.ViewModel = new ContactSelectorViewModel((FrameworkElement) this));
    }

    internal ContactSelectorViewModel ViewModel { get; private set; }

    public bool HasGroups
    {
      get => (bool) this.GetValue(ContactSelectorView.hasGroupsProperty);
      set => this.SetValue(ContactSelectorView.hasGroupsProperty, (object) value);
    }

    public bool IsPhonebookOnly
    {
      get => (bool) this.GetValue(ContactSelectorView.isPhonebookOnlyProperty);
      set => this.SetValue(ContactSelectorView.isPhonebookOnlyProperty, (object) value);
    }

    private void ContactItem_MouseDown(object sender, GestureEventArgs e)
    {
      SelectorContactItem viewModelOf = VisualUtils.GetViewModelOf<SelectorContactItem>(sender);
      if (viewModelOf == null)
        return;
      this.ViewModel.SelectContact((SelectorItemBase) viewModelOf);
      e.Handled = true;
    }

    public void Clear() => this.ViewModel.Clear();

    private void Remove_Click(object sender, RoutedEventArgs e)
    {
      SelectorItemBase viewModelOf = VisualUtils.GetViewModelOf<SelectorItemBase>(sender);
      if (viewModelOf == null)
        return;
      this.ViewModel.DeselectItem(viewModelOf);
    }

    private void SearchTextBox_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Back)
      {
        if (!string.IsNullOrEmpty(this.searchTextBox.Text))
          return;
        SelectorItemBase items = this.ViewModel.SelectedItems.LastOrDefault<SelectorItemBase>();
        if (items == null)
          return;
        this.ViewModel.DeselectItem(items);
        this.ScrollToBottom();
        e.Handled = true;
      }
      else
      {
        if (e.Key != Key.Enter || string.IsNullOrEmpty(this.searchTextBox.Text))
          return;
        SelectorItemBase selectorItem = this.ViewModel.SearchResults.FirstOrDefault<SelectorItemBase>();
        if (selectorItem == null)
          return;
        this.ViewModel.SelectContact(selectorItem);
        this.ScrollToBottom();
        e.Handled = true;
      }
    }

    private void scrollViewer_SizeChanged(object sender, SizeChangedEventArgs e)
    {
      this.ScrollToBottom();
    }

    private void ScrollToBottom()
    {
      if (this.scrollViewer.ExtentHeight - this.oldExtentHeight <= 0.1)
        return;
      this.scrollViewer.ScrollToVerticalOffset(this.scrollViewer.ExtentHeight - this.scrollViewer.ViewportHeight);
      this.oldExtentHeight = this.scrollViewer.ExtentHeight;
    }

    internal void FocusTextInput() => this.searchTextBox.FocusTextInput();

    private void PhonebookContactItem_MouseDown(object sender, GestureEventArgs e)
    {
      SelectorPhonebookContactItem viewModelOf = VisualUtils.GetViewModelOf<SelectorPhonebookContactItem>(sender);
      if (viewModelOf == null)
        return;
      this.ViewModel.SelectContact((SelectorItemBase) viewModelOf);
      e.Handled = true;
    }

    private void scrollViewer_ManipulationStarted(object sender, ManipulationStartedEventArgs e)
    {
      this.Focus();
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/ImoSilverlightApp;component/UI/Views/ContactSelectorView.xaml", UriKind.Relative));
      this.scrollViewer = (ScrollViewer) this.FindName("scrollViewer");
      this.searchTextBox = (ImoTextBox) this.FindName("searchTextBox");
      this.searchResultList = (LongListSelector) this.FindName("searchResultList");
    }
  }
}
