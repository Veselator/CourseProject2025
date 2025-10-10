using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartShit : MonoBehaviour
{
  public void Restart() {
    UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}
