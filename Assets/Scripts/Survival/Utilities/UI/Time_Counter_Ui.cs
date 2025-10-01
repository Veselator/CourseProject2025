using UnityEngine;
using TMPro;

public class Time_Counter_Ui : MonoBehaviour
{
    private TMP_Text timeText;
    [SerializeField] private string timeFormat = "mm\\:ss"; // Формат: минуты:секунды
    [SerializeField] private bool showMilliseconds = false;

    private void Awake()
    {
        timeText = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        UpdateTimeDisplay();
    }

    private void UpdateTimeDisplay()
    {
        if (TimeCounter.Instance == null)
        {
            timeText.text = "00:00";
            return;
        }

        float time = TimeCounter.Instance.TimeCount;

        if (showMilliseconds)
        {
            timeText.text = FormatTimeWithMilliseconds(time);
        }
        else
        {
            timeText.text = FormatTime(time);
        }
    }

    private string FormatTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60f);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60f);

        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private string FormatTimeWithMilliseconds(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60f);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60f);
        int milliseconds = Mathf.FloorToInt((timeInSeconds * 100f) % 100f);

        return string.Format("{0:00}:{1:00}.{2:00}", minutes, seconds, milliseconds);
    }
}