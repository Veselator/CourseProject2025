using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClickerUIShopItem : MonoBehaviour
{
    [Header("UI элементы")]
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private TextMeshProUGUI requirementsText;
    [SerializeField] private Image iconImage;
    [SerializeField] private Button buyButton;
    [SerializeField] private Image buyButtonImage;
    [SerializeField] private GameObject purchasedOverlay;
    [SerializeField] private GameObject lockedOverlay;
    [SerializeField] private GameObject requirementsPanel;

    [Header("Цвета состояний")]
    [SerializeField] private Color availableColor = Color.white;
    [SerializeField] private Color availableTextColor = Color.black;
    [SerializeField] private Color expensiveColor = new Color(1f, 0.8f, 0.3f);
    [SerializeField] private Color lockedColor = new Color(0.5f, 0.5f, 0.5f);
    [SerializeField] private Color purchasedColor = new Color(0.3f, 1f, 0.3f);

    [Header("Анимация")]
    [SerializeField] private float animationDuration = 0.3f;
    [SerializeField] private AnimationCurve purchaseAnimationCurve;

    private BaseClickerShopItem _shopItem;
    private ClickerManager _clickerManager;
    private ShopItemState _currentState = ShopItemState.Locked;

    private enum ShopItemState
    {
        Locked,      // Заблокирован (условия не выполнены)
        Expensive,   // Доступен, но дорого
        Available,   // Доступен и по карману
        Purchased    // Куплен
    }

    private void Start()
    {
        _clickerManager = ClickerManager.Instance;
        Initialize(GetComponent<BaseClickerShopItem>());
        buyButton.onClick.AddListener(OnBuyButtonClicked);
    }

    public void Initialize(BaseClickerShopItem shopItem)
    {
        _shopItem = shopItem;

        // Заполняем статичные данные
        titleText.text = _shopItem.Title;
        descriptionText.text = _shopItem.Description;
        iconImage.sprite = _shopItem.Icon;

        // Подписываемся на события
        _shopItem.OnItemPurchased += OnItemPurchased;
        _shopItem.OnAvailabilityChanged += OnAvailabilityChanged;
        if (_clickerManager == null) _clickerManager = ClickerManager.Instance;
        _clickerManager.OnMoneyChanged += OnMoneyChanged;

        UpdateUI();
        ApplyStateToUI();
    }

    private void OnDestroy()
    {
        if (_shopItem != null)
        {
            _shopItem.OnItemPurchased -= OnItemPurchased;
            _shopItem.OnAvailabilityChanged -= OnAvailabilityChanged;
        }

        if (_clickerManager != null)
        {
            _clickerManager.OnMoneyChanged -= OnMoneyChanged;
        }
    }

    private void UpdateUI()
    {
        if (_shopItem == null) return;

        // Определяем текущее состояние
        ShopItemState newState = DetermineState();

        // Обновляем UI только если состояние изменилось
        if (newState != _currentState)
        {
            _currentState = newState;
            ApplyStateToUI();
        }

        // Обновляем цену
        priceText.text = NumsFormatter.FormatMoney(_shopItem.Price);

        // Обновляем требования
        UpdateRequirements();
    }

    private ShopItemState DetermineState()
    {
        if (_shopItem.IsBought)
            return ShopItemState.Purchased;

        if (!_shopItem.IsAvailable)
            return ShopItemState.Locked;

        if (!_shopItem.IsAffordable)
            return ShopItemState.Expensive;

        return ShopItemState.Available;
    }

    private void ApplyStateToUI()
    {
        switch (_currentState)
        {
            case ShopItemState.Locked:
                buyButtonImage.color = lockedColor;
                buyButton.interactable = false;
                lockedOverlay.SetActive(true);
                purchasedOverlay.SetActive(false);
                requirementsPanel.SetActive(true);
                break;

            case ShopItemState.Expensive:
                buyButtonImage.color = expensiveColor;
                buyButton.interactable = false;
                lockedOverlay.SetActive(false);
                purchasedOverlay.SetActive(false);
                requirementsPanel.SetActive(false);
                //priceText.color = expensiveColor;
                break;

            case ShopItemState.Available:
                buyButtonImage.color = availableColor;
                buyButton.interactable = true;
                lockedOverlay.SetActive(false);
                purchasedOverlay.SetActive(false);
                requirementsPanel.SetActive(false);
                priceText.color = availableTextColor;
                StartCoroutine(PulseAnimation());
                break;

            case ShopItemState.Purchased:
                buyButtonImage.color = purchasedColor;
                buyButton.interactable = false;
                lockedOverlay.SetActive(false);
                purchasedOverlay.SetActive(true);
                requirementsPanel.SetActive(false);
                break;
        }
    }

    private void UpdateRequirements()
    {
        if (_shopItem is BaseClickerShopItem baseItem)
        {
            var unmetRequirements = baseItem.GetUnmetRequirements();

            if (unmetRequirements.Count > 0)
            {
                requirementsText.text = string.Join("\n", unmetRequirements);
                requirementsPanel.SetActive(true);
            }
            else
            {
                requirementsPanel.SetActive(false);
            }
        }
    }

    private void OnBuyButtonClicked()
    {
        if (_shopItem.TryToPurchase())
        {
            StartCoroutine(PurchaseAnimation());
        }
        else
        {
            StartCoroutine(FailedPurchaseAnimation());
        }
    }

    private IEnumerator PurchaseAnimation()
    {
        Vector3 originalScale = transform.localScale;
        float elapsed = 0f;

        while (elapsed < animationDuration)
        {
            float t = elapsed / animationDuration;
            float curveValue = purchaseAnimationCurve.Evaluate(t);
            transform.localScale = originalScale * (1f + curveValue * 0.2f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localScale = originalScale;
        UpdateUI();
    }

    private IEnumerator FailedPurchaseAnimation()
    {
        // Тряска при неудачной покупке
        Vector3 originalPosition = transform.localPosition;
        float shakeDuration = 0.2f;
        float shakeAmount = 10f;
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            float x = UnityEngine.Random.Range(-shakeAmount, shakeAmount);
            transform.localPosition = originalPosition + new Vector3(x, 0, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPosition;
    }

    private IEnumerator PulseAnimation()
    {
        // Пульсация для доступных предметов
        while (_currentState == ShopItemState.Available)
        {
            float scale = 1f + Mathf.Sin(Time.time * 2f) * 0.05f;
            buyButton.transform.localScale = Vector3.one * scale;
            yield return null;
        }

        buyButton.transform.localScale = Vector3.one;
    }

    private void OnItemPurchased(IClickerShopItem item)
    {
        UpdateUI();
    }

    private void OnAvailabilityChanged(IClickerShopItem item)
    {
        UpdateUI();
    }

    private void OnMoneyChanged(float newAmount)
    {
        UpdateUI();
    }
}