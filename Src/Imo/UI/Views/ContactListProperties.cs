// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.UI.Views.ContactListProperties
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Storage.Models;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace ImoSilverlightApp.UI.Views
{
  public static class ContactListProperties
  {
    private static readonly BitmapImage onlineImage = new BitmapImage(new Uri("/Assets/Icons/status_online_sm.png", UriKind.Relative));
    private static readonly BitmapImage awayImage = new BitmapImage(new Uri("/Assets/Icons/status_away_sm.png", UriKind.Relative));
    private static readonly BitmapImage offlineImage = new BitmapImage(new Uri("/Assets/Icons/status_offline_sm.png", UriKind.Relative));
    public static readonly DependencyProperty ContactPicUrlProperty = DependencyProperty.RegisterAttached("ContactPicUrl", typeof (string), typeof (ContactListProperties), new PropertyMetadata((object) "", (PropertyChangedCallback) (async (sender, e) =>
    {
      Ellipse ellipse = sender as Ellipse;
      ImageBrush ellipseBrush = ellipse?.Fill as ImageBrush;
      if (ellipse.Parent is UIElement && ((UIElement) ellipse.Parent).Visibility != Visibility.Visible || ellipseBrush == null)
        return;
      ellipseBrush.ImageSource = (ImageSource) null;
      string url = ContactListProperties.GetContactPicUrl((DependencyObject) ellipse);
      if (string.IsNullOrEmpty(url))
        return;
      BitmapImage bitmapImage = await IMO.ImageLoader.LoadImage(url);
      if (!(url == ContactListProperties.GetContactPicUrl((DependencyObject) ellipse)))
        return;
      ellipseBrush.ImageSource = (ImageSource) bitmapImage;
    })));
    public static readonly DependencyProperty PrimitiveProperty = DependencyProperty.RegisterAttached("Primitive", typeof (Primitive), typeof (ContactListProperties), new PropertyMetadata((object) Primitive.DoesNotApply, (PropertyChangedCallback) (async (sender, e) =>
    {
      await Task.Delay(200);
      if (!(sender is Image image2))
        return;
      switch (ContactListProperties.GetPrimitive((DependencyObject) image2))
      {
        case Primitive.Available:
          image2.Source = (ImageSource) ContactListProperties.onlineImage;
          break;
        case Primitive.Away:
          image2.Source = (ImageSource) ContactListProperties.awayImage;
          break;
        case Primitive.Offline:
          image2.Source = (ImageSource) ContactListProperties.offlineImage;
          break;
        default:
          image2.Source = (ImageSource) null;
          break;
      }
    })));

    public static string GetContactPicUrl(DependencyObject element)
    {
      return element != null ? (string) element.GetValue(ContactListProperties.ContactPicUrlProperty) : throw new ArgumentNullException(nameof (element));
    }

    public static void SetContactPicUrl(DependencyObject element, string value)
    {
      if (element == null)
        throw new ArgumentNullException(nameof (element));
      element.SetValue(ContactListProperties.ContactPicUrlProperty, (object) value);
    }

    public static Primitive GetPrimitive(DependencyObject obj)
    {
      return (Primitive) obj.GetValue(ContactListProperties.PrimitiveProperty);
    }

    public static void SetPrimitive(DependencyObject obj, Primitive value)
    {
      obj.SetValue(ContactListProperties.PrimitiveProperty, (object) value);
    }
  }
}
