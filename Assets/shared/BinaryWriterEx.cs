using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace CTRFramework.Shared
{
    public class BinaryWriterEx : BinaryWriter
    {
        private bool[] writtenTo;
        public BinaryWriterEx(MemoryStream ms) : base(ms)
        {
            writtenTo = new bool[2097152];
        }

        public BinaryWriterEx(FileStream ms) : base(ms)
        {
            writtenTo = new bool[2097152];
        }
    
        public void WriteInt32Big(int value)
        {
            byte[] x = BitConverter.GetBytes(value);
            for (int i = 0; i < 4; i++) Write(x[i]);
        }
        public void WriteUInt32Big(uint value)
        {
            byte[] x = BitConverter.GetBytes(value);
            for (int i = 0; i < 4; i++) Write(x[i]);
        }
        public void WriteUInt16Big(ushort value)
        {
            byte[] x = BitConverter.GetBytes(value);
            for (int i = 0; i < 2; i++) Write(x[i]);
        }
        public void WriteInt16Big(short value)
        {
            byte[] x = BitConverter.GetBytes(value);
            for (int i = 0; i < 2; i++) Write(x[i]);
        }
        public void WriteArrayInt16(short[] array)
        {
            foreach (short value in array)
            {
                byte[] x = BitConverter.GetBytes(value);
                for (int i = 0; i < 2; i++) Write(x[i]);
            }
        }
        public void WriteArrayUInt16(ushort[] array)
        {
            foreach (ushort value in array)
            {
                byte[] x = BitConverter.GetBytes(value);
                for (int i = 0; i < 2; i++) Write(x[i]);
            }
        }
        public void WriteArrayInt32(int[] array)
        {
            foreach (int value in array)
            {
                byte[] x = BitConverter.GetBytes(value);
                for (int i = 0; i < 4; i++) Write(x[i]);
            }
        }
        public void WriteArrayUInt32(uint[] array)
        {
            foreach (uint value in array)
            {
                byte[] x = BitConverter.GetBytes(value);
                for (int i = 0; i < 4; i++) Write(x[i]);
            }
        }
        public void WriteStringFixed(string value)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(value);
            for (int i = 0; i < bytes.Length; i++) Write(bytes[(bytes.Length-1) - i]);
        }
        
        public void WriteBig(int value)
        {
            byte[] x = BitConverter.GetBytes(value);
            for (int i = 0; i < 4; i++) Write(x[3 - i]);
        }


        private int second = 0;
        private void MemoryOverwriteVerify(int startPos, int endPos)
        {
            for (int addr = startPos; addr < endPos; addr++)
            {
                if ( writtenTo[addr] == false)
                {
                    if (addr == 618400)
                    {
                        if (second == 1)
                        {
                            throw new InvalidOperationException("Where do you come from " + addr);
                        }

                        second++;
                    }

                    writtenTo[addr] = true;
                }
                else
                {
                    throw new InvalidOperationException("Double Write " + addr);
                }
            }
        }
        
        
    public override void Write(bool value)
    {
        var startPos = BaseStream.Position;
        base.Write(value);
        var endPos = BaseStream.Position;
        MemoryOverwriteVerify( (int)startPos,  (int)endPos);
    }

    public override void Write(byte value)
    {
        var startPos = BaseStream.Position;
        base.Write(value);
        var endPos = BaseStream.Position;
        MemoryOverwriteVerify( (int)startPos,  (int)endPos);
    }

    public override void Write(sbyte value)
    {
        var startPos = BaseStream.Position;
        base.Write(value);
        var endPos = BaseStream.Position;
        MemoryOverwriteVerify( (int)startPos,  (int)endPos);
    }

    public override void Write(byte[] buffer)
    {
        var startPos = BaseStream.Position;
        base.Write(buffer);
        var endPos = BaseStream.Position;
        MemoryOverwriteVerify( (int)startPos,  (int)endPos);
    }

    public override void Write(char ch)
    {
        var startPos = BaseStream.Position;
        base.Write(ch);
        var endPos = BaseStream.Position;
        MemoryOverwriteVerify( (int)startPos,  (int)endPos);
    }

    public override void Write(char[] chars)
    {
        var startPos = BaseStream.Position;
        base.Write(chars);
        var endPos = BaseStream.Position;
        MemoryOverwriteVerify( (int)startPos,  (int)endPos);
    }

    public override void Write(double value)
    {
        var startPos = BaseStream.Position;
        base.Write(value);
        var endPos = BaseStream.Position;
        MemoryOverwriteVerify( (int)startPos,  (int)endPos);
    }

    public override void Write(Decimal value)
    {
        var startPos = BaseStream.Position;
        base.Write(value);
        var endPos = BaseStream.Position;
        MemoryOverwriteVerify( (int)startPos,  (int)endPos);
    }
    
    public override void Write(short value)
    {
        var startPos = BaseStream.Position;
        base.Write(value);
        var endPos = BaseStream.Position;
        MemoryOverwriteVerify( (int)startPos,  (int)endPos);
    }
    
    public override void Write(ushort value)
    {
        var startPos = BaseStream.Position;
        base.Write(value);
        var endPos = BaseStream.Position;
        MemoryOverwriteVerify( (int)startPos,  (int)endPos);
    }

    public override void Write(int value)
    {
        var startPos = BaseStream.Position;
        base.Write(value);
        var endPos = BaseStream.Position;
        MemoryOverwriteVerify( (int)startPos,  (int)endPos);
    }

    public override void Write(uint value)
    {
        var startPos = BaseStream.Position;
        base.Write(value);
        var endPos = BaseStream.Position;
        MemoryOverwriteVerify( (int)startPos,  (int)endPos);
    }

    public override void Write(long value)
    {
        var startPos = BaseStream.Position;
        base.Write(value);
        var endPos = BaseStream.Position;
        MemoryOverwriteVerify( (int)startPos,  (int)endPos);
    }

    public override void Write(ulong value)
    {
        var startPos = BaseStream.Position;
        base.Write(value);
        var endPos = BaseStream.Position;
        MemoryOverwriteVerify( (int)startPos,  (int)endPos);
    }
    public override void Write(float value)
    {
        var startPos = BaseStream.Position;
        base.Write(value);
        var endPos = BaseStream.Position;
        MemoryOverwriteVerify( (int)startPos,  (int)endPos);
    }

    public override void Write(string value)
    {
        var startPos = BaseStream.Position;
        base.Write(value);
        var endPos = BaseStream.Position;
        MemoryOverwriteVerify( (int)startPos,  (int)endPos);
    }
    }
}
