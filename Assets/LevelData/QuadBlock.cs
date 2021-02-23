using CTRFramework.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace CTRFramework
{
    public struct FaceFlags
    {
        public RotateFlipType rotateFlipType;
        public FaceMode faceMode;

        public FaceFlags(byte x)
        {
            rotateFlipType = (RotateFlipType)(x & 7);
            faceMode = (FaceMode)(x >> 3 & 3);
        }
    }
    
    [System.Serializable]
    public class QuadBlock : IReadWrite
    {
        /*
         * 0--4--1
         * | /| /|
         * |/ |/ |
         * 5--6--7
         * | /| /|
         * |/ |/ |
         * 2--8--3
         */

        //9 indices in vertex array, that form 4 quads, see above.
        public short[] ind = new short[9];
        public QuadFlags quadFlags;

        public uint bitvalue; //important! big endian!

        //these values are contained in bitvalue, mask is 8b5b5b5b5b4z where b is bit and z is empty. or is it?
        public byte drawOrderLow;
        public FaceFlags[] faceFlags = new FaceFlags[4];
        public byte extradata;

        public byte[] drawOrderHigh = new byte[4];

        public uint[] ptrTexMid = new uint[4];    //offsets to mid texture definition

        public BoundingBox bb;              //a box that bounds

        public TerrainFlags terrainFlag;
        public byte WeatherIntensity;
        public byte WeatherType;
        public byte TerrainFlagUnknown;

        public short id;
        public byte trackPos;
        public byte midunk;

        //public byte[] midflags = new byte[2];

        public uint ptrTexLow;                 //offset to LOD texture definition
        public int offset2;

        public List<Vector2s> unk3 = new List<Vector2s>();  //unknown

        //additional data
        public TextureLayout texlow;

        public List<CtrTex> tex = new List<CtrTex>();


        public QuadBlock()
        {
        }

        public QuadBlock(BinaryReaderEx br)
        {
            Read(br);
        }


        public void Read(BinaryReaderEx br)
        {
            long pos = br.BaseStream.Position;

            for (int i = 0; i < 9; i++)
                ind[i] = br.ReadInt16();

            quadFlags = (QuadFlags)br.ReadUInt16();

            bitvalue = br.ReadUInt32Big();
            {
                drawOrderLow = (byte)(bitvalue & 0xFF);

                for (int i = 0; i < 4; i++)
                {
                    byte val = (byte)(bitvalue >> 8 + 5 * i & 0x1F);
                    faceFlags[i] = new FaceFlags(val);
                }

                extradata = (byte)(bitvalue & 0xF0000000 >> 28);
            }

            drawOrderHigh = br.ReadBytes(4);

            for (int i = 0; i < 4; i++)
                ptrTexMid[i] = br.ReadUInt32();

            bb = new BoundingBox(br);

            byte tf = br.ReadByte();

            if (tf > 20)
                Helpers.Panic(this, "unexpected terrain flag value -> " + tf);

            terrainFlag = (TerrainFlags)tf;
            WeatherIntensity = br.ReadByte();
            WeatherType = br.ReadByte();
            TerrainFlagUnknown = br.ReadByte();

            id = br.ReadInt16();

            trackPos = br.ReadByte();
            midunk = br.ReadByte();

            //midflags = br.ReadBytes(2);
            Debug.Log(br.BaseStream.Position);
            ptrTexLow = br.ReadUInt32();
            offset2 = br.ReadInt32();
            Debug.Log(offset2);

            for (int i = 0; i < 5; i++)
            {
                Vector2s v = new Vector2s(br);
                unk3.Add(v);
                Debug.Log(v);
            }

            //struct done

            //read texture layouts
            int texpos = (int)br.BaseStream.Position;

            Debug.Log(ptrTexLow);
            br.Jump(ptrTexLow);
            texlow = TextureLayout.FromStream(br);

            Debug.Log(ptrTexLow/16 + ", " + ptrTexMid.Length);
            Debug.Log(ptrTexMid[0] + ", " + ptrTexMid[1] + ", " + ptrTexMid[2] + ", " + ptrTexMid[3] + "; ");
            
            if (38435 == ptrTexLow/ 16)
            {
                Debug.Log("----------THE INTERESTING ONE----------");
            }
            foreach (uint u in ptrTexMid)
            {
                if (u != 0)
                {
                    br.Jump(u);
                    tex.Add(new CtrTex(br));
                }
                else
                {
                    if (ptrTexLow != 0) Console.WriteLine("!");
                }
            }

            br.BaseStream.Position = texpos;
        }

        public void Write(BinaryWriterEx bw)
        {
            bw.WriteArrayInt16(ind);

            bw.Write((ushort)quadFlags);
            //bw.Write(unk1);

            // bw.Write(drawOrderLow);
            bw.Write(bitvalue);
            bw.Write(drawOrderHigh);

            bw.WriteArrayUInt32(ptrTexMid);

            bb.Write(bw);

            bw.Write((byte)terrainFlag);
            bw.Write(WeatherIntensity);
            bw.Write(WeatherType);
            bw.Write(TerrainFlagUnknown);

            bw.Write(id);

            bw.Write(trackPos);
            bw.Write(midunk);

            Debug.Log(bw.BaseStream.Position);
            bw.Write(ptrTexLow);
            bw.Write(offset2);
            Debug.Log(offset2);

            foreach (Vector2s v in unk3)
            {
                v.Write(bw);
                
                Debug.Log(v);
            }

            Debug.Log(ptrTexLow);
            
            long texpos = bw.BaseStream.Position;
            bw.Seek((int) ptrTexLow + 4, SeekOrigin.Begin);
            texlow.Write(bw);

            int texIndex = 0;
            Debug.Log((ptrTexLow + 4)/16 + ", " + ptrTexMid.Length/16);
            if (38435 == (ptrTexLow + 4) / 16)
            {
                Debug.Log("----------THE INTERESTING ONE----------");
            }
            foreach (uint u in ptrTexMid)
            {
                bw.Seek((int) u + 4, SeekOrigin.Begin);
                if (tex[texIndex] != null)
                {
                    tex[texIndex].Write(bw);
                }
                texIndex++;
            }
            if (38435 == (ptrTexLow + 4) / 16)
            {
                Debug.Log("----------THE INTERESTING ONE----------");
            }
            bw.Seek((int) texpos, SeekOrigin.Begin);
        }
        public void RecalcBB(List<Vertex> vert)
        {
            BoundingBox bb_new = new BoundingBox();

            foreach (int i in ind)
            {
                if (vert[i].coord.X < bb_new.Min.X) bb_new.Min.X = vert[i].coord.X;
                if (vert[i].coord.X > bb_new.Max.X) bb_new.Max.X = vert[i].coord.X;

                if (vert[i].coord.Y < bb_new.Min.Y) bb_new.Min.Y = vert[i].coord.Y;
                if (vert[i].coord.Y > bb_new.Max.Y) bb_new.Max.Y = vert[i].coord.Y;

                if (vert[i].coord.Z < bb_new.Min.Z) bb_new.Min.Z = vert[i].coord.Z;
                if (vert[i].coord.Z > bb_new.Max.Z) bb_new.Max.Z = vert[i].coord.Z;
            }
        }

        //magic array of indices, each line contains 2 quads
        public int[] inds = new int[]
        {
            6, 5, 1,
            6, 7, 5,
            7, 2, 5,
            7, 8, 2,
            3, 7, 6,
            3, 9, 7,
            9, 8, 7,
            9, 4, 8
        };

        /*
        //magic array of indices, each line contains 2 quads
        int[] inds = new int[]
        {
            1, 6, 5, 5, 6, 7,
            5, 7, 2, 2, 7, 8,
            6, 3, 7, 7, 3, 9,
            7, 9, 8, 8, 9, 4
        };
        */
        /*
         * 1--5--2
         * | /| /|
         * |/ |/ |
         * 6--7--8
         * | /| /|
         * |/ |/ |
         * 3--9--4
         */
        public Vector2[] vertUVPredefined =
        {
            new Vector2(0.0f, 0.0f),  //0
            new Vector2(0.0f, 1.0f),  //1
            new Vector2(1.0f, 0.0f),  //2
            new Vector2(1.0f, 1.0f),  //3
            new Vector2(0.0f, 0.5f),  //4
            new Vector2(0.5f, 0.0f),  //5
            new Vector2(0.5f, 0.5f),  //6
            new Vector2(0.5f, 1.0f),  //7
            new Vector2(1.0f, 0.5f)   //8
        };
        public List<CTRFramework.Vertex> GetVertexList(SceneHandler s, bool mid = true)
        {
            List<CTRFramework.Vertex> buf = new List<CTRFramework.Vertex>();
            if (mid)
            {
                for (int i = inds.Length - 1; i >= 0; i--)
                {
                    var vert = s.verts[ind[inds[i] - 1]];
                    long index = (inds[i] - 1) % vertUVPredefined.Length;
                    vert.uv = new Vector2(vertUVPredefined[index].y,vertUVPredefined[index].x);
                    vert.uv.x *= 2;
                    vert.uv.y *= 2;
                    buf.Add(vert);
                }
            }
            else
            {
                /*
                 * 0--1
                 * | /|
                 * |/ |
                 * 2--3
  
                 */
                int[] arrind = new int[] { 0, 1, 2, 1, 2, 3 };
                for (int j = 0; j < 6; j++)
                    buf.Add(s.verts[ind[arrind[j]]]);

                buf[0].uv = new Vector2(0f, 0f); // 0
                buf[1].uv = new Vector2(1f, 0f); // 1
                buf[2].uv = new Vector2(0f, 1f); // 2
                buf[3].uv = new Vector2(1f, 0f); // 1
                buf[4].uv = new Vector2(0f, 1f); // 2
                buf[5].uv = new Vector2(1f, 1f); // 3
            }

            return buf;
        }


        //use this later for obj export too
        public List<CTRFramework.Vertex> GetVertexListq(List<Vertex> v, int i)
        {
            try
            {
                List<CTRFramework.Vertex> buf = new List<CTRFramework.Vertex>();

                if (i == -1) // low
                {
                    int[] arrind = new int[] { 0, 1, 2, 3 };

                    for (int j = 0; j < 4; j++)
                        buf.Add(v[ind[arrind[j]]]);

                    for (int j = 0; j < 4; j++)
                    {
                        buf[j].uv = new Vector2(texlow.normuv[j].X, texlow.normuv[j].Y);
                    }

                    if (buf.Count != 4)
                    {
                        Helpers.Panic(this, "not a quad! " + buf.Count);
                        Console.ReadKey();
                    }

                    return buf;
                }
                else //mid
                {
                    int[] arrind;
                    int[] uvinds;

                    switch (faceFlags[i].rotateFlipType)
                    {
                        case RotateFlipType.None: uvinds = GetUVIndices2(1, 2, 3, 4); break;
                        case RotateFlipType.Rotate90: uvinds = GetUVIndices2(3, 1, 4, 2); break;
                        case RotateFlipType.Rotate180: uvinds = GetUVIndices2(4, 3, 2, 1); break;
                        case RotateFlipType.Rotate270: uvinds = GetUVIndices2(2, 4, 1, 3); break;
                        case RotateFlipType.Flip: uvinds = GetUVIndices2(2, 1, 4, 3); break;
                        case RotateFlipType.FlipRotate90: uvinds = GetUVIndices2(4, 2, 3, 1); break;
                        case RotateFlipType.FlipRotate180: uvinds = GetUVIndices2(3, 4, 1, 2); break;
                        case RotateFlipType.FlipRotate270: uvinds = GetUVIndices2(1, 3, 2, 4); break;
                        default: throw new Exception("Impossible rotatefliptype.");
                    }


                    switch (faceFlags[i].faceMode)
                    {
                        case FaceMode.SingleUV1:
                            {
                                uvinds = new int[] { uvinds[2], uvinds[0], uvinds[3], uvinds[1] };
                                //uvinds = new int[] { uvinds[0], uvinds[0], uvinds[0], uvinds[0] };
                                break;
                            }

                        case FaceMode.SingleUV2:
                            {
                                uvinds = new int[] { uvinds[1], uvinds[2], uvinds[3], uvinds[0] };
                                //uvinds = new int[] { uvinds[0], uvinds[0], uvinds[0], uvinds[0] };
                                break;
                            }
                    }


                    switch (i)
                    {
                        case 0: arrind = new int[4] { 0, 4, 5, 6 }; break;
                        case 1: arrind = new int[4] { 4, 1, 6, 7 }; break;
                        case 2: arrind = new int[4] { 5, 6, 2, 8 }; break;
                        case 3: arrind = new int[4] { 6, 7, 8, 3 }; break;
                        default: throw new Exception("Can't have more than 4 quads in a quad block.");
                    }

                    for (int j = 0; j < 4; j++)
                        buf.Add(v[ind[arrind[j]]]);


                    for (int j = 0; j < 4; j++)
                    {
                        if (tex.Count > 0)
                        {
                            if (!tex[i].isAnimated)
                            {
                                var iuv = tex[i].midlods[2].normuv[uvinds[j] - 1];
                                buf[j].uv = new Vector2(iuv.X,iuv.Y);
                            }
                            else
                            {
                                var iuv = tex[i].animframes[1].normuv[uvinds[j] - 1];
                                buf[j].uv = new Vector2(iuv.X,iuv.Y);
                            }
                        }
                        else
                        {
                            buf[j].uv = new Vector2((byte)((j & 3) >> 1), (byte)(j & 1));
                        }
                    }

                    if (buf.Count != 4)
                    {
                        Helpers.Panic(this, "not a quad! " + buf.Count);
                        Console.ReadKey();
                    }
                }

                return buf;
            }
            catch (Exception ex)
            {
                Helpers.Panic(this, "Can't export quad to MG. Give null.\r\n" + i + "\r\n" + ex.Message);
                return null;
            }
        }

        public int[] GetUVIndices2(int x, int y, int z, int w)
        {
            return new int[]
            {
                x, y, z, w
            };
        }
        
    }
}
