using UnityEngine;

public class ClickPowerShopItem : BaseClickerShopItem
{
    // Применяет эффекты от покупки
    public override void ApplyEffect()
    {
        var clickerManager = ClickerManager.Instance;

        foreach (var effect in itemData.effects)
        {
            switch (effect.type)
            {
                case EffectType.ClickPowerBonus:
                    clickerManager.BaseIncomePerClick += effect.value;
                    break;

                case EffectType.ClickPowerMultiplier:
                    clickerManager.IncomePerClickMultiplier += effect.value;
                    break;

                case EffectType.ChangePriceFactor:
                    clickerManager.MultiplyPriceFactor(effect.value);
                    break;
            }
        }
    }
}