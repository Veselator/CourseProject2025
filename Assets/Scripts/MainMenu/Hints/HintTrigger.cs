using UnityEngine;
using System;

public class HintTrigger : BaseTrigger
{
    [SerializeField] private BaseHint _linkedHint;
    protected override void ActionOnPlayerEnter()
    {
        _linkedHint.PlayAnimation();
    }
}
