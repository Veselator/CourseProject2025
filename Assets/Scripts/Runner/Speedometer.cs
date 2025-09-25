using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Speedometer : MonoBehaviour
{
    // ќтвечает за отображение текущей скорости игрока
    [SerializeField] DistanceTracker distanceTracker;
    [SerializeField] private Text _textField;

    private void Start()
    {
        distanceTracker = DistanceTracker.GetInstance();
        _textField = GetComponent<Text>();
    }

    private void Update()
    {
        // “ут можна прописать анимацию спидометра
        _textField.text = ((int)distanceTracker.CurrentSpeed).ToString() + " km/s";
    }
}
