using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interval_Upgrade : BaseUpgrade
{
    public override void Player_Gets_Upgrade(Collider2D collision)
    {
        Player_Attack.Instance.attackTime -= 0.05f;
        Destroy(gameObject);

    }
}
