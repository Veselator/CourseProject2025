using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Stamina_Sys : MonoBehaviour
{
    public static Stamina_Sys Instance { get; private set; }
    public Image StaminaBar;
    public float amountOfStamina { get;  set; } = 100f;
    public float Stamina => amountOfStamina;
    const float StaminaRegain = 30f;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

    }

    public void Take_Stamina(float staminaCost)
    {
        StaminaBar.fillAmount -= staminaCost / 100f;
        amountOfStamina = Mathf.Max(amountOfStamina - staminaCost, 0);

    }
    public void Regain_Some_Stamina() 
    {
        amountOfStamina = Mathf.Min(amountOfStamina + StaminaRegain, 100f);
        StaminaBar.fillAmount = amountOfStamina / 100f;

    }
}
