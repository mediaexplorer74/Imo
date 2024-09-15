// Decompiled with JetBrains decompiler
// Type: Coding4Fun.Toolkit.Controls.DataSource`1
// Assembly: Coding4Fun.Toolkit.Controls, Version=2.1.7.0, Culture=neutral, PublicKeyToken=c5fd7b72b1a17ce4
// MVID: 213D8A75-4B66-4004-95C2-D82C49ACB764
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\Coding4Fun.Toolkit.Controls.dll

using Coding4Fun.Toolkit.Controls.Primitives;
using System;
using System.Collections;
using System.Windows.Controls;


namespace Coding4Fun.Toolkit.Controls
{
  public abstract class DataSource<T> : ILoopingSelectorDataSource where T : struct
  {
    private ValueWrapper<T> _selectedItem;

    public object GetNext(object relativeTo)
    {
      if (relativeTo == null)
        return (object) null;
      T? relativeTo1 = this.GetRelativeTo(((ValueWrapper<T>) relativeTo).Value, 1);
      return !relativeTo1.HasValue ? (object) null : (object) this._selectedItem.CreateNew(new T?(relativeTo1.Value));
    }

    public object GetPrevious(object relativeTo)
    {
      if (relativeTo == null)
        return (object) null;
      T? relativeTo1 = this.GetRelativeTo(((ValueWrapper<T>) relativeTo).Value, -1);
      return !relativeTo1.HasValue ? (object) null : (object) this._selectedItem.CreateNew(new T?(relativeTo1.Value));
    }

    protected abstract T? GetRelativeTo(T relativeDate, int delta);

    public virtual bool IsEmpty => false;

    public object SelectedItem
    {
      get => (object) this._selectedItem;
      set
      {
        if (value == this._selectedItem || value is ValueWrapper<T> valueWrapper && this._selectedItem != null && valueWrapper.Value.Equals((object) this._selectedItem.Value))
          return;
        object selectedItem = (object) this._selectedItem;
        this._selectedItem = valueWrapper;
        EventHandler<SelectionChangedEventArgs> selectionChanged = this.SelectionChanged;
        if (selectionChanged == null)
          return;
        selectionChanged((object) this, new SelectionChangedEventArgs((IList) new object[1]
        {
          selectedItem
        }, (IList) new object[1]
        {
          (object) this._selectedItem
        }));
      }
    }

    public event EventHandler<SelectionChangedEventArgs> SelectionChanged;
  }
}
