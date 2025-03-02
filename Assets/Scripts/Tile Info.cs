using System.Collections;
using UnityEngine;

public class TileInfo : MonoBehaviour
{
    private Vector2Int _position;
    
    [SerializeField] private Color highlightColor = Color.green;
    [SerializeField] private Color selectionColor = Color.yellow;
    [SerializeField] private float colorChangeDuration = 0.2f;

    private Renderer _tileRenderer;
    private Color _originalColor;

    private void Start()
    {
        _tileRenderer = GetComponent<Renderer>();
        if (_tileRenderer) _originalColor = _tileRenderer.material.color;
    }

    public void SetPosition(Vector2Int position) => _position = position;
    public Vector2Int GetPosition() => _position;
    
    public void HighlightTile()
    {
        StopAllCoroutines();
        StartCoroutine(ChangeTileColor(highlightColor));
    }

    public void ResetTile()
    {
        StopAllCoroutines();
        StartCoroutine(ChangeTileColor(_originalColor));
    }

    public void SelectTile()
    {
        StopAllCoroutines();
        StartCoroutine(ChangeTileColor(selectionColor));
    }

    private IEnumerator ChangeTileColor(Color targetColor)
    {
        var elapsedTime = 0f;
        var startColor = _tileRenderer.material.color;

        while (elapsedTime < colorChangeDuration)
        {
            elapsedTime += Time.deltaTime;
            _tileRenderer.material.color = Color.Lerp(startColor, targetColor, elapsedTime / colorChangeDuration);
            yield return null;
        }

        _tileRenderer.material.color = targetColor;
    }
}