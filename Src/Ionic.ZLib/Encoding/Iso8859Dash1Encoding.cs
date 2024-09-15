// Decompiled with JetBrains decompiler
// Type: Ionic.Encoding.Iso8859Dash1Encoding
// Assembly: Ionic.ZLib, Version=2.0.0.14, Culture=neutral, PublicKeyToken=null
// MVID: AE1A51CF-981C-4AD1-9F5E-F78F9C4A3637
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\Ionic.ZLib.dll

using System;

#nullable disable
namespace Ionic.Encoding
{
  public class Iso8859Dash1Encoding : System.Text.Encoding
  {
    public override string WebName => "iso-8859-1";

    public override int GetBytes(char[] chars, int start, int count, byte[] bytes, int byteIndex)
    {
      if (chars == null)
        throw new ArgumentNullException(nameof (chars), "null array");
      if (bytes == null)
        throw new ArgumentNullException(nameof (bytes), "null array");
      if (start < 0)
        throw new ArgumentOutOfRangeException(nameof (start));
      if (count < 0)
        throw new ArgumentOutOfRangeException("charCount");
      if (chars.Length - start < count)
        throw new ArgumentOutOfRangeException(nameof (chars));
      if (byteIndex < 0 || byteIndex > bytes.Length)
        throw new ArgumentOutOfRangeException(nameof (byteIndex));
      for (int index = 0; index < count; ++index)
      {
        char ch = chars[start + index];
        bytes[byteIndex + index] = ch < 'ÿ' ? (byte) ch : (byte) 63;
      }
      return count;
    }

    public override int GetChars(byte[] bytes, int start, int count, char[] chars, int charIndex)
    {
      if (chars == null)
        throw new ArgumentNullException(nameof (chars), "null array");
      if (bytes == null)
        throw new ArgumentNullException(nameof (bytes), "null array");
      if (start < 0)
        throw new ArgumentOutOfRangeException(nameof (start));
      if (count < 0)
        throw new ArgumentOutOfRangeException("charCount");
      if (bytes.Length - start < count)
        throw new ArgumentOutOfRangeException(nameof (bytes));
      if (charIndex < 0 || charIndex > chars.Length)
        throw new ArgumentOutOfRangeException(nameof (charIndex));
      for (int index = 0; index < count; ++index)
        chars[charIndex + index] = (char) bytes[index + start];
      return count;
    }

    public override int GetByteCount(char[] chars, int index, int count) => count;

    public override int GetCharCount(byte[] bytes, int index, int count) => count;

    public override int GetMaxByteCount(int charCount) => charCount;

    public override int GetMaxCharCount(int byteCount) => byteCount;

    public static int CharacterCount => 256;
  }
}
