// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.Managers.QueryEngine
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;


namespace ImoSilverlightApp.Managers
{
  internal class QueryEngine : BaseManager
  {
    private int querySeq = -1;

    public event EventHandler<EventArg<JObject>> ReceivedResult;

    public void Search(string text, int numResults = 20)
    {
      Dictionary<string, object> data = new Dictionary<string, object>();
      ++this.querySeq;
      data.Add("uid", (object) IMO.User.Uid);
      data.Add("proto", (object) "imo");
      data.Add("qseq_no", (object) this.querySeq);
      data.Add("query", (object) text);
      data.Add("num_results", (object) numResults);
      BaseManager.Send("query_engine", "search", data);
    }

    public void HandleMessage(JObject message)
    {
      string str = message.Value<string>((object) "name");
      JObject edata = message.Value<JObject>((object) "edata");
      if (!str.Equals("search_result") || edata.Value<int>((object) "qseq_no") != this.querySeq)
        return;
      this.HandleSearchResult(edata);
    }

    private void HandleSearchResult(JObject edata) => this.OnReceivedResult(edata);

    private void OnReceivedResult(JObject result)
    {
      if (this.ReceivedResult == null)
        return;
      this.ReceivedResult((object) this, new EventArg<JObject>(result));
    }
  }
}
