using System;
using UnityEngine;

public class MedkitPickupable : MonoBehaviour, IPPickupableItem
{
    [SerializeField] private float healthBonus = 10f;
    public event Action OnItemPickedUp;

    public void PickUp()
    {
        PlayerHealthLinker.PlayerHealth.CurrentHealth += healthBonus;
        OnItemPickedUp?.Invoke();
        Destroy(gameObject);
    }
}
