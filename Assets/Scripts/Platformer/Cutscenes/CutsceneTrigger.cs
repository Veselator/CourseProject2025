using UnityEngine;

public class CutsceneTrigger : MonoBehaviour
{
    [SerializeField] private PlatformerCutscene _linkedCutscene;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerPlatformerHandler>())
        {
            PlatformerCutscenesManager.Instance.StartCutscene(_linkedCutscene);
            Destroy(gameObject);
        }
    }
}
