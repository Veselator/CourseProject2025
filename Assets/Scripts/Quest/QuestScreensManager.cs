using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestScreensManager : MonoBehaviour
{
    // Отвечает за контроль перемещения между экранами
    private QuestCameraManager _cameraManager;

    public int CurrentScreen {  get; private set; }
    public static QuestScreensManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        _cameraManager = GetComponent<QuestCameraManager>();
    }

    public void ChangeScreen(int newScreen)
    {
        if (CurrentScreen == newScreen) return;
        CurrentScreen = newScreen;
        _cameraManager.MoveCamera2Point(newScreen);
    }
}
