using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackToMainmenuPeremogaButton : MonoBehaviour
{
    [SerializeField] private Button _linkedButton;

    private void Start()
    {
        _linkedButton.onClick.AddListener(GameSceneManager.ExitToMenu);
    }

    private void OnDestroy()
    {
        _linkedButton.onClick.RemoveListener(GameSceneManager.ExitToMenu);
    }
}
