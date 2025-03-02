using System.Collections.Generic;
using UnityEngine;

public class AStarPathFinder
{
    private bool[,] _grid;
    private Vector2Int _gridSize;
    private List<Tile> _openTiles = new();
    private HashSet<Tile> _closedTiles = new ();

    public AStarPathFinder(bool[,] grid)
    {
        _grid = grid;
        _gridSize = new Vector2Int(_grid.GetLength(0), _grid.GetLength(1));
    }

    public List<Vector2Int> FindPath(Vector2Int start, Vector2Int end)
    {
        var startTile = new Tile(start) { g = 0, h = GetHeuristic(start, end) };
        _openTiles.Add(startTile);

        while (_openTiles.Count > 0)
        {
            var currentTile = GetLowestFCodeTile(_openTiles);
            if (currentTile.Position == end)
            {
                _openTiles.Clear();
                _closedTiles.Clear();
                return ReconstructPath(currentTile);
            }

            _openTiles.Remove(currentTile);
            _closedTiles.Add(currentTile);

            foreach (var adjacentTilePos in GetAdjacentTilesPos(currentTile))
            {
                if (ObstaclePresentIn(adjacentTilePos) || _closedTiles.Contains(new Tile(adjacentTilePos))) continue;

                var adjacentTile = new Tile(adjacentTilePos)
                {
                    g = currentTile.g + 1,
                    h = GetHeuristic(adjacentTilePos, end),
                    Parent = currentTile
                };
                
                if (!_openTiles.Contains(adjacentTile) || adjacentTile.g < currentTile.g) _openTiles.Add(adjacentTile);
            }
        }
        return null;
    }

    private static float GetHeuristic(Vector2Int a, Vector2Int b) => Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);

    private static Tile GetLowestFCodeTile(List<Tile> tiles)
    {
        tiles.Sort((a, b) => a.f.CompareTo(b.f));
        return tiles[0];
    }

    private List<Vector2Int> GetAdjacentTilesPos(Tile baseTile)
    {
        var basePos = baseTile.Position;
        var adjacentTilesPos = new List<Vector2Int>();
        if (basePos.x + 1 < _gridSize.x) adjacentTilesPos.Add(new Vector2Int(basePos.x + 1, basePos.y));
        if (basePos.x - 1 >= 0) adjacentTilesPos.Add(new Vector2Int(basePos.x - 1, basePos.y));
        if (basePos.y + 1 < _gridSize.y) adjacentTilesPos.Add(new Vector2Int(basePos.x, basePos.y + 1));
        if (basePos.y - 1 >= 0) adjacentTilesPos.Add(new Vector2Int(basePos.x, basePos.y - 1));
        return adjacentTilesPos;
    }

    private bool ObstaclePresentIn(Vector2Int tilePos) => _grid[tilePos.x, tilePos.y];

    private static List<Vector2Int> ReconstructPath(Tile tile)
    {
        var path = new List<Vector2Int>();
        while(tile != null)
        {
            path.Add(tile.Position);
            tile = tile.Parent;
        }
        path.Reverse();
        return path;
    }
}