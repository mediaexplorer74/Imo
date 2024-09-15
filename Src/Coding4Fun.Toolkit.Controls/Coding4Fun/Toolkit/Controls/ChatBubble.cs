// Decompiled with JetBrains decompiler
// Type: Coding4Fun.Toolkit.Controls.ChatBubble
// Assembly: Coding4Fun.Toolkit.Controls, Version=2.1.7.0, Culture=neutral, PublicKeyToken=c5fd7b72b1a17ce4
// MVID: 213D8A75-4B66-4004-95C2-D82C49ACB764
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\Coding4Fun.Toolkit.Controls.dll

using Coding4Fun.Toolkit.Controls.Common;
using System.Windows;
using System.Windows.Controls;


namespace Coding4Fun.Toolkit.Controls
{
  public class ChatBubble : ContentControl
  {
    public static readonly DependencyProperty ChatBubbleDirectionProperty = DependencyProperty.Register(nameof (ChatBubbleDirection), typeof (ChatBubbleDirection), typeof (ChatBubble), new PropertyMetadata((object) ChatBubbleDirection.UpperRight, new PropertyChangedCallback(ChatBubble.OnChatBubbleDirectionChanged)));
    public static readonly DependencyProperty IsEquallySpacedProperty = DependencyProperty.Register(nameof (IsEquallySpaced), typeof (bool), typeof (ChatBubble), new PropertyMetadata((object) true, new PropertyChangedCallback(ChatBubble.OnIsEquallySpacedChanged)));
    private static bool _triggered = false;

    public ChatBubble()
    {
      this.DefaultStyleKey = (object) typeof (ChatBubble);
      this.IsEnabledChanged += new DependencyPropertyChangedEventHandler(this.ChatBubbleIsEnabledChanged);
    }

    private void ChatBubbleIsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      this.UpdateIsEnabledVisualState();
    }

    public override void OnApplyTemplate()
    {
      base.OnApplyTemplate();
      this.UpdateChatBubbleDirection();
      this.UpdateIsEnabledVisualState();
      this.UpdateIsEquallySpaced();
    }

    public ChatBubbleDirection ChatBubbleDirection
    {
      get => (ChatBubbleDirection) this.GetValue(ChatBubble.ChatBubbleDirectionProperty);
      set => this.SetValue(ChatBubble.ChatBubbleDirectionProperty, (object) value);
    }

    private static void OnChatBubbleDirectionChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      if (!(d is ChatBubble chatBubble))
        return;
      chatBubble.UpdateChatBubbleDirection();
    }

    private void UpdateChatBubbleDirection()
    {
      VisualStateManager.GoToState((Control) this, this.ChatBubbleDirection.ToString(), true);
    }

    private void UpdateIsEnabledVisualState()
    {
      VisualStateManager.GoToState((Control) this, this.IsEnabled ? "Normal" : "Disabled", true);
    }

    public bool IsEquallySpaced
    {
      get => (bool) this.GetValue(ChatBubble.IsEquallySpacedProperty);
      set => this.SetValue(ChatBubble.IsEquallySpacedProperty, (object) value);
    }

    private static void OnIsEquallySpacedChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      if (!(d is ChatBubble chatBubble))
        return;
      ChatBubble._triggered = true;
      chatBubble.UpdateIsEquallySpaced();
    }

    private void UpdateIsEquallySpaced()
    {
      int num = this.IsEquallySpaced ? ControlHelper.MagicSpacingNumber : (ChatBubble._triggered ? -1 * ControlHelper.MagicSpacingNumber : 0);
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
