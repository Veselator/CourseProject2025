using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIBooster : MonoBehaviour
{
    // Скрипт для UI отображения бустера

    // Важные элементы интерфейса
    [SerializeField] private GameObject lockBoosterObject;
    [SerializeField] private GameObject mainGroup;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI incomePerSecond;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private TextMeshProUGUI priceUnlockText;
    [SerializeField] private Image buyUpgradeButtonImage;
    [SerializeField] private Image buyButtonImage;
    [SerializeField] private ParticleSystem buyParticle;
    private bool currentButtonState = true;

    [SerializeField] private Transform startPointOfUISpawning;
    [SerializeField] private Transform endPointOfUISpawning;

    private Box spawnUIBox;
    [SerializeField] private float spawnStep;

    [SerializeField] private Color buttonNotAvailableColor = Color.gray;
    [SerializeField] private Color buttonAvailableColor = Color.yellow;
    [SerializeField] private Color reachedMaxColor = Color.red;

    // Настройки анимации
    [SerializeField] private float animationDuration = 0.5f;
    [SerializeField] private float overshoot = 1.2f;

    private static Vector3 originalScale = new Vector3(1f, 1f, 1f);

    // Текущий бустер
    private BoosterHandler _currentBooster;
    private ClickerManager _clickerManager;

    public event Action OnBoosterAvailable;
    public event Action OnBoosterNotAvailable;

    private bool _isNeedToUdateInfo = true;

    private void Start()
    {
        _currentBooster = GetComponent<BoosterHandler>();
        _clickerManager = ClickerManager.Instance;
        //originalScale = transform.localScale;

        _clickerManager.OnMoneyChanged += UpdateButtonState;
        _clickerManager.OnPriceFactorChanged += HandlePriceFactorChanged;
        InitUIComponents();
    }

    private void OnDestroy()
    {
        if (_clickerManager == null) _clickerManager = ClickerManager.Instance;
        _clickerManager.OnMoneyChanged -= UpdateButtonState;
        _clickerManager.OnPriceFactorChanged -= HandlePriceFactorChanged;
    }

    private void InitUIComponents()
    {
        title.text = _currentBooster.Title;
        UpdateTextInfo();

        spawnUIBox.startPoint = startPointOfUISpawning.localPosition;
        spawnUIBox.endPoint = endPointOfUISpawning.localPosition;

        lockBoosterObject.SetActive(true);
        mainGroup.SetActive(false);

        UpdateButtonState(0f);
    }

    private void HandlePriceFactorChanged(float newPriceFactor)
    {
        if (!_isNeedToUdateInfo) return;
        UpdateTextInfo();
    }

    private void UpdateTextInfo()
    {
        levelText.text = $"Lv. {_currentBooster.CurrentLevel} / {_currentBooster.MaxLevel}";
        incomePerSecond.text = $"{_currentBooster.CurrentIncomePerTick}/c";
        if (_currentBooster.IsBought) priceText.text = $"{NumsFormatter.FormatMoney(Math.Ceiling(_currentBooster.PriceToUpgrade))}";
        else priceUnlockText.text = $"{NumsFormatter.FormatMoney(_currentBooster.PriceToUnlock)}";
    }

    private void UpdateButtonState(float _ = 0f)
    {
        if (!_isNeedToUdateInfo) return;

        if (_currentBooster.IsBought)
        {
            if (_currentBooster.IsReachedMaxLevel) ApplyMaxLevel();
            else
            {
                UpdateTextInfo();
                UpdateButtonState(_currentBooster.IsAvailableToUpgrade, buyUpgradeButtonImage);
            }
        }
        else UpdateButtonState(_currentBooster.IsAvailableToBuy, buyButtonImage);
    }

    private void ApplyMaxLevel()
    {
        buyUpgradeButtonImage.color = reachedMaxColor;
        priceText.text = "MAX";
        _isNeedToUdateInfo = false;
    }

    private void UpdateButtonState(bool isAvailable, Image buttonImage)
    {
        if (isAvailable && !currentButtonState)
        {
            currentButtonState = true;
            buttonImage.color = buttonAvailableColor;

            OnBoosterAvailable?.Invoke();
        }
        else if (!isAvailable && currentButtonState)
        {
            currentButtonState = false;
            buttonImage.color = buttonNotAvailableColor;
            OnBoosterNotAvailable?.Invoke();
        }
    }

    public void TryToUnlockBooster()
    {
        // _currentBooster.TryToBuy
        // Если уже куплен - то возвращаемся
        if (_currentBooster.IsBought) return;
        // Интерфейс взаимодействия реализован через булевый TryToBuy()
        // Просто и надёжно
        if (!_currentBooster.TryToBuy()) return;

        lockBoosterObject.SetActive(false);
        mainGroup.SetActive(true);
        buyParticle.Play();

        currentButtonState = true;
        UpdateButtonState();
    }

    public void TryToUpgradeBooster()
    {
        if (!_isNeedToUdateInfo) return;

        if (_currentBooster.TryToUpgrade())
        {
            // Графически отобразить апгрейд
            SpawnAnotherUICoolThingThatIEvenCantNameButWhichHasPrettyCoolLook();
            UpdateTextInfo();
        }
    }

    public void ShowAnimation()
    {
        gameObject.SetActive(true);
        StartCoroutine(ShowingAnimation());
    }

    private IEnumerator ShowingAnimation()
    {
        transform.localScale = Vector3.zero;
        Vector3 velocity = Vector3.zero;

        float elapsed = 0f;

        // Фаза 1: Увеличение до overshoot (с перелётом)
        Vector3 targetScale = originalScale * overshoot;
        float phase1Duration = animationDuration * 0.6f; // 60% времени на разгон

        while (elapsed < phase1Duration)
        {
            transform.localScale = Vector3.SmoothDamp(
                transform.localScale,
                targetScale,
                ref velocity,
                phase1Duration - elapsed
            );

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Фаза 2: Возврат к изначальному размеру
        elapsed = 0f;
        float phase2Duration = animationDuration * 0.4f; // 40% времени на возврат

        while (elapsed < phase2Duration)
        {
            transform.localScale = Vector3.SmoothDamp(
                transform.localScale,
                originalScale,
                ref velocity,
                phase2Duration - elapsed
            );

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Гарантируем точное значение
        transform.localScale = originalScale;
    }

    private void SpawnAnotherUICoolThingThatIEvenCantNameButWhichHasPrettyCoolLook()
    {
        // Я правда не знаю как назвать эти штуки
        GameObject newUiThing = Instantiate(_currentBooster.CurrentPrefab, transform);
        newUiThing.transform.localPosition = new Vector2(spawnUIBox.startPoint.x + spawnStep * (_currentBooster.CurrentLevel - 1), spawnUIBox.startPoint.y);//spawnUIBox.startPoint + spawnStep * _currentBooster.CurrentNumOfUpgrades;
    }
}
