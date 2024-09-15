// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.Collections.SortedObservableCollection`1
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;


namespace ImoSilverlightApp.Collections
{
  public class SortedObservableCollection<T> : 
    INotifyCollectionChanged,
    INotifyPropertyChanged,
    IEnumerable<T>,
    IEnumerable
    where T : INotifyPropertyChanged
  {
    private LinkedList<T> items;
    private IDictionary<T, LinkedListNode<T>> itemToNode;
    private Comparer<T> comparer;

    public SortedObservableCollection(Comparer<T> comparer = null, IEnumerable<T> initItems = null)
    {
      this.comparer = comparer != null || typeof (IComparable<T>).IsAssignableFrom(typeof (T)) ? comparer : throw new ArgumentNullException("No comparer was passed and T does not implement IComparable<T>");
      this.items = new LinkedList<T>();
      this.itemToNode = (IDictionary<T, LinkedListNode<T>>) new Dictionary<T, LinkedListNode<T>>();
      if (initItems == null)
        return;
      foreach (T initItem in initItems)
        this.Add(initItem);
    }

    private int CompareTo(T a, T b)
    {
      return this.comparer != null ? this.comparer.Compare(a, b) : ((IComparable<T>) (object) a).CompareTo(b);
    }

    public bool IsEmpty => this.items.Count == 0;

    public IEnumerable<T> Reverse()
    {
      if (this.items.Last != null)
      {
        LinkedListNode<T> itNode;
        for (itNode = this.items.Last; itNode != this.items.First.Previous; itNode = itNode.Previous)
          yield return itNode.Value;
        itNode = (LinkedListNode<T>) null;
      }
    }

    public void SetItems(IEnumerable<T> items)
    {
      this.InternalClear(true);
      List<T> objList = new List<T>(items);
      objList.Sort((IComparer<T>) this.comparer);
      foreach (T key in objList)
      {
        this.items.AddLast(key);
        this.itemToNode[key] = this.items.Last;
        key.PropertyChanged += new PropertyChangedEventHandler(this.Item_PropertyChanged);
      }
      this.OnCollectionChanged(NotifyCollectionChangedAction.Reset);
      this.OnPropertyChanged("Count");
      this.OnPropertyChanged("First");
    }

    public T Last => this.ToNodeValue(this.items.Last);

    public T First => this.ToNodeValue(this.items.First);

    public int Count => this.items.Count;

    public void Add(T item)
    {
      if (this.Contains(item))
        throw new Exception("Adding duplicate item");
      this.InternalAdd(item);
      this.OnPropertyChanged("Count");
    }

    public void AddSafe(T item)
    {
      if (this.Contains(item))
        return;
      this.Add(item);
    }

    private void InternalAdd(T item, bool surpressEvent = false)
    {
      int index;
      if (this.items.Count == 0 || this.CompareTo(item, this.items.Last<T>()) >= 0)
      {
        this.items.AddLast(item);
        index = this.items.Count - 1;
        this.itemToNode[item] = this.items.Last;
      }
      else
      {
        int num = 0;
        LinkedListNode<T> first = this.items.First;
        LinkedListNode<T> node = this.items.First;
        while (node != this.items.Last.Next && this.CompareTo(node.Value, item) < 0)
        {
          node = node.Next;
          ++num;
        }
        index = num;
        if (node == this.items.Last.Next)
        {
          this.items.AddLast(item);
          this.itemToNode[item] = this.items.Last;
        }
        else
        {
          this.items.AddBefore(node, item);
          this.itemToNode[item] = node.Previous;
        }
      }
      item.PropertyChanged += new PropertyChangedEventHandler(this.Item_PropertyChanged);
      if (surpressEvent)
        return;
      this.OnCollectionChanged(NotifyCollectionChangedAction.Add, item, index);
      if (index != 0)
        return;
      this.OnPropertyChanged("First");
    }

    public void Clear() => this.InternalClear();

    private void InternalClear(bool surpressEvent = false)
    {
      foreach (T obj in this.items)
        obj.PropertyChanged -= new PropertyChangedEventHandler(this.Item_PropertyChanged);
      this.items.Clear();
      this.itemToNode.Clear();
      if (surpressEvent)
        return;
      this.OnCollectionChanged(NotifyCollectionChangedAction.Reset);
      this.OnPropertyChanged("Count");
      this.OnPropertyChanged("First");
    }

    private void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      if (this.items.Count == 1)
        return;
      T obj = (T) sender;
      if (!this.Contains(obj))
        return;
      LinkedListNode<T> node1 = this.itemToNode[obj];
      bool flag = false;
      if (node1.Previous != null && this.CompareTo(node1.Previous.Value, obj) > 0)
        flag = true;
      if (node1.Next != null && this.CompareTo(obj, node1.Next.Value) > 0)
        flag = true;
      if (!flag)
        return;
      int num1 = 0;
      LinkedListNode<T> node2 = this.items.First;
      while (node2 != this.items.Last.Next && (node2 == node1 || this.CompareTo(node1.Value, node2.Value) > 0))
      {
        node2 = node2.Next;
        ++num1;
      }
      int num2 = num1;
      int index = this.GetIndex(node1);
      if (num2 > index)
        --num2;
      this.items.Remove(node1);
      if (node2 == this.items.Last.Next)
      {
        this.items.AddLast(obj);
        this.itemToNode[obj] = this.items.Last;
      }
      else
      {
        this.items.AddBefore(node2, obj);
        this.itemToNode[obj] = node2.Previous;
      }
      this.OnCollectionChanged(NotifyCollectionChangedAction.Reset);
      if (index != 0 && num2 != 0)
        return;
      this.OnPropertyChanged("First");
    }

    private int GetIndex(LinkedListNode<T> node)
    {
      int index = 0;
      for (LinkedListNode<T> previous = node.Previous; previous != this.items.First.Previous; previous = previous.Previous)
        ++index;
      return index;
    }

    public bool Contains(T item) => this.itemToNode.ContainsKey(item);

    public T Get(T item) => this.itemToNode[item].Value;

    public void Remove(T item)
    {
      this.InternalRemove(item);
      this.OnPropertyChanged("Count");
    }

    public void RemoveSafe(T item)
    {
      if (!this.Contains(item))
        return;
      this.Remove(item);
    }

    private void InternalRemove(T item, bool surpressEvent = false)
    {
      item = this.itemToNode[item].Value;
      LinkedListNode<T> node = this.itemToNode[item];
      int index = this.GetIndex(node);
      this.items.Remove(node);
      this.itemToNode.Remove(item);
      item.PropertyChanged -= new PropertyChangedEventHandler(this.Item_PropertyChanged);
      if (surpressEvent)
        return;
      this.OnCollectionChanged(NotifyCollectionChangedAction.Remove, item, index);
      if (index != 0)
        return;
      this.OnPropertyChanged("First");
    }

    internal void RemoveAllBefore(T item)
    {
      List<T> objList = new List<T>();
      LinkedListNode<T> linkedListNode1 = this.items.First;
      while (!linkedListNode1.Value.Equals((object) item))
      {
        LinkedListNode<T> linkedListNode2 = linkedListNode1;
        linkedListNode1 = linkedListNode1.Next;
        objList.Add(linkedListNode2.Value);
        this.InternalRemove(linkedListNode2.Value, true);
      }
      if (objList.Count <= 0)
        return;
      this.OnCollectionChanged(NotifyCollectionChangedAction.Reset);
      this.OnPropertyChanged("Count");
    }

    internal T GetAfter(T item) => this.ToNodeValue(this.itemToNode[item].Next);

    internal T GetAfterLooped(T item)
    {
      T after = this.GetAfter(item);
      return !object.Equals((object) after, (object) default (T)) ? after : this.First;
    }

    internal T GetBefore(T item) => this.ToNodeValue(this.itemToNode[item].Previous);

    internal T GetBeforeLooped(T item)
    {
      T before = this.GetBefore(item);
      return !object.Equals((object) before, (object) default (T)) ? before : this.Last;
    }

    private T ToNodeValue(LinkedListNode<T> node) => node == null ? default (T) : node.Value;

    private void OnCollectionChanged(
      NotifyCollectionChangedAction action,
      T item = null,
      int index = -1,
      int oldIndex = -1)
    {
      NotifyCollectionChangedEventHandler collectionChanged = this.CollectionChanged;
      if (collectionChanged == null)
        return;
      if (oldIndex != -1)
        collectionChanged((object) this, new NotifyCollectionChangedEventArgs(action, (object) item, index, oldIndex));
      else if (index != -1)
        collectionChanged((object) this, new NotifyCollectionChangedEventArgs(action, (object) item, index));
      else if (!object.Equals((object) item, (object) default (T)))
        collectionChanged((object) this, new NotifyCollectionChangedEventArgs(action, (object) item));
      else
        collectionChanged((object) this, new NotifyCollectionChangedEventArgs(action));
    }

    private void OnCollectionChanged(
      NotifyCollectionChangedAction action,
      IList items,
      int startIndex)
    {
      NotifyCollectionChangedEventHandler collectionChanged = this.CollectionChanged;
      if (collectionChanged == null)
        return;
      collectionChanged((object) this, new NotifyCollectionChangedEventArgs(action, items, startIndex));
    }

    public event NotifyCollectionChangedEventHandler CollectionChanged;

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string name)
    {
      PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
      if (propertyChanged == null)
        return;
      propertyChanged((object) this, new PropertyChangedEventArgs(name));
    }

    public IEnumerator<T> GetEnumerator() => (IEnumerator<T>) this.items.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.items.GetEnumerator();
  }
}
