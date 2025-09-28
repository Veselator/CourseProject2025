using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelsController : MonoBehaviour
{
    // Скрипт для вращения колёс
    private DistanceTracker _distanceTracker;
    private float speedFactor = -29.4f;

    private void Start()
    {
        _distanceTracker = DistanceTracker.Instance;
    }

    private void Update()
    {
        if (!GlobalFlags.GetFlag(GlobalFlags.Flags.GAME_OVER)) transform.Rotate(0, 0, _distanceTracker.CurrentSpeed * Time.deltaTime * speedFactor);
    }
}
