// Decompiled with JetBrains decompiler
// Type: Coding4Fun.Toolkit.Controls.MetroFlowItem
// Assembly: Coding4Fun.Toolkit.Controls, Version=2.1.7.0, Culture=neutral, PublicKeyToken=c5fd7b72b1a17ce4
// MVID: 213D8A75-4B66-4004-95C2-D82C49ACB764
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\Coding4Fun.Toolkit.Controls.dll

using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;


namespace Coding4Fun.Toolkit.Controls
{
  [ContentProperty("Title")]
  public class MetroFlowItem : Control
  {
    private const int DefaultStartIndex = 1;
    public static readonly DependencyProperty ImageSourceProperty = DependencyProperty.Register(nameof (ImageSource), typeof (ImageSource), typeof (MetroFlowItem), new PropertyMetadata(new PropertyChangedCallback(MetroFlowItem.OnImageSourceChanged)));
    public static readonly DependencyProperty ImageVisibilityProperty = DependencyProperty.Register(nameof (ImageVisibility), typeof (Visibility), typeof (MetroFlowItem), new PropertyMetadata((object) Visibility.Visible));
    public static readonly DependencyProperty ImageOpacityProperty = DependencyProperty.Register(nameof (ImageOpacity), typeof (double), typeof (MetroFlowItem), new PropertyMetadata((object) 1.0));
    public static readonly DependencyProperty ItemIndexStringProperty = DependencyProperty.Register(nameof (ItemIndexString), typeof (string), typeof (MetroFlowItem), new PropertyMetadata((object) 1.ToString()));
    public static readonly DependencyProperty ItemIndexProperty = DependencyProperty.Register(nameof (ItemIndex), typeof (int), typeof (MetroFlowItem), new PropertyMetadata((object) 1));
    public static readonly DependencyProperty ItemIndexVisibilityProperty = DependencyProperty.Register(nameof (ItemIndexVisibility), typeof (Visibility), typeof (MetroFlowItem), new PropertyMetadata((object) Visibility.Collapsed));
    public static readonly DependencyProperty ItemIndexOpacityProperty = DependencyProperty.Register(nameof (ItemIndexOpacity), typeof (double), typeof (MetroFlowItem), new PropertyMetadata((object) 0.0));
    public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(nameof (Title), typeof (string), typeof (MetroFlowItem), new PropertyMetadata((object) "Lorem ipsum dolor sit amet"));
    public static readonly DependencyProperty TitleVisibilityProperty = DependencyProperty.Register(nameof (TitleVisibility), typeof (Visibility), typeof (MetroFlowItem), new PropertyMetadata((object) Visibility.Visible));
    public static readonly DependencyProperty TitleOpacityProperty = DependencyProperty.Register(nameof (TitleOpacity), typeof (double), typeof (MetroFlowItem), new PropertyMetadata((object) 1.0));

    public MetroFlowItem() => this.DefaultStyleKey = (object) typeof (MetroFlowItem);

    public ImageSource ImageSource
    {
      get => (ImageSource) this.GetValue(MetroFlowItem.ImageSourceProperty);
      set => this.SetValue(MetroFlowItem.ImageSourceProperty, (object) value);
    }

    private static void OnImageSourceChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      if (!(d is MetroFlowItem metroFlowItem))
        return;
      metroFlowItem.UpdateLayout();
    }

    public Visibility ImageVisibility
    {
      get => (Visibility) this.GetValue(MetroFlowItem.ImageVisibilityProperty);
      set => this.SetValue(MetroFlowItem.ImageVisibilityProperty, (object) value);
    }

    public double ImageOpacity
    {
      get => (double) this.GetValue(MetroFlowItem.ImageOpacityProperty);
      set => this.SetValue(MetroFlowItem.ImageOpacityProperty, (object) value);
    }

    public string ItemIndexString
    {
      get => (string) this.GetValue(MetroFlowItem.ItemIndexStringProperty);
      private set => this.SetValue(MetroFlowItem.ItemIndexStringProperty, (object) value);
    }

    public int ItemIndex
    {
      get => (int) this.GetValue(MetroFlowItem.ItemIndexProperty);
      set
      {
        this.SetValue(MetroFlowItem.ItemIndexProperty, (object) value);
        this.ItemIndexString = this.ItemIndex.ToString();
      }
    }

    public Visibility ItemIndexVisibility
    {
      get => (Visibility) this.GetValue(MetroFlowItem.ItemIndexVisibilityProperty);
      set => this.SetValue(MetroFlowItem.ItemIndexVisibilityProperty, (object) value);
    }

    public double ItemIndexOpacity
    {
      get => (double) this.GetValue(MetroFlowItem.ItemIndexOpacityProperty);
      set => this.SetValue(MetroFlowItem.ItemIndexOpacityProperty, (object) value);
    }

    public string Title
    {
      get => (string) this.GetValue(MetroFlowItem.TitleProperty);
      set => this.SetValue(MetroFlowItem.TitleProperty, (object) value);
    }

    public Visibility TitleVisibility
    {
      get => (Visibility) this.GetValue(MetroFlowItem.TitleVisibilityProperty);
      set => this.SetValue(MetroFlowItem.TitleVisibilityProperty, (object) value);
    }

    public double TitleOpacity
    {
      get => (double) this.GetValue(MetroFlowItem.TitleOpacityProperty);
      set => this.SetValue(MetroFlowItem.TitleOpacityProperty, (object) value);
    }
  }
}
