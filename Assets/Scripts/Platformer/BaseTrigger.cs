using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerPlatformerHandler>())
        {
            ActionOnPlayerEnter();
            Destroy(gameObject);
        }
    }

    protected abstract void ActionOnPlayerEnter();
}
