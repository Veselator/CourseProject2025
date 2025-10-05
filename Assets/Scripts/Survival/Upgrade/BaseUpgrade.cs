using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public abstract class BaseUpgrade : MonoBehaviour
{
    public abstract void Player_Gets_Upgrade(Collider2D collision);
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        {
            Player_Gets_Upgrade(collision);
           
        }
    }
}
