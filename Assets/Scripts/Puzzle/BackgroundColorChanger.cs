using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundColorChanger : MonoBehaviour
{
    private Material _currentMaterial;
    private CurrentMainColorManager _colorManager;

    private void Start()
    {
        _colorManager = CurrentMainColorManager.Instance;
        _currentMaterial = GetComponent<SpriteRenderer>().material;
        _colorManager.OnColorPaletteChanged += UpdateColor;
    }

    private void OnDestroy()
    {
        _colorManager.OnColorPaletteChanged -= UpdateColor;
    }

    private void UpdateColor(ColorPalette newPalette)
    {
        _currentMaterial.SetColor("_BackgroundColor", newPalette.thirdColor);
        _currentMaterial.SetColor("_LineColor", newPalette.secondColor);
        //mainCamera.backgroundColor = newPalette.thirdColor;
    }
}
