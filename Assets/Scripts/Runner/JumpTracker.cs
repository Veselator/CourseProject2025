using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpTracker : MonoBehaviour
{
    public bool IsReadyToJump { get; private set; } = false;
    [SerializeField] private float cooldownTime = 6f;
    private float timer = 0f;

    public float ProgressToNextJump => Math.Clamp(timer / cooldownTime, 0f, 1f);

    public static JumpTracker Instance { get; private set; }
    public JumpAnimator jumpAnimator;
    private PlayerHealth playerHealth;

    public Action OnReadyToJump;
    public Action OnJumpAnimationStarted;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        transform.GetComponent<PlayerInputHandler>().OnJumpRequested += TryToJump;
    }

    private void Start()
    {
        playerHealth = PlayerHealth.Instance;
        jumpAnimator = JumpAnimator.Instance;

        jumpAnimator.OnJumpAnimationEnded += EndJump;
    }

    private void OnDestroy()
    {
        jumpAnimator.OnJumpAnimationEnded -= EndJump;
    }

    private void TryToJump()
    {
        if (!IsReadyToJump) return;

        ResetJump();
        GlobalFlags.SetFlag(GlobalFlags.Flags.BLOCK_PLAYER_MOVING);
        OnJumpAnimationStarted.Invoke();
        playerHealth.IsPossibleToHitPlayer = false;
        jumpAnimator.Jump();
    }

    private void EndJump()
    {
        // Вызывается когда прыжок закончился
        playerHealth.IsPossibleToHitPlayer = true;
        GlobalFlags.ClearFlag(GlobalFlags.Flags.BLOCK_PLAYER_MOVING);
    }

    public void ResetJump()
    {
        IsReadyToJump = false;
        timer = 0f;
    }

    private void Update()
    {
        if (!IsReadyToJump)
        {
            timer += Time.deltaTime;
            if(timer >= cooldownTime)
            {
                IsReadyToJump = true;
                OnReadyToJump?.Invoke();
                //timer = 0f;
            }
        }
    }
}
