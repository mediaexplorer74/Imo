// Decompiled with JetBrains decompiler
// Type: Coding4Fun.Toolkit.Controls.AppBarPrompt
// Assembly: Coding4Fun.Toolkit.Controls, Version=2.1.7.0, Culture=neutral, PublicKeyToken=c5fd7b72b1a17ce4
// MVID: 213D8A75-4B66-4004-95C2-D82C49ACB764
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\Coding4Fun.Toolkit.Controls.dll

using Clarity.Phone.Extensions;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace Coding4Fun.Toolkit.Controls
{
  public class AppBarPrompt : PopUp<string, PopUpResult>
  {
    protected StackPanel Body;
    private const string BodyName = "Body";
    private static readonly Color NullColor = Color.FromArgb((byte) 0, (byte) 0, (byte) 0, (byte) 0);
    private readonly AppBarPromptAction[] _theActions;

    public AppBarPrompt()
    {
      this.DefaultStyleKey = (object) typeof (AppBarPrompt);
      this.MainBodyDelay = TimeSpan.FromMilliseconds(100.0);
    }

    public AppBarPrompt(params AppBarPromptAction[] actions)
      : this()
    {
      this.AnimationType = DialogService.AnimationTypes.Swivel;
      this._theActions = actions;
    }

    public override void OnApplyTemplate()
    {
      base.OnApplyTemplate();
      this.VerifyAppBarBackgroundColor();
      this.Body = this.GetTemplateChild("Body") as StackPanel;
      if (this.Body == null)
        return;
      foreach (AppBarPromptAction theAction in this._theActions)
      {
        theAction.Parent = this;
        AppBarPromptItem appBarPromptItem = new AppBarPromptItem();
        appBarPromptItem.Content = theAction.Content;
        appBarPromptItem.Command = theAction.Command;
        this.Body.Children.Add((UIElement) appBarPromptItem);
      }
    }

    private void VerifyAppBarBackgroundColor()
    {
      Color backgroundColor = this.PopUpService.Page.ApplicationBar.BackgroundColor;
      if (!(backgroundColor != AppBarPrompt.NullColor))
        return;
      this.Background = (Brush) new SolidColorBrush(backgroundColor);
    }
  }
}
