using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class KillCounter : MonoBehaviour
{
  public static KillCounter Instance { get; private set; }
    public int count = 0;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    }
