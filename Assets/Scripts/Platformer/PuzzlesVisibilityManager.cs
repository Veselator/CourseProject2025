using UnityEngine;
using System.Collections;

public class PuzzlesVisibilityManager : MonoBehaviour
{
    [SerializeField] private GameObject _puzzlesParentObject;

    [Header("Animation Settings")]
    [SerializeField] private float revealDuration = 1.2f;
    [SerializeField] private AnimationCurve revealCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [SerializeField] private RevealEffectType effectType = RevealEffectType.RadialWave;

    [Header("Effect Parameters")]
    [SerializeField] private float waveSpeed = 3f;
    [SerializeField] private float dissolveNoiseScale = 10f;

    private bool _currentVisibility = false;
    private Coroutine _currentAnimation;

    // Shader property IDs
    private static readonly int RevealProgressID = Shader.PropertyToID("_PuzzleRevealProgress");
    private static readonly int RevealCenterID = Shader.PropertyToID("_PuzzleRevealCenter");
    private static readonly int WaveSpeedID = Shader.PropertyToID("_PuzzleWaveSpeed");
    private static readonly int NoiseScaleID = Shader.PropertyToID("_PuzzleNoiseScale");
    private static readonly int EffectTypeID = Shader.PropertyToID("_PuzzleEffectType");

    public enum RevealEffectType
    {
        Fade = 0,
        RadialWave = 1,
        PixelDissolve = 2,
        FromBottom = 3
    }

    private void Start()
    {
        Shader.SetGlobalFloat(WaveSpeedID, waveSpeed);
        Shader.SetGlobalFloat(NoiseScaleID, dissolveNoiseScale);
        Shader.SetGlobalInt(EffectTypeID, (int)effectType);

        Shader.SetGlobalFloat(RevealProgressID, _currentVisibility ? 1f : 0f);
        _puzzlesParentObject.SetActive(_currentVisibility);
    }

    public void SetVisibility(bool newState)
    {
        if (_currentVisibility == newState) return;

        _currentVisibility = newState;

        if (_currentAnimation != null)
            StopCoroutine(_currentAnimation);

        _currentAnimation = StartCoroutine(AnimateVisibility(newState));
    }

    public void ToggleVisibility()
    {
        SetVisibility(!_currentVisibility);
    }

    private IEnumerator AnimateVisibility(bool show)
    {
        if (show)
            _puzzlesParentObject.SetActive(true);

        Vector3 revealCenter = Camera.main != null ?
            Camera.main.transform.position :
            _puzzlesParentObject.transform.position;

        Shader.SetGlobalVector(RevealCenterID, new Vector4(revealCenter.x, revealCenter.y, revealCenter.z, 0));

        float startProgress = show ? 0f : 1f;
        float endProgress = show ? 1f : 0f;
        float elapsed = 0f;

        while (elapsed < revealDuration)
        {
            elapsed += Time.deltaTime;
            float t = revealCurve.Evaluate(elapsed / revealDuration);
            float progress = Mathf.Lerp(startProgress, endProgress, t);

            Shader.SetGlobalFloat(RevealProgressID, progress);

            yield return null;
        }

        Shader.SetGlobalFloat(RevealProgressID, endProgress);

        if (!show)
            _puzzlesParentObject.SetActive(false);

        _currentAnimation = null;
    }

    public void SetRevealCenter(Vector3 position)
    {
        Shader.SetGlobalVector(RevealCenterID, new Vector4(position.x, position.y, position.z, 0));
    }

    public void SetEffectType(RevealEffectType type)
    {
        effectType = type;
        Shader.SetGlobalInt(EffectTypeID, (int)type);
    }
}