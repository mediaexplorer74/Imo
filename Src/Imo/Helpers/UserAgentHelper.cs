// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.Helpers.UserAgentHelper
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using Microsoft.Phone.Info;
using System;


namespace ImoSilverlightApp.Helpers
{
  internal class UserAgentHelper
  {
    private static string model;
    private static string manufacturer;
    private static string windowsVersion;

    public static string GetUA()
    {
      return string.Format("{0}/{1}; {2}; {3}; {4}", (object) "imoWindowsPhone", (object) IMO.ApplicationProperties.Version, (object) UserAgentHelper.GetDeviceModel(), (object) UserAgentHelper.GetDeviceManufacturer(), (object) UserAgentHelper.GetWindowsVersion());
    }

    private static string GetWindowsVersion()
    {
      if (UserAgentHelper.windowsVersion == null)
        UserAgentHelper.windowsVersion = Environment.OSVersion.ToString();
      return UserAgentHelper.windowsVersion;
    }

    private static string GetDeviceManufacturer()
    {
      if (UserAgentHelper.manufacturer == null)
        UserAgentHelper.manufacturer = DeviceStatus.DeviceManufacturer;
      return UserAgentHelper.manufacturer;
    }

    public static string GetDeviceModel()
    {
      if (UserAgentHelper.model == null)
        UserAgentHelper.model = DeviceStatus.DeviceName;
      return UserAgentHelper.model;
    }
  }
}
