using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class QuestActionProccessor : MonoBehaviour
{
    // ������, ������� �������� �� ���������� �������� - ���� �� ����������� �������� ��� ������ �������������
    private QuestAnimationManager _questAnimationManager;
    private QuestGameManager _questGameManager;
    private QuestScreensManager _questScreensManager;
    private QuestInventoryManager _questInventoryManager;

    public static QuestActionProccessor Instance { get; private set; }

    private void Awake()
    {
        if(Instance == null) Instance = this;
    }

    private void Start()
    {
        _questGameManager = QuestGameManager.Instance;
        _questScreensManager = QuestScreensManager.Instance;
        _questInventoryManager = QuestInventoryManager.Instance;
    }

    public void ProcessAction(QuestAction action, GameObject sender)
    {
        // ������������ ��������

        // TODO: �������� ����� ��������
        // ���� �� ������� �������, �� ���������, �������� �� sender ������������� ��������
        // ���� �� - ���������, �������� �� id ������� sender � id target selectedItem
        // ���� ��������� - �� ������������� �������� selectedItem.ActionOnTarget
        // ������� ������� �� ���������

        // ������: ������� � ����������
        // �� ������� ����������� �����
        // ������ �� ������� ���������� ����������� ��� ����� �������, � ����� ���� ������������� ��������
        // ������ ������ � ��������� �� ��������� �������

        // ���� �� ����� ����������������� � ��������� ��������� � � ��� ���� ��������� ������� ���������, �� ������� ���������� ���
        if (sender != null && sender.TryGetComponent<InteractableItem>(out InteractableItem item) && _questInventoryManager.IsSelectedAnyItem)
        {
            TryToProcessSelectedItemAction(item, _questInventoryManager.SelectedItem);
            return;
        }

        if (action.actionEffects == null || action.actionEffects.Length == 0) return;

        foreach (var effect in action.actionEffects)
        {
            if (IsAbleToApplyEffect(effect, sender)) ApplyEffect(effect, sender);
        }
    }

    private void TryToProcessSelectedItemAction(InteractableItem item, QuestInventoryItem inventoryItem)
    {
        // ������� ���������� ���� ���������
        if (item.itemID != inventoryItem.targetItemId) return;

        // ���� �� ����� ��������� ��� ������� - �� ���������
        // ��������, ����� ��������� ������ �� ���� IsOneShot
        _questInventoryManager.RemoveItem(inventoryItem);
        ProcessAction(inventoryItem.actionOnTarget, null);
    }

    private bool IsAbleToApplyEffect(QuestActionEffect effect, GameObject sender)
    {
        if (effect.conditions == null || effect.conditions.Length == 0) return true;

        if (effect.conditionOperator == QuestConditionOperator.Or)
        {
            foreach (var condition in effect.conditions)
            {
                if (CheckCondition(condition)) return true;
            }
            return false;
        }

        foreach (var condition in effect.conditions)
        {
            if (!CheckCondition(condition)) return false;
        }
        return true;
    }

    private bool CheckCondition(QuestEffectCondition condition)
    {
        bool result = false;

        switch (condition.conditionType)
        {
            case QuestConditionType.IsGlobalFlag:
                Flags flag;
                if (GlobalFlags.IsStringFlag(condition.stringValue, out flag))
                {
                    result = GlobalFlags.GetFlag(flag);
                }
                break;

            case QuestConditionType.DoesHaveItem:
                result = _questInventoryManager.DoesHaveItem(condition.stringValue);
                break;
        }

        // ��������� �����������
        if (condition.modifier == QuestEffectConditionModifier.Not)
        {
            result = !result;
        }

        return result;
    }

    private void ApplyEffect(QuestActionEffect effect, GameObject sender)
    {
        switch (effect.effectType)
        {
            case QuestEffectType.ChangeScreen:
                _questScreensManager.ChangeScreen((int)effect.floatValue);
                break;
            case QuestEffectType.NextLevel:
                _questGameManager.NextLevel();
                break;
            case QuestEffectType.PlayAnimation:
                Animator animator;
                if (!sender.TryGetComponent<Animator>(out animator)) return;
                animator.SetTrigger(effect.stringValue);
                break;
        }
    }
}
