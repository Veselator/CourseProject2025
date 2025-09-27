using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_Gets_Damage : MonoBehaviour
{
    public Game_Manager manager;
    public int damage;
    public float damageInterval = 0.5f; 
    private float damageTimer = 0f;

    private void Game_Over()
    {
       
        gameObject.SetActive(false);
      manager.Game_Over();
        
       



}
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy")) 
        {
            Health_System.Instance.Take_Damage(damage);
            if (Health_System.Instance.Health <= 0)
            {

                Game_Over();
            }
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy")) 
        {
            damageTimer += Time.deltaTime;
            if (damageTimer > damageInterval)
            {
                Health_System.Instance.Take_Damage(damage);
                if (Health_System.Instance.Health <= 0)
                {

                    Game_Over();
                }
                damageTimer = 0f;
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        damageTimer = 0f;
    }

}
