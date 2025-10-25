using UnityEngine;

public class SignalsInitManager : MonoBehaviour
{
    // Класс для правильного порядка инициализации компонентов в рамках головоломки с сигналами платформера
    private void Awake()
    {
        InitializeAllPuzzles();
    }

    private void InitializeAllPuzzles()
    {
        SignalsManager[] signalsManagers = FindObjectsOfType<SignalsManager>();
        foreach (var manager in signalsManagers)
        {
            manager.Init();
        }

        SignalLineWiresVisual[] wiresVisuals = FindObjectsOfType<SignalLineWiresVisual>();
        foreach (var wiresVisual in wiresVisuals)
        {
            wiresVisual.Init();
        }

        SignalLineVisual[] lineVisuals = FindObjectsOfType<SignalLineVisual>();
        foreach (var lineVisual in lineVisuals)
        {
            lineVisual.Init();
        }

        SignalLineClickHandler[] clickHandlers = FindObjectsOfType<SignalLineClickHandler>();
        foreach (var clickHandler in clickHandlers)
        {
            clickHandler.Init();
        }

        foreach (var manager in signalsManagers)
        {
            manager.RecalculateSignals();
        }
    }
}