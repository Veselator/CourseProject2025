using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth
{
    float MaximumHealth { get; }
    float CurrentHealth { get; set; }
    GameObject Instance { get; }

    // Если есть броня, то урон сниженый
    float MaximumArmor { get; } // Сколько максимум может быть брони
    float Armor { get; set; } // Какой текущий показатель брони
    // Параметр урона по броне перенесён в struct Damage для большей гибкости
    //float ArmorFactor { get; set; } // Какой процент от изначального урона пройдёт по броне

    bool DoesHaveArmor { get; }
    IConditionToHit conditionToHit { get; set; } // Интерфейс условия, при соблюдении которого можно регистрировать урон
    bool IsDied { get; }
    Action OnDamaged { get; set; }
    Action OnDeath { get; set;  }
    abstract void TakeDamage(Damage damage); // заменить damage на struct damage, для большей модификации урона
    // Ps OnCollisionEnter2D обрабатывает пуля
}
