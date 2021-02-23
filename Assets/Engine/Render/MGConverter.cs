using CTRFramework.Shared;
using UnityEngine;

namespace ctrviewer
{
    class MGConverter
    {
        public static Vector3 ToVector3(Vector3s s, float scale = 1.0f)
        {
            return new Vector3(s.X * scale, s.Y * scale, s.Z * scale);
        }
        public static Vector3 ToVector3(Vector4s s, float scale = 1.0f)
        {
            return new Vector3(s.X * scale, s.Y * scale, s.Z * scale);
        }

        public static Color ToColor(Vector4 s)
        {
            return new Color(s.x, s.y, s.z, s.w);
        }

        public static VertexPositionColorTexture[] ToLineList(BoundingBox bbox)
        {
            Vector3 min = ToVector3(bbox.Min, 0.01f);
            Vector3 max = ToVector3(bbox.Max, 0.01f);

            return new VertexPositionColorTexture[]
            {
                new VertexPositionColorTexture(new Vector3(min.x, min.y, min.z), Color.white, new Vector2b(0,0)),
                new VertexPositionColorTexture(new Vector3(max.x, min.y, min.z), Color.white, new Vector2b(0,0)),
                new VertexPositionColorTexture(new Vector3(max.x, min.y, min.z), Color.white, new Vector2b(0,0)),
                new VertexPositionColorTexture(new Vector3(max.x, max.y, min.z), Color.white, new Vector2b(0,0)),

                new VertexPositionColorTexture(new Vector3(max.x, max.y, min.z), Color.white, new Vector2b(0,0)),
                new VertexPositionColorTexture(new Vector3(min.x, max.y, min.z), Color.white, new Vector2b(0,0)),
                new VertexPositionColorTexture(new Vector3(min.x, max.y, min.z), Color.white, new Vector2b(0,0)),
                new VertexPositionColorTexture(new Vector3(min.x, min.y, min.z), Color.white, new Vector2b(0,0)),

                new VertexPositionColorTexture(new Vector3(min.x, min.y, max.z), Color.white, new Vector2b(0,0)),
                new VertexPositionColorTexture(new Vector3(max.x, min.y, max.z), Color.white, new Vector2b(0,0)),
                new VertexPositionColorTexture(new Vector3(max.x, min.y, max.z), Color.white, new Vector2b(0,0)),
                new VertexPositionColorTexture(new Vector3(max.x, max.y, max.z), Color.white, new Vector2b(0,0)),

                new VertexPositionColorTexture(new Vector3(max.x, max.y, max.z), Color.white, new Vector2b(0,0)),
                new VertexPositionColorTexture(new Vector3(min.x, max.y, max.z), Color.white, new Vector2b(0,0)),
                new VertexPositionColorTexture(new Vector3(min.x, max.y, max.z), Color.white, new Vector2b(0,0)),
                new VertexPositionColorTexture(new Vector3(min.x, min.y, max.z), Color.white, new Vector2b(0,0)),


                new VertexPositionColorTexture(new Vector3(max.x, min.y, min.z), Color.white, new Vector2b(0,0)),
                new VertexPositionColorTexture(new Vector3(max.x, min.y, max.z), Color.white, new Vector2b(0,0)),
                new VertexPositionColorTexture(new Vector3(max.x, max.y, min.z), Color.white, new Vector2b(0,0)),
                new VertexPositionColorTexture(new Vector3(max.x, max.y, max.z), Color.white, new Vector2b(0,0)),

                new VertexPositionColorTexture(new Vector3(min.x, max.y, min.z), Color.white, new Vector2b(0,0)),
                new VertexPositionColorTexture(new Vector3(min.x, max.y, max.z), Color.white, new Vector2b(0,0)),
                new VertexPositionColorTexture(new Vector3(min.x, min.y, min.z), Color.white, new Vector2b(0,0)),
                new VertexPositionColorTexture(new Vector3(min.x, min.y, max.z), Color.white, new Vector2b(0,0))
            };
        }

        public static VertexPositionColorTexture ToVptc(CTRFramework.Vertex v, Vector2b uv, float scale = 1.0f)
        {
            VertexPositionColorTexture mono_v = new VertexPositionColorTexture();
            mono_v.Position = ToVector3(v.coord, scale);
            mono_v.Color = new Color(
                v.color.X / 255f,
                v.color.Y / 255f,
                v.color.Z / 255f
                );
            mono_v.TextureCoordinate = new Vector2b((byte)(uv.X / 255.0f), (byte)(uv.Y / 255.0f));
            return mono_v;
        }

        public static Color Blend(Color c1, Color c2)
        {
            Color x = Color.white;
            x.r = (byte)((c1.r + c2.r) / 2);
            x.g = (byte)((c1.g + c2.g) / 2);
            x.b = (byte)((c1.b + c2.b) / 2);
            return x;
        }

    }
}