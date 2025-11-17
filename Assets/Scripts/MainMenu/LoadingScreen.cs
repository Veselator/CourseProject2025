using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadingScreen : MonoBehaviour
{
    // Визуализация загрузки

    [Header("UI элементы")]
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Slider _progressBar;

    [Header("Настройки анимации")]
    [SerializeField] private float _fadeInDuration = 0.3f;
    [SerializeField] private float _fadeOutDuration = 0.3f;

    private float _loadingTime;

    private GameSceneManager _sceneManager;
    private Coroutine _fadeCoroutine;
    private Coroutine _progressCoroutine;

    private void Awake()
    {
        if (_canvasGroup == null)
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        _canvasGroup.alpha = 0f;
        _canvasGroup.blocksRaycasts = false;

        _canvasGroup.gameObject.SetActive(false);
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        _sceneManager = GameSceneManager.Instance;
        _loadingTime = _sceneManager.LoadingTime;

        if (_sceneManager != null)
        {
            _sceneManager.OnLoadingStarted += ShowLoadingScreen;
            _sceneManager.OnLoadingCompleted += HideLoadingScreen;
        }
        else
        {
            Debug.LogError("GameSceneManager не найден!");
        }
    }

    private void OnDestroy()
    {
        if (_sceneManager != null)
        {
            _sceneManager.OnLoadingStarted -= ShowLoadingScreen;
            _sceneManager.OnLoadingCompleted -= HideLoadingScreen;
        }
    }

    private void ShowLoadingScreen(int _, bool IsNeedToShow)
    {
        if (!IsNeedToShow) return;
        Debug.Log("Started loading animation");
        _canvasGroup.gameObject.SetActive(true);

        if (_progressBar != null)
        {
            _progressBar.value = 0f;
        }

        if (_fadeCoroutine != null)
        {
            StopCoroutine(_fadeCoroutine);
        }

        if (_progressCoroutine != null)
        {
            StopCoroutine(_progressCoroutine);
        }

        _fadeCoroutine = StartCoroutine(FadeCanvasGroup(1f, _fadeInDuration, true));
        _progressCoroutine = StartCoroutine(AnimateProgressBar());
    }

    private void HideLoadingScreen(int _)
    {
        if (_fadeCoroutine != null)
        {
            StopCoroutine(_fadeCoroutine);
        }

        _fadeCoroutine = StartCoroutine(FadeCanvasGroup(0f, _fadeOutDuration, false));
    }

    private IEnumerator FadeCanvasGroup(float targetAlpha, float duration, bool blockRaycasts)
    {
        float startAlpha = _canvasGroup.alpha;
        float elapsedTime = 0f;

        _canvasGroup.blocksRaycasts = blockRaycasts;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            _canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, t);
            yield return null;
        }

        _canvasGroup.alpha = targetAlpha;

        if (targetAlpha == 0f)
        {
            _canvasGroup.gameObject.SetActive(false);
        }
    }

    private IEnumerator AnimateProgressBar()
    {
        float elapsedTime = 0f;

        while (elapsedTime < _loadingTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / _loadingTime;

            if (_progressBar != null)
            {
                _progressBar.value = Mathf.Lerp(0f, 1f, t);
            }

            yield return null;
        }

        if (_progressBar != null)
        {
            _progressBar.value = 1f;
        }
    }
}