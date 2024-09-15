// Decompiled with JetBrains decompiler
// Type: Imo.Phone.Controls.Primitives.TemplatedListBox
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using System;
using System.Windows;
using System.Windows.Controls;


namespace Imo.Phone.Controls.Primitives
{
  public class TemplatedListBox : ListBox
  {
    public DataTemplate ListHeaderTemplate { get; set; }

    public DataTemplate ListFooterTemplate { get; set; }

    public DataTemplate GroupHeaderTemplate { get; set; }

    public DataTemplate GroupFooterTemplate { get; set; }

    public event EventHandler<LinkUnlinkEventArgs> Link;

    public event EventHandler<LinkUnlinkEventArgs> Unlink;

    protected override DependencyObject GetContainerForItemOverride()
    {
      return (DependencyObject) new TemplatedListBoxItem();
    }

    protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
    {
      base.PrepareContainerForItemOverride(element, item);
      DataTemplate dataTemplate = (DataTemplate) null;
      if (!(item is LongListSelectorItem listSelectorItem))
        return;
      switch (listSelectorItem.ItemType)
      {
        case LongListSelectorItemType.Item:
          dataTemplate = this.ItemTemplate;
          break;
        case LongListSelectorItemType.GroupHeader:
          dataTemplate = this.GroupHeaderTemplate;
          break;
        case LongListSelectorItemType.GroupFooter:
          dataTemplate = this.GroupFooterTemplate;
          break;
        case LongListSelectorItemType.ListHeader:
          dataTemplate = this.ListHeaderTemplate;
          break;
        case LongListSelectorItemType.ListFooter:
          dataTemplate = this.ListFooterTemplate;
          break;
      }
      TemplatedListBoxItem parent = (TemplatedListBoxItem) element;
      parent.Content = listSelectorItem.Item;
      parent.Tuple = listSelectorItem;
      parent.ContentTemplate = dataTemplate;
      ContentPresenter logicalChildByType = TemplatedVisualTreeExtensions.GetFirstLogicalChildByType<ContentPresenter>(parent, true);
      EventHandler<LinkUnlinkEventArgs> link = this.Link;
      if (logicalChildByType == null || link == null)
        return;
      link((object) this, new LinkUnlinkEventArgs(logicalChildByType));
    }

    protected override void ClearContainerForItemOverride(DependencyObject element, object item)
    {
      if (item is LongListSelectorItem)
      {
        ContentPresenter logicalChildByType = TemplatedVisualTreeExtensions.GetFirstLogicalChildByType<ContentPresenter>((FrameworkElement) element, true);
        EventHandler<LinkUnlinkEventArgs> unlink = this.Unlink;
        if (logicalChildByType != null && unlink != null)
          unlink((object) this, new LinkUnlinkEventArgs(logicalChildByType));
      }
      base.ClearContainerForItemOverride(element, item);
    }
  }
}
