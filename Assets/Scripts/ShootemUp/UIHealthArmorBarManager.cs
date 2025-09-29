using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthArmorBarManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image healthBarImage;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI healthTextBottom;
    [SerializeField] private TextMeshProUGUI armorText;
    [SerializeField] private TextMeshProUGUI armorTextBottom;
    [SerializeField] private Image armorBarImage;
    [SerializeField] private GameObject healthBarGameObject;
    [SerializeField] private GameObject armorBarGameObject;

    [Header("Settings")]
    [SerializeField] private Health trackingHealth;
    [SerializeField] private float lerpingSpeed = 10.4f;
    [SerializeField] private float textAnimationDuration = 0.5f;
    [SerializeField] private AnimationCurve textAnimationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    // Кэшированные значения для оптимизации
    private float currentHealthFill;
    private float currentArmorFill;
    private float currentDisplayedHealthPercent = 1f;
    private float currentDisplayedArmorPercent = 1f;

    // Состояние анимации текста
    private Coroutine healthTextAnimationCoroutine;
    private Coroutine armorTextAnimationCoroutine;
    private bool wasArmorActiveLastFrame;

    private void Awake()
    {
        ValidateReferences();
        CacheInitialValues();
    }

    private void Start()
    {
        InitializeBars();
        SubscribeToEvents();

        ForceUpdateUI();
    }

    private void OnDestroy()
    {
        UnsubscribeFromEvents();
    }

    private void Update()
    {
        UpdateBars();
        HandleArmorVisibility();
        UpdateBottomText();
    }

    private void UpdateBottomText()
    {
        if(armorTextBottom != null) armorTextBottom.text = armorText.text;
        healthTextBottom.text = healthText.text;
    }

    private void ValidateReferences()
    {
        if (healthBarImage == null)
            Debug.LogError($"Health bar image is not assigned in {gameObject.name}!", this);

        if (healthText == null)
            Debug.LogError($"Health text is not assigned in {gameObject.name}!", this);

        if (trackingHealth == null)
            Debug.LogError($"Tracking health is not assigned in {gameObject.name}!", this);
    }

    private void CacheInitialValues()
    {
        if (trackingHealth != null)
        {
            currentHealthFill = trackingHealth.CurrentHealthInPercentage;
            currentArmorFill = trackingHealth.CurrentArmorInPercentage;
        }
    }

    private void InitializeBars()
    {
        if (healthBarImage != null) healthBarImage.fillAmount = currentHealthFill;

        if (armorBarImage != null)
        {
            armorBarImage.fillAmount = currentArmorFill;
            wasArmorActiveLastFrame = trackingHealth.DoesHaveArmor;
            armorBarGameObject.SetActive(trackingHealth.DoesHaveArmor);
        }

        UpdateHealthText(currentHealthFill);
        UpdateArmorText(currentArmorFill);
    }

    private void SubscribeToEvents()
    {
        if (trackingHealth != null)
        {
            trackingHealth.OnHealthChanged += OnHealthChanged;
            trackingHealth.OnArmorChanged += OnArmorChanged;
        }
    }

    private void UnsubscribeFromEvents()
    {
        if (trackingHealth != null)
        {
            trackingHealth.OnHealthChanged -= OnHealthChanged;
            trackingHealth.OnArmorChanged -= OnArmorChanged;
        }
    }

    private void OnHealthChanged()
    {
        AnimateHealthText(trackingHealth.CurrentHealthInPercentage);
    }

    private void OnArmorChanged()
    {
        AnimateArmorText(trackingHealth.CurrentArmorInPercentage);
    }

    private void UpdateBars()
    {
        if (trackingHealth == null) return;

        UpdateHealthBar();

        if (trackingHealth.DoesHaveArmor && armorBarImage != null)
        {
            UpdateArmorBar();
        }
    }

    public void UpdateHealthBar()
    {
        float targetHealth = trackingHealth.CurrentHealthInPercentage;

        // Оптимизация: обновляем только если есть изменения
        if (Mathf.Abs(currentHealthFill - targetHealth) > 0.00001f)
        {
            currentHealthFill = Mathf.Lerp(currentHealthFill, targetHealth, lerpingSpeed * Time.deltaTime);
            healthBarImage.fillAmount = currentHealthFill;
        }
    }

    public void UpdateArmorBar()
    {
        float targetArmor = trackingHealth.CurrentArmorInPercentage;

        // Оптимизация: обновляем только если есть изменения
        if (Mathf.Abs(currentArmorFill - targetArmor) > 0.00001f)
        {
            currentArmorFill = Mathf.Lerp(currentArmorFill, targetArmor, lerpingSpeed * Time.deltaTime);
            armorBarImage.fillAmount = currentArmorFill;
        }
    }

    private void HandleArmorVisibility()
    {
        if (armorBarImage == null) return;

        bool shouldShowArmor = trackingHealth.DoesHaveArmor;

        // Оптимизация: изменяем видимость только при смене состояния
        if (wasArmorActiveLastFrame != shouldShowArmor)
        {
            armorBarGameObject.SetActive(shouldShowArmor);
            wasArmorActiveLastFrame = shouldShowArmor;
        }
    }

    private void AnimateHealthText(float targetHealthPercent)
    {
        if (healthTextAnimationCoroutine != null)
        {
            StopCoroutine(healthTextAnimationCoroutine);
        }

        healthTextAnimationCoroutine = StartCoroutine(AnimateHealthTextCoroutine(targetHealthPercent));
    }

    private void AnimateArmorText(float targetArmorPercent)
    {
        if (armorTextAnimationCoroutine != null)
        {
            StopCoroutine(armorTextAnimationCoroutine);
        }

        armorTextAnimationCoroutine = StartCoroutine(AnimateArmorTextCoroutine(targetArmorPercent));
    }

    private IEnumerator AnimateHealthTextCoroutine(float targetPercent)
    {
        float startPercent = currentDisplayedHealthPercent;
        float elapsedTime = 0f;

        while (elapsedTime < textAnimationDuration)
        {
            float progress = elapsedTime / textAnimationDuration;
            float curveValue = textAnimationCurve.Evaluate(progress);

            currentDisplayedHealthPercent = Mathf.Lerp(startPercent, targetPercent, curveValue);
            UpdateHealthText(currentDisplayedHealthPercent);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        currentDisplayedHealthPercent = targetPercent;
        UpdateHealthText(currentDisplayedHealthPercent);

        healthTextAnimationCoroutine = null;
    }

    private IEnumerator AnimateArmorTextCoroutine(float targetPercent)
    {
        float startPercent = currentDisplayedArmorPercent;
        float elapsedTime = 0f;

        while (elapsedTime < textAnimationDuration)
        {
            float progress = elapsedTime / textAnimationDuration;
            float curveValue = textAnimationCurve.Evaluate(progress);

            currentDisplayedArmorPercent = Mathf.Lerp(startPercent, targetPercent, curveValue);
            UpdateArmorText(currentDisplayedArmorPercent);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        currentDisplayedArmorPercent = targetPercent;
        UpdateArmorText(currentDisplayedArmorPercent);

        armorTextAnimationCoroutine = null;
    }

    private void UpdateHealthText(float healthPercent)
    {
        if (healthText != null)
        {
            int displayedPercent = Mathf.RoundToInt(healthPercent * 100);
            healthText.text = $"{displayedPercent}%";
        }
    }

    private void UpdateArmorText(float armorPercent)
    {
        if (armorText != null) // Исправлена ошибка: было healthText вместо armorText
        {
            int displayedPercent = Mathf.RoundToInt(armorPercent * 100);
            armorText.text = $"{displayedPercent}%";
        }
    }

    public void ForceUpdateUI()
    {
        if (trackingHealth == null) return;

        currentHealthFill = trackingHealth.CurrentHealthInPercentage;
        currentArmorFill = trackingHealth.CurrentArmorInPercentage;
        currentDisplayedHealthPercent = currentHealthFill;
        currentDisplayedArmorPercent = currentArmorFill;

        healthBarImage.fillAmount = currentHealthFill;

        if (armorBarImage != null)
        {
            armorBarImage.fillAmount = currentArmorFill;
            armorBarGameObject.SetActive(trackingHealth.DoesHaveArmor);
        }

        UpdateHealthText(currentHealthFill);
        UpdateArmorText(currentArmorFill);
    }

    public void SetLerpingSpeed(float newSpeed)
    {
        lerpingSpeed = Mathf.Max(0.1f, newSpeed);
    }
}