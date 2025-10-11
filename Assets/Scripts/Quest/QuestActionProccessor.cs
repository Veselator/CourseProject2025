using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestActionProccessor : MonoBehaviour
{
    private QuestAnimationManager _questAnimationManager;
    private QuestGameManager _questGameManager;

    public void ProccessAction(QuestAction action)
    {
        // Обрабатываем действия
        if (action.actionEffects == null || action.actionEffects.Length == 0) return;

        foreach (var effect in action.actionEffects)
        {
            ApplyEffect(effect);
        }
    }

    private void ApplyEffect(QuestActionEffect effect)
    {

    }
}
