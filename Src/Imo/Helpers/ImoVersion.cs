// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.Helpers.ImoVersion
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using System;


namespace ImoSilverlightApp.Helpers
{
  public class ImoVersion : IComparable<ImoVersion>
  {
    private int major;
    private int minor;
    private int build;

    public ImoVersion(int major, int minor, int build)
    {
      this.major = major;
      this.minor = minor;
      this.build = build;
    }

    public override string ToString()
    {
      return string.Format("{0}.{1}.{2}", (object) this.major, (object) this.minor, (object) this.build);
    }

    internal bool IsLessThan(string versionStr)
    {
      return this.IsLessThan(ImoVersion.FromString(versionStr));
    }

    internal bool IsLessThan(ImoVersion version) => this.CompareTo(version) < 0;

    public static ImoVersion FromString(string str)
    {
      string[] strArray = str.Split('.');
      return new ImoVersion(int.Parse(strArray[0]), int.Parse(strArray[1]), int.Parse(strArray[2]));
    }

    public int CompareTo(ImoVersion version)
    {
      if (this.major != version.major)
        return this.major.CompareTo(version.major);
      return this.minor != version.minor ? this.minor.CompareTo(version.minor) : this.build.CompareTo(version.build);
    }
  }
}
