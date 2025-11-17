using System.Collections;
using UnityEngine;

public class PlayerHitShakeHolder : MonoBehaviour
{
    // Скрипт что-бы подвязать урон по игроку* с тряской камеры
    // Интересно, что сам IHealth не знает о существовании CameraShake
    // Ровно как и CameraShake не знает о существовании конкретного экземпляра

    // * на самом деле не только игроку

    // Вешаем на объект управления игроком

    private IHealth trackingHealth;
    [SerializeField] private CameraShake _linkedShaker;

    public void Init(CameraShake shaker)
    {
        _linkedShaker = shaker;
    }

    private void Start()
    {
        trackingHealth = GetComponent<IHealth>();

        trackingHealth.OnDamaged += _linkedShaker.StartHitShake;
    }

    private void OnDestroy()
    {
        trackingHealth.OnDamaged -= _linkedShaker.StartHitShake;
    }
}
