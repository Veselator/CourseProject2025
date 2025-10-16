using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundRotatingLines : MonoBehaviour
{
    [SerializeField] private float _rotatingSpeed = 10f;

    private Material _material;
    private float _currentAngle = 0f;

    // Кэшируем ID для оптимизации
    private static readonly int AngleID = Shader.PropertyToID("_Angle");

    void Start()
    {
        // Получаем материал (создаёт instance)
        Renderer renderer = GetComponent<Renderer>();

        if (renderer != null)
        {
            _material = renderer.material;

            // Получаем текущий угол из материала
            _currentAngle = _material.GetFloat(AngleID);
        }
        else
        {
            Debug.LogError("Renderer component not found on " + gameObject.name);
        }
    }

    void Update()
    {
        if (_material == null) return;

        // Увеличиваем угол на основе скорости и времени
        _currentAngle += _rotatingSpeed * Time.deltaTime;

        // Нормализуем угол в диапазон -180 до 180 (опционально)
        if (_currentAngle > 180f)
            _currentAngle -= 360f;
        else if (_currentAngle < -180f)
            _currentAngle += 360f;

        // Применяем новый угол к шейдеру
        _material.SetFloat(AngleID, _currentAngle);
    }

    // Метод для изменения скорости вращения из других скриптов
    public void SetRotationSpeed(float speed)
    {
        _rotatingSpeed = speed;
    }

    // Метод для остановки вращения
    public void StopRotation()
    {
        _rotatingSpeed = 0f;
    }

    // Метод для сброса угла
    public void ResetAngle(float angle = 0f)
    {
        _currentAngle = angle;
        if (_material != null)
            _material.SetFloat(AngleID, _currentAngle);
    }

    void OnDestroy()
    {
        // Очищаем instance материала при уничтожении объекта
        if (_material != null)
        {
            Destroy(_material);
        }
    }
}
