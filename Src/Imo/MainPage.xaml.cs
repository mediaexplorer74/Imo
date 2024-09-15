using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ImoSilverlightApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }
    }
}

/*
 // Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.MainPage
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.UI.Views;
using NLog;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Navigation;


namespace ImoSilverlightApp
{
  public class MainPage : ImoPage
  {
    private static readonly Logger log = LogManager.GetLogger(typeof (MainPage).Name);
    private bool _contentLoaded;

    public MainPage() => this.InitializeComponent();

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
      string str1 = (string) null;
      string objectId = (string) null;
      if (this.NavigationContext.QueryString.TryGetValue("FileId", out objectId))
      {
        if (string.IsNullOrEmpty(objectId))
        {
          MainPage.log.Error("fileId is empty/null", 55, nameof (OnNavigatedTo));
        }
        else
        {
          IMO.NavigationManager.NavigateToShareObjectToMembersPage(objectId, "file");
          this.NavigationService.RemoveBackEntry();
        }
      }
      else if (this.NavigationContext.QueryString.TryGetValue("incoming", out str1))
      {
        string str2;
        if (this.NavigationContext.QueryString.TryGetValue("gid", out str2))
        {
          IMO.AVManager.InitiateGroupCall(str2 + ";", "push_notification");
        }
        else
        {
          if (!IMO.AVManager.IsInCall)
            App.AnswerIncommingCall();
          else
            IMO.NavigationManager.NavigateToAVCallPage();
          this.NavigationService.RemoveBackEntry();
        }
      }
      else if (!IMO.ApplicationSettings.IsCookieSignedOn)
      {
        this.NavigationService.Navigate(new Uri("/UI/Views/SignInPage.xaml", UriKind.Relative));
        this.NavigationService.RemoveBackEntry();
      }
      else
        this.NavigationService.Navigate(new Uri("/UI/Views/HomePage.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/ImoSilverlightApp;component/MainPage.xaml", UriKind.Relative));
    }
  }
}

 
 */
