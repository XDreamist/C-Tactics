using UnityEngine;
using UnityEngine.UI;

public class TileSelector : MonoBehaviour
{
    [SerializeField] private Text tilePositionText;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private CharNavigation playerNavigation;

    private bool _isLeftMouseDown;
    
    private Camera _mainCamera;
    private TileInfo _selectedTile;
    private TileInfo _currentHoverTile;
    private TileInfo _lastHoveredTile;

    private void Start()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        SetupInput();
        if (!playerNavigation.GetIsMoving()) DetectHover();
    }

    private void SetupInput()
    {
        _isLeftMouseDown = Input.GetMouseButtonDown(0);
    }

    private void DetectHover()
    {
        if (!_mainCamera) return;

        var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out var hit, 100f, layerMask))
        {
            ClearHoveredTile();
            return;
        }

        if (!hit.collider.TryGetComponent(out _currentHoverTile))
        {
            ClearHoveredTile();
            return;
        }

        if (_lastHoveredTile != _currentHoverTile)
        {
            if (_lastHoveredTile != _selectedTile) _lastHoveredTile?.ResetTile();
            if (_currentHoverTile != _selectedTile) _currentHoverTile.HighlightTile();
        }

        DetectSelection();

        tilePositionText.text = $"Tile Position: ({_currentHoverTile.GetPosition().x}, {_currentHoverTile.GetPosition().y})";
        _lastHoveredTile = _currentHoverTile;
    }

    private void ClearHoveredTile()
    {
        if (_lastHoveredTile && _lastHoveredTile != _selectedTile)
        {
            _lastHoveredTile.ResetTile();
            _lastHoveredTile = null;
        }

        tilePositionText.text = "Tile Position: None";
    }

    private void DetectSelection()
    {
        if (!_isLeftMouseDown) return;
        if (_currentHoverTile == _selectedTile) return;

        var tilePos = _currentHoverTile.GetPosition();
        if (playerNavigation.ObstacleData[tilePos.x, tilePos.y])
        {
            print("Obstacle is present there!");
            return;
        }
        
        _selectedTile?.ResetTile();
        _currentHoverTile.SelectTile();

        _selectedTile = _currentHoverTile;

        playerNavigation.MoveTo(_selectedTile.GetPosition());
    }
}