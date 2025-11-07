using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : BaseTrigger
{
    [SerializeField] private Door _linkedDoor;
    [SerializeField] private bool newDoorState = false;

    protected override void ActionOnPlayerEnter()
    {
        _linkedDoor.SetIsOpen(newDoorState);
    }
}
