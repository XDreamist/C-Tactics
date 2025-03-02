using System.Collections.Generic;
using UnityEngine;

public class CharNavigation : MonoBehaviour
{
    public Vector2Int initialPosition;
    
    private Transform _charTransform;
    
    private Vector3 _charPosition;
    private Vector3 _targetPosition;
    private bool _isMoving;
    [SerializeField] private float moveSpeed = 2.5f;

    private Quaternion _charRotation;
    [SerializeField] private Quaternion targetRotation;
    [SerializeField] private float rotationRate = 4.0f;

    private AStarPathFinder _pathFinder;
    private List<Vector2Int> _path;
    private int _pathCounter;
    public bool[,] ObstacleData;
    
    private void Start()
    {
        _charTransform = transform;
        _charPosition = _charTransform.position = new Vector3(initialPosition.x, _charTransform.position.y, initialPosition.y);
        _charRotation = _charTransform.rotation = targetRotation;
    }

    private void Update()
    {
        UpdateMovement();
        UpdateRotation();
    }

    private void UpdateMovement()
    {
        if (!_isMoving) return;

        var nextPosition = CalculateNextPosition();
        _charPosition = _charTransform.position = Vector3.MoveTowards(_charPosition, nextPosition, moveSpeed * Time.deltaTime);

        if (!(Vector3.Distance(_charTransform.position, _targetPosition) < 0.01f)) return;
        
        _charTransform.position = _targetPosition;
        initialPosition = new Vector2Int((int) _targetPosition.x,  (int) _targetPosition.z);
        _isMoving = false;
        
        _path = null;
        _pathCounter = 0;
    }

    private void UpdateRotation()
    {
        _charRotation = _charTransform.rotation;

        Quaternion.RotateTowards(_charRotation, targetRotation, rotationRate * Time.deltaTime);
    }

    private Vector3 CalculateNextPosition()
    {
        var nextPos = _charPosition;
        if (_path == null)
        {
            print("No path found!");
            return nextPos;
        }

        nextPos.x = _path[_pathCounter].x;
        nextPos.z = _path[_pathCounter].y;

        if (Vector3.Distance(_charPosition, nextPos) < 0.01f) _pathCounter++;
        
        return nextPos;
    }

    public void SetObstaclesData(bool[,] obstacleData)
    {
        ObstacleData = obstacleData;
        _pathFinder = new AStarPathFinder(obstacleData);
    }

    public bool MoveTo(Vector2Int position)
    {
        if (_isMoving) return false;

        _pathCounter = 0;
        _path = _pathFinder.FindPath(initialPosition, position);
        
        _targetPosition = new Vector3(position.x, _charPosition.y, position.y);

        var direction = (_targetPosition - _charPosition).normalized;
        if (direction != Vector3.zero) targetRotation = Quaternion.LookRotation(direction);
        
        _isMoving = true;
        return true;
    }

    public bool GetIsMoving() => _isMoving;
}