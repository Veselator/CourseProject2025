using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableItem : BaseItem
{
    [SerializeField] private QuestAction action;
    [SerializeField] private bool interactOnlyOnce = true;
    private bool isInteractable = true;
    [SerializeField] private bool isNeedToHide = false;

    protected override void Start()
    {
        base.Start();
        if(isNeedToHide) gameObject.SetActive(false);
    }

    public override bool CanInteract() => isInteractable;

    public override void Interact()
    {
        QuestActionProccessor.Instance.ProcessAction(action, this.gameObject);

        if (interactOnlyOnce)
        {
            isInteractable = false;
        }
    }
}
