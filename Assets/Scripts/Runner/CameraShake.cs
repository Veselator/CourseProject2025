using System;
using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [Header("Shake Settings")]
    [SerializeField] private float shakeIntensity = 0.42f;
    [SerializeField] private float shakeHitIntensity = 0.3f;
    [SerializeField] private float shakeLightHitIntensity = 0.14f;
    [SerializeField] private float ShakeDuration = 0.8f;
    [SerializeField] private float ShakeHitDuration = 0.4f;
    public float ShakeHitTime => ShakeHitDuration;
    [SerializeField] private AnimationCurve shakeCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);

    protected Vector3 originalLocalPosition;
    protected Quaternion originalLocalRotation;
    protected bool isShaking = false;

    // Offset для тряски
    protected Vector3 currentShakeOffset = Vector3.zero;
    protected Quaternion currentShakeRotation = Quaternion.identity;

    public bool IsAbleToShake = true;

    public static Action ShakeCamera;

    // Публичные свойства для получения offset
    public Vector3 ShakeOffset => currentShakeOffset;
    public Quaternion ShakeRotation => currentShakeRotation;

    private void Awake()
    {
        OnAwakeInitialize();
    }

    protected virtual void OnAwakeInitialize() { }

    void Start()
    {
        originalLocalPosition = transform.localPosition;
        originalLocalRotation = transform.localRotation;
        ShakeCamera += StartHitShake;
        OnStartInitialize();
    }

    protected virtual void OnStartInitialize() { }

    private void OnDestroy()
    {
        ShakeCamera -= StartHitShake;
    }

    public void HandleShake()
    {
        if (!isShaking && IsAbleToShake)
        {
            StartCoroutine(RandomDragCamera(ShakeDuration, shakeIntensity));
        }
    }

    public void StartHitShake()
    {
        Debug.Log("Starting shaking");
        StartCoroutine(HitShake(ShakeHitDuration, shakeHitIntensity));
    }

    public void StartLightHitShake()
    {
        Debug.Log("Starting shaking");
        StartCoroutine(HitShake(ShakeHitDuration, shakeLightHitIntensity));
    }

    public void StartHitShake(float shakeHitIntensityFactor)
    {
        Debug.Log("Starting shaking");
        StartCoroutine(HitShake(ShakeHitDuration, shakeHitIntensity * shakeHitIntensityFactor));
    }

    protected virtual IEnumerator RandomDragCamera(float duration, float intensity)
    {
        originalLocalPosition = transform.localPosition;
        originalLocalRotation = transform.localRotation;
        isShaking = true;
        float elapsedTime = 0f;

        float seedX = UnityEngine.Random.Range(0f, 100f);
        float seedY = UnityEngine.Random.Range(100f, 200f);
        float seedRot = UnityEngine.Random.Range(200f, 300f);

        float baseFreq = UnityEngine.Random.Range(0.9f, 1.6f);
        float rotFreq = baseFreq * UnityEngine.Random.Range(0.6f, 1.2f);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float normalizedTime = Mathf.Clamp01(elapsedTime / duration);
            float curveValue = shakeCurve.Evaluate(normalizedTime);

            float t = Time.time * baseFreq;

            float noiseX = (Mathf.PerlinNoise(seedX, t) - 0.5f) * 2f;
            float noiseY = (Mathf.PerlinNoise(seedY, t * 1.1f) - 0.5f) * 2f;

            float driftX = (Mathf.PerlinNoise(seedX + 50f, Time.time * 0.18f) - 0.5f) * 2f;
            float driftY = (Mathf.PerlinNoise(seedY + 50f, Time.time * 0.18f) - 0.5f) * 2f;

            Vector3 rawOffset = new Vector3(noiseX * 0.7f + driftX * 0.3f, noiseY * 0.7f + driftY * 0.3f, 0f);
            Vector3 targetOffset = rawOffset * intensity * curveValue;

            currentShakeOffset = Vector3.Lerp(currentShakeOffset, targetOffset, Time.deltaTime * 8f);

            float rotNoise = (Mathf.PerlinNoise(seedRot, Time.time * rotFreq) - 0.5f) * 2f;
            float maxRotationDeg = Mathf.Clamp(intensity * 3f, 0.5f, 6f);
            float targetZ = rotNoise * maxRotationDeg * curveValue;
            Quaternion targetRot = Quaternion.Euler(0f, 0f, targetZ);

            currentShakeRotation = Quaternion.Slerp(currentShakeRotation, targetRot, Time.deltaTime * 6f);

            ApplyShake();
            yield return null;
        }

        currentShakeOffset = Vector3.zero;
        currentShakeRotation = Quaternion.identity;
        ApplyShake();
        isShaking = false;
    }

    protected virtual IEnumerator HitShake(float duration, float intensity)
    {
        //originalLocalPosition = transform.localPosition;
        //originalLocalRotation = transform.localRotation;
        Debug.Log("Started shaking");
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float normalizedTime = Mathf.Clamp01(elapsedTime / duration);
            float curveValue = shakeCurve.Evaluate(normalizedTime);

            currentShakeOffset = new Vector3(
                UnityEngine.Random.Range(-1f, 1f) * intensity * curveValue,
                UnityEngine.Random.Range(-1f, 1f) * intensity * curveValue,
                0f
            );

            float rot = UnityEngine.Random.Range(-1f, 1f) * intensity * 4f * curveValue;
            currentShakeRotation = Quaternion.Euler(0f, 0f, rot);

            //ApplyShake();
            yield return null;
        }

        currentShakeOffset = Vector3.zero;
        currentShakeRotation = Quaternion.identity;
        //ApplyShake();
    }

    protected virtual void ApplyShake()
    {
        transform.localPosition = originalLocalPosition + currentShakeOffset;
        transform.localRotation = currentShakeRotation * originalLocalRotation;
    }

    public virtual void StopShake()
    {
        if (isShaking)
        {
            StopAllCoroutines();
            currentShakeOffset = Vector3.zero;
            currentShakeRotation = Quaternion.identity;
            transform.localPosition = originalLocalPosition;
            transform.localRotation = originalLocalRotation;
            isShaking = false;
        }
    }

    public void ResetOriginalPosition()
    {
        if (!isShaking)
        {
            originalLocalPosition = transform.localPosition;
            originalLocalRotation = transform.localRotation;
        }
    }

    void Update()
    {
        HandleShake();
    }
}