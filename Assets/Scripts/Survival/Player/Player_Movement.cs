using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    public static Player_Movement Instance { get; private set; }
    public float speed;
    public float sprint;
    private float hor;
    private float vert;
    private Rigidbody2D rb;
    private Knockback_System ks;
    private Stamina_Sys s;
   
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

       

        rb = GetComponent<Rigidbody2D>();
        ks = GetComponent<Knockback_System>();
        s = GetComponent<Stamina_Sys>();
      
       
    }
  
    private void Movement() 
    {
        if (ks != null && ks.isKnockBack) 
        {
            return;
        }
        hor = Input.GetAxis("Horizontal");
        vert = Input.GetAxis("Vertical");
      
       
            var movement = new Vector2(hor, vert);
            if (movement.magnitude > 1)
            {
                movement.Normalize();
            }
            if (s.amountOfStamina >0&&Input.GetKey(KeyCode.LeftShift))
            {
                rb.velocity = movement * sprint;
                s.Take_Stamina(10f * Time.deltaTime);
            }
            else
            {
                rb.velocity = movement * speed;
            }
        
       
        
    }
    private void Update()
    {
        Movement();
    }
}
