using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Time_Counter_Ui : MonoBehaviour
{

    private TMP_Text timeText;

    private void Awake()
    {
        timeText = GetComponent<TMP_Text>();
    }

    
    void Update()
    {
        timeText.text = Mathf.CeilToInt(TimeCounter.Instance.TimeCount).ToString();
    }
}
