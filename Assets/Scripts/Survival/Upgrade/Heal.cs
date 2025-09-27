using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : MonoBehaviour
{
    public int HowMuchHeal = 30;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Health_System.Instance.Take_Heal(HowMuchHeal);
            Destroy(gameObject);
        }
    }
}
