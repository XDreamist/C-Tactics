using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    [SerializeField] private ObstacleData obstacleData;
    [SerializeField] private GameObject obstaclePrefab;
    [SerializeField] private CharNavigation playerNavigation;

    private bool[,] _obstacleData = new bool[10, 10];
    
    private void Start()
    {
        if (obstacleData == null || obstaclePrefab == null || playerNavigation == null)
        {
            Debug.LogError("ObstacleData or ObstaclePrefab or Player is not assigned!");
            return;
        }

        for (var x = 0; x < 10; x++)
        {
            for (var y = 0; y < 10; y++)
            {
                if (!obstacleData.GetTile(x, y)) continue;

                _obstacleData[x, y] = true;
                var position = new Vector3(x, 0.8f, y);
                Instantiate(obstaclePrefab, position, Quaternion.identity, transform);
            }
        }
        
        playerNavigation.SetObstaclesData(_obstacleData);
    }
}