// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.UI.Views.ImoTextBox
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using Microsoft.Phone.Controls;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;


namespace ImoSilverlightApp.UI.Views
{
  public class ImoTextBox : UserControl
  {
    public static readonly DependencyProperty textProperty = DependencyProperty.Register(nameof (Text), typeof (string), typeof (ImoTextBox), new PropertyMetadata((object) string.Empty));
    public static readonly DependencyProperty placeholderTextProperty = DependencyProperty.Register(nameof (PlaceholderText), typeof (string), typeof (ImoTextBox), new PropertyMetadata((object) string.Empty));
    public static readonly DependencyProperty maxLengthProperty = DependencyProperty.Register(nameof (MaxLength), typeof (int), typeof (ImoTextBox), new PropertyMetadata((object) 0));
    public static readonly DependencyProperty inputScopeProperty = DependencyProperty.Register(nameof (InputScope), typeof (InputScope), typeof (ImoTextBox), new PropertyMetadata((PropertyChangedCallback) null));
    internal UserControl imoTextBoxRoot;
    internal PhoneTextBox textBox;
    internal VisualStateGroup FocusStates;
    internal VisualState Focused;
    internal VisualState Unfocused;
    internal Border border;
    private bool _contentLoaded;

    public ImoTextBox()
    {
      this.InitializeComponent();
      this.textBox.GotFocus += new RoutedEventHandler(this.TextBox_GotFocus);
      this.textBox.LostFocus += new RoutedEventHandler(this.TextBox_LostFocus);
    }

    private void TextBox_LostFocus(object sender, RoutedEventArgs e)
    {
      this.border.BorderBrush = (Brush) Application.Current.Resources[(object) "UIElementBorderBrush"];
    }

    private void TextBox_GotFocus(object sender, RoutedEventArgs e)
    {
      this.border.BorderBrush = (Brush) Application.Current.Resources[(object) "UIElementFocusedBorderBrush"];
    }

    public string Text
    {
      get => this.GetValue(ImoTextBox.textProperty)?.ToString();
      set => this.SetValue(ImoTextBox.textProperty, (object) value);
    }

    public string PlaceholderText
    {
      get => this.GetValue(ImoTextBox.placeholderTextProperty)?.ToString();
      set => this.SetValue(ImoTextBox.placeholderTextProperty, (object) value);
    }

    public int MaxLength
    {
      get => (int) this.GetValue(ImoTextBox.maxLengthProperty);
      set => this.SetValue(ImoTextBox.maxLengthProperty, (object) value);
    }

    public InputScope InputScope
    {
      get => (InputScope) this.GetValue(ImoTextBox.inputScopeProperty);
      set => this.SetValue(ImoTextBox.inputScopeProperty, (object) value);
    }

    public event TextChangedEventHandler TextChanged;

    private void OnTextChanged(TextChangedEventArgs args)
    {
      if (this.TextChanged == null)
        return;
      this.TextChanged((object) this, args);
    }

    private void textBox_TextChanged(object sender, TextChangedEventArgs e)
    {
      (sender as TextBox).GetBindingExpression(TextBox.TextProperty).UpdateSource();
      this.OnTextChanged(e);
    }

    public void FocusTextInput() => this.textBox.Focus();

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/ImoSilverlightApp;component/UI/Views/ImoTextBox.xaml", UriKind.Relative));
      this.imoTextBoxRoot = (UserControl) this.FindName("imoTextBoxRoot");
      this.textBox = (PhoneTextBox) this.FindName("textBox");
      this.FocusStates = (VisualStateGroup) this.FindName("FocusStates");
      this.Focused = (VisualState) this.FindName("Focused");
      this.Unfocused = (VisualState) this.FindName("Unfocused");
      this.border = (Border) this.FindName("border");
    }
  }
}
