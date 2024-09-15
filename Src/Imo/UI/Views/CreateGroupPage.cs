// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.UI.Views.CreateGroupPage
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Helpers;
using ImoSilverlightApp.Storage.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;


namespace ImoSilverlightApp.UI.Views
{
  public class CreateGroupPage : ImoPage
  {
    internal Microsoft.Phone.Shell.ApplicationBar appBar;
    internal Grid LayoutRoot;
    internal Grid ContentRoot;
    internal ImoTextBox groupNameTextBox;
    internal ContactSelectorView selectorControl;
    private bool _contentLoaded;

    public CreateGroupPage()
    {
      this.InitializeComponent();
      this.selectorControl.ViewModel.SelectedItems.CollectionChanged += new NotifyCollectionChangedEventHandler(this.SelectedItems_CollectionChanged);
      this.Loaded += (RoutedEventHandler) ((s, e) => this.groupNameTextBox.FocusTextInput());
      this.DataContext = (object) (this.ViewModel = new CreateGroupViewModel((FrameworkElement) this));
    }

    private CreateGroupViewModel ViewModel { get; set; }

    private void SelectedItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      this.ViewModel.IsCreateEnabled = this.selectorControl.ViewModel.SelectedItems.Count > 0 && !string.IsNullOrEmpty(this.groupNameTextBox.Text.Trim());
    }

    private void groupNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
      this.ViewModel.IsCreateEnabled = this.selectorControl.ViewModel.SelectedItems.Count > 0 && !string.IsNullOrEmpty(this.groupNameTextBox.Text.Trim());
      this.ViewModel.ShowNoNameError = string.IsNullOrEmpty(this.groupNameTextBox.Text);
    }

    private void CreateGroup_Click(object sender, EventArgs e)
    {
      IList<Contact> selectedContacts = this.selectorControl.ViewModel.GetSelectedContacts();
      if (string.IsNullOrEmpty(this.groupNameTextBox.Text))
      {
        Utils.ShowPopup("Please specify group name!");
        this.Focus();
      }
      else if (selectedContacts.Count == 0)
      {
        Utils.ShowPopup("Please add at least one contact!");
        this.Focus();
      }
      else
      {
        IMO.MonitorLog.Log("group_chat", "group_create_click", (object) selectedContacts.Count);
        IMO.ContactsManager.CreateGroup(this.groupNameTextBox.Text, (Action<string>) (t =>
        {
          IMO.MonitorLog.Log("group_chat", "group_created");
          IMO.ContactsManager.SendInviteBuddiesToGroup(t, selectedContacts);
          this.selectorControl.Clear();
          this.groupNameTextBox.Text = "";
          IMO.NavigationManager.NavigateToConversation(t + ";");
          this.NavigationService.RemoveBackEntry();
        }));
      }
    }

    private void Cancel_Click(object sender, EventArgs e)
    {
      IMO.NavigationManager.NavigateBackOrExit();
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/ImoSilverlightApp;component/UI/Views/CreateGroupPage.xaml", UriKind.Relative));
      this.appBar = (Microsoft.Phone.Shell.ApplicationBar) this.FindName("appBar");
      this.LayoutRoot = (Grid) this.FindName("LayoutRoot");
      this.ContentRoot = (Grid) this.FindName("ContentRoot");
      this.groupNameTextBox = (ImoTextBox) this.FindName("groupNameTextBox");
      this.selectorControl = (ContactSelectorView) this.FindName("selectorControl");
    }
  }
}
