// Decompiled with JetBrains decompiler
// Type: Coding4Fun.Toolkit.Controls.MessagePrompt
// Assembly: Coding4Fun.Toolkit.Controls, Version=2.1.7.0, Culture=neutral, PublicKeyToken=c5fd7b72b1a17ce4
// MVID: 213D8A75-4B66-4004-95C2-D82C49ACB764
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\Coding4Fun.Toolkit.Controls.dll

using System;
using System.Windows;
using System.Windows.Controls;


namespace Coding4Fun.Toolkit.Controls
{
  public class MessagePrompt : UserPrompt
  {
    public static readonly DependencyProperty BodyProperty = DependencyProperty.Register(nameof (Body), typeof (object), typeof (MessagePrompt), new PropertyMetadata((PropertyChangedCallback) null));

    public MessagePrompt()
    {
      this.DefaultStyleKey = (object) typeof (MessagePrompt);
      this.MessageChanged = new Action(this.SetBodyMessage);
    }

    protected internal void SetBodyMessage()
    {
      this.Body = (object) new TextBlock()
      {
        Text = this.Message,
        TextWrapping = TextWrapping.Wrap
      };
    }

    public object Body
    {
      get => this.GetValue(MessagePrompt.BodyProperty);
      set => this.SetValue(MessagePrompt.BodyProperty, value);
    }
  }
}
