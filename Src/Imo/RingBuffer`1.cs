// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.RingBuffer`1
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using System;
using System.Threading;


namespace ImoSilverlightApp
{
  internal class RingBuffer<T>
  {
    private T[] buffer;
    private int rp;
    private int wp;
    private volatile int totalRead;
    private volatile int totalWritten;
    private int size;
    private bool shouldClear;

    public RingBuffer(int capacity)
    {
      this.size = capacity;
      this.rp = 0;
      this.wp = 0;
      this.totalRead = 0;
      this.totalWritten = 0;
      this.shouldClear = false;
      this.buffer = new T[this.size];
    }

    public int Read(T[] output, int samples)
    {
      if (this.shouldClear)
      {
        this.Clear();
        this.shouldClear = false;
      }
      int length1 = Math.Min(samples, this.AvailableReadSize);
      if (this.rp + length1 <= this.size)
      {
        Array.Copy((Array) this.buffer, this.rp, (Array) output, 0, length1);
      }
      else
      {
        int num = this.size - this.rp;
        int length2 = length1 - num;
        Array.Copy((Array) this.buffer, this.rp, (Array) output, 0, num);
        Array.Copy((Array) this.buffer, 0, (Array) output, num, length2);
      }
      Interlocked.MemoryBarrier();
      this.totalRead += length1;
      this.rp = (this.rp + length1) % this.size;
      return length1;
    }

    public int Read(out T output)
    {
      T[] output1 = new T[1];
      int num = this.Read(output1, 1);
      output = output1[0];
      return num;
    }

    public int Write(T[] input, int samples)
    {
      int length1 = Math.Min(samples, this.AvailableWriteSize);
      if (this.wp + length1 <= this.size)
      {
        Array.Copy((Array) input, 0, (Array) this.buffer, this.wp, length1);
      }
      else
      {
        int num = this.size - this.wp;
        int length2 = length1 - num;
        Array.Copy((Array) input, 0, (Array) this.buffer, this.wp, num);
        Array.Copy((Array) input, num, (Array) this.buffer, 0, length2);
      }
      Interlocked.MemoryBarrier();
      this.totalWritten += length1;
      this.wp = (this.wp + length1) % this.size;
      return length1;
    }

    public int Write(T input)
    {
      return this.Write(new T[1]{ input }, 1);
    }

    public void PostClear() => this.shouldClear = true;

    public void Clear(int samples = 2147483647)
    {
      samples = Math.Min(samples, this.AvailableReadSize);
      this.totalRead += samples;
      this.rp = (this.rp + samples) % this.size;
    }

    public int AvailableReadSize => this.totalWritten - this.totalRead;

    public int AvailableWriteSize => this.size - (this.totalWritten - this.totalRead);
  }
}
