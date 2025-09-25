using UnityEngine;

[System.Serializable]
public struct Damage
{
    [Min(0f)]
    public float damageHealth; // Сколько пройдёт урона без брони или если броню сняли
    [Min(0f)]
    public float damageArmor; // Сколько пройдёт урона с бронёй
    [Min(0f)]
    public float damageMultiplier;
}
