// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.AccountManager
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Helpers;
using Newtonsoft.Json.Linq;
using NLog;
using System;


namespace ImoSilverlightApp
{
  public class AccountManager : BaseManager
  {
    private static readonly Logger log = LogManager.GetLogger(typeof (AccountManager).Name);
    private string countryCode;
    private string phoneNumber;
    private string currentUserUid;
    public bool isCodeSignIn;
    public bool isCodeRegister;

    public void HandleMessage(JObject data)
    {
      string str = data.Value<string>((object) "name");
      if ("signed_on".Equals(str))
      {
        this.HandleSignedOn(data);
      }
      else
      {
        if ("report_account_data".Equals(str))
          return;
        if ("disconnect".Equals(str))
          this.HandleDisconnect(data);
        else if ("signoff_all".Equals(str))
        {
          this.SignOff("dispatcher_signoff_all");
        }
        else
        {
          if ("reflect".Equals(str))
            return;
          if ("not_authenticated".Equals(str))
          {
            this.SignOff("dispatcher_not_authenticated");
          }
          else
          {
            if ("cookie_login_failed".Equals(str))
              return;
            if ("delete_data".Equals(str))
              DataDeleteHelper.HandleDeleteData(data.Value<JObject>((object) "edata"));
            else if ("sync_unknown_buids_list".Equals(str))
              DataDeleteHelper.SyncUnknownAndSend();
            else
              AccountManager.log.Warn("bad account event: " + str);
          }
        }
      }
    }

    private void HandleSignedOn(JObject data)
    {
      string str = data.Value<string>((object) "uid");
      AccountManager.log.Info("Signed on: " + str);
      bool isInitialSignOn = this.currentUserUid == null;
      this.currentUserUid = str;
      IMO.ApplicationSettings.CookieUid = str;
      IMO.ApplicationSettings.IsCookieSignedOn = true;
      IMO.User.UpdateFromSignedOnData(data);
      IMO.ApplicationProperties.IsSignedOn = true;
      if (!IMO.ApplicationSettings.HasSignedOnBefore)
      {
        IMO.ApplicationSettings.HasSignedOnBefore = true;
        IMO.MonitorLog.Log("application", "first_user_sign_on");
      }
      this.LogAccountSignOn(isInitialSignOn);
      IMO.ImoProfile.GetMyProfile((Action<JToken>) (result => IMO.User.ProfilePhotoId = result.Value<string>((object) "profile_photo_id")));
      IMO.PhonebookManager.UploadPhonebook();
      IMO.ApplicationSettings.CodeRequestedTimestamp = 0L;
      this.OnSignedOn(new SignOnData(data, isInitialSignOn));
    }

    private void LogAccountSignOn(bool isInitialSignOn)
    {
      JObject eventsMap = new JObject();
      if (isInitialSignOn)
        eventsMap.Add("sign_on", (JToken) 1);
      if (this.isCodeSignIn || this.isCodeRegister)
      {
        this.isCodeSignIn = false;
        eventsMap.Add("code_sign_on", (JToken) 1);
        if (this.isCodeRegister)
        {
          this.isCodeRegister = false;
          eventsMap.Add("code_register", (JToken) 1);
        }
      }
      if (!eventsMap.HasValues)
        return;
      IMO.MonitorLog.Log("account", eventsMap);
    }

    private void HandleDisconnect(JObject message)
    {
      AccountManager.log.Info("in handleDisconnect message: " + (object) message);
      string str = (string) ((JObject) message["edata"])["reason"];
      AccountManager.log.Info("reason: " + str);
      this.SignOff("dispatcher_disconnect");
    }

    public event EventHandler<EventArg<SignOnData>> SignedOn;

    private void OnSignedOn(SignOnData jObj)
    {
      EventHandler<EventArg<SignOnData>> signedOn = this.SignedOn;
      if (signedOn == null)
        return;
      signedOn((object) this, new EventArg<SignOnData>(jObj));
    }

    public event EventHandler SignedOff;

    private void OnSignedOff()
    {
      EventHandler signedOff = this.SignedOff;
      if (signedOff == null)
        return;
      signedOff((object) this, new EventArgs());
    }

    public void SignOff(string reason)
    {
      AccountManager.log.Info("Signing off: " + reason);
      IMO.MonitorLog.LogSignOff(reason, Utils.WaitCallbackExecute(1000, (Action) (() =>
      {
        this.currentUserUid = (string) null;
        IMO.User.Clear();
        IMO.ApplicationSettings.CookieUid = (string) null;
        IMO.ApplicationSettings.IsCookieSignedOn = false;
        IMO.ApplicationProperties.IsSignedOn = false;
        IMO.ApplicationStorage.ClearConversations();
        IMO.ApplicationStorage.ClearPendingSendMessages();
        IMO.ApplicationStorage.ClearUser();
        IMO.ApplicationStorage.ClearUploadedPhonebook();
        IMO.Network.Reconnect(true);
        IMO.NavigationManager.NavigateToSignInPage();
        this.OnSignedOff();
      })));
    }

    public void GetStarted(
      string phoneNumberUnstripped,
      string countryCode,
      Action<LoginResultData> callback)
    {
      this.phoneNumber = Utils.NormalizePhoneNumber(phoneNumberUnstripped);
      this.countryCode = countryCode;
      string cookieUid = IMO.ApplicationSettings.CookieUid;
      IMO.ImoAccount.GetStarted(this.phoneNumber, countryCode, (Action<JToken>) (jObj =>
      {
        switch (jObj.Value<string>((object) "result"))
        {
          case "fail":
            if (jObj.Value<string>((object) "reason") == "invalid_phone")
            {
              callback(new LoginResultData()
              {
                ErrorMessage = "Invalid phone number!"
              });
              break;
            }
            callback(new LoginResultData()
            {
              ErrorMessage = "There was a problem, please try again later!"
            });
            break;
          case "login":
            if (Utils.GetTimestamp() - IMO.ApplicationSettings.CodeRequestedTimestamp < (long) TimeSpan.FromHours(2.0).Milliseconds)
            {
              callback(new LoginResultData()
              {
                CodeRequested = true
              });
              break;
            }
            this.VerifyPhone(false, callback);
            break;
          case "register":
            if (IMO.ApplicationSettings.CodeRequestedTimestamp < (long) TimeSpan.FromHours(2.0).Milliseconds)
            {
              callback(new LoginResultData()
              {
                CodeRequested = true,
                NeedRegister = true
              });
              break;
            }
            this.VerifyPhone(true, callback);
            break;
          case "iat_login":
            callback(new LoginResultData()
            {
              DoPhoneLogin = true
            });
            break;
          case "iat_register":
            callback(new LoginResultData()
            {
              DoRegister = true
            });
            break;
        }
      }));
    }

    internal void PhoneRegister(string code, Action<JToken> callback)
    {
      this.isCodeRegister = true;
      this.isCodeSignIn = false;
      IMO.ImoAccount.PhoneRegister(this.phoneNumber, this.countryCode, code, IMO.ApplicationSettings.UserFullName, IMO.ApplicationSettings.UserAge, (string) null, (Action<JToken>) (result =>
      {
        if (result.Value<string>((object) nameof (result)) == "ok")
          IMO.Session.CookieLogin("register");
        callback(result);
      }));
    }

    public void PhoneLogin(string code, Action<LoginResultData> callback = null)
    {
      this.isCodeSignIn = true;
      this.isCodeRegister = false;
      IMO.ImoAccount.PhoneLogin(this.countryCode, this.phoneNumber, code, (Action<JToken>) (result =>
      {
        if (result.Value<string>((object) nameof (result)) == "fail")
        {
          if (result.Value<string>((object) "reason") == "wrong_code")
          {
            if (callback == null)
              return;
            callback(new LoginResultData()
            {
              ErrorMessage = "Wrong phone code!"
            });
          }
          else
          {
            if (callback == null)
              return;
            callback(new LoginResultData()
            {
              ErrorMessage = "Error while signing in!"
            });
          }
        }
        else
          IMO.Session.CookieLogin("login");
      }));
    }

    public void CheckPhoneCode(string code, Action<JToken> callback)
    {
      IMO.ImoAccount.CheckPhoneCode(this.phoneNumber, code, this.countryCode, callback);
    }

    private void VerifyPhone(bool needRegister, Action<LoginResultData> callback)
    {
      IMO.ImoAccount.VerifyPhone(this.phoneNumber, this.countryCode, (Action<JToken>) (result =>
      {
        if (result.Value<string>((object) "status") == "error")
          callback(new LoginResultData()
          {
            ErrorMessage = result.Value<string>((object) "error")
          });
        else
          callback(new LoginResultData()
          {
            CodeRequested = true,
            NeedRegister = needRegister
          });
      }));
    }
  }
}
