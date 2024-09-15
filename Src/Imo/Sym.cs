// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.Sym
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using System;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;


namespace ImoSilverlightApp
{
  internal sealed class Sym
  {
    private static Random random = new Random();
    public const int VERSION = 1;
    private static byte[] secretKey = (byte[]) null;

    public static byte[] GetSecretKey()
    {
      if (Sym.secretKey == null)
        Sym.secretKey = CryptographicBuffer.GenerateRandom(16U).ToArray();
      return Sym.secretKey;
    }

    public static byte[] GetNotSoSecretKey()
    {
      return Convert.FromBase64String("GkN6SXh8Pg9UZSlcEON4nw==");
    }

    public static byte[] Decrypt(byte[] data, byte[] key, byte[] iv)
    {
      byte[] numArray = new byte[16];
      Array.Copy((Array) iv, 0, (Array) numArray, 0, 12);
      return Sym.DecryptAES(data, key, numArray, Sym.PaddingMode.PKCS7);
    }

    public static int FirstDecrypt(byte[] first, byte[] key, byte[] iv, bool isNameChannel = false)
    {
      byte[] sourceArray = Sym.Decrypt16(first, key);
      Array.Copy((Array) sourceArray, 4, (Array) iv, 0, 12);
      int num1 = (int) sourceArray[0];
      int num2 = ((int) sourceArray[1] & (int) byte.MaxValue) << 16 | ((int) sourceArray[2] & (int) byte.MaxValue) << 8 | (int) sourceArray[3] & (int) byte.MaxValue;
      if (!isNameChannel)
        return num2;
      return num2;
    }

    private static byte[] Decrypt16(byte[] input, byte[] key)
    {
      return Sym.DecryptAES(input, key, new byte[16], Sym.PaddingMode.None);
    }

    public static byte[] GenerateIv(int length)
    {
      byte[] buffer = new byte[length];
      Sym.random.NextBytes(buffer);
      return buffer;
    }

    private static byte[] EncryptFirstBlock(int length, byte[] iv, byte[] key, bool isNameChannel)
    {
      byte[] numArray = new byte[16];
      if (isNameChannel)
        numArray[0] = (byte) 1;
      numArray[1] = (byte) (length >> 16);
      numArray[2] = (byte) (length >> 8);
      numArray[3] = (byte) length;
      Array.Copy((Array) iv, 0, (Array) numArray, 4, 12);
      return Sym.Encrypt16(numArray, key);
    }

    private static byte[] Encrypt16(byte[] input, byte[] key)
    {
      return Sym.EncryptAES(input, key, new byte[16], Sym.PaddingMode.None);
    }

    public static byte[] Encrypt(byte[] input, byte[] key, bool isNameChannel = false)
    {
      byte[] iv = Sym.GenerateIv(12);
      byte[] numArray = new byte[16];
      Array.Copy((Array) iv, 0, (Array) numArray, 0, 12);
      byte[] sourceArray1 = Sym.EncryptAES(input, key, numArray, Sym.PaddingMode.PKCS7);
      byte[] sourceArray2 = Sym.EncryptFirstBlock(sourceArray1.Length, iv, key, isNameChannel);
      byte[] destinationArray = new byte[sourceArray2.Length + sourceArray1.Length];
      Array.Copy((Array) sourceArray2, (Array) destinationArray, sourceArray2.Length);
      Array.Copy((Array) sourceArray1, 0, (Array) destinationArray, sourceArray2.Length, sourceArray1.Length);
      return destinationArray;
    }

    private static byte[] EncryptAES(
      byte[] input,
      byte[] Key,
      byte[] IV,
      Sym.PaddingMode paddingMode)
    {
      if (input == null || input.Length == 0)
        throw new ArgumentNullException(nameof (input));
      if (Key == null || Key.Length == 0)
        throw new ArgumentNullException(nameof (Key));
      if (IV == null || IV.Length == 0)
        throw new ArgumentNullException(nameof (IV));
      return CryptographicEngine.Encrypt(SymmetricKeyAlgorithmProvider.OpenAlgorithm(paddingMode == Sym.PaddingMode.None ? SymmetricAlgorithmNames.AesCbc : SymmetricAlgorithmNames.AesCbcPkcs7).CreateSymmetricKey(Key.AsBuffer()), input.AsBuffer(), IV.AsBuffer()).ToArray();
    }

    private static byte[] DecryptAES(
      byte[] input,
      byte[] Key,
      byte[] IV,
      Sym.PaddingMode paddingMode)
    {
      if (input == null || input.Length == 0)
        throw new ArgumentNullException(nameof (input));
      if (Key == null || Key.Length == 0)
        throw new ArgumentNullException(nameof (Key));
      if (IV == null || IV.Length == 0)
        throw new ArgumentNullException(nameof (IV));
      return CryptographicEngine.Decrypt(SymmetricKeyAlgorithmProvider.OpenAlgorithm(paddingMode == Sym.PaddingMode.None ? SymmetricAlgorithmNames.AesCbc : SymmetricAlgorithmNames.AesCbcPkcs7).CreateSymmetricKey(Key.AsBuffer()), input.AsBuffer(), IV.AsBuffer()).ToArray();
    }

    private enum PaddingMode
    {
      None,
      PKCS7,
    }
  }
}
