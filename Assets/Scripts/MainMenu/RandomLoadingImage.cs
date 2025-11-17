using UnityEngine;
using UnityEngine.UI;

public class RandomLoadingImage : MonoBehaviour
{
    private GameSceneManager _gameSceneManager;
    [SerializeField] private Sprite[] _sprites;
    [SerializeField] private Image _loadingScreenImage;

    private void Start()
    {
        _gameSceneManager = GameSceneManager.Instance;

        _gameSceneManager.OnLoadingStarted += SetRandomLoadingScreen;
    }

    private void OnDestroy()
    {
        if (_gameSceneManager == null) return;
        _gameSceneManager.OnLoadingStarted -= SetRandomLoadingScreen;
    }

    private void SetRandomLoadingScreen(int _, bool someBool)
    {
        _loadingScreenImage.sprite = _sprites[Random.Range(0, _sprites.Length)];
    }
}
