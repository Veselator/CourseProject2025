using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestSecurityCounterManager : MonoBehaviour
{
    // Отвечает за проверку комбинации
    [SerializeField] private QuestSecurityCounterHolder[] _holders;
    [SerializeField] private int[] _correctCombination;
    [SerializeField] private QuestAction _onWonAction;
    private bool isWon = false;

    private void Start()
    {
        if (_holders.Length < 4) Debug.LogError("Как минимум должно быть 4 QuestSecurityCounterHolder");
    }

    private void OnEnable()
    {
        foreach (var holder in _holders)
        {
            holder.OnValueChanged += CheckCombination;
        }
    }

    private void OnDisable()
    {
        foreach (var holder in _holders)
        {
            holder.OnValueChanged -= CheckCombination;
        }
    }

    private void CheckCombination()
    {
        if (isWon) return;
        for (int i = 0; i < _holders.Length; i++)
        {
            if (_holders[i].CurrentValue != _correctCombination[i]) return;
        }

        isWon = true;
        QuestActionProccessor.Instance.ProcessAction(_onWonAction, gameObject);
    }
}
