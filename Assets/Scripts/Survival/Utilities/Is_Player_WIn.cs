using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Is_Player_WIn : MonoBehaviour
{
    private void Check_For_Win() 
    {
        if (TimeCounter.Instance.TimeCount <= 0) 
        {
            GameSaveManager.Instance.SetLevelCompleted(4);
            GameSceneManager.LoadNextScene();
            //GM.Player_Win();
        }
    }
    private void Update()
    {
        Check_For_Win();
    }
}
