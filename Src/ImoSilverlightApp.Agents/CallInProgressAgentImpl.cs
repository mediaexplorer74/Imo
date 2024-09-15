// Decompiled with JetBrains decompiler
// Type: PhoneVoIPApp.Agents.CallInProgressAgentImpl
// Assembly: ImoSilverlightApp.Agents, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E5312DD8-91BD-43C5-A29E-03979D5872E9
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.Agents.dll

using Microsoft.Phone.Networking.Voip;

namespace PhoneVoIPApp.Agents
{
  public class CallInProgressAgentImpl : VoipCallInProgressAgent
  {
    protected override void OnFirstCallStarting()
    {
    }

    protected override void OnCancel() => this.NotifyComplete();
  }
}
