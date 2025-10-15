using System.Collections;
using TMPro;
using UnityEngine;

public class QuestThinkingManager : MonoBehaviour
{
    // Скрипт, которые отвечает за отображение мыслей главного героя
    [SerializeField] private TextMeshProUGUI _thinkingText;
    [SerializeField] private GameObject _thinkingPanel;
    [SerializeField] private float _delayAfterEndOfMessage = 2f;
    [SerializeField] private float _delay = 0.05f;
    public static QuestThinkingManager Instance { get; private set; }
    private bool isPlayingAnimation = false;

    private void Awake()
    {
        if(Instance == null) Instance = this;
        _thinkingPanel.SetActive(false);
    }

    public void Think(string text, float delayBeforeStart)
    {
        if (isPlayingAnimation)
        {
            StopAllCoroutines();
        }

        StartCoroutine(Thinking(text, delayBeforeStart));
    }

    private IEnumerator Thinking(string text, float delayBeforeStart)
    {
        isPlayingAnimation = true;
        if (delayBeforeStart > 0f) yield return new WaitForSeconds(delayBeforeStart);
        _thinkingPanel.SetActive(true);

        _thinkingText.text = text;
        _thinkingText.maxVisibleCharacters = 0;

        int totalCharacters = text.Length;
        for (int i = 0; i <= totalCharacters; i++)
        {
            _thinkingText.maxVisibleCharacters = i;
            yield return new WaitForSeconds(_delay);
        }

        yield return new WaitForSeconds(_delayAfterEndOfMessage);
        _thinkingPanel.SetActive(false);
        isPlayingAnimation = false;
    }
}
