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
    private Vignette vignette;
    private ColorAdjustments colorAdjustments;
    private ChromaticAberration chromaticAbberation;
    [SerializeField] private float animationDuration = 4f;

    private float targetVignetteIntensity;
    private Vector4 targetColorFilter;
    public Action OnDeathUIAnimationEnded;
    private bool isPlayingAnimation = false;

    private void Start()
    {
        GlobalFlags.onFlagChanged += CheckGlobalFlags;

        _volume = GetComponent<Volume>();

        if (_volume.profile.TryGet<Vignette>(out vignette))
        {
            targetVignetteIntensity = vignette.intensity.value;
            vignette.intensity.value = 0f; // �������� � ����
        }

        if (_volume.profile.TryGet<ColorAdjustments>(out colorAdjustments))
        {
            targetColorFilter = colorAdjustments.colorFilter.value;
            colorAdjustments.colorFilter.value = Color.white; // �������� � ������ (�����������)
        }

        if (_volume.profile.TryGet<ChromaticAberration>(out chromaticAbberation))
        {
            // �����������: .intensity � ��� ClampedFloatParameter, ����� ����������� ����� .value
            chromaticAbberation.intensity.value = 1f;
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

        // ��������� ��������
        float startVignetteIntensity = vignette != null ? vignette.intensity.value : 0f;
        Vector4 startColorFilter = colorAdjustments != null ? colorAdjustments.colorFilter.value : Color.white;

        vignette.active = true;
        vignette.intensity.overrideState = true;
        colorAdjustments.active = true;
        colorAdjustments.colorFilter.overrideState = true;
        chromaticAbberation.active = true;

        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / animationDuration;

            // ������� ������������
            float smoothProgress = Mathf.SmoothStep(0f, 1f, progress);

            // ��������� Vignette Intensity
            if (vignette != null)
            {
                vignette.intensity.value = Mathf.Lerp(startVignetteIntensity, targetVignetteIntensity, smoothProgress);
            }

            // ��������� Color Filter
            if (colorAdjustments != null)
            {
                colorAdjustments.colorFilter.value = Vector4.Lerp(startColorFilter, targetColorFilter, smoothProgress);
            }

            if (chromaticAbberation != null)
            {
                chromaticAbberation.intensity.value = Mathf.Lerp(0f, 1f, progress);
            }

            yield return null;
        }

        // ����������� ������ �������� ��������
        if (vignette != null)
            vignette.intensity.value = targetVignetteIntensity;

        if (colorAdjustments != null)
            colorAdjustments.colorFilter.value = targetColorFilter;
        isPlayingAnimation = false;
    }
}
