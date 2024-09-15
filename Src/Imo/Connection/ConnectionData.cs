// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.Connection.ConnectionData
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll


namespace ImoSilverlightApp.Connection
{
  public class ConnectionData
  {
    public string Host;
    public int Port;
    public bool ResetDispatcher;
    private string warpyHost;
    private int warpyPort;
    private string restRpcPostUrl;

    public ConnectionData(string host, int port, bool resetDispatcher)
    {
      this.Host = host;
      this.Port = port;
      this.ResetDispatcher = resetDispatcher;
      if (this.IsLocalHost)
      {
        this.warpyHost = this.Host;
        this.warpyPort = 4443;
        this.restRpcPostUrl = "https://" + this.Host + ":4443/imo";
      }
      else
      {
        this.warpyHost = Constants.GetImoHost();
        this.warpyPort = 443;
        this.restRpcPostUrl = "https://" + Constants.GetImoHost() + "/imo";
      }
    }

    public static ConnectionData CreateFromApplicationSettings()
    {
      ImoDNSEndpoint fasterEndpoint = IMO.ImoDNSManager.GetFasterEndpoint();
      return new ConnectionData(fasterEndpoint.IP, fasterEndpoint.GetRandomPort(), false);
    }

    public bool IsLocalHost => false;

    public string WarpyHost => this.warpyHost;

    public int WarpyPort => this.warpyPort;

    public string RestRpcPostUrl => this.restRpcPostUrl;
  }
}
