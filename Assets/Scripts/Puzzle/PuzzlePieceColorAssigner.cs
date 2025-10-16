using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzlePieceColorAssigner : MonoBehaviour
{
    private CurrentMainColorManager _colorManager;

    private void Start()
    {
        _colorManager = CurrentMainColorManager.Instance;
        if (_colorManager != null) _colorManager.OnColorPaletteChanged += UpdatePuzzlePieceColor;
    }

    private void OnDestroy()
    {
        if(_colorManager != null) _colorManager.OnColorPaletteChanged -= UpdatePuzzlePieceColor;
    }

    // Обновляем цвет trailRenderer
    private void UpdatePuzzlePieceColor(ColorPalette newPalette)
    {
        var trailRenderer = GetComponent<TrailRenderer>();
        if (trailRenderer != null)
        {
            trailRenderer.startColor = newPalette.mainColor;
        }
    }
}
