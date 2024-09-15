// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.Storage.Models.UploadedPhoneContact
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using Newtonsoft.Json;


namespace ImoSilverlightApp.Storage.Models
{
  internal class UploadedPhoneContact : ModelBase
  {
    [JsonProperty(PropertyName = "phone_number")]
    private string phoneNumber;
    [JsonProperty(PropertyName = "display_name")]
    private string displayName;
    [JsonProperty(PropertyName = "uid")]
    private string uid;

    public UploadedPhoneContact(string phoneNumber, string displayName, string uid = null)
    {
      this.phoneNumber = phoneNumber;
      this.displayName = displayName;
      this.uid = uid;
    }

    public bool IsExistingUser => this.uid != null;

    public string PhoneNumber => this.phoneNumber;

    public string DisplayName => this.displayName;
  }
}
