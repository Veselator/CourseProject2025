using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Is_Player_WIn : MonoBehaviour
{
    public Game_Manager GM;
    private void Check_For_Win() 
    {
        if (TimeCounter.Instance.TimeCount <= 0) 
        {
            GM.Player_Win();
        }
    }
    private void Update()
    {
        Check_For_Win();
    }
}
