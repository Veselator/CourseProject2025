using System.Collections;
using TMPro;
using UnityEngine;

public class UIUpgradesManager : MonoBehaviour
{
    // ����� �������� �� ����������� ������ � ������� ���������

    private UpgradesManager upgradesManager;
    [Header("Animation Settings")]
    [SerializeField] private Transform upgradesMenuTransform;
    [SerializeField] private GameObject[] buttons; // 0 � 1 - �����, 2 � 3 - ������
    private TextMeshProUGUI[] buttonTexts;

    private void Start()
    {
        upgradesManager = UpgradesManager.Instance;
        upgradesManager.OnUpgradesReady += ShowUI;

        InitButtons();
    }

    private void InitButtons()
    {
        buttonTexts = new TextMeshProUGUI[buttons.Length];
        for (int i = 0; i < buttons.Length; i++)
        {
            Debug.Log($"{i} - {buttonTexts[i]}");
            buttonTexts[i] = buttons[i].GetComponent<TextMeshProUGUI>();
        }
    }

    private void OnDestroy()
    {
        upgradesManager.OnUpgradesReady -= ShowUI;
    }

    private void ShowUI()
    {
        // ��������� �����
        for (int i = 0; i < 2; i++)
        {
            IUpgrade chosenUpgrade = upgradesManager.choosenUpgrades[i];
            Debug.Log($"Index {i}: The chosen upgrade is {chosenUpgrade.MainText} buttonTexts[i] == null: {buttonTexts[i] == null}");
            buttonTexts[2 * i].text = chosenUpgrade.MainText;// + " " + chosenUpgrade.SecondText;
            buttonTexts[2 * i + 1].text = chosenUpgrade.SecondText;
        }

        StartCoroutine(UIShowingAnimation(upgradesMenuTransform));
    }

    public void HideUI()
    {
        StartCoroutine(UIHidingAnimation(upgradesMenuTransform));
    }

    private IEnumerator UIShowingAnimation(Transform target, float duration = 0.6f, float overshoot = 1.2f)
    {
        Vector3 originalScale = target.localScale;
        target.localScale = Vector3.zero;
        target.gameObject.SetActive(true);

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float progress = elapsedTime / duration;

            // ��������� easing ������� � overshoot
            float easeValue = EaseInBackWithOvershoot(progress, overshoot);

            target.localScale = originalScale * easeValue;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // ��������� ��������� ������� ��������
        target.localScale = originalScale;
    }

    private IEnumerator UIHidingAnimation(Transform target, float duration = 0.6f, float overshoot = 1.2f)
    {
        Vector3 originalScale = target.localScale;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float progress = elapsedTime / duration;

            // ��������� easing ������� � overshoot
            float easeValue = EaseInBackWithOvershoot(1f - progress, overshoot);

            target.localScale = originalScale * easeValue;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        target.gameObject.SetActive(false);

        target.localScale = originalScale;
    }

    private float EaseInBackWithOvershoot(float t, float overshoot = 1.2f)
    {
        // Ease-in ����� (0 -> 0.7)
        if (t < 0.7f)
        {
            float normalizedT = t / 0.7f;
            // ���������� ease-in
            return normalizedT * normalizedT * normalizedT;
        }
        // Overshoot ����� (0.7 -> 0.85)
        else if (t < 0.85f)
        {
            float normalizedT = (t - 0.7f) / 0.15f;
            float startValue = 1f;
            float endValue = overshoot;
            // ������� ���� � overshoot
            return Mathf.Lerp(startValue, endValue, normalizedT * normalizedT);
        }
        // Settle ������� � 1 (0.85 -> 1.0)
        else
        {
            float normalizedT = (t - 0.85f) / 0.15f;
            float startValue = overshoot;
            float endValue = 1f;
            // ������� �����������
            return Mathf.Lerp(startValue, endValue, normalizedT * normalizedT);
        }
    }
}
