using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Upgrade_Spawner : MonoBehaviour
{
    public TMP_Text Text;
    public GameObject[] upgrades;
    public float notifyTime = 2f;
    public int Kills_For_Upgrade;
    private int currentKills = 0;
    private float notifyTimer;

    private void Awake()
    {
        currentKills = Kills_For_Upgrade;
       
    }
    private void Spawn_Upgrade()
    {

        GameObject currUpgrade = upgrades[Random.Range(0, upgrades.Length)];
        float rand_x = Random.Range(-17.5f, 17.5f);
        float rand_y = Random.Range(-10f, 10f);
        Instantiate(currUpgrade, new Vector3(rand_x, rand_y, 0), Quaternion.identity);
        Debug.Log("Upgrade is spawned");


    }
    

    private void Update()
    {
        if (KillCounter.Instance.count >= currentKills)
        {
            Spawn_Upgrade();
            currentKills += Kills_For_Upgrade;
            notifyTimer = notifyTime;
        }
        if (notifyTimer >= 0)
        {
            Text.text = "Upgrade is spawned!!!";
            notifyTimer -= Time.deltaTime;
        }
        else 
        {
            Text.text = "";
        }



    }
}
