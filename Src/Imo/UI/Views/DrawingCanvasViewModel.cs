// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.UI.Views.DrawingCanvasViewModel
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll


namespace ImoSilverlightApp.UI.Views
{
  internal class DrawingCanvasViewModel : StickerItemViewModel
  {
    private static DrawingCanvasViewModel instance;

    private DrawingCanvasViewModel()
    {
    }

    public static DrawingCanvasViewModel Instance
    {
      get
      {
        if (DrawingCanvasViewModel.instance == null)
          DrawingCanvasViewModel.instance = new DrawingCanvasViewModel();
        return DrawingCanvasViewModel.instance;
      }
    }
  }
}
