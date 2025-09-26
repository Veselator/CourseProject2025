using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitShakeHolder : MonoBehaviour
{
    // Скрипт что-бы подвязать урон по игроку с тряской камеры
    // Интересно, что сам IHealth не знает о существовании CameraShake
    // Ровно как и CameraShake не знает о существовании конкретного экземпляра

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
        // Удерживаем движение на время тряски
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
