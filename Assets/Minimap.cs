using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Component for displaying objects. Changes display area scale depending on early displayed objects
/// </summary>
public class Minimap : MonoBehaviour
{
    [SerializeField] private Vector2 leftBottomPosition = new Vector2(-10000, -10000);
    [SerializeField] private Vector2 rightUpPosition = new Vector2(10000, 10000);
    [SerializeField] private float physicalMaxSize = 200;
    [SerializeField] private Text _maxXText = null, _minXText = null;
    [SerializeField] private Text _maxYText = null, _minYText = null;

    private int _maxSize;
    private float _maxAbsX, _maxAbsY;

    private void Start()
    {
        var width = rightUpPosition.x - leftBottomPosition.x;
        var height = rightUpPosition.y - leftBottomPosition.y;
        _maxSize = (int) (width > height ? width : height);
        UpdateCoordinateLabels();
    }

    /// <summary>
    /// Draws object with required position on minimap
    /// </summary>
    /// <param name="objToDraw">Game object to draw</param>
    /// <param name="worldPosition">World position</param>
    public void Draw(GameObject objToDraw, Vector2 worldPosition)
    {
        UpdateMaxValue(ref _maxAbsX, worldPosition.x);
        UpdateMaxValue(ref _maxAbsY, worldPosition.y);
        objToDraw.transform.position = ConvertToMiniMapCoordinates(worldPosition);
    }

    private Vector2 ConvertToMiniMapCoordinates(Vector2 position)
    {
        return (physicalMaxSize / _maxSize) * position;
    }

    private void UpdateMaxValue(ref float maxValue, float newValue)
    {
        var v = newValue > 0 ? newValue : -newValue;
        if (v > maxValue)
            maxValue = v;
    }

    /// <summary>
    /// Clears and rescales minimap
    /// </summary>
    public void Clear()
    {
        RecalculateArea();
    }

    private void RecalculateArea()
    {
        var width = 2 * _maxAbsX;
        var height = 2 * _maxAbsY;
        var max = width > height ? width : height;
        if (max * 1.1 > _maxSize)
        {
            _maxSize = (((int) (1.2 * _maxSize) / 1000) + 1) * 1000;
            UpdateCoordinateLabels();
        }

        _maxAbsX = 0;
        _maxAbsY = 0;
    }

    private void UpdateCoordinateLabels()
    {
        var maxValue = _maxSize / 2;
        _maxXText.text = $"{maxValue}";
        _maxYText.text = _maxXText.text;
        _minXText.text = $"{-maxValue}";
        _minYText.text = _minXText.text;
    }
}