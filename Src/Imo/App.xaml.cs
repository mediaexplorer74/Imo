using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace ImoSilverlightApp
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // When the navigation stack isn't restored navigate to the first page,
                    // configuring the new page by passing required information as a navigation
                    // parameter
                    rootFrame.Navigate(typeof(MainPage), e.Arguments);
                }
                // Ensure the current window is active
                Window.Current.Activate();
            }
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}

/*
 
 // Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.App
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.AV;
using ImoSilverlightApp.Helpers;
using ImoSilverlightApp.Resources;
using ImoSilverlightApp.UI.Views;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Threading;
using Windows.ApplicationModel.Activation;


namespace ImoSilverlightApp
{
  public class App : Application
  {
    private static readonly Logger log = LogManager.GetLogger(typeof (App).Name);
    private DispatcherTimer notifyUserActiveTimer;
    private EventWaitHandle waitHandle;
    private volatile bool isActive;
    private bool phoneApplicationInitialized;
    private bool _contentLoaded;

    public static PhoneApplicationFrame RootFrame { get; private set; }

    public App()
    {
      ImoPage.DisableAnimations = true;
      this.UnhandledException += new EventHandler<ApplicationUnhandledExceptionEventArgs>(this.Application_UnhandledException);
      TaskScheduler.UnobservedTaskException += new EventHandler<UnobservedTaskExceptionEventArgs>(this.TaskScheduler_UnobservedTaskException);
      this.InitializeComponent();
      this.InitializePhoneApplication();
      this.InitializeLanguage();
      if (Debugger.IsAttached)
      {
        Application.Current.Host.Settings.EnableFrameRateCounter = true;
        PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
      }
      this.SetupLightTheme();
      this.InitVersion();
      IMO.Init();
      IMO.ImoDNSManager.RefreshEndpoints();
      RegularActionsUtils.ScheduleRegularIntervalAction();
      IMO.Dispatcher.Resetted += new EventHandler(this.Dispatcher_Resetted);
      IMO.Network.Reconnect();
      IMO.Session.CookieLogin("start", true);
      if (!IMO.ApplicationSettings.HasStartedAppBefore)
        IMO.MonitorLog.Log("application", "first_start", (Action) (() => IMO.ApplicationSettings.HasStartedAppBefore = true));
      IMO.ApplicationStorage.ClearBroadcasts();
      this.StartNotifyUserActivityTimer();
    }

    private void TaskScheduler_UnobservedTaskException(
      object sender,
      UnobservedTaskExceptionEventArgs e)
    {
      App.log.Error((Exception) e.Exception, 108, nameof (TaskScheduler_UnobservedTaskException));
    }

    private void SetupLightTheme()
    {
      ((SolidColorBrush) this.Resources[(object) "PhoneRadioCheckBoxCheckBrush"]).Color = ((SolidColorBrush) this.Resources[(object) "PhoneRadioCheckBoxBorderBrush"]).Color = ((SolidColorBrush) this.Resources[(object) "PhoneForegroundBrush"]).Color = Color.FromArgb((byte) 222, (byte) 0, (byte) 0, (byte) 0);
      ((SolidColorBrush) this.Resources[(object) "PhoneBackgroundBrush"]).Color = Color.FromArgb(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
      ((SolidColorBrush) this.Resources[(object) "PhoneContrastForegroundBrush"]).Color = Color.FromArgb(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
      ((SolidColorBrush) this.Resources[(object) "PhoneContrastBackgroundBrush"]).Color = Color.FromArgb((byte) 222, (byte) 0, (byte) 0, (byte) 0);
      ((SolidColorBrush) this.Resources[(object) "PhoneDisabledBrush"]).Color = Color.FromArgb((byte) 77, (byte) 0, (byte) 0, (byte) 0);
      ((SolidColorBrush) this.Resources[(object) "PhoneProgressBarBackgroundBrush"]).Color = Color.FromArgb((byte) 25, (byte) 0, (byte) 0, (byte) 0);
      ((SolidColorBrush) this.Resources[(object) "PhoneTextCaretBrush"]).Color = Color.FromArgb((byte) 222, (byte) 0, (byte) 0, (byte) 0);
      ((SolidColorBrush) this.Resources[(object) "PhoneTextBoxBrush"]).Color = Color.FromArgb((byte) 38, (byte) 0, (byte) 0, (byte) 0);
      ((SolidColorBrush) this.Resources[(object) "PhoneTextBoxForegroundBrush"]).Color = Color.FromArgb((byte) 222, (byte) 0, (byte) 0, (byte) 0);
      ((SolidColorBrush) this.Resources[(object) "PhoneTextBoxEditBackgroundBrush"]).Color = Color.FromArgb((byte) 0, (byte) 0, (byte) 0, (byte) 0);
      ((SolidColorBrush) this.Resources[(object) "PhoneTextBoxReadOnlyBrush"]).Color = Color.FromArgb((byte) 46, (byte) 0, (byte) 0, (byte) 0);
      ((SolidColorBrush) this.Resources[(object) "PhoneSubtleBrush"]).Color = Color.FromArgb((byte) 102, (byte) 0, (byte) 0, (byte) 0);
      ((SolidColorBrush) this.Resources[(object) "PhoneTextBoxSelectionForegroundBrush"]).Color = Color.FromArgb(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
      ((SolidColorBrush) this.Resources[(object) "PhoneButtonBasePressedForegroundBrush"]).Color = Color.FromArgb(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
      ((SolidColorBrush) this.Resources[(object) "PhoneTextHighContrastBrush"]).Color = Color.FromArgb((byte) 222, (byte) 0, (byte) 0, (byte) 0);
      ((SolidColorBrush) this.Resources[(object) "PhoneTextMidContrastBrush"]).Color = Color.FromArgb((byte) 115, (byte) 0, (byte) 0, (byte) 0);
      ((SolidColorBrush) this.Resources[(object) "PhoneTextLowContrastBrush"]).Color = Color.FromArgb((byte) 64, (byte) 0, (byte) 0, (byte) 0);
      ((SolidColorBrush) this.Resources[(object) "PhoneSemitransparentBrush"]).Color = Color.FromArgb((byte) 170, byte.MaxValue, byte.MaxValue, byte.MaxValue);
      ((SolidColorBrush) this.Resources[(object) "PhoneInactiveBrush"]).Color = Color.FromArgb((byte) 51, (byte) 0, (byte) 0, (byte) 0);
      ((SolidColorBrush) this.Resources[(object) "PhoneInverseInactiveBrush"]).Color = Color.FromArgb(byte.MaxValue, (byte) 229, (byte) 229, (byte) 229);
      ((SolidColorBrush) this.Resources[(object) "PhoneInverseBackgroundBrush"]).Color = Color.FromArgb(byte.MaxValue, (byte) 221, (byte) 221, (byte) 221);
      ((SolidColorBrush) this.Resources[(object) "PhoneBorderBrush"]).Color = Color.FromArgb((byte) 153, (byte) 0, (byte) 0, (byte) 0);
      ((SolidColorBrush) this.Resources[(object) "PhoneAccentBrush"]).Color = Color.FromArgb(byte.MaxValue, (byte) 0, (byte) 73, (byte) 99);
      ((SolidColorBrush) this.Resources[(object) "PhoneChromeBrush"]).Color = Color.FromArgb(byte.MaxValue, (byte) 221, (byte) 221, (byte) 221);
    }

    private void StartNotifyUserActivityTimer()
    {
      if (this.notifyUserActiveTimer != null)
        this.notifyUserActiveTimer.Stop();
      this.notifyUserActiveTimer = new DispatcherTimer();
      this.notifyUserActiveTimer.Tick += (EventHandler) ((s, e) =>
      {
        if (!this.isActive)
          return;
        IMO.Session.NotifyUserIsActive();
      });
      this.notifyUserActiveTimer.Interval = TimeSpan.FromSeconds(30.0);
      this.notifyUserActiveTimer.Start();
    }

    public static void AnswerIncommingCall()
    {
      string str1 = "streams_info";
      if (!IsolatedStorageFile.GetUserStoreForApplication().FileExists(str1))
        return;
      if (DateTime.Now - IsolatedStorageFile.GetUserStoreForApplication().GetCreationTime(str1).DateTime > TimeSpan.FromSeconds(60.0))
      {
        IsolatedStorageFile.GetUserStoreForApplication().DeleteFile(str1);
      }
      else
      {
        IsolatedStorageFileStream storageFileStream = IsolatedStorageFile.GetUserStoreForApplication().OpenFile(str1, FileMode.Open);
        byte[] numArray = new byte[storageFileStream.Length];
        storageFileStream.Read(numArray, 0, numArray.Length);
        storageFileStream.Close();
        storageFileStream.Dispose();
        IsolatedStorageFile.GetUserStoreForApplication().DeleteFile(str1);
        string s = Encoding.UTF8.GetString(numArray, 0, numArray.Length);
        if (s.StartsWith("group_call"))
        {
          string str2 = s.Substring(s.IndexOf(":") + 1);
          IMO.AVManager.InitiateGroupCall(str2 + ";", "push_in_app");
        }
        else
        {
          IMO.ApplicationSettings.StreamsInfo = ZLibHelper.Decompress(Convert.FromBase64String(s));
          if (IMO.ApplicationSettings.StreamsInfo == null)
            return;
          IMO.AVManager.HandleStreamsInfo(JObject.Parse(IMO.ApplicationSettings.StreamsInfo));
          if (IMO.AVManager.CallController != null && IMO.AVManager.CallController.CallState == AVState.RECEIVING)
            IMO.AVManager.SelfAcceptCall();
          IMO.ApplicationSettings.StreamsInfo = (string) null;
        }
      }
    }

    private void Dispatcher_Resetted(object sender, EventArgs e)
    {
      RegularActionsUtils.InvalidatePendingRequests();
    }

    private void Application_ContractActivated(object sender, IActivatedEventArgs e)
    {
    }

    private void Application_Launching(object sender, LaunchingEventArgs e)
    {
      this.HandleActivate();
    }

    private void Application_Activated(object sender, ActivatedEventArgs e)
    {
      this.HandleActivate();
    }

    private void Application_Deactivated(object sender, DeactivatedEventArgs e)
    {
      this.HandleDeactivate();
    }

    private void Application_Closing(object sender, ClosingEventArgs e) => this.HandleDeactivate();

    private void HandleActivate()
    {
      if (IMO.ApplicationSettings.DidAppExitWithUnreportedledCrash)
        IMO.MonitorLog.Log("unreported_app_crashes", new JObject()
        {
          {
            "unreported_crash",
            (JToken) 1
          },
          {
            "is_in_call",
            (JToken) (IMO.ApplicationSettings.WasAppInCall ? 1 : 0)
          }
        });
      IMO.ApplicationSettings.DidAppExitWithUnreportedledCrash = true;
      foreach (ShellTile activeTile in ShellTile.ActiveTiles)
      {
        IconicTileData data = new IconicTileData()
        {
          Count = new int?(0),
          WideContent1 = "",
          WideContent2 = "",
          WideContent3 = "",
          BackgroundColor = Color.FromArgb(byte.MaxValue, (byte) 14, (byte) 85, (byte) 181)
        };
        try
        {
          activeTile.Update((ShellTileData) data);
        }
        catch (Exception ex)
        {
        }
      }
      IMO.Session.SetSessionActivity(true);
      IMO.PhonebookManager.UploadPhonebook(true);
      this.waitHandle = new EventWaitHandle(false, EventResetMode.AutoReset, "ImoSilverlightAppVoipAgent");
      this.waitHandle.Reset();
      this.isActive = true;
      Task.Run((Func<Task>) (async () =>
      {
        while (true)
        {
          this.waitHandle.WaitOne();
          if (this.isActive)
            await Utils.InvokeOnUI(new Action(App.AnswerIncommingCall));
          else
            break;
        }
      }));
      IMO.PushNotificationsManager.HandleAppActivated();
    }

    private void HandleDeactivate()
    {
      LogManager.HandleDeactivated();
      IMO.Session.SetSessionActivity(false);
      this.isActive = false;
      this.waitHandle?.Set();
      this.waitHandle?.Dispose();
      this.waitHandle = (EventWaitHandle) null;
      if (IMO.AVManager != null && IMO.AVManager.IsInCall)
        IMO.AVManager.EndCallWithReason("app_closing");
      IMO.ApplicationSettings.DidAppExitWithUnreportedledCrash = false;
      IMO.PushNotificationsManager.HandleAppDeactivated();
    }

    public bool IsActive => this.isActive;

    private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
    {
      if (!Debugger.IsAttached)
        return;
      Debugger.Break();
    }

    private void Application_UnhandledException(
      object sender,
      ApplicationUnhandledExceptionEventArgs e)
    {
      try
      {
        if (Debugger.IsAttached)
          Debugger.Break();
        App.log.AppCrash(e.ExceptionObject, 355, nameof (Application_UnhandledException));
        IMO.ApplicationSettings.DidAppExitWithUnreportedledCrash = false;
      }
      catch (Exception ex)
      {
        App.log.Warn(ex, "Exception while reporting exception");
      }
    }

    private void InitializePhoneApplication()
    {
      if (this.phoneApplicationInitialized)
        return;
      App.RootFrame = (PhoneApplicationFrame) new TransitionFrame();
      App.RootFrame.Background = (Brush) new SolidColorBrush(Colors.White);
      App.RootFrame.Navigated += new NavigatedEventHandler(this.CompleteInitializePhoneApplication);
      App.RootFrame.NavigationFailed += new NavigationFailedEventHandler(this.RootFrame_NavigationFailed);
      App.RootFrame.Navigated += new NavigatedEventHandler(this.CheckForResetNavigation);
      PhoneApplicationService.Current.ContractActivated += new EventHandler<IActivatedEventArgs>(this.Application_ContractActivated);
      this.phoneApplicationInitialized = true;
    }

    private void InitVersion()
    {
      Version version = Assembly.GetExecutingAssembly().GetName().Version;
      IMO.ApplicationProperties.Version = new ImoVersion(version.Major, version.Minor, version.Build);
    }

    private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
    {
      if (this.RootVisual != App.RootFrame)
        this.RootVisual = (UIElement) App.RootFrame;
      App.RootFrame.Navigated -= new NavigatedEventHandler(this.CompleteInitializePhoneApplication);
    }

    private void CheckForResetNavigation(object sender, NavigationEventArgs e)
    {
      if (e.NavigationMode != NavigationMode.Reset)
        return;
      App.RootFrame.Navigated += new NavigatedEventHandler(this.ClearBackStackAfterReset);
    }

    private void ClearBackStackAfterReset(object sender, NavigationEventArgs e)
    {
      App.RootFrame.Navigated -= new NavigatedEventHandler(this.ClearBackStackAfterReset);
      if (e.NavigationMode != NavigationMode.New && e.NavigationMode != NavigationMode.Refresh)
        return;
      do
        ;
      while (App.RootFrame.RemoveBackEntry() != null);
    }

    private void InitializeLanguage()
    {
      try
      {
        App.RootFrame.Language = XmlLanguage.GetLanguage(AppResources.ResourceLanguage);
        App.RootFrame.FlowDirection = (FlowDirection) Enum.Parse(typeof (FlowDirection), AppResources.ResourceFlowDirection);
      }
      catch
      {
        if (Debugger.IsAttached)
          Debugger.Break();
        throw;
      }
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/ImoSilverlightApp;component/App.xaml", UriKind.Relative));
    }
  }
}

 */
