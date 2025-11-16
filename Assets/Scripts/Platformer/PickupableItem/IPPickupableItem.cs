using System;

public interface IPPickupableItem
{
    // I Platformer Pickupable Item
    public void PickUp();
    public event Action OnItemPickedUp;
}
