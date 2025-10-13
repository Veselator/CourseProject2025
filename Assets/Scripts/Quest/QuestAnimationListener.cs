using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class QuestAnimationListener : StateMachineBehaviour
{
    // Скрипт, что-бы выполнить действие при конце анимации
    [SerializeField] private QuestAction _actionOnAnimationEnded;

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ProcessAction();
    }

    private void ProcessAction()
    {
        if (_actionOnAnimationEnded != null) QuestActionProccessor.Instance.ProcessAction(_actionOnAnimationEnded, null);
    }
}
