using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitButtonLinker : MonoBehaviour
{
    [SerializeField] private Button _linkedButton;
    private GameSceneManager _gameSceneManager;
    private void Start()
    {
        _gameSceneManager = GameSceneManager.Instance;
        _linkedButton.onClick.AddListener(_gameSceneManager.QuitGame);
    }

    private void OnDestroy()
    {
        // Отписываемся именно от того, на кого подписались ранее
        _linkedButton.onClick.RemoveListener(_gameSceneManager.QuitGame);
    }
}
