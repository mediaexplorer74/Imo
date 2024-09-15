// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.Collections.TimestampedObject`1
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Helpers;


namespace ImoSilverlightApp.Collections
{
  internal class TimestampedObject<T>
  {
    private long timestamp;
    private T value;

    public TimestampedObject(T value)
      : this(Utils.GetTimestamp(), value)
    {
    }

    public TimestampedObject(long timestamp, T value)
    {
      this.timestamp = timestamp;
      this.value = value;
    }

    public long Timestamp => this.timestamp;

    public T Value => this.value;
  }
}
