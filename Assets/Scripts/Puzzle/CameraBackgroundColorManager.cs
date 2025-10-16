using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBackgroundColorManager : MonoBehaviour
{
    private Camera mainCamera;
    private CurrentMainColorManager _colorManager;

    private void Start()
    {
        _colorManager = CurrentMainColorManager.Instance;
        mainCamera = Camera.main;
        _colorManager.OnColorPaletteChanged += UpdateCameraBackgroundColor;
    }

    private void OnDestroy()
    {
        _colorManager.OnColorPaletteChanged -= UpdateCameraBackgroundColor;
    }

    private void UpdateCameraBackgroundColor(ColorPalette newPalette)
    {
        mainCamera.backgroundColor = newPalette.thirdColor;
    }
}
