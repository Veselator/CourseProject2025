using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundRotatingLines : MonoBehaviour
{
    [SerializeField] private float _rotatingSpeed = 10f;

    private Material _material;
    private float _currentAngle = 0f;

    // �������� ID ��� �����������
    private static readonly int AngleID = Shader.PropertyToID("_Angle");

    void Start()
    {
        // �������� �������� (������ instance)
        Renderer renderer = GetComponent<Renderer>();

        if (renderer != null)
        {
            _material = renderer.material;

            // �������� ������� ���� �� ���������
            _currentAngle = _material.GetFloat(AngleID);
        }
        else
        {
            Debug.LogError("Renderer component not found on " + gameObject.name);
        }
    }

    void Update()
    {
        if (_material == null) return;

        // ����������� ���� �� ������ �������� � �������
        _currentAngle += _rotatingSpeed * Time.deltaTime;

        // ����������� ���� � �������� -180 �� 180 (�����������)
        if (_currentAngle > 180f)
            _currentAngle -= 360f;
        else if (_currentAngle < -180f)
            _currentAngle += 360f;

        // ��������� ����� ���� � �������
        _material.SetFloat(AngleID, _currentAngle);
    }

    // ����� ��� ��������� �������� �������� �� ������ ��������
    public void SetRotationSpeed(float speed)
    {
        _rotatingSpeed = speed;
    }

    // ����� ��� ��������� ��������
    public void StopRotation()
    {
        _rotatingSpeed = 0f;
    }

    // ����� ��� ������ ����
    public void ResetAngle(float angle = 0f)
    {
        _currentAngle = angle;
        if (_material != null)
            _material.SetFloat(AngleID, _currentAngle);
    }

    void OnDestroy()
    {
        // ������� instance ��������� ��� ����������� �������
        if (_material != null)
        {
            Destroy(_material);
        }
    }
}
