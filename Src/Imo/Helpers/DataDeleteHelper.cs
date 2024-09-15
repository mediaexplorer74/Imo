// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.Helpers.DataDeleteHelper
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Storage.Models;
using Newtonsoft.Json.Linq;
using NLog;
using System.Collections.Generic;
using System.Linq;


namespace ImoSilverlightApp.Helpers
{
  internal class DataDeleteHelper
  {
    private static readonly Logger log = LogManager.GetLogger(typeof (DataDeleteHelper).Name);

    public static void HandleDeleteData(JObject edata)
    {
      JArray jarray = edata.Value<JArray>((object) "buids");
      for (int index = 0; index < jarray.Count; ++index)
        IMO.ConversationsManager.GetOrCreateConversation(jarray[index].Value<string>()).HideFromChatList(true);
      if (((int) edata.Value<bool?>((object) "delete_cache") ?? 0) != 0)
      {
        FSUtils.EmptyTmpDir();
        FSUtils.EmptyImagesDir();
        IMO.ApplicationStorage.ClearUploadedPhonebook();
        IMO.PhonebookManager.UploadPhonebook();
        IMO.ApplicationStorage.ClearPendingSendMessages();
      }
      long objectSeq = edata.Value<long?>((object) "object_seq") ?? -1L;
      IMO.ImoAccount.SetInterOpDeletionStatus(objectSeq);
    }

    public static void SyncUnknownAndSend()
    {
      List<string> unknownBuids = DataDeleteHelper.FindUnknownBuids();
      IMO.ImoAccount.CheckUnknownBuidsToDelete(unknownBuids);
    }

    private static List<string> FindUnknownBuids()
    {
      return DataDeleteHelper.FindAllBuids().ToList<string>();
    }

    private static HashSet<string> FindAllBuids()
    {
      HashSet<string> allBuids = new HashSet<string>();
      foreach (Contact allContacts in (IEnumerable<Contact>) IMO.ContactsManager.GetAllContactsList())
      {
        if (!allBuids.Contains(allContacts.Buid))
          allBuids.Add(allContacts.Buid);
      }
      foreach (Conversation allConversations in (IEnumerable<Conversation>) IMO.ConversationsManager.GetAllConversationsList())
      {
        if (!allBuids.Contains(allConversations.Buid))
          allBuids.Add(allConversations.Buid);
      }
      return allBuids;
    }
  }
}
