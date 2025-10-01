using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coconat : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        {
            var player = collision.GetComponent<Stamina_Sys>();
            if (player != null) {
                player.Regain_Some_Stamina();
            }
            
            Destroy(gameObject);

        }
    }
}
