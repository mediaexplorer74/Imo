// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.AV.SampleGrabberEventArgs
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using System;


namespace ImoSilverlightApp.AV
{
  public class SampleGrabberEventArgs : EventArgs
  {
    private byte[] buffer;

    public SampleGrabberEventArgs(byte[] buffer) => this.buffer = buffer;

    public byte[] Buffer => this.buffer;
  }
}
