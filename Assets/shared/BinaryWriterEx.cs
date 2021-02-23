using System;
using System.IO;
using System.Text;

namespace CTRFramework.Shared
{
    public class BinaryWriterEx : BinaryWriter
    {
        public BinaryWriterEx(MemoryStream ms) : base(ms)
        {
        }

        public BinaryWriterEx(FileStream ms) : base(ms)
        {
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

    }
}
