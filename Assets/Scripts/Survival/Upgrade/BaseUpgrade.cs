using System;
using UnityEngine;

public abstract class BaseUpgrade : MonoBehaviour
{
    public event Action OnDamageUpgradeTaken;

    public abstract void Player_Gets_Upgrade(Collider2D collision);

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        {
            OnDamageUpgradeTaken?.Invoke();
            Player_Gets_Upgrade(collision);
        }
    }
}
