using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Rotation : MonoBehaviour
{
    private Player2dANimator player2DANimator;
    private Camera mainCamera;

    private void Start()
    {
        player2DANimator = Player2dANimator.Instance;
        mainCamera = Camera.main;
    }

    private void Update()
    {
        // ��������� �������� �� ����� �����
        if (Player_Attack.Instance != null && Player_Attack.Instance.IsAttacking)
            return;

        Check_Mouse_Pos();
    }

    private void Check_Mouse_Pos()
    {
        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = transform.position.z;

        Vector3 direction = mousePos - transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // ������������ ���� � ����������� �� ���������� ���������
        if (player2DANimator.IsFlipped)
        {
            // ����� �������� ���������, ����������� ����
            angle = 180f - angle;
        }

        transform.localRotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}