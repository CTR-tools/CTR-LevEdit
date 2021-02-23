using System.ComponentModel;
using UnityEngine;

namespace CTRFramework.Shared
{
    public class BoundingBox
    {
        [CategoryAttribute("Values"), DescriptionAttribute("Mininum.")]
        public Vector3s Min
        {
            get { return min; }
            set { Min = value; }
        }

        [CategoryAttribute("Values"), DescriptionAttribute("Maximum.")]
        public Vector3s Max
        {
            get { return max; }
            set { Max = value; }
        }


        private Vector3s min;
        private Vector3s max;


        public BoundingBox()
        {
            min = new Vector3s(short.MaxValue);
            max = new Vector3s(short.MinValue);
        }

        public BoundingBox(BinaryReaderEx br)
        {
            Read(br);
        }

        public void Read(BinaryReaderEx br)
        {
            min = new Vector3s(br);
            max = new Vector3s(br);
        }

        public void Write(BinaryWriterEx bw)
        {
            min.Write(bw);
            max.Write(bw);
        }

        public override string ToString()
        {
            return "BB: min " + min.ToString() + " max " + max.ToString();
        }

        public Bounds GetBounds()
        {
            Vector3 bmax = new Vector3(Max.X / 255.0f, Max.Y / 255.0f,-Max.Z / 255.0f);
            Vector3 bmin = new Vector3(Min.X / 255.0f, Min.Y / 255.0f,-Min.Z / 255.0f);
            Vector3 bsize = new Vector3(Mathf.Abs(bmax.x - bmin.x), Mathf.Abs(bmax.y - bmin.y),Mathf.Abs(bmax.z - bmin.z));
            Vector3 bpos = new Vector3((bmax.x + bmin.x) / 2.0f, (bmax.y + bmin.y) / 2.0f,
                (bmax.z + bmin.z) / 2.0f);
            Bounds b = new Bounds(bpos, bsize);
            
            return b;
        }
        public bool IsInside(Vector3 vec)
        {
            Bounds b = GetBounds();
            return b.Contains(vec);
        }

        /*
        public int Max(int x, int y, int z)
        {
            int max = x;
            if (y > max) max = y;
            if (z > max) max = z;
            return max;
        }

        public int Min(int x, int y, int z)
        {
            int min = x;
            if (y < min) min = y;
            if (z < min) min = z;
            return min;
        }
        */

    }
}