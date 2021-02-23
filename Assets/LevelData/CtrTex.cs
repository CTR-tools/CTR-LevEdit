using CTRFramework.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace CTRFramework
{
    public class CtrTex
    {
        public TextureLayout[] midlods = new TextureLayout[3];
        public TextureLayout[] hi = new TextureLayout[16];
        public List<TextureLayout> animframes = new List<TextureLayout>();

        public uint ptrHi;
        public bool isAnimated = false;
        private uint texpos;
        private short numFrames;
        private short whatsthat;
        private uint[] ptrs;

        public CtrTex(BinaryReaderEx br)
        {
            Read(br);
        }

        public void Read(BinaryReaderEx br)
        {
            int pos = (int)br.BaseStream.Position;

            if ((pos & 2) > 0)
            {
                Console.WriteLine("!!!");
                Console.ReadKey();
            }

            //this apparently defines animated texture, really
            if ((pos & 1) == 1)
            {
                isAnimated = true;

                br.BaseStream.Position -= 1;

                texpos = br.ReadUInt32();
                numFrames = br.ReadInt16();
                whatsthat = br.ReadInt16();

                if (br.ReadUInt32() != 0)
                    Helpers.Panic(this, "not 0!");

                ptrs = br.ReadArrayUInt32(numFrames);

                foreach (uint ptr in ptrs)
                {
                    if (ptr < br.BaseStream.Length)
                    {
                        br.Jump(ptr);
                        animframes.Add(TextureLayout.FromStream(br));
                    }
                    else
                    {
                        Debug.LogWarning("CtrTex -- Animation Frame Overflow");
                    }
                }

                br.Jump(texpos);
            }

            for (int i = 0; i < 3; i++)
                midlods[i] = TextureLayout.FromStream(br);

            //Console.WriteLine(br.BaseStream.Position.ToString("X8"));
            //Console.ReadKey();
            
            ptrHi = br.ReadUInt32();

            //loosely assume we got a valid pointer
            if (ptrHi > 0x30000 && ptrHi < 0xB0000)
            {
                br.Jump(ptrHi);

                for (int i = 0; i < 16; i++)
                {
                    hi[i] = TextureLayout.FromStream(br);
                }
            }

        }

        public void Write(BinaryWriterEx bw)
        {
            int pos = (int)bw.BaseStream.Position;
            if (isAnimated)
            {
                if ((pos & 1) != 1)
                {
                    bw.BaseStream.Position -= 1;
                }


                bw.Write(texpos);
                bw.Write(numFrames);
                bw.Write(numFrames);

                bw.Write(0);

                bw.WriteArrayUInt32(ptrs);
                int i = 0;
                foreach (uint ptr in ptrs)
                {
                    bw.Seek((int) ptr + 4, SeekOrigin.Begin);
                    animframes[i].Write(bw);
                    i++;
                }

                bw.Seek((int) texpos + 4, SeekOrigin.Begin);
            }
            
            for (int i = 0; i < 3; i++)
                midlods[i].Write(bw);

            bw.Write(ptrHi);

            //loosely assume we got a valid pointer
            if (ptrHi > 0x30000 && ptrHi < 0xB0000)
            {
                bw.Seek((int) ptrHi + 4, SeekOrigin.Begin);

                for (int i = 0; i < 16; i++)
                {
                    hi[i].Write(bw);
                }
            }
        }
    }
}
