using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadePickupableItem : MonoBehaviour, IPPickupableItem
{
    public void PickUp()
    {
        GrenadesManager.Instance.AddGrenades();
        Destroy(gameObject);
    }
}
