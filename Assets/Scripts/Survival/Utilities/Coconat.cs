using System;
using UnityEngine;

public class Coconat : MonoBehaviour
{
    public event Action OnCoconutTaken;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        {
            var player = collision.GetComponent<Stamina_Sys>();
            if (player != null) {
                player.Regain_Some_Stamina();
            }

            OnCoconutTaken?.Invoke();
            Destroy(gameObject);
        }
    }
}
