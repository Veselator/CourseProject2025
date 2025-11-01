using UnityEngine;

public class PlayerPickUpItemsManager : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        IPPickupableItem tempItem;
        if (!collision.gameObject.TryGetComponent<IPPickupableItem>(out tempItem)) return;

        tempItem.PickUp();
    }
}
