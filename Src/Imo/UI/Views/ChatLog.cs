// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.UI.Views.ChatLog
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Selectors;
using ImoSilverlightApp.Storage.Models;
using System;
using System.Collections;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;


namespace ImoSilverlightApp.UI.Views
{
  public class ChatLog : UserControl
  {
    private double oldExtentHeight;
    internal ScrollViewer scrollViewer;
    internal Button loadHistoryButton;
    internal ItemsControl msgsListView;
    private bool _contentLoaded;

    public ChatLog()
    {
      this.InitializeComponent();
      this.Loaded += new RoutedEventHandler(this.ChatLog_Loaded);
    }

    private async void ChatLog_Loaded(object sender, RoutedEventArgs e)
    {
      this.Conversation.UpdateLastUserActivity();
      this.scrollViewer.UpdateLayout();
      this.IsAnchoredBottom = true;
      this.scrollViewer.LayoutUpdated += new EventHandler(this.ScrollViewer_ViewChanged);
      this.msgsListView.SizeChanged += new SizeChangedEventHandler(this.MsgsListView_SizeChanged);
      this.scrollViewer.SizeChanged += new SizeChangedEventHandler(this.ScrollViewer_SizeChanged);
      this.UpdateScrollPosition();
      await Task.Delay(250);
      this.msgsListView.ItemsSource = (IEnumerable) this.Conversation.Messages;
    }

    private void ScrollViewer_SizeChanged(object sender, SizeChangedEventArgs e)
    {
      if (!this.IsAnchoredBottom)
        return;
      this.scrollViewer.ScrollToVerticalOffset(this.scrollViewer.ExtentHeight - this.scrollViewer.ViewportHeight);
    }

    private void MsgsListView_SizeChanged(object sender, SizeChangedEventArgs e)
    {
      this.UpdateScrollPosition();
    }

    private void ScrollViewer_ViewChanged(object sender, EventArgs e)
    {
      this.UpdateScrollPosition();
    }

    private void UpdateScrollPosition()
    {
      double num = this.scrollViewer.ExtentHeight - this.oldExtentHeight;
      if (num > 0.1)
      {
        if (this.IsAnchoredBottom)
          this.scrollViewer.ScrollToVerticalOffset(this.scrollViewer.ExtentHeight - this.scrollViewer.ViewportHeight);
        else if (num > 100.0)
          this.scrollViewer.ScrollToVerticalOffset(this.scrollViewer.VerticalOffset + num);
        this.oldExtentHeight = this.scrollViewer.ExtentHeight;
      }
      else
      {
        this.IsAnchoredBottom = this.scrollViewer.VerticalOffset + this.scrollViewer.ViewportHeight > this.scrollViewer.ExtentHeight - 50.0;
        this.oldExtentHeight = this.scrollViewer.ExtentHeight;
      }
    }

    public bool IsAnchoredBottom { get; private set; }

    private ChatLogViewModel ViewModel => (ChatLogViewModel) this.DataContext;

    private Conversation Conversation => this.ViewModel.Conversation;

    private void LoadHistoryButton_Click(object sender, RoutedEventArgs e)
    {
      this.Conversation.LoadHistory();
    }

    private void Message_Loaded(object sender, RoutedEventArgs e)
    {
      if (!(sender is MessageTemplateSelector))
        return;
      MessageTemplateSelector templateSelector = sender as MessageTemplateSelector;
      if (!(templateSelector.DataContext is Message))
        return;
      if ((templateSelector.DataContext as Message).Origin == MessageOrigin.STORAGE)
        IMO.NavigationManager.LogMessageLoaded(true);
      else
        IMO.NavigationManager.LogMessageLoaded(false);
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/ImoSilverlightApp;component/UI/Views/ChatLog.xaml", UriKind.Relative));
      this.scrollViewer = (ScrollViewer) this.FindName("scrollViewer");
      this.loadHistoryButton = (Button) this.FindName("loadHistoryButton");
      this.msgsListView = (ItemsControl) this.FindName("msgsListView");
    }
  }
}
