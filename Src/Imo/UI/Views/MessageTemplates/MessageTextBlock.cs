// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.UI.Views.MessageTemplates.MessageTextBlock
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Storage.Models;
using NLog;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;


namespace ImoSilverlightApp.UI.Views.MessageTemplates
{
  public class MessageTextBlock : ContentControl
  {
    private static readonly Logger log = LogManager.GetLogger(typeof (MessageTextBlock).Name);
    private ChatLog chatLog;
    private const string urlPrefix = "(?:(?:https?|ftp)://|\\bwww\\.)";
    private const string urlDomain = "[A-Za-z0-9\\.\\-]+";
    private const string urlPort = "(?:\\:[0-9]{1,5})?";
    private const string urlRegexString = "((?:(?:https?|ftp)://|\\bwww\\.)[A-Za-z0-9\\.\\-]+(?:\\:[0-9]{1,5})?(?:\\/)?\\S*)";
    private static readonly Regex tokenizerRegex;
    private static readonly Regex urlRegex;
    private static readonly ResourceDictionary patternsDictionary;
    public static readonly DependencyProperty MessageProperty = DependencyProperty.Register(nameof (Message), typeof (Message), typeof (MessageTextBlock), new PropertyMetadata((object) null, new PropertyChangedCallback(MessageTextBlock.MessageChangedCallback)));
    private RichTextBox textBlock;
    private Paragraph paragraph;

    static MessageTextBlock()
    {
      MessageTextBlock.patternsDictionary = (ResourceDictionary) Application.Current.Resources[(object) "SmileyStrings"];
      MessageTextBlock.tokenizerRegex = new Regex("((?:(?:https?|ftp)://|\\bwww\\.)[A-Za-z0-9\\.\\-]+(?:\\:[0-9]{1,5})?(?:\\/)?\\S*)" + "|" + string.Join("|", MessageTextBlock.patternsDictionary.Keys.Cast<object>().Select<object, string>((Func<object, string>) (x => string.Format("({0})", (object) Regex.Escape((string) x)))).ToArray<string>()) + "|(\\r\\n)|(\\n)");
      MessageTextBlock.urlRegex = new Regex("^((?:(?:https?|ftp)://|\\bwww\\.)[A-Za-z0-9\\.\\-]+(?:\\:[0-9]{1,5})?(?:\\/)?\\S*)$");
    }

    public Message Message
    {
      get => (Message) this.GetValue(MessageTextBlock.MessageProperty);
      set => this.SetValue(MessageTextBlock.MessageProperty, (object) value);
    }

    public bool IsChatLogMessage => this.chatLog != null;

    public MessageTextBlock()
    {
      RichTextBox richTextBox = new RichTextBox();
      richTextBox.FontSize = 24.0;
      richTextBox.HorizontalAlignment = HorizontalAlignment.Stretch;
      richTextBox.HorizontalContentAlignment = HorizontalAlignment.Left;
      this.textBlock = richTextBox;
      this.HorizontalAlignment = HorizontalAlignment.Stretch;
      this.HorizontalContentAlignment = HorizontalAlignment.Stretch;
      this.paragraph = new Paragraph();
      this.paragraph.TextAlignment = TextAlignment.Left;
      this.textBlock.Blocks.Add((Block) this.paragraph);
      this.textBlock.TextWrapping = TextWrapping.Wrap;
      this.Content = (object) this.textBlock;
      this.Loaded += (RoutedEventHandler) ((s, e) => this.InvalidateRuns());
    }

    private static void MessageChangedCallback(
      DependencyObject d,
      DependencyPropertyChangedEventArgs args)
    {
      (d as MessageTextBlock).InvalidateRuns();
    }

    private void InvalidateRuns()
    {
      this.paragraph.Inlines.Clear();
      if (this.Message == null)
        return;
      foreach (string str in MessageTextBlock.tokenizerRegex.Split(this.Message.Msg))
      {
        if (!string.IsNullOrEmpty(str))
        {
          if (str == "\r\n" || str == "\n")
            this.paragraph.Inlines.Add(this.CreateRun(str));
          else if (MessageTextBlock.patternsDictionary.Contains((object) str))
            this.paragraph.Inlines.Add(this.CreateSmileyInline(str));
          else if (MessageTextBlock.urlRegex.IsMatch(str))
            this.paragraph.Inlines.Add(this.CreateHyperlinkInline(str));
          else
            this.paragraph.Inlines.Add(this.CreateRun(str));
        }
      }
    }

    private Inline CreateSmileyInline(string token)
    {
      ContentControl contentControl1 = new ContentControl();
      contentControl1.Margin = new Thickness(3.0, 3.0, 3.0, -3.0);
      contentControl1.Width = 18.0;
      contentControl1.Height = 18.0;
      contentControl1.Foreground = this.Foreground;
      contentControl1.Template = (ControlTemplate) Application.Current.Resources[MessageTextBlock.patternsDictionary[(object) token]];
      ContentControl contentControl2 = contentControl1;
      return (Inline) new InlineUIContainer()
      {
        Child = (UIElement) contentControl2
      };
    }

    private Inline CreateHyperlinkInline(string token)
    {
      Uri uri;
      try
      {
        uri = !(!token.StartsWith("http") | !token.StartsWith("ftp")) ? new Uri(token, UriKind.Absolute) : new Uri("http://" + token, UriKind.Absolute);
      }
      catch (Exception ex)
      {
        return this.CreateRun(token);
      }
      Hyperlink hyperlinkInline = new Hyperlink();
      hyperlinkInline.NavigateUri = uri;
      hyperlinkInline.TargetName = "_blank";
      InlineCollection inlines = hyperlinkInline.Inlines;
      Run run = new Run();
      run.Text = token;
      run.Foreground = this.Message.MessageType == MessageType.Sent ? (Brush) Application.Current.Resources[(object) "MyLinkTextForegroundBrush"] : (Brush) Application.Current.Resources[(object) "LinkTextForegroundBrush"];
      inlines.Add((Inline) run);
      return (Inline) hyperlinkInline;
    }

    private Inline CreateRun(string token)
    {
      Run run = new Run();
      run.Text = token;
      run.Foreground = this.Foreground;
      return (Inline) run;
    }
  }
}
