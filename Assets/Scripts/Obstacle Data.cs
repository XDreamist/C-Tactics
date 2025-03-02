using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ObstacleData", menuName = "Object Data/Obstacle Data", order = 4)]
public class ObstacleData : ScriptableObject
{
    public List<bool> obstacleGrid = new(new bool[100]);

    public List<bool> GetObstacleGrid() => obstacleGrid;
        
    public bool GetTile(int x, int y) => obstacleGrid[x * 10 + y];
    
    public void SetTile(int x, int y, bool value) => obstacleGrid[x * 10 + y] = value;
}