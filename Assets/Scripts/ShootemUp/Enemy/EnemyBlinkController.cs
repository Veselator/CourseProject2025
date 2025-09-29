using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBlinkController : MonoBehaviour
{   
    private static float animationDuration = 0.25f;
    private Health trackingHealth;
    [SerializeField] private SpriteRenderer spriteRenderer;

    void Start()
    {
        trackingHealth = GetComponent<Health>();

        trackingHealth.OnDamaged += StartAnimation;
    }

    private void OnDestroy()
    {
        trackingHealth.OnDamaged -= StartAnimation;
    }

    private void StartAnimation()
    {
        StartCoroutine(FadeInAndOut());
    }

    IEnumerator FadeInAndOut()
    {

        Color color = spriteRenderer.color;
        float elapsedTime = 0f;
        spriteRenderer.color = new Color(color.r, color.g, color.b, 0f);

        spriteRenderer.gameObject.SetActive(true);

        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / animationDuration);
            spriteRenderer.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        spriteRenderer.color = new Color(color.r, color.g, color.b, 1f);

        elapsedTime = 0f;

        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / animationDuration);
            spriteRenderer.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        spriteRenderer.color = new Color(color.r, color.g, color.b, 0f);

        spriteRenderer.gameObject.SetActive(false);
    }
}
