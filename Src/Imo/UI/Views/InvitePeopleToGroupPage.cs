// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.UI.Views.InvitePeopleToGroupPage
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Storage.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;


namespace ImoSilverlightApp.UI.Views
{
  public class InvitePeopleToGroupPage : ImoPage
  {
    internal Microsoft.Phone.Shell.ApplicationBar appBar;
    internal Grid LayoutRoot;
    internal Grid ContentRoot;
    internal ContactSelectorView selectorControl;
    private bool _contentLoaded;

    internal InvitePeopleToGroupPageViewModel ViewModel
    {
      get => this.DataContext as InvitePeopleToGroupPageViewModel;
    }

    public InvitePeopleToGroupPage()
    {
      this.InitializeComponent();
      this.selectorControl.ViewModel.SelectedItems.CollectionChanged += new NotifyCollectionChangedEventHandler(this.SelectedItems_CollectionChanged);
      this.Loaded += (RoutedEventHandler) ((s, e) => this.selectorControl.FocusTextInput());
    }

    private void SelectedItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      this.ViewModel.IsCreateEnabled = this.selectorControl.ViewModel.SelectedItems.Count > 0 && !this.ViewModel.ShowBusyIndicator;
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
      base.OnNavigatedTo(e);
      string buid = "";
      if (!this.NavigationContext.QueryString.TryGetValue("buid", out buid))
        return;
      this.DataContext = (object) new InvitePeopleToGroupPageViewModel(buid, (FrameworkElement) this);
      this.selectorControl.ViewModel.ExistingGroup = this.ViewModel.Contact;
    }

    private void AddMembers_Click(object sender, EventArgs e)
    {
      IList<Contact> selectedContacts = this.selectorControl.ViewModel.GetSelectedContacts();
      if (selectedContacts.Count <= 0)
        return;
      IMO.MonitorLog.Log("group_chat", "added_members", (object) selectedContacts.Count);
      this.ViewModel.IsCreateEnabled = false;
      this.ViewModel.ShowBusyIndicator = true;
      IMO.ContactsManager.SendInviteBuddiesToGroup(this.ViewModel.Contact.Gid, selectedContacts, (Action<JToken>) (t =>
      {
        this.ViewModel.ShowBusyIndicator = false;
        this.ViewModel.IsCreateEnabled = selectedContacts.Count > 0 && !this.ViewModel.ShowBusyIndicator;
        IMO.NavigationManager.NavigateToConversation(this.ViewModel.Contact.Buid);
      }));
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
      Application.LoadComponent((object) this, new Uri("/ImoSilverlightApp;component/UI/Views/InvitePeopleToGroupPage.xaml", UriKind.Relative));
      this.appBar = (Microsoft.Phone.Shell.ApplicationBar) this.FindName("appBar");
      this.LayoutRoot = (Grid) this.FindName("LayoutRoot");
      this.ContentRoot = (Grid) this.FindName("ContentRoot");
      this.selectorControl = (ContactSelectorView) this.FindName("selectorControl");
    }
  }
}
