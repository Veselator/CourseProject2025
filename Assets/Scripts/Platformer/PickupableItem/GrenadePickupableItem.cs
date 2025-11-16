using System;
using UnityEngine;

public class GrenadePickupableItem : MonoBehaviour, IPPickupableItem
{
    public event Action OnItemPickedUp;

    public void PickUp()
    {
        GrenadesManager.Instance.AddGrenades();
        OnItemPickedUp?.Invoke();
        Destroy(gameObject);
    }
}
