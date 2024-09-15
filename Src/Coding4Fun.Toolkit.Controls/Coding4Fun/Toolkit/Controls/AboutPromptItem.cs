// Decompiled with JetBrains decompiler
// Type: Coding4Fun.Toolkit.Controls.AboutPromptItem
// Assembly: Coding4Fun.Toolkit.Controls, Version=2.1.7.0, Culture=neutral, PublicKeyToken=c5fd7b72b1a17ce4
// MVID: 213D8A75-4B66-4004-95C2-D82C49ACB764
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\Coding4Fun.Toolkit.Controls.dll

using Coding4Fun.Toolkit.Controls.Common;
using Microsoft.Phone.Tasks;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace Coding4Fun.Toolkit.Controls
{
  public class AboutPromptItem : Control
  {
    private TextBlock _emailAddress;
    private TextBlock _website;
    private TextBlock _author;
    private const string EmailAddressName = "emailAddress";
    private const string WebsiteName = "website";
    private const string AuthorTxtBlockName = "author";
    private const string Http = "http://";
    private const string Https = "https://";
    private const string Twitter = "www.twitter.com";
    private string _webSiteUrl;
    public static readonly DependencyProperty WebSiteDisplayProperty = DependencyProperty.Register(nameof (WebSiteDisplay), typeof (string), typeof (AboutPromptItem), new PropertyMetadata((object) ""));
    public static readonly DependencyProperty RoleProperty = DependencyProperty.Register(nameof (Role), typeof (string), typeof (AboutPromptItem), new PropertyMetadata((object) ""));
    public static readonly DependencyProperty EmailAddressProperty = DependencyProperty.Register(nameof (EmailAddress), typeof (string), typeof (AboutPromptItem), new PropertyMetadata((object) ""));
    public static readonly DependencyProperty AuthorNameProperty = DependencyProperty.Register(nameof (AuthorName), typeof (string), typeof (AboutPromptItem), new PropertyMetadata((object) ""));

    public AboutPromptItem() => this.DefaultStyleKey = (object) typeof (AboutPromptItem);

    public override void OnApplyTemplate()
    {
      base.OnApplyTemplate();
      if (this._website != null)
        this._website.ManipulationCompleted -= new EventHandler<ManipulationCompletedEventArgs>(this.websiteClick_ManipulationCompleted);
      if (this._emailAddress != null)
        this._emailAddress.ManipulationCompleted -= new EventHandler<ManipulationCompletedEventArgs>(this.email_ManipulationCompleted);
      this._emailAddress = this.GetTemplateChild("emailAddress") as TextBlock;
      this._website = this.GetTemplateChild("website") as TextBlock;
      this._author = this.GetTemplateChild("author") as TextBlock;
      AboutPromptItem.SetVisibility(this._emailAddress);
      AboutPromptItem.SetVisibility(this._website);
      AboutPromptItem.SetVisibility(this._author);
      if (this._emailAddress != null)
        this._emailAddress.ManipulationCompleted += new EventHandler<ManipulationCompletedEventArgs>(this.email_ManipulationCompleted);
      if (this._website == null)
        return;
      this._website.ManipulationCompleted += new EventHandler<ManipulationCompletedEventArgs>(this.websiteClick_ManipulationCompleted);
    }

    protected internal void email_ManipulationCompleted(
      object sender,
      ManipulationCompletedEventArgs e)
    {
      new EmailComposeTask()
      {
        To = this.EmailAddress,
        Subject = (PhoneHelper.GetAppAttribute("Title") + " Feedback")
      }.Show();
    }

    protected internal void websiteClick_ManipulationCompleted(
      object sender,
      ManipulationCompletedEventArgs e)
    {
      new WebBrowserTask()
      {
        Uri = new Uri(this.WebSiteUrl)
      }.Show();
    }

    private static void SetVisibility(TextBlock control)
    {
      if (control == null)
        return;
      control.Visibility = string.IsNullOrEmpty(control.Text) ? Visibility.Collapsed : Visibility.Visible;
    }

    public string WebSiteUrl
    {
      get => this._webSiteUrl;
      set
      {
        this._webSiteUrl = value;
        this.WebSiteDisplay = value;
        AboutPromptItem.SetVisibility(this._website);
      }
    }

    protected internal string WebSiteDisplay
    {
      get => (string) this.GetValue(AboutPromptItem.WebSiteDisplayProperty);
      set
      {
        if (value != null)
        {
          value = value.ToLowerInvariant().TrimEnd('/');
          if (value.StartsWith("https://"))
            value = value.Remove(0, "https://".Length);
          if (value.StartsWith("http://"))
            value = value.Remove(0, "http://".Length);
          if (!string.IsNullOrEmpty(value) && value.StartsWith("www.twitter.com"))
            value = "@" + value.Remove(0, "www.twitter.com".Length).TrimStart('/');
        }
        this.SetValue(AboutPromptItem.WebSiteDisplayProperty, (object) value);
      }
    }

    public string Role
    {
      get => (string) this.GetValue(AboutPromptItem.RoleProperty);
      set
      {
        if (value != null)
          value = value.ToLowerInvariant();
        this.SetValue(AboutPromptItem.RoleProperty, (object) value);
      }
    }

    public string EmailAddress
    {
      get => (string) this.GetValue(AboutPromptItem.EmailAddressProperty);
      set
      {
        if (value != null)
          value = value.ToLowerInvariant();
        this.SetValue(AboutPromptItem.EmailAddressProperty, (object) value);
        AboutPromptItem.SetVisibility(this._emailAddress);
      }
    }

    public string AuthorName
    {
      get => (string) this.GetValue(AboutPromptItem.AuthorNameProperty);
      set
      {
        if (value != null)
          value = value.ToLowerInvariant();
        this.SetValue(AboutPromptItem.AuthorNameProperty, (object) value);
        AboutPromptItem.SetVisibility(this._author);
      }
    }
  }
}
