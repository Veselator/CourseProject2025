using UnityEngine;

public class CutsceneTrigger : BaseTrigger
{
    [SerializeField] private PlatformerCutscene _linkedCutscene;

    protected override void ActionOnPlayerEnter()
    {
        PlatformerCutscenesManager.Instance.StartCutscene(_linkedCutscene);
    }
}
