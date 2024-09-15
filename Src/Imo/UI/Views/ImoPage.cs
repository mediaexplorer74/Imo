// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.UI.Views.ImoPage
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Connection;
using ImoSilverlightApp.Helpers;
using Microsoft.Phone.Controls;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;


namespace ImoSilverlightApp.UI.Views
{
  public class ImoPage : PhoneApplicationPage
  {
    private Grid rootGrid;
    private Border disconnectedIndicator;
    private bool addedDisconnectedIndicator;

    public static bool DisableAnimations { get; set; }

    public ImoPage()
    {
      this.disconnectedIndicator = new Border();
      Border disconnectedIndicator = this.disconnectedIndicator;
      TextBlock textBlock = new TextBlock();
      textBlock.Text = "Reconnecting...";
      textBlock.Margin = new Thickness(10.0);
      textBlock.Foreground = (Brush) new SolidColorBrush(Colors.White);
      disconnectedIndicator.Child = (UIElement) textBlock;
      this.disconnectedIndicator.Background = (Brush) new SolidColorBrush(Colors.Red);
      this.disconnectedIndicator.VerticalAlignment = VerticalAlignment.Top;
      if (ImoPage.DisableAnimations)
        return;
      this.EnableAnimations();
    }

    protected void EnableAnimations()
    {
      NavigationInTransition navigationInTransition = new NavigationInTransition();
      navigationInTransition.Forward = (TransitionElement) new TurnstileTransition()
      {
        Mode = TurnstileTransitionMode.BackwardIn
      };
      navigationInTransition.Backward = (TransitionElement) new TurnstileTransition()
      {
        Mode = TurnstileTransitionMode.ForwardIn
      };
      TransitionService.SetNavigationInTransition((UIElement) this, navigationInTransition);
      NavigationOutTransition navigationOutTransition = new NavigationOutTransition();
      navigationOutTransition.Forward = (TransitionElement) new TurnstileTransition()
      {
        Mode = TurnstileTransitionMode.BackwardOut
      };
      navigationOutTransition.Backward = (TransitionElement) new TurnstileTransition()
      {
        Mode = TurnstileTransitionMode.ForwardOut
      };
      TransitionService.SetNavigationOutTransition((UIElement) this, navigationOutTransition);
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
      this.rootGrid = this.Content as Grid;
      if (this.rootGrid.RowDefinitions.Count > 1)
        Grid.SetRowSpan((FrameworkElement) this.disconnectedIndicator, this.rootGrid.RowDefinitions.Count);
      if (!IMO.Network.IsConnected)
        this.AddDisconnectedIndicator();
      IMO.Session.UpdateLastUserActivity();
      IMO.Network.Connected += new EventHandler<EventArg<ConnectionData>>(this.Network_Connected);
      IMO.Network.Disconnected += new EventHandler<EventArgs>(this.Network_Disconnected);
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
      if (!IMO.Network.IsConnected)
        this.RemoveDisconnectedIndicator();
      IMO.Session.UpdateLastUserActivity();
      IMO.Network.Connected -= new EventHandler<EventArg<ConnectionData>>(this.Network_Connected);
      IMO.Network.Disconnected -= new EventHandler<EventArgs>(this.Network_Disconnected);
    }

    protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
    {
      Uri uri = e.Uri;
      if (((object) uri != null ? (uri.IsAbsoluteUri ? 1 : 0) : 0) != 0 && IMO.AVManager != null && IMO.AVManager.IsInCall)
      {
        IMO.AVManager.IsCallOpened = false;
        IMO.AVManager.EndCallWithReason("app_closing");
        IMO.MonitorLog.Log("av_calls", "home_button_while_in_call");
        IMO.Network.FlushOutgoingMessages();
      }
      base.OnNavigatingFrom(e);
    }

    protected override void OnManipulationStarted(ManipulationStartedEventArgs e)
    {
      IMO.Session.UpdateLastUserActivity();
      base.OnManipulationStarted(e);
    }

    protected override void OnTap(System.Windows.Input.GestureEventArgs e)
    {
      IMO.Session.UpdateLastUserActivity();
      base.OnTap(e);
    }

    protected override void OnBackKeyPress(CancelEventArgs e)
    {
      if (!this.NavigationService.CanGoBack && IMO.AVManager != null && IMO.AVManager.IsInCall)
      {
        e.Cancel = true;
        ImoMessageBox.Show("Cannot close app while in call");
      }
      base.OnBackKeyPress(e);
    }

    private void Network_Disconnected(object sender, EventArgs e)
    {
      this.AddDisconnectedIndicator();
    }

    private void Network_Connected(object sender, EventArg<ConnectionData> e)
    {
      this.RemoveDisconnectedIndicator();
    }

    private void AddDisconnectedIndicator()
    {
    }

    private void RemoveDisconnectedIndicator()
    {
    }
  }
}
