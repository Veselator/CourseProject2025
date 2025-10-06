using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIBooster : MonoBehaviour
{
    // ������ ��� UI ����������� �������

    // ������ �������� ����������
    [SerializeField] private GameObject lockBoosterObject;
    [SerializeField] private GameObject mainGroup;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI incomePerSecond;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private TextMeshProUGUI priceUnlockText;
    [SerializeField] private Image buyUpgradeButtonImage;
    [SerializeField] private Image buyButtonImage;
    private bool currentButtonState = true;

    [SerializeField] private Transform startPointOfUISpawning;
    [SerializeField] private Transform endPointOfUISpawning;

    private Box spawnUIBox;

    private Color buttonNotAvailableColor = Color.gray;
    private Color buttonAvailableColor = Color.white;

    // ��������� ��������
    [SerializeField] private float animationDuration = 0.5f;
    [SerializeField] private float overshoot = 1.2f;

    private static Vector3 originalScale = new Vector3(1f, 1f, 1f);

    // ������� ������
    private BoosterHandler _currentBooster;
    private ClickerManager _clickerManager;

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
        levelText.text = $"Lv. {_currentBooster.CurrentNumOfUpgrades}";
        incomePerSecond.text = $"{_currentBooster.CurrentIncomePerTick}/c";
        priceUnlockText.text = $"{NumsFormatter.FormatMoney(_currentBooster.PriceToUnlock)}";
        priceText.text = $"{NumsFormatter.FormatMoney(Math.Ceiling(_currentBooster.PriceToUpgrade))}";

        spawnUIBox.startPoint = startPointOfUISpawning.localPosition;
        spawnUIBox.endPoint = endPointOfUISpawning.localPosition;

        lockBoosterObject.SetActive(true);
        mainGroup.SetActive(false);

        UpdateButtonState(0f);
    }

    // ������� ���������� �������� ��� �������� � �� ��������� ������
    // 03.10.25
    // ���� � �������� ����� - �� ������ �������� ����� ����������� "�� ���������"

    //

    private void HandlePriceFactorChanged(float newPriceFactor)
    {
        UpdateTextInfo();
    }

    private void UpdateTextInfo()
    {
        levelText.text = $"Lv. {_currentBooster.CurrentNumOfUpgrades}";
        incomePerSecond.text = $"{_currentBooster.CurrentIncomePerTick}/c";
        if (_currentBooster.IsBought) priceText.text = $"{NumsFormatter.FormatMoney(Math.Ceiling(_currentBooster.PriceToUpgrade))}";
        else priceUnlockText.text = $"{NumsFormatter.FormatMoney(_currentBooster.PriceToUnlock)}";
    }

    private void UpdateButtonState(float _ = 0f)
    {
        if (_currentBooster.IsBought) UpdateUpgradeButtonState(_currentBooster.IsAvailableToUpgrade);
        else UpdateBuyButtonState(_currentBooster.IsAvailableToBuy);
    }

    private void UpdateBuyButtonState(bool isAvailableToBuy)
    {
        if (isAvailableToBuy && !currentButtonState)
        {
            currentButtonState = true;
            buyButtonImage.color = buttonAvailableColor;
        }
        else if (!isAvailableToBuy && currentButtonState)
        {
            currentButtonState = false;
            buyButtonImage.color = buttonNotAvailableColor;
        }
    }

    private void UpdateUpgradeButtonState(bool isAvailableToDoUpgrade)
    {
        if (isAvailableToDoUpgrade && !currentButtonState)
        {
            currentButtonState = true;
            buyUpgradeButtonImage.color = buttonAvailableColor;
        }
        else if (!isAvailableToDoUpgrade && currentButtonState)
        {
            currentButtonState = false;
            buyUpgradeButtonImage.color = buttonNotAvailableColor;
        }
    }

    public void TryToUnlockBooster()
    {
        // _currentBooster.TryToBuy
        // ���� ��� ������ - �� ������������
        if (_currentBooster.IsBought) return;
        // ��������� �������������� ���������� ����� ������� TryToBuy()
        // ������ � ������
        if (!_currentBooster.TryToBuy()) return;

        lockBoosterObject.SetActive(false);
        mainGroup.SetActive(true);

        currentButtonState = true;
        UpdateButtonState();
    }

    public void TryToUpgradeBooster()
    {
        if (_currentBooster.TryToUpgrade())
        {
            // ���������� ���������� �������
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

        // ���� 1: ���������� �� overshoot (� ��������)
        Vector3 targetScale = originalScale * overshoot;
        float phase1Duration = animationDuration * 0.6f; // 60% ������� �� ������

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

        // ���� 2: ������� � ������������ �������
        elapsed = 0f;
        float phase2Duration = animationDuration * 0.4f; // 40% ������� �� �������

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

        // ����������� ������ ��������
        transform.localScale = originalScale;
    }

    private Vector2 GetRandomPositionInsideSpawnBox()
    {
        return new Vector2(UnityEngine.Random.Range(spawnUIBox.startPoint.x, spawnUIBox.endPoint.x), 
            UnityEngine.Random.Range(spawnUIBox.startPoint.y, spawnUIBox.endPoint.y));
    }

    private void SpawnAnotherUICoolThingThatIEvenCantNameButWhichHasPrettyCoolLook()
    {
        // � ������ �� ���� ��� ������� ��� �����
        GameObject newUiThing = Instantiate(_currentBooster.CurrentPrefab, transform);
        newUiThing.transform.localPosition = GetRandomPositionInsideSpawnBox();
    }
}
