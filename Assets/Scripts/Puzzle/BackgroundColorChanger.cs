using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundColorChanger : MonoBehaviour
{
    [SerializeField] private float _animationDuration = 0.2f;
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
        StartCoroutine(ChangeColorCoroutine(newPalette));
        //_currentMaterial.SetColor("_BackgroundColor", newPalette.thirdColor);
        //_currentMaterial.SetColor("_LineColor", newPalette.secondColor);
        //mainCamera.backgroundColor = newPalette.thirdColor;
    }

    private IEnumerator ChangeColorCoroutine(ColorPalette newPalette)
    {
        Color initialBackgroundColor = _currentMaterial.GetColor("_BackgroundColor");
        Color initialLineColor = _currentMaterial.GetColor("_LineColor");
        float elapsed = 0f;

        while (elapsed < _animationDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / _animationDuration);
            Color blendedBackgroundColor = Color.Lerp(initialBackgroundColor, newPalette.thirdColor, t);
            Color blendedLineColor = Color.Lerp(initialLineColor, newPalette.secondColor, t);

            _currentMaterial.SetColor("_BackgroundColor", blendedBackgroundColor);
            _currentMaterial.SetColor("_LineColor", blendedLineColor);
            yield return null;
        }

        // Ensure final colors are set
        _currentMaterial.SetColor("_BackgroundColor", newPalette.thirdColor);
        _currentMaterial.SetColor("_LineColor", newPalette.secondColor);
    }
}
