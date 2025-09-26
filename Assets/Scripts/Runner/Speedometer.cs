using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Speedometer : MonoBehaviour
{
    // �������� �� ����������� ������� �������� ������
    [SerializeField] DistanceTracker distanceTracker;
    [SerializeField] private Text _textField;

    private void Start()
    {
        distanceTracker = DistanceTracker.GetInstance();
        _textField = GetComponent<Text>();
    }

    private void Update()
    {
        // ��� ����� ��������� �������� ����������
        _textField.text = ((int)distanceTracker.CurrentSpeed).ToString() + " km/s";
    }
}
