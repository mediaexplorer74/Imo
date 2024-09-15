// Decompiled with JetBrains decompiler
// Type: Coding4Fun.Toolkit.Controls.Common.PhoneHelper
// Assembly: Coding4Fun.Toolkit.Controls, Version=2.1.7.0, Culture=neutral, PublicKeyToken=c5fd7b72b1a17ce4
// MVID: 213D8A75-4B66-4004-95C2-D82C49ACB764
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\Coding4Fun.Toolkit.Controls.dll

using System;
using System.Xml;


namespace Coding4Fun.Toolkit.Controls.Common
{
  public class PhoneHelper
  {
    private const string AppManifestName = "WMAppManifest.xml";
    private const string AppNodeName = "App";

    public static string GetAppAttribute(string attributeName)
    {
      if (ApplicationSpace.IsDesignMode)
        return "";
      try
      {
        using (XmlReader xmlReader = XmlReader.Create("WMAppManifest.xml", new XmlReaderSettings()
        {
          XmlResolver = (XmlResolver) new XmlXapResolver()
        }))
        {
          xmlReader.ReadToDescendant("App");
          return xmlReader.IsStartElement() ? xmlReader.GetAttribute(attributeName) : throw new FormatException("WMAppManifest.xml is missing App");
        }
      }
      catch (Exception ex)
      {
        return "";
      }
    }
  }
}
