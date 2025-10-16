using System;
using System.Collections;
using UnityEngine;

public class CurrentMainColorManager : MonoBehaviour
{
    // Скрипт, отвечающий за выбор текущей световой палитры - главный цвет + дополнительный
    [SerializeField] private ColorPalette[] colorPalettes;
    private int currentPaletteIndex = 0;

    public ColorPalette CurrentPalette => colorPalettes[currentPaletteIndex];

    public static CurrentMainColorManager Instance { get; private set; }
    public event Action<ColorPalette> OnColorPaletteChanged;

    private void Awake()
    {
        if(Instance == null) Instance = this;
    }

    private void Start()
    {
        if (colorPalettes.Length == 0)
        {
            Debug.LogError("No color palettes assigned in the inspector.");
            return;
        }

        StartCoroutine(InitializePalette());
    }

    private IEnumerator InitializePalette()
    {
        // Ждём один кадр, чтобы все подписчики успели подписаться на событие
        yield return null;
        AssignNewColorPalette();
    }

    private int GetRandomColorPaletteIndex()
    {
        return UnityEngine.Random.Range(0, colorPalettes.Length);
    }
    // Assign - обязательно после того как генерируем паззл
    public void AssignNewColorPalette()
    {
        int newCurrentIndex = currentPaletteIndex;
        // Для гарантирования смены палитры
        do
        {
            currentPaletteIndex = GetRandomColorPaletteIndex();
        } 
        while (newCurrentIndex == currentPaletteIndex);

        OnColorPaletteChanged?.Invoke(CurrentPalette);
    }
}

[Serializable]
public struct ColorPalette
{
    public Color mainColor;
    public Color secondColor;
    public Color thirdColor;

    public ColorPalette(Color main, Color second, Color third)
    {
        mainColor = main;
        secondColor = second;
        thirdColor = third;
    }
}
