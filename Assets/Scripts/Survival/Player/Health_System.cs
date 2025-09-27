using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health_System : MonoBehaviour
{
    public static Health_System Instance { get; private set; }
    public Image HealthBar;
    private int amountOfHealth = 100;
    public int Health => amountOfHealth;

    private void Awake()
    {
        if(Instance == null ) Instance = this;
        else Destroy(gameObject);
        
    }
    public void Take_Heal(int heal)
    {
        amountOfHealth = Mathf.Min(amountOfHealth + heal, 100);
        HealthBar.fillAmount = amountOfHealth;
    }
   
    public void Take_Damage(int damage) 
    {
        HealthBar.fillAmount -= damage / 100f;
        amountOfHealth -= damage;
      
    }

}
