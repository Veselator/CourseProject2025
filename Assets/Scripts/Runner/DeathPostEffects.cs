using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DeathPostEffects : MonoBehaviour
{
    //private PlayerHealth _playerHealth;
    private Volume _volume;
    private Vignette _vignette;
    private ColorAdjustments _colorAdjustments;
    private ChromaticAberration _chromaticAbberation;
    [SerializeField] private float animationDuration = 4f;

    private float targetVignetteIntensity;
    private Vector4 targetColorFilter;
    public Action OnDeathUIAnimationEnded;
    private bool isPlayingAnimation = false;

    private void Start()
    {
        GlobalFlags.onFlagChanged += CheckGlobalFlags;

        _volume = GetComponent<Volume>();

        if (_volume.profile.TryGet<Vignette>(out _vignette))
        {
            targetVignetteIntensity = _vignette.intensity.value;
            _vignette.intensity.value = 0f; // Начинаем с нуля
        }

        if (_volume.profile.TryGet<ColorAdjustments>(out _colorAdjustments))
        {
            targetColorFilter = _colorAdjustments.colorFilter.value;
            _colorAdjustments.colorFilter.value = Color.white; // Начинаем с белого (нейтральный)
        }

        if (_volume.profile.TryGet<ChromaticAberration>(out _chromaticAbberation))
        {
            // Исправление: .intensity — это ClampedFloatParameter, нужно присваивать через .value
            _chromaticAbberation.intensity.value = 1f;
        }
    }

    private void CheckGlobalFlags(string flagName, bool flagState)
    {
        if (flagName == Flags.GameOver.ToString() && !isPlayingAnimation)
        {
            StartAnimation();
        }
    }

    private void OnDestroy()
    {
        GlobalFlags.onFlagChanged -= CheckGlobalFlags;
    }

    private void StartAnimation()
    {
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        isPlayingAnimation = true;
        float elapsedTime = 0f;

        // Стартовые значения
        float startVignetteIntensity = _vignette != null ? _vignette.intensity.value : 0f;
        Vector4 startColorFilter = _colorAdjustments != null ? _colorAdjustments.colorFilter.value : Color.white;

        if(_vignette != null)
        {
            _vignette.active = true;
            _vignette.intensity.overrideState = true;
        }

        if (_colorAdjustments != null)
        {
            _colorAdjustments.active = true;
            _colorAdjustments.colorFilter.overrideState = true;
        }

        if (_chromaticAbberation != null)
        {
            _chromaticAbberation.active = true;
        }

        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / animationDuration;

            // Плавная интерполяция
            float smoothProgress = Mathf.SmoothStep(0f, 1f, progress);

            // Анимируем Vignette Intensity
            if (_vignette != null)
            {
                _vignette.intensity.value = Mathf.Lerp(startVignetteIntensity, targetVignetteIntensity, smoothProgress);
            }

            // Анимируем Color Filter
            if (_colorAdjustments != null)
            {
                _colorAdjustments.colorFilter.value = Vector4.Lerp(startColorFilter, targetColorFilter, smoothProgress);
            }

            if (_chromaticAbberation != null)
            {
                _chromaticAbberation.intensity.value = Mathf.Lerp(0f, 1f, progress);
            }

            yield return null;
        }

        // Гарантируем точные конечные значения
        if (_vignette != null)
            _vignette.intensity.value = targetVignetteIntensity;

        if (_colorAdjustments != null)
            _colorAdjustments.colorFilter.value = targetColorFilter;
        isPlayingAnimation = false;
    }
}
