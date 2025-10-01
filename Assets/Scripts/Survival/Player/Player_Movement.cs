using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
public class Player_Movement : MonoBehaviour
{
    public static Player_Movement Instance { get; private set; }
    public float speed;
    public float sprintFactor;
    private float hor;
    private float vert;
    private Rigidbody2D rb;
    private Knockback_System ks;
    private Stamina_Sys s;


    private void Awake()
    {
        if (Instance != null)
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
        float currentSpeed = 0f;

        var movement = new Vector2(hor, vert);
        if (movement.magnitude > 1)
        {
            movement.Normalize();
        }
        currentSpeed = speed;

        if (s.amountOfStamina > 0 && Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed *= sprintFactor;
            //rb.MovePosition(movement * sprint);//rb.velocity = movement * sprint;
            s.Take_Stamina(10f * Time.deltaTime);
        }

        rb.velocity = currentSpeed * movement * Time.fixedDeltaTime;//MovePosition(rb.position + currentSpeed * Time.deltaTime * movement); //rb.velocity = movement * sprint;
    }
    private void FixedUpdate()
    {
        Movement();
    }
}
