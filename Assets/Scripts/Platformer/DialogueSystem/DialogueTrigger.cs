using UnityEngine;

public class DialogueTrigger : BaseTrigger
{
    [SerializeField] private DialogueSO _linkedDialogue;

    protected override void ActionOnPlayerEnter()
    {
        DialoguesManager.Instance.StartDialogue(_linkedDialogue);
    }
}
