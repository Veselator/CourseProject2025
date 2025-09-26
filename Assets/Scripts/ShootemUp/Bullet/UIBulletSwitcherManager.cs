using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIBulletSwitcherManager : MonoBehaviour
{
    private BulletsManagmentSystem _bms;
    private BulletSwitcher _bs;

    // Важно, что-бы ихний порядок соотвествовал порядку пуль в enum
    [SerializeField] private GameObject[] _bulletsImages;
    [SerializeField] private GameObject selector;
    private float selectorAnimationDuration = 0.5f;
    private float unlockAnimationDuration = 3.2f;

    private void Start()
    {
        _bs = BulletSwitcher.Instance;
        _bms = BulletsManagmentSystem.Instance;

        _bs.OnBulletSwitched += SwitchBullet;
        _bms.OnNewBulletUnlocked += UnlockBullet;

        selector.transform.position = _bulletsImages[0].transform.position;
    }

    private void OnDestroy()
    {
        _bs.OnBulletSwitched -= SwitchBullet;
        _bms.OnNewBulletUnlocked -= UnlockBullet;
    }

    private void UnlockBullet(BulletType bulletType)
    {
        StartCoroutine(UnlockAnimation(_bulletsImages[(int)bulletType]));
    }

    private void SwitchBullet(int newBulletIndex)
    {
        StartCoroutine(MoveSelector(_bulletsImages[newBulletIndex].transform.position));
    }

    private IEnumerator UnlockAnimation(GameObject unlockBulletImage)
    {
        Image bulletImage = unlockBulletImage.GetComponent<Image>();
        if (bulletImage == null) yield break;

        // Активируем объект
        unlockBulletImage.SetActive(true);

        // Сохраняем исходный цвет
        Color originalColor = bulletImage.color;

        float elapsedTime = 0f;

        while (elapsedTime < unlockAnimationDuration)
        {
            // Создаем эффект мерцания с разными скоростями
            float fastBlink = Mathf.Sin(elapsedTime * 15f); // Быстрое мерцание
            float slowPulse = Mathf.Sin(elapsedTime * 3f);  // Медленная пульсация

            // Комбинируем эффекты для более сложного мерцания
            float blinkIntensity = (fastBlink * 0.3f + slowPulse * 0.7f + 1f) * 0.5f;

            // Добавляем затухание в конце анимации
            float fadeMultiplier = elapsedTime < unlockAnimationDuration * 0.8f ? 1f :
                                  Mathf.Lerp(1f, 1f, (elapsedTime - unlockAnimationDuration * 0.8f) / (unlockAnimationDuration * 0.2f));

            // Применяем эффект к альфа-каналу
            Color newColor = originalColor;
            newColor.a = originalColor.a * blinkIntensity * fadeMultiplier;
            bulletImage.color = newColor;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Возвращаем исходный цвет в конце
        bulletImage.color = originalColor;
    }

    private IEnumerator MoveSelector(Vector2 endPosition)
    {
        float elapsedTime = 0f;
        Vector2 selectorStartPos = selector.transform.position;
        while (elapsedTime < selectorAnimationDuration)
        {
            float currentFactor = elapsedTime / selectorAnimationDuration;
            selector.transform.position = Vector2.Lerp(selectorStartPos, endPosition, currentFactor);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        selector.transform.position = endPosition;
    }
}
