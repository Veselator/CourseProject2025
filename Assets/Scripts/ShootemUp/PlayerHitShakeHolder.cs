using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitShakeHolder : MonoBehaviour
{
    // ������ ���-�� ��������� ���� �� ������ � ������� ������
    // ���������, ��� ��� IHealth �� ����� � ������������� CameraShake
    // ����� ��� � CameraShake �� ����� � ������������� ����������� ����������

    private IHealth trackingHealth;
    private float movementHoldingTime;

    private void Start()
    {
        trackingHealth = GetComponent<IHealth>();
        movementHoldingTime = CameraShake.Instace.ShakeHitTime;

        trackingHealth.OnDamaged += CameraShake.Instace.StartHitShake;
        trackingHealth.OnDamaged += HoldCameraMovement;
    }

    private void HoldCameraMovement()
    {
        // ���������� �������� �� ����� ������
        StartCoroutine(HoldMainCameraMovement(movementHoldingTime));
    }

    private IEnumerator HoldMainCameraMovement(float time)
    {
        CameraController.IsAbleToUpdate = false;
        yield return new WaitForSeconds(time);
        CameraController.IsAbleToUpdate = true;
    }

    private void OnDestroy()
    {
        trackingHealth.OnDamaged -= CameraShake.ShakeCamera;
        trackingHealth.OnDamaged -= HoldCameraMovement;
    }
}
