// Decompiled with JetBrains decompiler
// Type: Coding4Fun.Toolkit.Controls.ActionPopUp`2
// Assembly: Coding4Fun.Toolkit.Controls, Version=2.1.7.0, Culture=neutral, PublicKeyToken=c5fd7b72b1a17ce4
// MVID: 213D8A75-4B66-4004-95C2-D82C49ACB764
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\Coding4Fun.Toolkit.Controls.dll

using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;


namespace Coding4Fun.Toolkit.Controls
{
  public class ActionPopUp<T, TPopUpResult> : PopUp<T, TPopUpResult>
  {
    private const string ActionButtonAreaName = "actionButtonArea";
    protected Panel ActionButtonArea;
    public readonly DependencyProperty ActionPopUpButtonsProperty = DependencyProperty.Register(nameof (ActionPopUpButtons), typeof (List<Button>), typeof (ActionPopUp<T, TPopUpResult>), new PropertyMetadata((object) new List<Button>(), new PropertyChangedCallback(ActionPopUp<T, TPopUpResult>.OnActionPopUpButtonsChanged)));

    public override void OnApplyTemplate()
    {
      this.Focus();
      base.OnApplyTemplate();
      this.ActionButtonArea = this.GetTemplateChild("actionButtonArea") as Panel;
      this.SetButtons();
    }

    private void SetButtons()
    {
      if (this.ActionButtonArea == null)
        return;
      this.ActionButtonArea.Children.Clear();
      bool flag = false;
      foreach (Button actionPopUpButton in this.ActionPopUpButtons)
      {
        this.ActionButtonArea.Children.Add((UIElement) actionPopUpButton);
        flag |= actionPopUpButton.Content != null;
      }
      if (!flag)
        return;
      this.ActionButtonArea.Margin = new Thickness();
    }

    private static void OnActionPopUpButtonsChanged(
      DependencyObject o,
      DependencyPropertyChangedEventArgs e)
    {
      ActionPopUp<T, TPopUpResult> actionPopUp = (ActionPopUp<T, TPopUpResult>) o;
      if (actionPopUp == null || e.NewValue == e.OldValue)
        return;
      actionPopUp.SetButtons();
    }

    public List<Button> ActionPopUpButtons
    {
      get => (List<Button>) this.GetValue(this.ActionPopUpButtonsProperty);
      set => this.SetValue(this.ActionPopUpButtonsProperty, (object) value);
    }
  }
}
