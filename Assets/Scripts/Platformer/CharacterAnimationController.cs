using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationController : MonoBehaviour
{
    [SerializeField] private Animator[] _linkedAnimators;
    [SerializeField] private PlayerPlatformerHandler _linkedPlayerPlatformerHandler;

    private static readonly int IsWalkingHash = Animator.StringToHash("IsWalking");
    private static readonly int JumpHash = Animator.StringToHash("Jump");
    private static readonly int GroundedHash = Animator.StringToHash("Grounded");

    private void OnEnable()
    {
        if (_linkedPlayerPlatformerHandler == null) return;

        _linkedPlayerPlatformerHandler.OnPlayerJumped += HandleJump;
        _linkedPlayerPlatformerHandler.OnPlayerWalking += HandleWalking;
        _linkedPlayerPlatformerHandler.OnPlayerDoesntWalking += HandleNotWalking;
        _linkedPlayerPlatformerHandler.OnPlayerGrounded += HandleGrounded;
    }

    private void OnDisable()
    {
        if (_linkedPlayerPlatformerHandler == null) return;

        _linkedPlayerPlatformerHandler.OnPlayerJumped -= HandleJump;
        _linkedPlayerPlatformerHandler.OnPlayerWalking -= HandleWalking;
        _linkedPlayerPlatformerHandler.OnPlayerDoesntWalking -= HandleNotWalking;
        _linkedPlayerPlatformerHandler.OnPlayerGrounded -= HandleGrounded;
    }

    private void HandleJump()
    {
        foreach (var animator in _linkedAnimators)
        {
            if (animator != null)
            {
                animator.SetTrigger(JumpHash);
            }
        }
    }

    private void HandleWalking()
    {
        foreach (var animator in _linkedAnimators)
        {
            if (animator != null)
            {
                animator.SetBool(IsWalkingHash, true);
            }
        }
    }

    private void HandleNotWalking()
    {
        foreach (var animator in _linkedAnimators)
        {
            if (animator != null)
            {
                animator.SetBool(IsWalkingHash, false);
            }
        }
    }

    private void HandleGrounded()
    {
        foreach (var animator in _linkedAnimators)
        {
            if (animator != null)
            {
                animator.SetTrigger(GroundedHash);
            }
        }
    }
}