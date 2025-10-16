using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectColorAssigner : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private CurrentMainColorManager _colorManager;
    [SerializeField] private ColorType _currentColorType;

    private void Start()
    {
        _colorManager = CurrentMainColorManager.Instance;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _colorManager.OnColorPaletteChanged += UpdateColor;
    }

    private void OnDestroy()
    {
        if (_colorManager != null)
        {
            _colorManager.OnColorPaletteChanged -= UpdateColor;
        }
    }

    private Color GetColor(ColorPalette palette)
    {
        Color tempColor = Color.black;

        switch (_currentColorType)
        {
            case ColorType.First:
                tempColor = palette.mainColor;
                break;
            case ColorType.Second:
                tempColor = palette.secondColor;
                break;
            case ColorType.Third:
                tempColor = palette.thirdColor;
                break;
        }

        return tempColor;
    }

    private void UpdateColor(ColorPalette newPalette)
    {
        if (_spriteRenderer == null)
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            if (_spriteRenderer == null)
            {
                Debug.LogError("SpriteRenderer component not found on the GameObject.");
                return;
            }
        }

        _spriteRenderer.color = GetColor(newPalette);
    }
}

public enum ColorType
{
    First,
    Second,
    Third
}
