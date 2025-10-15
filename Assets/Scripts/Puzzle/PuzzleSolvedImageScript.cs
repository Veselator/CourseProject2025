using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleSolvedImageScript : MonoBehaviour
{
    [SerializeField] private Vector3 target = Vector3.zero;
    [SerializeField] private float duration = 1.0f; // Продолжительность анимации
    [SerializeField] private float _endScale = 1.4f;

    public void Init()
    {
        StartCoroutine(MoveToTarget());
    }

    public void Init(Vector3 target)
    {
        this.target = target;
        StartCoroutine(MoveToTarget());
    }

    public void Init(Vector3 target, float endScale)
    {
        this.target = target;
        this._endScale = endScale;
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

        while (elapsed < duration) 
        {
            float t = elapsed / duration;
            t = t * t * (3f - 2f * t); // Ease out функция

            transform.position = Vector3.Lerp(startingPos, target, t);
            transform.localScale = Vector3.Lerp(startScale, targetScale, t);

            elapsed += Time.deltaTime;
            yield return null; // Ждем следующий кадр
        }

        transform.position = target; // Устанавливаем точную позицию в конце
    }
}
