using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleSolvedImageScript : MonoBehaviour
{
    [SerializeField] private Vector3 _target = Vector3.zero;
    [SerializeField] private float duration = 1.0f; // Продолжительность анимации
    [SerializeField] private float _endScale = 1.4f;
    private Transform _border;

    public void Init(GameObject border)
    {
        _border = border.transform;
        StartCoroutine(MoveToTarget());
    }

    public void Init(Vector3 target, GameObject border)
    {
        _target = target;
        _border = border.transform;
        StartCoroutine(MoveToTarget());
    }

    public void Init(Vector3 target, float endScale, GameObject border)
    {
        _target = target;
        _endScale = endScale;
        _border = border.transform;
        StartCoroutine(MoveToTarget());
    }

    // Корутина движеня от начальной позиции к target
    //IEnumerator MoveToTarget() 
    //{
    //    // Easein

    //    float elapsed = 0f; // Время, прошедшее с начала анимации
    //    Vector3 startingPos = transform.position; // Начальная позиция

    //    while (elapsed < duration) 
    //    {
    //        transform.position = Vector3.Lerp(startingPos, target, elapsed / duration);
    //        elapsed += Time.deltaTime;
    //        yield return null; // Ждем следующий кадр
    //    }

    //    transform.position = target; // Устанавливаем точную позицию в конце
    //}

    // Перемещение с ease out

    IEnumerator MoveToTarget() 
    {
        float elapsed = 0f; // Время, прошедшее с начала анимации
        Vector3 startingPos = transform.position; // Начальная позиция

        Vector3 startScale = transform.localScale;
        Vector3 targetScale = startScale * _endScale;

        Vector3 startBorderScale = _border.transform.localScale;
        Vector3 targetBorderScale = startBorderScale * _endScale * 1.1f;

        while (elapsed < duration) 
        {
            float t = elapsed / duration;
            t = t * t * (3f - 2f * t); // Ease out функция

            transform.position = Vector3.Lerp(startingPos, _target, t);
            transform.localScale = Vector3.Lerp(startScale, targetScale, t);

            _border.transform.position = transform.position;
            _border.transform.localScale = Vector3.Lerp(startBorderScale, targetBorderScale, t);

            elapsed += Time.deltaTime;
            yield return null; // Ждем следующий кадр
        }

        transform.position = _target; // Устанавливаем точную позицию в конце
        _border.transform.position = transform.position;

    }
}
