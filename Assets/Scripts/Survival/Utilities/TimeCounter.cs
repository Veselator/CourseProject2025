using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeCounter : MonoBehaviour
{
  public static TimeCounter Instance { get; private set; }
    public float TimeCount = 30f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }

        Instance = this;
    }
    private void Update()
    {
        
            TimeCount -= Time.deltaTime;
            TimeCount = Mathf.Max(TimeCount, 0f);
        
    }
}
