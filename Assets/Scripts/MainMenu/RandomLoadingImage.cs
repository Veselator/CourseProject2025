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
        _gameSceneManager.OnLoadingStarted -= SetRandomLoadingScreen;
    }

    private void SetRandomLoadingScreen(int _)
    {
        _loadingScreenImage.sprite = _sprites[Random.Range(0, _sprites.Length)];
    }
}
