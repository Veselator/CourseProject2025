using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableItem : BaseItem
{
    [SerializeField] private QuestAction action;
    [SerializeField] private bool interactOnlyOnce = true;
    private bool isInteractable = true;

    public override bool CanInteract() => isInteractable;

    public override void Interact()
    {
        QuestActionProccessor.Instance.ProcessAction(action, this.gameObject);

        if (interactOnlyOnce)
        {
            isInteractable = false;
            HideVisual();
        }
    }
}
