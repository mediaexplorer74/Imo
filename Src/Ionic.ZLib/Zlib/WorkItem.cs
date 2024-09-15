// Decompiled with JetBrains decompiler
// Type: Ionic.Zlib.WorkItem
// Assembly: Ionic.ZLib, Version=2.0.0.14, Culture=neutral, PublicKeyToken=null
// MVID: AE1A51CF-981C-4AD1-9F5E-F78F9C4A3637
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\Ionic.ZLib.dll

#nullable disable
namespace Ionic.Zlib
{
  internal class WorkItem
  {
    public byte[] buffer;
    public byte[] compressed;
    public int crc;
    public int index;
    public int ordinal;
    public int inputBytesAvailable;
    public int compressedBytesAvailable;
    public ZlibCodec compressor;

    public WorkItem(
      int size,
      CompressionLevel compressLevel,
      CompressionStrategy strategy,
      int ix)
    {
      this.buffer = new byte[size];
      this.compressed = new byte[size + (size / 32768 + 1) * 5 * 2];
      this.compressor = new ZlibCodec();
      this.compressor.InitializeDeflate(compressLevel, false);
      this.compressor.OutputBuffer = this.compressed;
      this.compressor.InputBuffer = this.buffer;
      this.index = ix;
    }
  }
}
