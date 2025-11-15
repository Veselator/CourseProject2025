using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class KillCounterUi : MonoBehaviour
{
    private TMP_Text _killText;
    private KillCounter _killCounter;

    private void Start()
    {
        _killText = GetComponent<TMP_Text>();
        _killCounter = KillCounter.Instance;
    }

    private void Update()
    {
        if (_killCounter != null) _killText.text = $"{_killCounter.count}";
    }
}
