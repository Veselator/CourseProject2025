using System;
using TMPro;
using UnityEngine;

public class WordVIsualSelection : MonoBehaviour
{
    [SerializeField] private GameObject Selector;
    [SerializeField] private GameObject SecondSelector;
    [SerializeField] private float padding = 5f; // ������ � �������� UI (������� � ����������� ��������)

    private TMP_Text _text;
    private Vector3 _secondInitialLocalPos;
    private bool _hasSecondInitialPos = false;

    private TypingGameplay _typingGameplay;

    private void Start()
    {
        _text = GetComponent<TMP_Text>();
        if (SecondSelector != null)
        {
            _secondInitialLocalPos = SecondSelector.transform.localPosition;
            _hasSecondInitialPos = true;
        }
    }

    public void Init(TypingGameplay tg)
    {
        _typingGameplay = tg;
        _typingGameplay.OnWordChanged += CheckWord;
    }

    private void OnDestroy()
    {
        _typingGameplay.OnWordChanged -= CheckWord;
    }

    private void CheckWord(TextPiece tp)
    {
        Debug.Log($"Word Visual Selection CheckWord {tp.tmpText}");
        if(tp.tmpText.text == _text.text)
        {
            Select();
        }
    }

    public void Select()
    {
        if (Selector != null)
            Selector.SetActive(true);

        if (SecondSelector == null || _text == null || !_hasSecondInitialPos)
            return;

        SecondSelector.SetActive(true);

        // �������� ���������������� ������ ����� ������
        string currentText = _text.text ?? string.Empty;
        Vector2 preferred = _text.GetPreferredValues(currentText);
        float textWidth = preferred.x;

        // ���� ������ ���, ���������� ����������� ��������� �����
        if (string.IsNullOrEmpty(currentText))
            textWidth = 0f;

        // ���������� ����������� ��������: ������ �� ���������, ����� ���� ������������ ������
        int direction = 1;

        // ������������ ������������� �������� � ����������� padding
        Vector3 offset = new Vector3(direction * (textWidth + padding), 0f, 0f);

        // ������������� ����� ��������� ������� SecondSelector ������������ ���������
        SecondSelector.transform.localPosition = _secondInitialLocalPos + offset;
    }

    public void Unselect()
    {
        if (Selector != null)
            Selector.SetActive(false);
        if (SecondSelector != null)
            SecondSelector.SetActive(false);
    }
}
