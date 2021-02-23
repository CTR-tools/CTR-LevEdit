using CTRFramework.Shared;
using System;
using System.Collections.Generic;

namespace CTRFramework
{
    public class UnkAdv
    {
        public List<uint> uintDat = new List<uint>();
        public List<PosAng> smth = new List<PosAng>();
        private uint ttl;
        public UnkAdv(BinaryReaderEx br, uint cnt)
        {
            ttl = 0;
            for (int i = 0; i < cnt; i++)
            {
                uint c = br.ReadUInt32();
                uintDat.Add(c);
                ttl += c;
                uint d = br.ReadUInt32(); //is this used for anything?
                uintDat.Add(d);

                Console.WriteLine(c);
                Console.WriteLine(br.BaseStream.Position.ToString("X8"));
            }

            Console.WriteLine(br.BaseStream.Position.ToString("X8"));
            //Console.ReadKey();

            for (int i = 0; i < ttl; i++)
            {
                smth.Add(new PosAng(br));
            }
        }
        public void Write(BinaryWriterEx bw) {

            bw.WriteArrayUInt32(uintDat.ToArray());
            
            for (int i = 0; i < ttl; i++)
            {
                smth[i].Write(bw);
            }
        }
    }
}