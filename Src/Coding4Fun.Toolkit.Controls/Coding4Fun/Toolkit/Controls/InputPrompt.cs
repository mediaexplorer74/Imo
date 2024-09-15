// Decompiled with JetBrains decompiler
// Type: Coding4Fun.Toolkit.Controls.InputPrompt
// Assembly: Coding4Fun.Toolkit.Controls, Version=2.1.7.0, Culture=neutral, PublicKeyToken=c5fd7b72b1a17ce4
// MVID: 213D8A75-4B66-4004-95C2-D82C49ACB764
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\Coding4Fun.Toolkit.Controls.dll

using Coding4Fun.Toolkit.Controls.Binding;
using Coding4Fun.Toolkit.Controls.Common;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace Coding4Fun.Toolkit.Controls
{
  public class InputPrompt : UserPrompt
  {
    protected TextBox InputBox;
    private const string InputBoxName = "inputBox";
    public static readonly DependencyProperty IsSubmitOnEnterKeyProperty = DependencyProperty.Register(nameof (IsSubmitOnEnterKey), typeof (bool), typeof (InputPrompt), new PropertyMetadata((object) true, new PropertyChangedCallback(InputPrompt.OnIsSubmitOnEnterKeyPropertyChanged)));
    public static readonly DependencyProperty MessageTextWrappingProperty = DependencyProperty.Register(nameof (MessageTextWrapping), typeof (TextWrapping), typeof (InputPrompt), new PropertyMetadata((object) TextWrapping.NoWrap));
    public static readonly DependencyProperty InputScopeProperty = DependencyProperty.Register(nameof (InputScope), typeof (InputScope), typeof (InputPrompt), (PropertyMetadata) null);

    public InputPrompt() => this.DefaultStyleKey = (object) typeof (InputPrompt);

    public override async void OnApplyTemplate()
    {
      base.OnApplyTemplate();
      this.InputBox = this.GetTemplateChild("inputBox") as TextBox;
      if (this.InputBox == null)
        return;
      this.SetBinding(UserPrompt.ValueProperty, new System.Windows.Data.Binding()
      {
        Source = (object) this.InputBox,
        Path = new PropertyPath("Text", new object[0])
      });
      TextBinding.SetUpdateSourceOnChange((DependencyObject) this.InputBox, true);
      this.HookUpEventForIsSubmitOnEnterKey();
      if (ApplicationSpace.IsDesignMode)
        return;
      await this.DelayInputSelect();
    }

    private async Task DelayInputSelect()
    {
      await Task.Delay(250);
      ApplicationSpace.CurrentDispatcher.BeginInvoke((Action) (() =>
      {
        this.InputBox.Focus();
        this.InputBox.SelectAll();
      }));
    }

    private void HookUpEventForIsSubmitOnEnterKey()
    {
      this.InputBox = this.GetTemplateChild("inputBox") as TextBox;
      if (this.InputBox == null)
        return;
      this.InputBox.KeyDown -= new KeyEventHandler(this.InputBoxKeyDown);
      if (!this.IsSubmitOnEnterKey)
        return;
      this.InputBox.KeyDown += new KeyEventHandler(this.InputBoxKeyDown);
    }

    private void InputBoxKeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key != Key.Enter)
        return;
      this.OnCompleted(new PopUpEventArgs<string, PopUpResult>()
      {
        Result = this.Value,
        PopUpResult = PopUpResult.Ok
      });
    }

    private static void OnIsSubmitOnEnterKeyPropertyChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      if (!(d is InputPrompt inputPrompt))
        return;
      inputPrompt.HookUpEventForIsSubmitOnEnterKey();
    }

    public bool IsSubmitOnEnterKey
    {
      get => (bool) this.GetValue(InputPrompt.IsSubmitOnEnterKeyProperty);
      set => this.SetValue(InputPrompt.IsSubmitOnEnterKeyProperty, (object) value);
    }

    public TextWrapping MessageTextWrapping
    {
      get => (TextWrapping) this.GetValue(InputPrompt.MessageTextWrappingProperty);
      set => this.SetValue(InputPrompt.MessageTextWrappingProperty, (object) value);
    }

    public InputScope InputScope
    {
      get => (InputScope) this.GetValue(InputPrompt.InputScopeProperty);
      set => this.SetValue(InputPrompt.InputScopeProperty, (object) value);
    }
  }
}
