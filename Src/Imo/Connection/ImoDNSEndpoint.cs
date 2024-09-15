// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.Connection.ImoDNSEndpoint
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Helpers;
using System.Collections.Generic;


namespace ImoSilverlightApp.Connection
{
  public class ImoDNSEndpoint
  {
    public string IP { get; private set; }

    public string SessionPrefix { get; private set; }

    public long LastUsedTime { get; private set; }

    public int SuccessConnectionCount { get; private set; }

    public int FailConnectionCount { get; private set; }

    public IList<int> Ports { get; private set; }

    public long TTL { get; private set; }

    public long CreationTime { get; private set; }

    public ImoDNSEndpoint(string ip, IList<int> ports, long ttl, string sessionPrefix)
    {
      this.IP = ip;
      this.Ports = ports;
      this.TTL = ttl;
      this.SessionPrefix = sessionPrefix;
      this.CreationTime = Utils.GetTimestamp();
    }

    public void BumpSuccessConnectionCount() => ++this.SuccessConnectionCount;

    public void BumpFailConnectionCount() => ++this.FailConnectionCount;

    public int GetRandomPort() => this.Ports[Utils.GetUnsafeRandomInt(this.Ports.Count)];

    public void UpdateLastUsedTime() => this.LastUsedTime = Utils.GetTimestamp();

    public bool IsValid() => this.CreationTime + this.TTL > Utils.GetTimestamp();
  }
}
