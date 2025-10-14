using TMPro;
using UnityEngine;

public class QuestTimerUI : MonoBehaviour
{
    private TMP_Text timeText;
    private QuestTimerManager _timerManager;
    private ShakingObject _shakingObject;

    private bool isHighlighted = false;
    [SerializeField] private float startHighlightWhen = 10f; // Если времени меньше этого значения - то выделяем
    [SerializeField] private Color highlightedColor = Color.red;
    [SerializeField] private Color standartColor = Color.white;

    private void Awake()
    {
        timeText = GetComponent<TMP_Text>();
        _shakingObject = GetComponent<ShakingObject>();

        ChangeColor();
    }

    private void Start()
    {
        _timerManager = QuestTimerManager.Instance;
        _timerManager.OnTimerChanged += UpdateTimeDisplay;
        _timerManager.OnTimerReset += ResetTimer;

        UpdateTimeDisplay(_timerManager.Timer);
    }

    private void OnDestroy()
    {
        _timerManager.OnTimerChanged -= UpdateTimeDisplay;
        _timerManager.OnTimerReset -= ResetTimer;
    }

    private void ResetTimer()
    {
        isHighlighted = false;
        timeText.color = standartColor;
        _shakingObject.StopShaking();
    }

    private void ChangeColor()
    {
        if (isHighlighted) timeText.color = highlightedColor;
        else timeText.color = standartColor;
    }

    private void UpdateTimeDisplay(float time)
    {
        timeText.text = FormatTime(Mathf.Max(time, 0));

        if(time < startHighlightWhen && !isHighlighted)
        {
            isHighlighted = true;
            ChangeColor();
            _shakingObject.StartShaking();
        }
    }

    private string FormatTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60f);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60f);

        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
