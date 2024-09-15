// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.Helpers.SystemInfoHelper
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Devices.Enumeration.Pnp;
using Windows.System;


namespace ImoSilverlightApp.Helpers
{
  public class SystemInfoHelper
  {
    private const string ItemNameKey = "System.ItemNameDisplay";
    private const string ModelNameKey = "System.Devices.ModelName";
    private const string ManufacturerKey = "System.Devices.Manufacturer";
    private const string DeviceClassKey = "{A45C254E-DF1C-4EFD-8020-67D146A850E0},10";
    private const string PrimaryCategoryKey = "{78C34FC8-104A-4ACA-9EA4-524D52996E57},97";
    private const string DeviceDriverVersionKey = "{A8B865DD-2E3D-4094-AD97-E593A70C75D6},3";
    private const string RootContainer = "{00000000-0000-0000-FFFF-FFFFFFFFFFFF}";
    private const string RootQuery = "System.Devices.ContainerId:=\"{00000000-0000-0000-FFFF-FFFFFFFFFFFF}\"";
    private const string HalDeviceClass = "4d36e966-e325-11ce-bfc1-08002be10318";

    public static async Task<ProcessorArchitecture> GetProcessorArchitectureAsync()
    {
      PnpObject halDevice = await SystemInfoHelper.GetHalDevice("System.ItemNameDisplay");
      if (halDevice == null || halDevice.Properties["System.ItemNameDisplay"] == null)
        return (ProcessorArchitecture) (int) ushort.MaxValue;
      string str = halDevice.Properties["System.ItemNameDisplay"].ToString();
      return !str.Contains("x64") ? (!str.Contains("ARM") ? (ProcessorArchitecture) 0 : (ProcessorArchitecture) 5) : (ProcessorArchitecture) 9;
    }

    public static Task<string> GetDeviceManufacturerAsync()
    {
      return SystemInfoHelper.GetRootDeviceInfoAsync("System.Devices.Manufacturer");
    }

    public static Task<string> GetDeviceModelAsync()
    {
      return SystemInfoHelper.GetRootDeviceInfoAsync("System.Devices.ModelName");
    }

    public static Task<string> GetDeviceCategoryAsync()
    {
      return SystemInfoHelper.GetRootDeviceInfoAsync("{78C34FC8-104A-4ACA-9EA4-524D52996E57},97");
    }

    public static async Task<string> GetWindowsVersionAsync()
    {
      PnpObject halDevice = await SystemInfoHelper.GetHalDevice("{A8B865DD-2E3D-4094-AD97-E593A70C75D6},3");
      if (halDevice == null || !halDevice.Properties.ContainsKey("{A8B865DD-2E3D-4094-AD97-E593A70C75D6},3"))
        return (string) null;
      return string.Join(".", ((IEnumerable<string>) halDevice.Properties["{A8B865DD-2E3D-4094-AD97-E593A70C75D6},3"].ToString().Split('.')).Take<string>(2).ToArray<string>());
    }

    private static async Task<string> GetRootDeviceInfoAsync(string propertyKey)
    {
      return (string) (await PnpObject.CreateFromIdAsync((PnpObjectType) 2, "{00000000-0000-0000-FFFF-FFFFFFFFFFFF}", (IEnumerable<string>) new string[1]
      {
        propertyKey
      })).Properties[propertyKey];
    }

    private static async Task<PnpObject> GetHalDevice(params string[] properties)
    {
      foreach (PnpObject halDevice in ((IEnumerable<PnpObject>) await PnpObject.FindAllAsync((PnpObjectType) 3, ((IEnumerable<string>) properties).Concat<string>((IEnumerable<string>) new string[1]
      {
        "{A45C254E-DF1C-4EFD-8020-67D146A850E0},10"
      }), "System.Devices.ContainerId:=\"{00000000-0000-0000-FFFF-FFFFFFFFFFFF}\"")).Where<PnpObject>((Func<PnpObject, bool>) (d => d.Properties != null && d.Properties.Any<KeyValuePair<string, object>>())))
      {
        KeyValuePair<string, object> keyValuePair = halDevice.Properties.Last<KeyValuePair<string, object>>();
        if (keyValuePair.Value != null && keyValuePair.Value.ToString().Equals("4d36e966-e325-11ce-bfc1-08002be10318"))
          return halDevice;
      }
      return (PnpObject) null;
    }
  }
}
