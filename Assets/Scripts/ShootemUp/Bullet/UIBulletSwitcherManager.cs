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

    private Image selectorImage;
    private Transform targetTransform;
    private bool isAtTarget = true;

    private float unlockAnimationDuration = 1.2f;
    private float selectorSpeedFactor = 0.2f;
    private float selectorAccuracy = 0.1f;

    private void Start()
    {
        _bs = BulletSwitcher.Instance;
        _bms = BulletsManagmentSystem.Instance;
        selectorImage = selector.GetComponent<Image>();

        _bs.OnBulletSwitched += SwitchBullet;
        _bms.OnNewBulletUnlocked += UnlockBullet;

        selector.transform.position = _bulletsImages[0].transform.position;

        _bulletsImages[1].SetActive(false);
        _bulletsImages[2].SetActive(false);
        _bulletsImages[3].SetActive(false);

        targetTransform = _bulletsImages[0].transform;

        SwitchBullet(0);
    }

    private void OnDestroy()
    {
        _bs.OnBulletSwitched -= SwitchBullet;
        _bms.OnNewBulletUnlocked -= UnlockBullet;
    }

    private void Update()
    {
        if (isAtTarget) return;
        selector.transform.position = Vector2.Lerp(selector.transform.position, targetTransform.position, selectorSpeedFactor);

        if(Mathf.Abs(selector.transform.position.x - targetTransform.position.x) < selectorAccuracy)
        {
            selector.transform.position = targetTransform.position;
            isAtTarget = true;
        }
    }

    private void UnlockBullet(BulletType bulletType)
    {
        if(GlobalFlags.GetFlag(GlobalFlags.Flags.GAME_OVER)) return; 
        StartCoroutine(UnlockAnimation(_bulletsImages[(int)bulletType]));
    }

    private void SwitchBullet(int newBulletIndex)
    {
        if(GlobalFlags.GetFlag(GlobalFlags.Flags.GAME_OVER)) return;
        targetTransform = _bulletsImages[newBulletIndex].transform;
        selectorImage.color = _bulletsImages[newBulletIndex].GetComponent<Image>().color;
        isAtTarget = false;
        //StartCoroutine(MoveSelector(_bulletsImages[newBulletIndex].transform.position));
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

    //private IEnumerator MoveSelector(Vector2 endPosition)
    //{
    //    while (Mathf.Abs(selector.transform.position.x - endPosition.x) > selectorAccuracy)
    //    {
    //        selector.transform.position = Vector2.Lerp(selector.transform.position, endPosition, selectorSpeedFactor * Time.deltaTime);
    //        yield return null;
    //    }

    //    selector.transform.position = endPosition;
    //}
}
