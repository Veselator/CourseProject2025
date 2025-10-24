using System;
using UnityEngine;

public class Laser : MonoBehaviour, IPossible2DealDamage
{
    [SerializeField] private Damage _damage;
    public Damage DealedDamage => _damage;

    private bool _isVisible = true;
    public bool IsVisible => _isVisible;

    private BoxCollider2D _collider;
    public event Action<bool> OnVisibilityChanged;
    private void Start()
    {
        _collider = GetComponent<BoxCollider2D>();
    }

    public void ToggleLaserVisibility()
    {
        SetLaserActive(!IsVisible);
    }

    public void SetLaserActive(bool state)
    {
        if (state)
        {
            _collider.enabled = true;
            _isVisible = true;
        }
        else
        {
            _collider.enabled = false;
            _isVisible = false;
        }

        OnVisibilityChanged?.Invoke(state);
    }
}
