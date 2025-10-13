using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeremikachAtLaba : InteractableItem
{
    private const int MAX_VALUE = 4;
    public int CurrentValue { get; private set; } = 1;
    [SerializeField] private Quaternion[] rotations;

    private void Start()
    {
        transform.rotation = rotations[0];
    }

    public override void Interact()
    {
        GetNextValue();
        UpdateVisual();
    }

    private void GetNextValue()
    {
        CurrentValue++;
        if (CurrentValue > MAX_VALUE) CurrentValue = 1;
    }

    private void UpdateVisual()
    {
        transform.rotation = rotations[CurrentValue - 1];
    }
}
