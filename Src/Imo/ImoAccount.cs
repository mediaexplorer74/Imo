// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.ImoAccount
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Helpers;
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Collections.Generic;


namespace ImoSilverlightApp
{
  public class ImoAccount : BaseManager
  {
    private static readonly Logger log = LogManager.GetLogger(typeof (ImoAccount).Name);

    public void GetStarted(string phone, string phoneCC, Action<JToken> callback)
    {
      BaseManager.Send("imo_account", "get_started", new Dictionary<string, object>()
      {
        {
          nameof (phone),
          (object) phone
        },
        {
          "phone_cc",
          (object) phoneCC
        },
        {
          "ssid",
          (object) IMO.Dispatcher.GetSSID()
        }
      }, callback);
    }

    internal void AddPhonebook(IList<JObject> contacts, Action<JToken> callback)
    {
      BaseManager.Send("friendsfinder", "add_phonebook", new Dictionary<string, object>()
      {
        {
          "ssid",
          (object) IMO.Dispatcher.GetSSID()
        },
        {
          "uid",
          (object) IMO.User.Uid
        },
        {
          "is_partial",
          (object) true
        },
        {
          nameof (contacts),
          (object) contacts
        }
      }, callback);
    }

    internal void AddContact(string phone, Action<JToken> callback = null)
    {
      Dictionary<string, object> dictionary = new Dictionary<string, object>();
      this.AddPhonebook((IList<JObject>) new List<JObject>()
      {
        new JObject()
        {
          {
            "contact",
            (JToken) phone
          },
          {
            "contact_type",
            (JToken) nameof (phone)
          }
        }
      }, callback);
    }

    public void PhoneLogin(
      string phoneCC,
      string phone,
      string verification_code,
      Action<JToken> callback)
    {
      Dictionary<string, object> data = new Dictionary<string, object>();
      data.Add(nameof (phone), (object) phone);
      data.Add("phone_cc", (object) phoneCC);
      if (verification_code != null)
        data.Add(nameof (verification_code), (object) verification_code);
      data.Add("ssid", (object) IMO.Dispatcher.GetSSID());
      BaseManager.Send("imo_account", "phone_login", data, callback);
    }

    public void PhoneRegister(
      string phone,
      string phoneCC,
      string verification_code,
      string full_name,
      string age,
      string email,
      Action<JToken> callback)
    {
      Dictionary<string, object> data = new Dictionary<string, object>();
      data.Add(nameof (phone), (object) phone);
      data.Add("phone_cc", (object) phoneCC);
      if (verification_code != null)
        data.Add(nameof (verification_code), (object) verification_code);
      data.Add(nameof (full_name), (object) full_name);
      data.Add(nameof (age), (object) age);
      data.Add(nameof (email), (object) email);
      data.Add("ssid", (object) IMO.Dispatcher.GetSSID());
      BaseManager.Send("imo_account", "phone_register", data, callback);
    }

    public void TokenLogin(string email, string google_id_token, Action<JToken> callback)
    {
      BaseManager.Send("imo_account", "token_login", new Dictionary<string, object>()
      {
        {
          nameof (email),
          (object) email
        },
        {
          nameof (google_id_token),
          (object) google_id_token
        },
        {
          "ssid",
          (object) IMO.Dispatcher.GetSSID()
        }
      }, callback);
    }

    public void VerifyPhone(string phone, string phone_cc, Action<JToken> callback)
    {
      BaseManager.Send("imo_account", "verify_phone", new Dictionary<string, object>()
      {
        {
          InviteType.PHONE,
          (object) phone
        },
        {
          "lang",
          (object) "enUS"
        },
        {
          "optional_tts",
          (object) false
        },
        {
          "use_tts",
          (object) false
        },
        {
          "ssid",
          (object) IMO.Dispatcher.GetSSID()
        },
        {
          "sim_cc",
          (object) phone_cc
        }
      }, callback);
    }

    public void CheckPhoneCode(
      string phone,
      string verification_code,
      string phone_cc,
      Action<JToken> callback)
    {
      BaseManager.Send("imo_account", "check_phone_code2", new Dictionary<string, object>()
      {
        {
          nameof (verification_code),
          (object) verification_code
        },
        {
          nameof (phone),
          (object) phone
        },
        {
          "verification_ui",
          (object) "manual"
        },
        {
          "ssid",
          (object) IMO.Dispatcher.GetSSID()
        },
        {
          "sim_cc",
          (object) phone_cc
        }
      }, callback);
    }

    public void RequestAppCode(string cc, string phone, Action<JToken> callback)
    {
      Dictionary<string, object> data = new Dictionary<string, object>();
      string phoneCc = CountryCodesHelper.GetPhoneCC(cc);
      data.Add(nameof (phone), (object) (phoneCc + phone));
      BaseManager.Send("imo_account", "request_app_code", data, callback);
    }

    public void DeleteAccount(Action<JToken> callback)
    {
      ImoAccount.log.Info("deleting account for uid " + IMO.User.Uid);
      BaseManager.Send("imo_account", "delete_account", new Dictionary<string, object>()
      {
        {
          "uid",
          (object) IMO.User.Uid
        },
        {
          "ssid",
          (object) IMO.Dispatcher.GetSSID()
        }
      }, callback);
    }

    public void SetInterOpDeletionStatus(long objectSeq)
    {
      BaseManager.Send("pin", "set_interop_deletion_status", new Dictionary<string, object>()
      {
        {
          "uid",
          (object) IMO.User.Uid
        },
        {
          "ssid",
          (object) IMO.Dispatcher.GetSSID()
        },
        {
          "object_seq",
          (object) objectSeq
        }
      });
    }

    public void CheckUnknownBuidsToDelete(List<string> buids)
    {
      BaseManager.Send("pin", "check_unknown_buids_to_delete", new Dictionary<string, object>()
      {
        {
          "uid",
          (object) IMO.User.Uid
        },
        {
          "ssid",
          (object) IMO.Dispatcher.GetSSID()
        },
        {
          nameof (buids),
          (object) buids
        }
      });
    }

    public void AcceptGDPR()
    {
      BaseManager.Send("pin", "accept_gdpr", new Dictionary<string, object>()
      {
        {
          "uid",
          (object) IMO.User.Uid
        },
        {
          "ssid",
          (object) IMO.Dispatcher.GetSSID()
        }
      }, (Action<JToken>) (x =>
      {
        ImoAccount.log.Info("accept_gdpr: " + (object) x);
        IMO.ApplicationSettings.HasAcceptedGdpr = true;
        IMO.NavigationManager.NavigateToHome();
      }));
    }

    private void Send(string method, Dictionary<string, object> data)
    {
      BaseManager.Send("imo_account", method, data);
    }

    public void HandleMessage(JObject message)
    {
      string str = (string) message["name"];
      JObject edata = (JObject) message["edata"];
      if ("oauth_failed".Equals(str))
        this.HandleOAuthFailed(edata);
      else if ("oauth_data_fetched".Equals(str))
        this.HandleOAuthDataFetched(edata);
      else if ("oauth_result".Equals(str))
        this.HandleOAuthResult(edata);
      else
        ImoAccount.log.Warn("bad account msg " + str + " " + (object) message);
    }

    private void HandleOAuthFailed(JObject edata)
    {
    }

    private void HandleOAuthDataFetched(JObject edata)
    {
    }

    private void HandleOAuthResult(JObject edata)
    {
    }
  }
}
