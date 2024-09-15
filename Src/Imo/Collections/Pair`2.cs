// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.Collections.Pair`2
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll


namespace ImoSilverlightApp.Collections
{
  public class Pair<T, U>
  {
    public Pair()
    {
    }

    public Pair(T first, U second)
    {
      this.First = first;
      this.Second = second;
    }

    public T First { get; set; }

    public U Second { get; set; }
  }
}
