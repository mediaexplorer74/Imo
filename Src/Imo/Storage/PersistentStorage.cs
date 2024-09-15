// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.Storage.PersistentStorage
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Helpers;
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;


namespace ImoSilverlightApp.Storage
{
  internal abstract class PersistentStorage
  {
    private static readonly Logger log = LogManager.GetLogger(typeof (PersistentStorage).Name);
    private IsolatedStorageSettings container;

    protected void InitTree(string fileName)
    {
      this.container = IsolatedStorageSettings.ApplicationSettings;
    }

    protected JObject InternalGetDictionary(string table)
    {
      string str1 = table + "_";
      JObject dictionary = new JObject();
      foreach (KeyValuePair<string, object> keyValuePair in (IEnumerable<KeyValuePair<string, object>>) this.container)
      {
        if (keyValuePair.Key.StartsWith(str1))
        {
          string str2 = keyValuePair.Key.Substring(str1.Length);
          if (dictionary.Property(str2) == null)
            dictionary.Add(str2, (JToken) string.Concat(keyValuePair.Value));
        }
      }
      return dictionary;
    }

    protected void InternalSetProperty(string table, string key, string value)
    {
      this.container[table + "_" + key] = (object) value;
      this.SaveChanges();
    }

    private void SaveChanges()
    {
      IsolatedStorageFile storeForApplication = IsolatedStorageFile.GetUserStoreForApplication();
      try
      {
        this.container.Save();
        return;
      }
      catch (IsolatedStorageException ex)
      {
        PersistentStorage.log.Error((Exception) ex, string.Format("Failed to save settings. Quota {0}, Available {1}", (object) storeForApplication.Quota, (object) storeForApplication.AvailableFreeSpace), 66, nameof (SaveChanges));
      }
      try
      {
        if (FSUtils.EmptyTmpDir() || FSUtils.EmptyImagesDir())
          this.container.Save();
        else
          ImoMessageBox.Show("Your phone memory is full and IMO might not function properly. Please free up some space.");
      }
      catch (IsolatedStorageException ex)
      {
        PersistentStorage.log.Error((Exception) ex, string.Format("Failed to increase quota. Quota {0}, Available {1}", (object) storeForApplication.Quota, (object) storeForApplication.AvailableFreeSpace), 83, nameof (SaveChanges));
        ImoMessageBox.Show("Your phone memory is full and IMO might not function properly. Please free up some space.");
      }
    }

    protected bool InternalContainsKey(string table, string key)
    {
      return this.container.Contains(table + "_" + key);
    }

    protected void InternalRemoveProperty(string table, string key)
    {
      string key1 = table + "_" + key;
      if (!this.container.Contains(key1))
        return;
      this.container.Remove(key1);
      this.SaveChanges();
    }

    protected string InternalGetValue(string table, string key)
    {
      object obj = (object) null;
      this.container.TryGetValue<object>(table + "_" + key, out obj);
      return string.Concat(obj);
    }

    protected void InternalClearTable(string table)
    {
      string str = table + "_";
      List<string> stringList = new List<string>();
      foreach (KeyValuePair<string, object> keyValuePair in (IEnumerable<KeyValuePair<string, object>>) this.container)
      {
        if (keyValuePair.Key.StartsWith(str))
          stringList.Add(keyValuePair.Key);
      }
      foreach (string key in stringList)
        this.container.Remove(key);
      this.SaveChanges();
    }

    public virtual void Dispose()
    {
    }
  }
}
