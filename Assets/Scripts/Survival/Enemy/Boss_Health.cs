using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Health : Base_Enemy
{
    public GameObject enemyPrefab;
    private void Spawn_4_Minions() 
    { 
    var currPos = this.transform.position;
        Instantiate(enemyPrefab, currPos + new Vector3(-2, 2, 0), Quaternion.identity);
        Instantiate(enemyPrefab, currPos + new Vector3(2, 2, 0), Quaternion.identity);
        Instantiate(enemyPrefab, currPos + new Vector3(-2, -2, 0), Quaternion.identity);
        Instantiate(enemyPrefab, currPos + new Vector3(2, -2, 0), Quaternion.identity);
    }
    public override void OnDeath()
    {
        base.OnDeath();
        Spawn_4_Minions();
    }
    public void Gain_Additional_Health() 
    {
        this.amountOfHealth += 25;
    }
}
