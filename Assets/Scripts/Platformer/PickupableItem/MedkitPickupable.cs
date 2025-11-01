using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedkitPickupable : MonoBehaviour, IPPickupableItem
{
    [SerializeField] private float healthBonus = 10f;
    public void PickUp()
    {
        PlayerHealthLinker.PlayerHealth.CurrentHealth += healthBonus;
        Destroy(gameObject);
    }
}
