using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage_Upgrade : BaseUpgrade
{
    public override void Player_Gets_Upgrade(Collider2D collision) 
    {
        Player_Attack.Instance.DAMAGE += 10;
        Destroy(gameObject);
        
    }

}
