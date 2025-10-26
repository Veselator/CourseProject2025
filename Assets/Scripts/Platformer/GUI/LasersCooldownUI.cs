using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LasersCooldownUI : MonoBehaviour
{
    [SerializeField] private Image _linkedImage;
    [SerializeField] private TMP_Text _linkedText;
    [SerializeField] private LaserTurnOffer _linkedTurnOff;

    [Header("Visual Settings")]
    [SerializeField] private Color _cooldownColor = new Color(1f, 0.3f, 0.3f, 0.8f);
    [SerializeField] private Color _readyColor = new Color(0.3f, 1f, 0.3f, 0.8f);

    private Coroutine _cooldownCoroutine;

    public void Start()
    {
        _linkedTurnOff.OnLaserTimer += ShowCooldown;
        SetUIVisible(false);
    }

    private void OnDestroy()
    {
        _linkedTurnOff.OnLaserTimer -= ShowCooldown;
    }

    private void ShowCooldown(float cooldownTime)
    {
        if (_cooldownCoroutine != null)
        {
            StopCoroutine(_cooldownCoroutine);
        }

        _cooldownCoroutine = StartCoroutine(CooldownRoutine(cooldownTime));
    }

    private IEnumerator CooldownRoutine(float totalTime)
    {
        SetUIVisible(true);

        float remainingTime = totalTime;

        while (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;

            float progress = remainingTime / totalTime;
            if (_linkedImage != null)
            {
                _linkedImage.fillAmount = progress;
                _linkedImage.color = Color.Lerp(_readyColor, _cooldownColor, progress);
            }

            if (_linkedText != null)
            {
                _linkedText.text = Mathf.Ceil(remainingTime).ToString("0");
            }

            yield return null;
        }

        if (_linkedImage != null)
        {
            _linkedImage.fillAmount = 0f;
            _linkedImage.color = _readyColor;
        }

        SetUIVisible(false);
        _cooldownCoroutine = null;
    }

    private void SetUIVisible(bool visible)
    {
        if (_linkedImage != null)
        {
            _linkedImage.gameObject.SetActive(visible);
        }

        if (_linkedText != null)
        {
            _linkedText.gameObject.SetActive(visible);
        }
    }
}