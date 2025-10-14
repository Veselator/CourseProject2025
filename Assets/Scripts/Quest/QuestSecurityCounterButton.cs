using System;
using UnityEngine;

public class QuestSecurityCounterButton : InteractableItem
{
    // Отвечает непосредственно за кнопку
    [SerializeField] private int deltaValue = 1;
    
    public event Action<int> OnValueChanged;

    public override void Interact()
    {
        OnValueChanged?.Invoke(deltaValue);
    }
}
