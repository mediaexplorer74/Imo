// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.Selectors.EnumTemplateSelector
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;


namespace ImoSilverlightApp.Selectors
{
  [System.Windows.Markup.ContentProperty("Templates")]
  public class EnumTemplateSelector : DataTemplateSelector
  {
    private Dictionary<string, DataTemplate> _templates = new Dictionary<string, DataTemplate>();

    public string EnumType { get; set; }

    public string PropertyName { get; set; }

    public Dictionary<string, DataTemplate> Templates
    {
      get => this._templates;
      set => this._templates = value;
    }

    public DataTemplate DefaultTemplate { get; set; }

    public override DataTemplate SelectTemplate(object item, DependencyObject container)
    {
      if (item == null)
        return this.DefaultTemplate;
      if (!string.IsNullOrWhiteSpace(this.PropertyName))
        item = item.GetType().GetTypeInfo().GetDeclaredProperty(this.PropertyName).GetValue(item);
      string name = Enum.GetName(Type.GetType(this.EnumType, true), item);
      return this.Templates.ContainsKey(name) ? this.Templates[name] : this.DefaultTemplate;
    }
  }
}
