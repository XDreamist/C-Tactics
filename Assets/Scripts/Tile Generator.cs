using UnityEngine;

public class TileGenerator : MonoBehaviour
{
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private float tileSize = 1.0f;
    [SerializeField] private int gridWidth = 10;
    [SerializeField] private int gridHeight = 10;

    private void Start()
    {
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        for (var x = 0; x < gridWidth; x++)
        {
            for (var y = 0; y < gridHeight; y++)
            {
                var tilePosition = new Vector3(x * tileSize, 0, y * tileSize);

                var tile = Instantiate(tilePrefab, tilePosition, Quaternion.identity, transform);

                tile.name = $"Tile_{x}_{y}";

                var tileInfo = tile.GetComponent<TileInfo>();
                tileInfo.SetPosition(new Vector2Int(x, y));
            }
        }
    }
}