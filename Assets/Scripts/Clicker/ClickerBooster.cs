using UnityEngine;

[CreateAssetMenu(fileName = "YetAnotherClickerBooster", menuName = "Clicker/Booster")]
public class ClickerBooster : ScriptableObject
{
    // Название
    public string title;
    // Доход от одной единицы
    public float incomePerUnit;
    // Цена для разблокировки
    public float priceToUnlock;
    // Базовая цена за единицу апгрейда
    public float basePriceForUnit;
    // Множитель цены за каждый апгрейд
    public float priceScalerFactor;
    // Максимальный уровень
    public int maxLevel;
}
