// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.Collections.PriorityQueue`1
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


namespace ImoSilverlightApp.Collections
{
  public class PriorityQueue<T>
  {
    private Comparison<T> comparison;
    private List<T> data;

    public PriorityQueue(Comparison<T> comparison = null)
    {
      this.data = new List<T>();
      this.comparison = comparison != null || typeof (T).GetTypeInfo().IsAssignableFrom(typeof (IComparable<T>).GetTypeInfo()) ? comparison : throw new ArgumentException("No comparison was passed and T does not implement IComparable<T>");
    }

    public void Enqueue(T item)
    {
      this.data.Add(item);
      int index1;
      for (int index2 = this.data.Count - 1; index2 > 0; index2 = index1)
      {
        index1 = (index2 - 1) / 2;
        if (this.comparison != null)
        {
          if (this.comparison(this.data[index2], this.data[index1]) <= 0)
            break;
        }
        else if (((IComparable<T>) (object) this.data[index2]).CompareTo(this.data[index1]) <= 0)
          break;
        T obj = this.data[index2];
        this.data[index2] = this.data[index1];
        this.data[index1] = obj;
      }
    }

    public int Count => this.data.Count;

    public T Peek() => this.data.Last<T>();

    public T Dequeue()
    {
      T obj = this.data.Last<T>();
      this.data.RemoveAt(this.data.Count - 1);
      return obj;
    }

    public void Clear() => this.data.Clear();
  }
}
