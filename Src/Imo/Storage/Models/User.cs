// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.Storage.Models.User
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Helpers;
using Newtonsoft.Json.Linq;


namespace ImoSilverlightApp.Storage.Models
{
  public class User : ModelBase
  {
    private static User instance;
    private string uid;
    private string alias;
    private string phoneNumber;
    private string contactsHash;
    private string profilePhotoId;
    private string phoneCC;
    private const string UID_KEY = "uid";
    private const string ALIAS_KEY = "alias";
    private const string PHONE_NUMBER_KEY = "phone_number";
    private const string CONTACTS_HASH_KEY = "contacts_hash";
    private const string PROFILE_PHOTO_ID_KEY = "profile_photo_id";
    private const string PHONE_CC_KEY = "phone_cc";

    private User() => this.InitiFromStorage();

    private void InitiFromStorage()
    {
      JObject user = IMO.ApplicationStorage.GetUser();
      this.uid = user.Value<string>((object) "uid");
      this.alias = user.Value<string>((object) "alias");
      this.phoneNumber = user.Value<string>((object) "phone_number");
      this.contactsHash = user.Value<string>((object) "contacts_hash");
      this.profilePhotoId = user.Value<string>((object) "profile_photo_id");
      this.phoneCC = user.Value<string>((object) "phone_cc");
    }

    public static User Instance
    {
      get
      {
        if (User.instance == null)
          User.instance = new User();
        return User.instance;
      }
    }

    public void UpdateFromSignedOnData(JObject data)
    {
      this.Uid = data.Value<string>((object) "uid");
      JObject jobject = data.Value<JObject>((object) "edata");
      if (jobject == null)
        return;
      IMO.User.Alias = jobject.Value<string>((object) "alias");
      IMO.User.PhoneNumber = jobject.Value<string>((object) "verified_phone");
      IMO.User.PhoneCC = jobject.Value<string>((object) "phone_cc");
    }

    public string Uid
    {
      get => this.uid;
      private set
      {
        if (!(this.uid != value))
          return;
        this.uid = value;
        IMO.ApplicationStorage.SetUserProperty("uid", value);
        this.OnPropertyChanged(nameof (Uid));
      }
    }

    public string Alias
    {
      get => this.alias;
      set
      {
        if (!(this.alias != value))
          return;
        this.alias = value;
        IMO.ApplicationStorage.SetUserProperty("alias", value);
        this.OnPropertyChanged(nameof (Alias));
      }
    }

    public string PhoneNumber
    {
      get => this.phoneNumber;
      set
      {
        if (!(this.phoneNumber != value))
          return;
        this.phoneNumber = value;
        IMO.ApplicationStorage.SetUserProperty("phone_number", value);
        this.OnPropertyChanged(nameof (PhoneNumber));
      }
    }

    public string ContactsHash
    {
      get => this.contactsHash;
      set
      {
        if (!(this.contactsHash != value))
          return;
        this.contactsHash = value;
        IMO.ApplicationStorage.SetUserProperty("contacts_hash", value);
        this.OnPropertyChanged(nameof (ContactsHash));
      }
    }

    public string ProfilePhotoId
    {
      get => this.profilePhotoId;
      set
      {
        if (!(this.profilePhotoId != value))
          return;
        this.profilePhotoId = value;
        IMO.ApplicationStorage.SetUserProperty("profile_photo_id", value);
        this.OnPropertyChanged(nameof (ProfilePhotoId));
        this.OnPropertyChanged("ProfilePhotoUrl");
      }
    }

    public string PhoneCC
    {
      get => this.phoneCC;
      set
      {
        if (!(this.phoneCC != value))
          return;
        this.phoneCC = value;
        IMO.ApplicationStorage.SetUserProperty("phone_cc", value);
        this.OnPropertyChanged(nameof (PhoneCC));
      }
    }

    public string ProfilePhotoUrl
    {
      get
      {
        return this.profilePhotoId == null ? (string) null : ImageUtils.GetPhotoUrlFromId(this.profilePhotoId);
      }
    }

    public void InitFromStorage() => this.InitFromJObject(IMO.ApplicationStorage.GetUser());

    private void InitFromJObject(JObject jObj)
    {
      this.Uid = jObj.Value<string>((object) "uid");
      this.Alias = jObj.Value<string>((object) "alias");
      this.PhoneNumber = jObj.Value<string>((object) "phone_number");
      this.ContactsHash = jObj.Value<string>((object) "contacts_hash");
      this.ProfilePhotoId = jObj.Value<string>((object) "profile_photo_id");
    }

    public void Clear()
    {
      this.InitFromJObject(new JObject());
      this.OnPropertyChanged((string) null);
    }
  }
}
