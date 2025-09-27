using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IsPlayerWin : MonoBehaviour
{
    private void Check_For_Win()
    {
        if (TimeCounter.Instance.TimeCount <= 0)
        {
            gameObject.SetActive(false);
        }
        
    }
    private void Update()
    {
        Check_For_Win();
    }
}