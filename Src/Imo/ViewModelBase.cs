// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.ViewModelBase
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using NLog;
using System.ComponentModel;
using System.Windows;


namespace ImoSilverlightApp
{
  public abstract class ViewModelBase : INotifyPropertyChanged
  {
    private static readonly Logger log = LogManager.GetLogger(typeof (ViewModelBase).Name);
    private FrameworkElement element;

    public ViewModelBase(FrameworkElement element)
    {
      this.element = element;
      element.Loaded += new RoutedEventHandler(this.OnLoaded);
      element.Unloaded += new RoutedEventHandler(this.OnUnloaded);
    }

    public virtual void Dispose()
    {
      if (this.element != null)
      {
        this.element.Loaded -= new RoutedEventHandler(this.OnLoaded);
        this.element.Unloaded -= new RoutedEventHandler(this.OnUnloaded);
      }
      else
        ViewModelBase.log.Error("Trying to dispose already disposed ViewModel of type " + this.GetType().Name, 33, nameof (Dispose));
      this.element = (FrameworkElement) null;
    }

    protected virtual void OnLoaded(object sender, RoutedEventArgs e)
    {
    }

    protected virtual void OnUnloaded(object sender, RoutedEventArgs e)
    {
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string name)
    {
      PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
      if (propertyChanged == null)
        return;
      propertyChanged((object) this, new PropertyChangedEventArgs(name));
    }
  }
}
