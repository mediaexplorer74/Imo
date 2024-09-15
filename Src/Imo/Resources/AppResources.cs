// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.Resources.AppResources
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;


namespace ImoSilverlightApp.Resources
{
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
  [DebuggerNonUserCode]
  [CompilerGenerated]
  public class AppResources
  {
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    internal AppResources()
    {
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static ResourceManager ResourceManager
    {
      get
      {
        if (AppResources.resourceMan == null)
          AppResources.resourceMan = new ResourceManager("ImoSilverlightApp.Resources.AppResources", typeof (AppResources).Assembly);
        return AppResources.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static CultureInfo Culture
    {
      get => AppResources.resourceCulture;
      set => AppResources.resourceCulture = value;
    }

    public static string AppBarButtonText
    {
      get
      {
        return AppResources.ResourceManager.GetString(nameof (AppBarButtonText), AppResources.resourceCulture);
      }
    }

    public static string AppBarMenuItemText
    {
      get
      {
        return AppResources.ResourceManager.GetString(nameof (AppBarMenuItemText), AppResources.resourceCulture);
      }
    }

    public static string ApplicationTitle
    {
      get
      {
        return AppResources.ResourceManager.GetString(nameof (ApplicationTitle), AppResources.resourceCulture);
      }
    }

    public static string ResourceFlowDirection
    {
      get
      {
        return AppResources.ResourceManager.GetString(nameof (ResourceFlowDirection), AppResources.resourceCulture);
      }
    }

    public static string ResourceLanguage
    {
      get
      {
        return AppResources.ResourceManager.GetString(nameof (ResourceLanguage), AppResources.resourceCulture);
      }
    }
  }
}
