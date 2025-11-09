using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationController : MonoBehaviour
{
    [SerializeField] private Animator[] _linkedAnimators;
    [SerializeField] private PlayerPlatformerHandler _linkedPlayerPlatformerHandler;
    [SerializeField] private PlayerDamageManager _damageManager;
    [SerializeField] private PlayerAbilityManager _abilityManager;
    [SerializeField] private float _runThreshold = 1f;

    private static readonly int IsWalkingHash = Animator.StringToHash("IsWalking");
    private static readonly int JumpHash = Animator.StringToHash("Jump");
    private static readonly int IsGroundedHash = Animator.StringToHash("IsGrounded");
    private static readonly int DamageHash = Animator.StringToHash("Damage");
    private static readonly int RunHash = Animator.StringToHash("Run");
    private static readonly int PunchHash = Animator.StringToHash("Punch");
    private static readonly int ThrowHash = Animator.StringToHash("Throw");

    private float _runTimer = 0f;
    private bool _isWalking = false;
    private bool _isRunTriggered = false;

    private void OnEnable()
    {
        AnimatorCheck.CacheAnimators(_linkedAnimators);

        if (_linkedPlayerPlatformerHandler != null)
        {
            _linkedPlayerPlatformerHandler.OnPlayerJumped += HandleJump;
            _linkedPlayerPlatformerHandler.OnPlayerWalking += HandleWalking;
            _linkedPlayerPlatformerHandler.OnPlayerDoesntWalking += HandleNotWalking;
            _linkedPlayerPlatformerHandler.OnPlayerGrounded += HandleGrounded;
            _linkedPlayerPlatformerHandler.OnPlayerDegrounded += HandleDegrounded;
        }

        if (_abilityManager != null)
        {
            _abilityManager.OnAbilityApplied += HoldAbilityAnimation;
        }

        if (_damageManager == null)
        {
            _damageManager.OnPlayerDamaged += HandleDamage;
        }

        if (_linkedPlayerPlatformerHandler.IsGrounded) HandleGrounded();
        else HandleDegrounded();
    }

    private void OnDisable()
    {
        if (_linkedPlayerPlatformerHandler != null)
        {
            _linkedPlayerPlatformerHandler.OnPlayerJumped -= HandleJump;
            _linkedPlayerPlatformerHandler.OnPlayerWalking -= HandleWalking;
            _linkedPlayerPlatformerHandler.OnPlayerDoesntWalking -= HandleNotWalking;
            _linkedPlayerPlatformerHandler.OnPlayerGrounded -= HandleGrounded;
            _linkedPlayerPlatformerHandler.OnPlayerDegrounded -= HandleDegrounded;
        }

        //if (_abilityManager != null)
        //{
        //    _abilityManager.OnAbilityApplied -= HoldAbilityAnimation;
        //}

        if (_damageManager == null)
        {
            _damageManager.OnPlayerDamaged -= HandleDamage;
        }
    }

    private void Update()
    {
        if (_isWalking)
        {
            _runTimer += Time.deltaTime;

            if (_runTimer >= _runThreshold && !_isRunTriggered)
            {
                _isRunTriggered = true;
                Run();
            }
        }
    }

    private void ApplyTrigger(int property)
    {
        if (!gameObject.activeInHierarchy) return;

        foreach (var animator in _linkedAnimators)
        {
            if (animator != null && AnimatorCheck.HasParameter(animator, property))
            {
                animator.SetTrigger(property);
            }
        }
    }

    // Нет
    private void HoldAbilityAnimation(IAbility ability)
    {
        if (!gameObject.activeInHierarchy) return;

        if (ability.Type == AbilityType.Mechanic) ApplyTrigger(ThrowHash);
        else if (ability.Type == AbilityType.StrongPunch) ApplyTrigger(PunchHash);
    }

    private void Run()
    {
        if (!gameObject.activeInHierarchy) return;

        foreach (var animator in _linkedAnimators)
        {
            if (animator != null && AnimatorCheck.HasParameter(animator, RunHash))
            {
                animator.SetTrigger(RunHash);
            }
        }
    }

    private void HandleJump(float _)
    {
        if (!gameObject.activeInHierarchy) return;

        foreach (var animator in _linkedAnimators)
        {
            if (animator != null && AnimatorCheck.HasParameter(animator, JumpHash))
            {
                animator.SetTrigger(JumpHash);
            }
        }
    }

    private void HandleDamage()
    {
        if (!gameObject.activeInHierarchy) return;

        foreach (var animator in _linkedAnimators)
        {
            if (animator != null && AnimatorCheck.HasParameter(animator, DamageHash))
            {
                animator.SetTrigger(DamageHash);
            }
        }
    }

    private void HandleWalking()
    {
        _isWalking = true;

        if (!gameObject.activeInHierarchy) return;
        foreach (var animator in _linkedAnimators)
        {
            if (animator != null && AnimatorCheck.HasParameter(animator, IsWalkingHash))
            {
                animator.SetBool(IsWalkingHash, true);
            }
        }
    }

    private void HandleNotWalking()
    {
        _isWalking = false;
        _runTimer = 0f;
        _isRunTriggered = false;

        if (!gameObject.activeInHierarchy) return;
        foreach (var animator in _linkedAnimators)
        {
            if (animator != null && AnimatorCheck.HasParameter(animator, IsWalkingHash))
            {
                animator.SetBool(IsWalkingHash, false);
            }
        }
    }

    private void HandleDegrounded()
    {
        if (!gameObject.activeInHierarchy) return;
        foreach (var animator in _linkedAnimators)
        {
            if (animator != null && AnimatorCheck.HasParameter(animator, IsGroundedHash))
            {
                animator.SetBool(IsGroundedHash, false);
            }
        }
    }

    private void HandleGrounded()
    {
        if (!gameObject.activeInHierarchy) return;
        foreach (var animator in _linkedAnimators)
        {
            if (animator != null && AnimatorCheck.HasParameter(animator, IsGroundedHash))
            {
                animator.SetBool(IsGroundedHash, true);
            }
        }
    }
}

public static class AnimatorCheck
{
    private static Dictionary<Animator, HashSet<int>> _cachedParameters = new Dictionary<Animator, HashSet<int>>();

    public static void CacheAnimators(Animator[] animators)
    {
        foreach (var animator in animators)
        {
            if (animator != null && animator.runtimeAnimatorController != null)
            {
                var paramSet = new HashSet<int>();
                foreach (var param in animator.parameters)
                {
                    paramSet.Add(param.nameHash);
                }
                _cachedParameters[animator] = paramSet;
            }
        }
    }

    public static bool HasParameter(this Animator animator, int parameterHash)
    {
        return _cachedParameters.TryGetValue(animator, out var paramSet) && paramSet.Contains(parameterHash);
    }
}