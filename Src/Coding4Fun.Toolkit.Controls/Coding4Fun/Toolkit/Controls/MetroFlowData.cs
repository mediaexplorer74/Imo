// Decompiled with JetBrains decompiler
// Type: Coding4Fun.Toolkit.Controls.MetroFlowData
// Assembly: Coding4Fun.Toolkit.Controls, Version=2.1.7.0, Culture=neutral, PublicKeyToken=c5fd7b72b1a17ce4
// MVID: 213D8A75-4B66-4004-95C2-D82C49ACB764
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\Coding4Fun.Toolkit.Controls.dll

using System;
using System.ComponentModel;


namespace Coding4Fun.Toolkit.Controls
{
  public class MetroFlowData : INotifyPropertyChanged
  {
    private Uri _imageUri;
    private string _title;

    public Uri ImageUri
    {
      get => this._imageUri;
      set
      {
        this._imageUri = value;
        this.RaisePropertyChanged(nameof (ImageUri));
      }
    }

    public string Title
    {
      get => this._title;
      set
      {
        this._title = value;
        this.RaisePropertyChanged(nameof (Title));
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    private void RaisePropertyChanged(string propertyName)
    {
      if (this.PropertyChanged == null)
        return;
      this.PropertyChanged((object) this, new PropertyChangedEventArgs(propertyName));
    }
  }
}
