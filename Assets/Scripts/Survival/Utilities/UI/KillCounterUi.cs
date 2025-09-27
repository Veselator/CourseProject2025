using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class KillCounterUi : MonoBehaviour
{
 private TMP_Text killText;

    private void Awake()
    {
        killText = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        if (KillCounter.Instance != null)
        {
            killText.text = $"Enemy killed: {KillCounter.Instance.count}";
        }
    }
}
