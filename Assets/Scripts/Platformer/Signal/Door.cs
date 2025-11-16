using System;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private bool _isOpen = false;
    private Collider2D _doorCollider;
    public bool IsOpen => _isOpen;

    public event Action<bool, bool> OnStateChanged;

    private void Start()
    {
        _doorCollider = GetComponent<Collider2D>();
        SetIsOpen(_isOpen, true);
    }

    public void SetIsOpen(bool newState, bool isSilent = false)
    {
        _isOpen = newState;
        _doorCollider.enabled = !newState;
        OnStateChanged?.Invoke(newState, isSilent);
    }
}
