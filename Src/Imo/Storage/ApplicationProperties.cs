// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.Storage.ApplicationProperties
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Helpers;
using ImoSilverlightApp.Storage.Models;


namespace ImoSilverlightApp.Storage
{
  internal class ApplicationProperties : ModelBase
  {
    private static ApplicationProperties instance;
    private bool isSignedOn;
    private int dispatcherResets;
    private bool cookieSignOnFailed;

    private ApplicationProperties()
    {
    }

    public static ApplicationProperties Instance
    {
      get
      {
        if (ApplicationProperties.instance == null)
          ApplicationProperties.instance = new ApplicationProperties();
        return ApplicationProperties.instance;
      }
    }

    public ImoVersion Version { get; set; }

    public bool IsSignedOn
    {
      get => this.isSignedOn;
      set
      {
        if (this.isSignedOn == value)
          return;
        this.isSignedOn = value;
        this.OnPropertyChanged(nameof (IsSignedOn));
      }
    }

    public bool CookieSignOnFailed
    {
      get => this.cookieSignOnFailed;
      set
      {
        if (this.cookieSignOnFailed == value)
          return;
        this.cookieSignOnFailed = value;
        this.OnPropertyChanged(nameof (CookieSignOnFailed));
      }
    }

    public int DispatcherResets
    {
      get => this.dispatcherResets;
      set
      {
        if (this.dispatcherResets == value)
          return;
        this.dispatcherResets = value;
        this.OnPropertyChanged(nameof (DispatcherResets));
      }
    }

    public bool StartMinimized { get; set; }

    public bool GeneratedUDID { get; set; }

    public bool IsRelease => true;

    public bool TabThroughChats { get; set; }
  }
}
