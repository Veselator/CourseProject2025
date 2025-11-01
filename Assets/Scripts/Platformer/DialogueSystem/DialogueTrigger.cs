using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private DialogueSO _linkedDialogue;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerPlatformerHandler>())
        {
            DialoguesManager.Instance.StartDialogue(_linkedDialogue);
            Destroy(gameObject);
        }
    }
}
