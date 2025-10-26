using System;
using System.Collections;
using UnityEngine;

public class LaserTurnOffer : MonoBehaviour
{
    private Laser[] _lasers;

    public event Action<float> OnLaserTimer;
    private void Start()
    {
        _lasers = FindObjectsOfType<Laser>();
    }

    public void SetLasersActve(bool state)
    {
        foreach (var laser in _lasers)
        {
            laser.SetLaserActive(state);
        }
    }

    // Да, не очень решение
    // Но такова архитектура
    public void StartCoroutineForTurinngOffLasers(IAbility trackingAbility, float duration, float durationAfterEnd)
    {
        StartCoroutine(TurningOffLasers(trackingAbility, duration, durationAfterEnd));
    }

    private IEnumerator TurningOffLasers(IAbility trackingAbility, float duration, float durationAfterEnd)
    {
        OnLaserTimer?.Invoke(duration);

        trackingAbility.IsAvailable = false;
        SetLasersActve(false);

        yield return new WaitForSeconds(duration);
        SetLasersActve(true);

        yield return new WaitForSeconds(durationAfterEnd);
        trackingAbility.IsAvailable = true;
    }
}
