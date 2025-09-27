using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Range_Upgrade : BaseUpgrade
{
    public override void Player_Gets_Upgrade(Collider2D collision)
    {
        Player_Attack.Instance.attackR += 0.10f;
        Destroy(gameObject);

    }
}
