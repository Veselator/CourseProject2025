using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBattleTrigger : BaseTrigger
{
    protected override void ActionOnPlayerEnter()
    {
        BossPhasesManager.Instance.StartBossBattle();
    }
}
