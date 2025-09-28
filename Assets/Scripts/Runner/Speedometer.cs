using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Speedometer : MonoBehaviour
{
    // ќтвечает за отображение текущей скорости игрока
    DistanceTracker distanceTracker;
    [SerializeField] private Text _textField;
    [SerializeField] private float maxDisplayedSpeed;
    [SerializeField] private Transform speedArrow;

    [SerializeField] private float minRotation;
    [SerializeField] private float maxRotation;

    private void Start()
    {
        distanceTracker = DistanceTracker.GetInstance();
    }

    private float GetCurrentRotation()
    {
        return Mathf.Lerp(minRotation, maxRotation, Mathf.Clamp(distanceTracker.CurrentSpeed / maxDisplayedSpeed, 0f, 1f));
    }

    private void Update()
    {
        // “ут можна прописать анимацию спидометра
        _textField.text = ((int)distanceTracker.CurrentSpeed).ToString("D3");
        speedArrow.transform.rotation = Quaternion.Euler(0, 0, GetCurrentRotation());
    }
}
