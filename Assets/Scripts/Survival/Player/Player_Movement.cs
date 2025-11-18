using System;
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

    public float CurrentSpeed;

    public bool IsMoving { get; private set; } = false;
    public bool IsSprinting { get; private set; } = false;
    public event Action OnMovingStarted;
    public event Action OnMovingEnded;
    public event Action OnSprintingStarted;
    public event Action OnSprintingEnded;

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
        bool currentRunning = false, currentMoving = false;

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
        CurrentSpeed = speed;
        if(CurrentSpeed != 0f) currentMoving = true;

        if (s.amountOfStamina > 0 && Input.GetKey(KeyCode.LeftShift) && currentMoving)
        {
            CurrentSpeed *= sprintFactor;
            //rb.MovePosition(movement * sprint);//rb.velocity = movement * sprint;
            s.Take_Stamina(10f * Time.deltaTime);
            currentRunning = true;
        }

        rb.velocity = CurrentSpeed * movement * Time.fixedDeltaTime; //MovePosition(rb.position + currentSpeed * Time.deltaTime * movement); //rb.velocity = movement * sprint;

        if (IsMoving && !currentMoving)
        {
            // До этого двигался, а сейчас нет
            OnMovingEnded?.Invoke();
        }
        else if (!IsMoving && currentMoving)
        {
            // До этого стоял, а сейчас двинулся
            OnMovingStarted?.Invoke();
        }

        if(IsSprinting && !currentRunning)
        {
            // До этого бежал, а сейчас нет
            OnSprintingEnded?.Invoke();
        }
        else if (!IsSprinting && currentRunning)
        {
            // До этого не бежал, а сейчас бежит
            OnSprintingStarted?.Invoke();
        }

        IsMoving = currentMoving;
        IsSprinting = currentRunning;
    }

    private void FixedUpdate()
    {
        Movement();
    }
}
