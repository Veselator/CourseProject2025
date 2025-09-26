using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DistanceProgressBarTracker : MonoBehaviour
{
    private DistanceTracker _distanceTracker;
    private Slider slider;

    private void Start()
    {
        _distanceTracker = DistanceTracker.Instance;
        slider = GetComponent<Slider>();
    }

    private void Update()
    {
        slider.value = _distanceTracker.ProgressToEnd;
    }
}
