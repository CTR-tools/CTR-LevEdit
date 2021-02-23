using System.Collections;
using System.Collections.Generic;
using CTRFramework.Shared;
using UnityEngine;

public class VertexPositionColorTexture
{
    public Vector3 Position;
    public Vector2b TextureCoordinate;
    public Color Color;

    public VertexPositionColorTexture(Vector3 position, Color color, Vector2b textureCoordinate)
    {
        Position = position;
        TextureCoordinate = textureCoordinate;
        Color = color;
    }
    public VertexPositionColorTexture()
    {
        Position = Vector3.zero;
        TextureCoordinate = new Vector2b(0,0);
        Color = Color.white;
    }
}
