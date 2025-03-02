using UnityEngine;

public class Tile
{
    public Vector2Int Position;
    public Tile Parent;
    public float g;
    public float h;
    public float f => g + h;

    public Tile(Vector2Int position)
    {
        Position = position;
        g = float.MaxValue;
        h = 0;
        Parent = null;
    }
}