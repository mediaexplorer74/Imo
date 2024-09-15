// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.Selectors.TypeBasedTemplateSelector
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using System.Collections.Generic;
using System.Windows;


namespace ImoSilverlightApp.Selectors
{
  [System.Windows.Markup.ContentProperty("Templates")]
  public class TypeBasedTemplateSelector : DataTemplateSelector
  {
    private Dictionary<string, DataTemplate> _templates = new Dictionary<string, DataTemplate>();

    public Dictionary<string, DataTemplate> Templates
    {
      get => this._templates;
      set => this._templates = value;
    }

    public DataTemplate DefaultTemplate { get; set; }

    public override DataTemplate SelectTemplate(object item, DependencyObject container)
    {
      DataTemplate defaultTemplate;
      if (item == null || !this.Templates.TryGetValue(item.GetType().Name, out defaultTemplate))
        defaultTemplate = this.DefaultTemplate;
      return defaultTemplate;
    }
  }
}
