using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelButtonHandler : MonoBehaviour
{
    [SerializeField] private LevelButtonInfo _linkedInfo;

    [SerializeField] private Image _image;
    [SerializeField] private TMP_Text _title;
    [SerializeField] private TMP_Text _subtitle;
    [SerializeField] private Button _button;
    [SerializeField] private GameSceneManager _gameSceneManager;
    [SerializeField] private Image _backgroundImage;

    [SerializeField] private Color _colorIfComplete = Color.green;
    [SerializeField] private Color _colorIfNotCompleted = Color.red;

    private void Start()
    {
        if (_linkedInfo == null) return;

        _image.sprite = _linkedInfo.image;
        _title.text = _linkedInfo.title;
        _subtitle.text = _linkedInfo.subtitle;
        _backgroundImage.color = GameSaveManager.Instance.IsLevelCompleted(_linkedInfo.levelId) ? _colorIfComplete : _colorIfNotCompleted;

        _button.onClick.AddListener(LoadLevelHandler);
    }

    private void LoadLevelHandler()
    {
        _gameSceneManager.LoadLevel(_linkedInfo.sceneId);
    }
}
