using UnityEngine;

public class OpenDoorSignalAction : MonoBehaviour, ISignalAction
{
    private bool _isExecuted = false;
    public bool IsExecuted => _isExecuted;
    [SerializeField] private Door _linkedDoor;

    public void Execute()
    {
        _linkedDoor.SetIsOpen(true);
        _isExecuted = true;
    }

    public void Undo()
    {
        _linkedDoor.SetIsOpen(false);
        _isExecuted = false;
    }
}
