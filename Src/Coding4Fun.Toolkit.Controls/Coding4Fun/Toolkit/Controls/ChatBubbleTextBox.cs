// Decompiled with JetBrains decompiler
// Type: Coding4Fun.Toolkit.Controls.ChatBubbleTextBox
// Assembly: Coding4Fun.Toolkit.Controls, Version=2.1.7.0, Culture=neutral, PublicKeyToken=c5fd7b72b1a17ce4
// MVID: 213D8A75-4B66-4004-95C2-D82C49ACB764
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\Coding4Fun.Toolkit.Controls.dll

using Coding4Fun.Toolkit.Controls.Common;
using System.Windows;
using System.Windows.Controls;


namespace Coding4Fun.Toolkit.Controls
{
  public class ChatBubbleTextBox : TextBox
  {
    protected ContentControl HintContentElement;
    private const string HintContentElementName = "HintContentElement";
    private bool _hasFocus;
    public static readonly DependencyProperty ChatBubbleDirectionProperty = DependencyProperty.Register(nameof (ChatBubbleDirection), typeof (ChatBubbleDirection), typeof (ChatBubbleTextBox), new PropertyMetadata((object) ChatBubbleDirection.UpperRight, new PropertyChangedCallback(ChatBubbleTextBox.OnChatBubbleDirectionChanged)));
    public static readonly DependencyProperty HintProperty = DependencyProperty.Register(nameof (Hint), typeof (string), typeof (ChatBubbleTextBox), new PropertyMetadata((object) ""));
    public static readonly DependencyProperty HintStyleProperty = DependencyProperty.Register(nameof (HintStyle), typeof (Style), typeof (ChatBubbleTextBox), new PropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty IsEquallySpacedProperty = DependencyProperty.Register(nameof (IsEquallySpaced), typeof (bool), typeof (ChatBubbleTextBox), new PropertyMetadata((object) true, new PropertyChangedCallback(ChatBubbleTextBox.OnIsEquallySpacedChanged)));
    private static bool _triggered = false;

    public ChatBubbleTextBox()
    {
      this.DefaultStyleKey = (object) typeof (ChatBubbleTextBox);
      this.TextChanged += new TextChangedEventHandler(this.ChatBubbleTextBoxTextChanged);
    }

    public ChatBubbleDirection ChatBubbleDirection
    {
      get => (ChatBubbleDirection) this.GetValue(ChatBubbleTextBox.ChatBubbleDirectionProperty);
      set => this.SetValue(ChatBubbleTextBox.ChatBubbleDirectionProperty, (object) value);
    }

    public string Hint
    {
      get => (string) this.GetValue(ChatBubbleTextBox.HintProperty);
      set => this.SetValue(ChatBubbleTextBox.HintProperty, (object) value);
    }

    public Style HintStyle
    {
      get => (Style) this.GetValue(ChatBubbleTextBox.HintStyleProperty);
      set => this.SetValue(ChatBubbleTextBox.HintStyleProperty, (object) value);
    }

    public override void OnApplyTemplate()
    {
      base.OnApplyTemplate();
      this.HintContentElement = this.GetTemplateChild("HintContentElement") as ContentControl;
      this.UpdateHintVisibility();
      this.UpdateChatBubbleDirection();
      this.UpdateIsEquallySpaced();
    }

    protected override void OnGotFocus(RoutedEventArgs e)
    {
      this._hasFocus = true;
      this.SetHintVisibility(Visibility.Collapsed);
      base.OnGotFocus(e);
    }

    protected override void OnLostFocus(RoutedEventArgs e)
    {
      this._hasFocus = false;
      this.UpdateHintVisibility();
      base.OnLostFocus(e);
    }

    private void UpdateHintVisibility()
    {
      if (this._hasFocus)
        return;
      this.SetHintVisibility(string.IsNullOrEmpty(this.Text) ? Visibility.Visible : Visibility.Collapsed);
    }

    private void SetHintVisibility(Visibility value)
    {
      if (this.HintContentElement == null)
        return;
      this.HintContentElement.Visibility = value;
    }

    private static void OnChatBubbleDirectionChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      if (!(d is ChatBubbleTextBox chatBubbleTextBox))
        return;
      chatBubbleTextBox.UpdateChatBubbleDirection();
    }

    private void UpdateChatBubbleDirection()
    {
      VisualStateManager.GoToState((Control) this, this.ChatBubbleDirection.ToString(), true);
    }

    private void ChatBubbleTextBoxTextChanged(object sender, TextChangedEventArgs e)
    {
      this.UpdateHintVisibility();
    }

    public bool IsEquallySpaced
    {
      get => (bool) this.GetValue(ChatBubbleTextBox.IsEquallySpacedProperty);
      set => this.SetValue(ChatBubbleTextBox.IsEquallySpacedProperty, (object) value);
    }

    private static void OnIsEquallySpacedChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      if (!(d is ChatBubbleTextBox chatBubbleTextBox))
        return;
      ChatBubbleTextBox._triggered = true;
      chatBubbleTextBox.UpdateIsEquallySpaced();
    }

    private void UpdateIsEquallySpaced()
    {
      int num = this.IsEquallySpaced ? ControlHelper.MagicSpacingNumber : (ChatBubbleTextBox._triggered ? -1 * ControlHelper.MagicSpacingNumber : 0);
      Thickness margin = this.Margin;
      switch (this.ChatBubbleDirection)
      {
        case ChatBubbleDirection.UpperRight:
        case ChatBubbleDirection.UpperLeft:
          margin.Bottom += (double) num;
          break;
        case ChatBubbleDirection.LowerRight:
        case ChatBubbleDirection.LowerLeft:
          margin.Top += (double) num;
          break;
      }
      this.Margin = margin;
    }
  }
}
