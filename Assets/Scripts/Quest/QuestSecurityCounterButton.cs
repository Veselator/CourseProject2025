using System;
using UnityEngine;

public class QuestSecurityCounterButton : InteractableItem
{
    // �������� ��������������� �� ������
    [SerializeField] private int deltaValue = 1;
    
    public event Action<int> OnValueChanged;

    public override void Interact()
    {
        OnValueChanged?.Invoke(deltaValue);
    }
}
