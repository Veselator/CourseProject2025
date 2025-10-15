using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartActionScript : MonoBehaviour
{
    [SerializeField] private QuestAction _startAction;

    private void Start()
    {
        if (_startAction != null) QuestActionProccessor.Instance.ProcessAction(_startAction, null);
    }
}
