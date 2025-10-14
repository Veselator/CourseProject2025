using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGameEndTracker : MonoBehaviour
{
    [SerializeField] Animator _horseAnimator;
    [SerializeField] QuestDarkScreenManager _darkScreenManager;
    QuestTimerManager questTimerManager;

    private void Start()
    {
        questTimerManager = QuestTimerManager.Instance;
        questTimerManager.OnTimerExpired += EndGame;
    }

    private void OnDisable()
    {
        questTimerManager.OnTimerExpired -= EndGame;
    }

    private void EndGame()
    {
        _horseAnimator.SetTrigger("Death");
        _darkScreenManager.StartAnimation();
        UIAppearManager.Instance.ShowUI();
        QuestVisibilityUIManager.Instance.HideUI();
    }
}
