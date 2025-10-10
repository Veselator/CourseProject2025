using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedOfText : MonoBehaviour
{
    [SerializeField] public float speedOfTextFly = 500f;
    [SerializeField] private float speedUpForCorrect = 50f;

    public static SpeedOfText Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this; 
        }
        else
        {
            Destroy(gameObject); 
        }

    }

    public void SpeedUp() 
    {
        speedOfTextFly += speedUpForCorrect;
    }
}
