using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Camera_Movement : MonoBehaviour
{
    public static Camera_Movement Instance { get; private set; }
    private GameObject player;
    public Vector3 offset = new Vector3(0, 5, -10);

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }

        Instance = this;
      
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Move_Camera_To_Player() 
    {
        if (player != null)
        {
         Vector3 currPos = player.transform.position + offset;
            float smoothSpeed = 5f;
            transform.position = Vector3.Lerp(transform.position, currPos, smoothSpeed * Time.deltaTime);
        }

    }
   
   
    void Update()
    {
        
        Move_Camera_To_Player();
    }
}
