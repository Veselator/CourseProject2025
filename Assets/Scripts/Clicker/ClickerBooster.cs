using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "YetAnotherClickerBooster", menuName = "Clicker/Booster")]
public class ClickerBooster : ScriptableObject
{
    // Доход от одной единицы
    public float incomePerUnit;
    // Цена для разблокировки
    public float priceToUnlock;
    // Базовая цена за единицу апгрейда
    public float basePriceForUnit;
    // Множитель цены за каждый апгрейд
    public float priceScalerFactor;
}
