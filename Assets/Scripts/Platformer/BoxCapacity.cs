using UnityEngine;

public class BoxCapacity : MonoBehaviour
{
    // Скрипт, который управляет коробкой
    // Следит за прочностью и разрушает её в случае, если прочность исчерпана

    // LEGACY

    //[SerializeField] private int MaxCapacity = 2;
    //private int currentCapacity;

    //private void Start()
    //{
    //    currentCapacity = MaxCapacity;
    //}

    //public void Punch()
    //{
    //    currentCapacity--;
    //    if(currentCapacity == 0)
    //    {
    //        Destroy(gameObject);
    //    }
    //}

    //private void OnDestroy()
    //{
    //    // Дополнительные эффекты
    //}

    // Новое решение - на основе здоровья
    private IHealth health;

    private void Start()
    {
        health = GetComponent<IHealth>();
        health.OnDeath += DestroyBox;
    }

    private void DestroyBox()
    {
        // дополнительные эффекты
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        health.OnDeath -= DestroyBox;
    }
}
