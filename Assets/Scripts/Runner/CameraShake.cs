using System;
using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [Header("Shake Settings")]
    [SerializeField] private float shakeIntensity = 0.42f;
    [SerializeField] private float shakeHitIntensity = 0.3f;
    [SerializeField] private float ShakeDuration = 0.8f;
    [SerializeField] private float ShakeHitDuration = 0.4f;
    public float ShakeHitTime => ShakeHitDuration;
    [SerializeField] private AnimationCurve shakeCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);

    private Vector3 originalLocalPosition;
    private Quaternion originalLocalRotation;
    // Flag to prevent overlapping shakes
    private bool isShaking = false;
    // Property to enable/disable shaking
    public bool IsAbleToShake = true;

    public static CameraShake Instace;

    public static Action ShakeCamera;

    private void Awake()
    {
        if (Instace == null) Instace = this;
    }

    void Start()
    {
        // Сохраняем изначальную локальную позицию камеры
        originalLocalPosition = transform.localPosition;
        originalLocalRotation = transform.localRotation;
        ShakeCamera += StartHitShake;
    }

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

    private IEnumerator RandomDragCamera(float duration, float intensity)
    {
        isShaking = true;
        float elapsedTime = 0f;

        // Сохраняем текущую локальную позицию и ротацию как стартовые
        Vector3 startPosition = transform.localPosition;
        Quaternion startRotation = transform.localRotation;

        // Случайные семена для Perlin noise, чтобы каждый вызов отличался
        float seedX = UnityEngine.Random.Range(0f, 100f);
        float seedY = UnityEngine.Random.Range(100f, 200f);
        float seedRot = UnityEngine.Random.Range(200f, 300f);

        // Базовые частоты — немного варьируем, чтобы имитировать человеческую руку
        float baseFreq = UnityEngine.Random.Range(0.9f, 1.6f);
        float rotFreq = baseFreq * UnityEngine.Random.Range(0.6f, 1.2f);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float normalizedTime = Mathf.Clamp01(elapsedTime / duration);

            // Используем кривую для плавного затухания дрожания
            float curveValue = shakeCurve.Evaluate(normalizedTime);

            // Временной фактор для Perlin noise
            float t = Time.time * baseFreq;

            // Perlin noise в диапазоне [-1,1]
            float noiseX = (Mathf.PerlinNoise(seedX, t) - 0.5f) * 2f;
            float noiseY = (Mathf.PerlinNoise(seedY, t * 1.1f) - 0.5f) * 2f;

            // Низкочастотный дрейф (медленное перемещение камеры)
            float driftX = (Mathf.PerlinNoise(seedX + 50f, Time.time * 0.18f) - 0.5f) * 2f;
            float driftY = (Mathf.PerlinNoise(seedY + 50f, Time.time * 0.18f) - 0.5f) * 2f;

            // Комбинируем быстрый шум и медленный дрейф
            Vector3 rawOffset = new Vector3(noiseX * 0.7f + driftX * 0.3f, noiseY * 0.7f + driftY * 0.3f, 0f);

            // Масштабируем по интенсивности и кривой
            Vector3 offset = rawOffset * intensity * curveValue;

            // Плавное подмешивание для ощущения инерции (handheld)
            Vector3 targetPos = startPosition + offset;
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, Time.deltaTime * 8f);

            // Небольшая ротация по Z для имитации лёгкого наклона камеры в руке
            float rotNoise = (Mathf.PerlinNoise(seedRot, Time.time * rotFreq) - 0.5f) * 2f;
            float maxRotationDeg = Mathf.Clamp(intensity * 3f, 0.5f, 6f); // градусы
            float targetZ = rotNoise * maxRotationDeg * curveValue;
            Quaternion targetRot = Quaternion.Euler(0f, 0f, targetZ) * startRotation;
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRot, Time.deltaTime * 6f);

            yield return null;
        }

        // Возвращаем камеру в стартовую позицию и ротацию
        transform.localPosition = startPosition;
        transform.localRotation = startRotation;
        isShaking = false;
    }

    private IEnumerator HitShake(float duration, float intensity)
    {
        float elapsedTime = 0f;

        // Сохраняем текущую локальную позицию как стартовую
        Vector3 startPosition = transform.localPosition;
        Quaternion startRotation = transform.localRotation;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float normalizedTime = Mathf.Clamp01(elapsedTime / duration);

            // Используем кривую для плавного затухания дрожания
            float curveValue = shakeCurve.Evaluate(normalizedTime);

            // Создаём случайное смещение (удар более резкий)
            Vector3 randomOffset = new Vector3(
                UnityEngine.Random.Range(-1f, 1f) * intensity * curveValue,
                UnityEngine.Random.Range(-1f, 1f) * intensity * curveValue,
                0f
            );

            transform.localPosition = startPosition + randomOffset;

            // Лёгкая резкая ротация для удара
            float rot = UnityEngine.Random.Range(-1f, 1f) * intensity * 4f * curveValue;
            transform.localRotation = Quaternion.Euler(0f, 0f, rot) * startRotation;

            yield return null;
        }

        // Возвращаем камеру в стартовую позицию и ротацию
        transform.localPosition = startPosition;
        transform.localRotation = startRotation;
    }

    /// <summary>
    /// Принудительно останавливает дрожание и возвращает камеру в исходную позицию
    /// </summary>
    public void StopShake()
    {
        if (isShaking)
        {
            StopAllCoroutines();
            transform.localPosition = originalLocalPosition;
            transform.localRotation = originalLocalRotation;
            isShaking = false;
        }
    }

    /// <summary>
    /// Сбрасывает исходную позицию камеры (полезно если Player переместился)
    /// </summary>
    public void ResetOriginalPosition()
    {
        if (!isShaking)
        {
            originalLocalPosition = transform.localPosition;
            originalLocalRotation = transform.localRotation;
        }
    }

    // Примеры использования для тестирования
    void Update()
    {
        HandleShake();
    }
}