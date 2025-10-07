using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ClickedIncomeHidingText : MonoBehaviour
{
    // ������, ������� �������� �� ��������� ��������� ������ � ������� �� ����
    // ������, ������� �� ������ - ���������� �������� ������� +1 - ������ �������� ���������

    [SerializeField] private GameObject _textPrefab;
    [SerializeField] private Transform canvas;
    private ClickerHandler _clickerHandler;

    private void Start()
    {
        _clickerHandler = ClickerHandler.Instance;
        _clickerHandler.OnUserClicked += ShowText;
    }

    private void OnDestroy()
    {
        _clickerHandler.OnUserClicked -= ShowText;
    }

    private void ShowText(Vector2 mousePos, float income)
    {
        Debug.Log($"Given position is {mousePos}");
        GameObject yetAnotherTextLabel = Instantiate(_textPrefab, mousePos, Quaternion.identity, canvas);

        yetAnotherTextLabel.GetComponent<TextMeshProUGUI>().text = $"+{income}";
    }
}
