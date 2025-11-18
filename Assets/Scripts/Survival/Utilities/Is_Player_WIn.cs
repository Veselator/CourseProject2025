using UnityEngine;

public class Is_Player_WIn : MonoBehaviour
{
    private void Check_For_Win() 
    {
        if (TimeCounter.Instance.TimeCount <= 0 && !GlobalFlags.GetFlag(Flags.GameWin)) 
        {
            GlobalFlags.SetFlag(Flags.GameWin);
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
