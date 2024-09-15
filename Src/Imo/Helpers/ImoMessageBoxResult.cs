// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.Helpers.ImoMessageBoxResult
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll


namespace ImoSilverlightApp.Helpers
{
  public class ImoMessageBoxResult
  {
    private ImoMessageBoxResultType resultType;
    private string text;
    private static ImoMessageBoxResult noneResult = new ImoMessageBoxResult(ImoMessageBoxResultType.None);
    private static ImoMessageBoxResult yesResult = new ImoMessageBoxResult(ImoMessageBoxResultType.Yes);
    private static ImoMessageBoxResult noResult = new ImoMessageBoxResult(ImoMessageBoxResultType.No);
    private static ImoMessageBoxResult okResult = new ImoMessageBoxResult(ImoMessageBoxResultType.OK);
    private static ImoMessageBoxResult cancelResult = new ImoMessageBoxResult(ImoMessageBoxResultType.Cancel);

    public ImoMessageBoxResult(ImoMessageBoxResultType type, string text = null)
    {
      this.resultType = type;
      this.text = text;
    }

    public ImoMessageBoxResultType ResultType => this.resultType;

    public string Text => this.text;

    public static ImoMessageBoxResult None => ImoMessageBoxResult.noneResult;

    public static ImoMessageBoxResult Yes => ImoMessageBoxResult.yesResult;

    public static ImoMessageBoxResult No => ImoMessageBoxResult.noResult;

    public static ImoMessageBoxResult OK => ImoMessageBoxResult.okResult;

    public static ImoMessageBoxResult Cancel => ImoMessageBoxResult.cancelResult;

    public override bool Equals(object obj)
    {
      if (!(obj is ImoMessageBoxResult messageBoxResult))
        return false;
      return this.resultType == ImoMessageBoxResultType.Text && messageBoxResult.resultType == ImoMessageBoxResultType.Text ? this.text.Equals(messageBoxResult.Text) : this.resultType.Equals((object) messageBoxResult.resultType);
    }

    public override int GetHashCode()
    {
      return this.resultType == ImoMessageBoxResultType.Text ? (1073741827 * 16777619 ^ this.resultType.GetHashCode()) * 16777619 ^ (this.text ?? "").GetHashCode() : this.resultType.GetHashCode();
    }

    internal static ImoMessageBoxResult CreateTextResult(string text)
    {
      return new ImoMessageBoxResult(ImoMessageBoxResultType.Text, text);
    }
  }
}
