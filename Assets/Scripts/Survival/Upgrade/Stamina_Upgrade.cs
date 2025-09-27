using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stamina_Upgrade : BaseUpgrade
{
    public override void Player_Gets_Upgrade(Collider2D collision)
    {
        Stamina_Sys.Instance.amountOfStamina += 10;
        Destroy(gameObject);

    }
}
