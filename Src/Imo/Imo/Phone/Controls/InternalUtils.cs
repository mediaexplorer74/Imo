// Decompiled with JetBrains decompiler
// Type: Imo.Phone.Controls.InternalUtils
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll


namespace Imo.Phone.Controls
{
  internal static class InternalUtils
  {
    internal static bool AreValuesEqual(object o1, object o2)
    {
      if (o1 == o2)
        return true;
      if (o1 == null || o2 == null)
        return false;
      return o1.GetType().IsValueType || o1.GetType() == typeof (string) ? object.Equals(o1, o2) : o1 == o2;
    }
  }
}
