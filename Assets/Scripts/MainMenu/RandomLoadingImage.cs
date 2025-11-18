using UnityEngine;
using UnityEngine.UI;

public class RandomLoadingImage : MonoBehaviour
{
    [SerializeField] private Sprite[] _sprites;
    [SerializeField] private Image _loadingScreenImage;

    private void Start()
    {
        GameSceneManager.Instance.OnLoadingStarted += SetRandomLoadingScreen;
    }

    private void OnDestroy()
    {
        if (GameSceneManager.Instance == null) return;
        GameSceneManager.Instance.OnLoadingStarted -= SetRandomLoadingScreen;
    }

    private void SetRandomLoadingScreen(int _, bool someBool)
    {
        _loadingScreenImage.sprite = _sprites[Random.Range(0, _sprites.Length)];
    }
}
