// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.UI.Views.IconCircleButton
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace ImoSilverlightApp.UI.Views
{
  public class IconCircleButton : UserControl
  {
    public static readonly DependencyProperty iconNameProperty = DependencyProperty.Register(nameof (IconName), typeof (string), typeof (IconCircleButton), new PropertyMetadata((PropertyChangedCallback) null));
    internal UserControl iconCircleRoot;
    internal Border disabledContainer;
    internal Border iconContainer;
    internal Icon icon;
    private bool _contentLoaded;

    public event EventHandler Click;

    public IconCircleButton()
    {
      this.InitializeComponent();
      this.Loaded += new RoutedEventHandler(this.IconCircleButton_Loaded);
    }

    public string IconName
    {
      get => (string) this.GetValue(IconCircleButton.iconNameProperty);
      set => this.SetValue(IconCircleButton.iconNameProperty, (object) value);
    }

    private void IconCircleButton_Loaded(object sender, RoutedEventArgs e)
    {
      this.HandleSizeChanged();
    }

    private void OnClick()
    {
      if (this.Click == null)
        return;
      this.Click((object) this, EventArgs.Empty);
    }

    private void Grid_PointerPressed(object sender, RoutedEventArgs e)
    {
      this.iconContainer.Opacity = 0.7;
    }

    private void Grid_PointerReleased(object sender, RoutedEventArgs e)
    {
      this.iconContainer.Opacity = 1.0;
    }

    private void Grid_PointerExited(object sender, RoutedEventArgs e)
    {
      this.iconContainer.Opacity = 1.0;
    }

    private void root_SizeChanged(object sender, SizeChangedEventArgs e)
    {
      this.HandleSizeChanged();
    }

    private void HandleSizeChanged()
    {
      Border disabledContainer = this.disabledContainer;
      Border iconContainer = this.iconContainer;
      CornerRadius cornerRadius1 = new CornerRadius(this.iconCircleRoot.Height * 0.5);
      CornerRadius cornerRadius2 = cornerRadius1;
      iconContainer.CornerRadius = cornerRadius2;
      CornerRadius cornerRadius3 = cornerRadius1;
      disabledContainer.CornerRadius = cornerRadius3;
      this.icon.Margin = new Thickness(this.iconCircleRoot.Height * 0.25);
    }

    private void Grid_Tap(object sender, GestureEventArgs e)
    {
      if (!this.IsEnabled)
        return;
      this.OnClick();
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/ImoSilverlightApp;component/UI/Views/IconCircleButton.xaml", UriKind.Relative));
      this.iconCircleRoot = (UserControl) this.FindName("iconCircleRoot");
      this.disabledContainer = (Border) this.FindName("disabledContainer");
      this.iconContainer = (Border) this.FindName("iconContainer");
      this.icon = (Icon) this.FindName("icon");
    }
  }
}
