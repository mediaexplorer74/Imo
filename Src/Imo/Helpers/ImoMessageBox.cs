// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.Helpers.ImoMessageBox
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using Microsoft.Phone.Controls;
using System;
using System.Threading.Tasks;
using System.Windows;


namespace ImoSilverlightApp.Helpers
{
  internal class ImoMessageBox
  {
    private static ImoMessageBoxResult noneResult = new ImoMessageBoxResult(ImoMessageBoxResultType.None);
    private static ImoMessageBoxResult yesResult = new ImoMessageBoxResult(ImoMessageBoxResultType.Yes);
    private static ImoMessageBoxResult noResult = new ImoMessageBoxResult(ImoMessageBoxResultType.No);
    private static ImoMessageBoxResult okResult = new ImoMessageBoxResult(ImoMessageBoxResultType.OK);
    private static ImoMessageBoxResult cancelResult = new ImoMessageBoxResult(ImoMessageBoxResultType.Cancel);

    public static Task<ImoMessageBoxResult> Show(string message, ImoMessageBoxButton messageBoxType = ImoMessageBoxButton.OK)
    {
      TaskCompletionSource<ImoMessageBoxResult> taskCompletionSource = new TaskCompletionSource<ImoMessageBoxResult>();
      switch (messageBoxType)
      {
        case ImoMessageBoxButton.OK:
          CustomMessageBox customMessageBox1 = new CustomMessageBox();
          customMessageBox1.Caption = "";
          customMessageBox1.Message = message;
          customMessageBox1.IsLeftButtonEnabled = false;
          customMessageBox1.RightButtonContent = (object) "OK";
          customMessageBox1.VerticalAlignment = VerticalAlignment.Center;
          customMessageBox1.IsFullScreen = false;
          customMessageBox1.Dismissed += (EventHandler<DismissedEventArgs>) ((s1, e1) =>
          {
            if (e1.Result == CustomMessageBoxResult.RightButton)
              taskCompletionSource.SetResult(ImoMessageBoxResult.OK);
          });
          customMessageBox1.Show();
          break;
        case ImoMessageBoxButton.OKCancel:
          CustomMessageBox customMessageBox2 = new CustomMessageBox();
          customMessageBox2.Caption = "";
          customMessageBox2.Message = message;
          customMessageBox2.LeftButtonContent = (object) "OK";
          customMessageBox2.RightButtonContent = (object) "Cancel";
          customMessageBox2.IsFullScreen = false;
          customMessageBox2.VerticalAlignment = VerticalAlignment.Center;
          customMessageBox2.Dismissed += (EventHandler<DismissedEventArgs>) ((s1, e1) =>
          {
            switch (e1.Result)
            {
              case CustomMessageBoxResult.LeftButton:
                taskCompletionSource.SetResult(ImoMessageBoxResult.OK);
                break;
              case CustomMessageBoxResult.RightButton:
                taskCompletionSource.SetResult(ImoMessageBoxResult.Cancel);
                break;
            }
          });
          customMessageBox2.Show();
          break;
        case ImoMessageBoxButton.YesNo:
          CustomMessageBox customMessageBox3 = new CustomMessageBox();
          customMessageBox3.Caption = "";
          customMessageBox3.Message = message;
          customMessageBox3.LeftButtonContent = (object) "Yes";
          customMessageBox3.RightButtonContent = (object) "No";
          customMessageBox3.IsFullScreen = false;
          customMessageBox3.Dismissed += (EventHandler<DismissedEventArgs>) ((s1, e1) =>
          {
            switch (e1.Result)
            {
              case CustomMessageBoxResult.LeftButton:
                taskCompletionSource.SetResult(ImoMessageBoxResult.Yes);
                break;
              case CustomMessageBoxResult.RightButton:
                taskCompletionSource.SetResult(ImoMessageBoxResult.No);
                break;
            }
          });
          customMessageBox3.Show();
          break;
        default:
          throw new Exception("Unsupported message box type: " + (object) messageBoxType);
      }
      return taskCompletionSource.Task;
    }
  }
}
