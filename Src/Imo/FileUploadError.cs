// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.FileUploadError
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll


namespace ImoSilverlightApp
{
  internal static class FileUploadError
  {
    public const string FILE_TOO_BIG = "file_too_big";
    public const string FILE_EMPTY = "file_empty";
    public const string UID_NULL = "uid_null";
    public const string ERROR = "error";

    internal static string ErrorKeyToMessage(string errorKey)
    {
      switch (errorKey)
      {
        case "file_too_big":
          return "File is too big";
        case "file_empty":
          return "Cannot send empty file";
        case "uid_null":
          return "No user logged in";
        default:
          return "Error while sending file";
      }
    }
  }
}
