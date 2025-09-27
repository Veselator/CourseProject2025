using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable 
{
    public void Enemy_Gets_Damage(int damage, Vector2 knockDir);
}
