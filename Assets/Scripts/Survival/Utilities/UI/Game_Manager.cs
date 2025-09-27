using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game_Manager : MonoBehaviour
{

    public TMP_Text kills_count;
    public TMP_Text mainText;


    public void Show_Kills_In_Game_Over()
    {
        kills_count.text = $" You killed:{KillCounter.Instance.count.ToString()}";
    }
    private void Blue_Print(string s) 
    {
        mainText.text = s;
        gameObject.SetActive(true);
        Show_Kills_In_Game_Over();
    }
    public void Game_Over()
    {
        Blue_Print("You DEAD!!!");

    }
    public void Player_Win() 
    {
        Blue_Print("You WON!!!");
    }
    public void Restart_Level() 
    {
        SceneManager.LoadScene("Survival");
    }
   
}
