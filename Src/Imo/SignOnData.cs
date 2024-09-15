// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.SignOnData
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using Newtonsoft.Json.Linq;


namespace ImoSilverlightApp
{
  public class SignOnData
  {
    public JObject data;
    public bool IsInitialSignOn;

    public SignOnData(JObject data, bool isInitialSignOn = false)
    {
      this.data = data;
      this.IsInitialSignOn = isInitialSignOn;
    }
  }
}
