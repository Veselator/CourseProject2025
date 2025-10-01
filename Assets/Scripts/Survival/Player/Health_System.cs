using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health_System : MonoBehaviour
{
    public static Health_System Instance { get; private set; }
    public Image HealthBar;
    // По нормальному надо добавить переменную MaxAmountOfHealth
    private int amountOfHealth = 100;
    public int Health => amountOfHealth;
    private float lerpFactor = 0.002f;

    private void Awake()
    {
        if(Instance == null ) Instance = this;
        else Destroy(gameObject);
        
    }
    public void Take_Heal(int heal)
    {
        amountOfHealth = Mathf.Min(amountOfHealth + heal, 100);
        //HealthBar.fillAmount = amountOfHealth;
    }
   
    public void Take_Damage(int damage) 
    {
        //HealthBar.fillAmount -= damage / 100f;
        amountOfHealth -= damage;
    }

    private void Update()
    {
        // Это ужасно - логика интерфейса в логике системы
        // Но имеем что имеем
        HealthBar.fillAmount = Mathf.Lerp(HealthBar.fillAmount, amountOfHealth / 100f, lerpFactor);
    }

}
