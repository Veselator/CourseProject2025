using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerGameEndTrigger : MonoBehaviour
{
    private IHealth health;

    private void Start()
    {
        health = GetComponent<Health>();
        health.OnDeath += EndGame;
    }

    private void OnDestroy()
    {
        health.OnDeath -= EndGame;
    }

    private void EndGame()
    {
        GameSceneManager.LoadNextScene();
    }
}
