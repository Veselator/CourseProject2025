using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class QuestActionProccessor : MonoBehaviour
{
    // Скрипт, который отвечает за применение эффектов - будь то подбираемые предметы или просто интерактивные
    private QuestGameManager _questGameManager;
    private QuestScreensManager _questScreensManager;
    private QuestInventoryManager _questInventoryManager;
    private QuestObjectRegistry _questObjectRegistry;
    private QuestTimerManager _questTimerManager;
    private QuestVisibilityUIManager _questVisibilityUIManager;
    private QuestThinkingManager _questThinkingManager;
    private QuestThinkingManager QuestThinkingManager
    {
        get
        {
            if (_questThinkingManager == null)
                _questThinkingManager = QuestThinkingManager.Instance;
            return _questThinkingManager;
        }
    }

    public static QuestActionProccessor Instance { get; private set; }

    private void Awake()
    {
        if(Instance == null) Instance = this;
    }

    private void Start()
    {
        InitManagers();
    }

    private void InitManagers()
    {
        _questGameManager = QuestGameManager.Instance;
        _questScreensManager = QuestScreensManager.Instance;
        _questInventoryManager = QuestInventoryManager.Instance;
        _questObjectRegistry = QuestObjectRegistry.Instance;
        _questTimerManager = QuestTimerManager.Instance;
        _questVisibilityUIManager = QuestVisibilityUIManager.Instance;
        _questThinkingManager = QuestThinkingManager.Instance;
    }

    public void ProcessAction(QuestAction action, GameObject sender, bool isBlockItemUse = false)
    {
        if (GlobalFlags.GetFlag(Flags.GameOver)) return;

        // Обрабатываем действия

        // isBlockItemUse - для предотвращения рекурсии

        if (!isBlockItemUse && sender != null && sender.TryGetComponent<InteractableItem>(out InteractableItem item) 
            && _questInventoryManager.IsSelectedAnyItem && _questInventoryManager.SelectedItem.targetItemId.Contains(item.itemID))
        {
            ProcessSelectedItemAction(item, _questInventoryManager.SelectedItem, sender);
            return;
        }

        if (action == null || action.actionEffects == null || action.actionEffects.Length == 0) return;

        foreach (var effect in action.actionEffects)
        {
            if (IsAbleToApplyEffect(effect, sender)) ApplyEffect(effect, sender);
        }
    }

    private void ProcessSelectedItemAction(InteractableItem item, QuestInventoryItem inventoryItem, GameObject sender)
    {
        Debug.Log($"Process item {item.itemID} inventoryItem {inventoryItem.itemId}");

        QuestInventoryItem tempItem = inventoryItem;
        _questInventoryManager.DeselectItem();

        ProcessAction(tempItem.actionOnTarget, sender, true);
        if (inventoryItem.IsOneShoot) _questInventoryManager.RemoveItem(inventoryItem);
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
        GameObject tempObject;

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

            case QuestConditionType.GetBoolValueAtObject:
                tempObject = _questObjectRegistry.GetObject(condition.stringValue);
                IPossibleToGetBool tempInterface;
                if(tempObject == null || !tempObject.TryGetComponent<IPossibleToGetBool>(out tempInterface)) return false; // В любом случае возвращает false
                result = tempInterface.GetBool();
                break;

            case QuestConditionType.IsGameObjectActive:
                tempObject = _questObjectRegistry.GetObject(condition.stringValue);
                if(tempObject == null) return false;
                result = tempObject.activeInHierarchy;
                break;
        }

        // Применяем модификатор
        if (condition.modifier == QuestEffectConditionModifier.Not)
        {
            result = !result;
        }

        return result;
    }

    private void ApplyEffect(QuestActionEffect effect, GameObject sender)
    {
        // Временные переменные
        GameObject tempTarget;
        Flags tempFlag;
        Animator tempAnimator;

        switch (effect.effectType)
        {
            // Общее
            case QuestEffectType.ChangeScreen:
                _questScreensManager.ChangeScreen((int)effect.floatValue);
                break;
            case QuestEffectType.NextLevel:
                _questGameManager.NextLevel();
                break;
            case QuestEffectType.LoadSpecificLevel:
                _questGameManager.LoadSpecificLevel((int)effect.floatValue);
                break;
            case QuestEffectType.WinGame:
                GlobalFlags.SetFlag(Flags.GameWin);
                GameSceneManager.LoadNextScene();
                break;

            // Анимация
            case QuestEffectType.PlayAnimation:
                Debug.Log($"Detected PlayAnimation {sender.name} {effect.stringValue}");
                if (!sender.TryGetComponent<Animator>(out tempAnimator)) return;
                Debug.Log($"SetTrigger {effect.stringValue}");
                tempAnimator.SetTrigger(effect.stringValue);
                break;

            case QuestEffectType.PlayAnimationAtSpecificObject:
                tempTarget = _questObjectRegistry.GetObject(effect.stringValue);
                if (tempTarget == null || !tempTarget.TryGetComponent<Animator>(out tempAnimator)) return;
                tempAnimator.SetTrigger(effect.additionalStringValue);
                break;

            // Спрятать / показать объект
            case QuestEffectType.HideObject:
                tempTarget = _questObjectRegistry.GetObject(effect.stringValue);
                if (tempTarget != null) tempTarget.SetActive(false);
                break;
            case QuestEffectType.ShowObject:
                tempTarget = _questObjectRegistry.GetObject(effect.stringValue);
                if (tempTarget != null) tempTarget.SetActive(true);
                break;

            // Глобальные флаги
            case QuestEffectType.SetGlobalFlag:
                if (GlobalFlags.IsStringFlag(effect.stringValue, out tempFlag))
                {
                    GlobalFlags.SetFlag(tempFlag);
                }
                break;
            case QuestEffectType.RemoveGlobalFlag:
                if (GlobalFlags.IsStringFlag(effect.stringValue, out tempFlag))
                {
                    GlobalFlags.ClearFlag(tempFlag);
                }
                break;
            case QuestEffectType.ToggleGlobalFlag:
                if (GlobalFlags.IsStringFlag(effect.stringValue, out tempFlag))
                {
                    GlobalFlags.ToggleFlag(tempFlag);
                }
                break;

            // Инвентарь
            case QuestEffectType.RemoveItem:
                _questInventoryManager.RemoveItem(effect.stringValue);
                break;

            // Показать масль главного героя
            case QuestEffectType.ShowMessage:
                QuestThinkingManager.Think(effect.stringValue, effect.floatValue);
                break;

            // Передать сообщение конкретному объекту
            case QuestEffectType.InvokeMessage:
                tempTarget = _questObjectRegistry.GetObject(effect.stringValue);
                if (tempTarget == null) return;
                tempTarget.GetComponent<IMessageReceiver>().ProcessMessage(effect.additionalStringValue);
                break;

            case QuestEffectType.InvokeMessageAtCurrentTarget:
                IMessageReceiver imr;
                if (sender == null || !sender.TryGetComponent<IMessageReceiver>(out imr)) return;
                imr.ProcessMessage(effect.additionalStringValue);
                break;

            // Таймер
            case QuestEffectType.StopTimer:
                _questTimerManager.StopTimer();
                break;
            case QuestEffectType.ResumeTimer:
                _questTimerManager.ResumeTimer();
                break;

            // UI
            case QuestEffectType.HideUI:
                _questVisibilityUIManager.HideUI();
                break;
            case QuestEffectType.ShowUI:
                _questVisibilityUIManager.ShowUI();
                break;
        }
    }
}
