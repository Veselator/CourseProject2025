using System;
using UnityEngine;

public class Door : MonoBehaviour
{
    private bool _isOpen = false;
    private Collider2D _doorCollider;
    public bool IsOpen => _isOpen;

    public event Action<bool> OnStateChanged;

    private void Start()
    {
        _doorCollider = GetComponent<Collider2D>();
    }

    public void SetIsOpen(bool newState)
    {
        _isOpen = newState;
        _doorCollider.enabled = !newState;
        OnStateChanged?.Invoke(newState);
    }
}
