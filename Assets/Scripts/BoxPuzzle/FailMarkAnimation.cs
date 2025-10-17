using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FailMarkAnimation : MonoBehaviour
{
    [SerializeField] private float animationDuration = 0.5f;
    [SerializeField] private float scaleMultiplier = 1.2f;
    private Vector3 startScale = Vector3.zero;
    private Vector3 targetScale;

    private void Start()
    {
        targetScale = transform.localScale * scaleMultiplier;
        transform.localScale = startScale;
        StartCoroutine(PlayFailAnimation());
    }

    //  орутина котора€ ease-in увличивает размер на заданный множитель и затем уменьшает обратно
    private IEnumerator PlayFailAnimation()
    {
        float halfDuration = animationDuration / 2f;
        float timer = 0f;
        // ”величение

        while (timer < halfDuration)
        {
            float t = timer / halfDuration;
            transform.localScale = Vector3.Lerp(startScale, targetScale, t * t);
            timer += Time.deltaTime;
            yield return null;
        }

        // ”бедитьс€ что достигли максимального размера
        transform.localScale = targetScale;
        timer = 0f;
        // ”меньшение
        while (timer < halfDuration)
        {
            float t = timer / halfDuration;
            transform.localScale = Vector3.Lerp(targetScale, startScale, t * t);
            timer += Time.deltaTime;
            yield return null;
        }
        // ”бедитьс€ что вернулись к исходному размеру
        transform.localScale = startScale;
        Destroy(gameObject);
    }
}
