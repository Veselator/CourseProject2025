using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DistanceProgressBarTracker : MonoBehaviour
{
    private DistanceTracker _distanceTracker;
    [SerializeField] private Image slider;

    private void Start()
    {
        _distanceTracker = DistanceTracker.Instance;
    }

    private void Update()
    {
        slider.fillAmount = _distanceTracker.ProgressToEnd;
    }
}
