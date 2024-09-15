// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.Selectors.DataTemplateSelector
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using System.Windows;
using System.Windows.Controls;


namespace ImoSilverlightApp.Selectors
{
  public abstract class DataTemplateSelector : ContentControl
  {
    public DataTemplateSelector()
    {
      this.HorizontalContentAlignment = HorizontalAlignment.Stretch;
      this.VerticalContentAlignment = VerticalAlignment.Stretch;
      this.HorizontalAlignment = HorizontalAlignment.Stretch;
      this.VerticalAlignment = VerticalAlignment.Stretch;
    }

    public virtual DataTemplate SelectTemplate(object item, DependencyObject container)
    {
      return (DataTemplate) null;
    }

    protected override void OnContentChanged(object oldContent, object newContent)
    {
      base.OnContentChanged(oldContent, newContent);
      this.ContentTemplate = this.SelectTemplate(newContent, (DependencyObject) this);
    }
  }
}
