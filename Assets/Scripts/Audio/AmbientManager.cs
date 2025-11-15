using System;
using UnityEngine;

public class AmbientManager : MonoBehaviour
{
    // Меняет эмбиент при смене сцены

    private GameAudioManager _audioManager;
    private GameSceneManager _sceneManager;
    [SerializeField] private SceneToAmbient[] _ambient;

    private void Start()
    {
        _sceneManager = GameSceneManager.Instance;
        _audioManager = GameAudioManager.Instance;

        _sceneManager.OnLoadingStarted += HandleAmbientChange;

        HandleAmbientChange(-1); // меню
    }

    private void OnDestroy()
    {
        if (_sceneManager == null) return;
        _sceneManager.OnLoadingStarted -= HandleAmbientChange;
    }

    private string GetAmbientNameBySceneID(int id)
    {
        foreach (var sceneToAmbient in _ambient)
        {
            if (sceneToAmbient.SceneID == id)
            {
                return sceneToAmbient.AmbientName;
            }
        }
        return null;
    }

    private void HandleAmbientChange(int id)
    {
        // -1 - меню
        if (id < -1) return;
        string ambientName = GetAmbientNameBySceneID(id);
        if(ambientName != null) _audioManager.PlayAmbient(ambientName);
        else _audioManager.StopAmbient();
    }
}

[Serializable]
public struct SceneToAmbient
{
    public int SceneID;
    public string AmbientName;
}