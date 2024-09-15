// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.OutgoingMessage
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;


namespace ImoSilverlightApp
{
  public class OutgoingMessage
  {
    private const int MIN_ACK_TIMEOUT_PER_CONNECTION = 15000;
    private const int MAX_SEND_RETRIES_COUNT = 10;
    private const int DATA_PER_SECOND = 4000;
    private DateTime lastTrySendTime;
    private int lastTrySendRouteNumber;
    private int retriesSendCount;
    private int extraAckTimeout;

    public JObject Message { get; private set; }

    public int Seq => this.Message.Value<int>((object) "seq");

    public OutgoingMessage(JObject jObject, int seq)
    {
      this.Message = jObject;
      this.Message.Add(nameof (seq), (JToken) seq);
      this.lastTrySendTime = DateTime.MinValue;
      this.retriesSendCount = 0;
      this.lastTrySendRouteNumber = -1;
      this.extraAckTimeout = ((this.Message.ToString(Formatting.None).Length - 1) / 4000 + 1) * 1000;
    }

    public bool ShouldTrySend(int routeNumber)
    {
      return this.lastTrySendRouteNumber < routeNumber && !this.ExceededSendRetriesLimit();
    }

    public void WillTrySend(int routeNumber)
    {
      if (this.lastTrySendRouteNumber >= routeNumber)
        throw new Exception("Bad route number");
      if (this.ExceededSendRetriesLimit())
        throw new Exception("Exceeded max send tries.");
      ++this.retriesSendCount;
      this.lastTrySendRouteNumber = routeNumber;
      this.lastTrySendTime = DateTime.Now;
    }

    public bool ExceededAckTimeout(int routeNumber)
    {
      return this.lastTrySendRouteNumber == routeNumber && (DateTime.Now - this.lastTrySendTime).TotalMilliseconds > (double) (15000 + this.extraAckTimeout);
    }

    public bool ExceededSendRetriesLimit() => this.retriesSendCount > 10;
  }
}
