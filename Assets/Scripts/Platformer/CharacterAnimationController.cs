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

        if (_abilityManager != null)
        {
            _abilityManager.OnAbilityApplied -= HoldAbilityAnimation;
        }

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
        foreach (var animator in _linkedAnimators)
        {
            if (animator != null)
            {
                animator.SetTrigger(property);
            }
        }
    }

    private void HoldAbilityAnimation(IAbility ability)
    {
        if (ability.Type == AbilityType.Mechanic) ApplyTrigger(ThrowHash);
        else if (ability.Type == AbilityType.StrongPunch) ApplyTrigger(PunchHash);
    }

    private void Run()
    {
        foreach (var animator in _linkedAnimators)
        {
            if (animator != null)
            {
                animator.SetTrigger(RunHash);
            }
        }
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

    private void HandleDamage()
    {
        foreach (var animator in _linkedAnimators)
        {
            if (animator != null)
            {
                animator.SetTrigger(DamageHash);
            }
        }
    }

    private void HandleWalking()
    {
        _isWalking = true;

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
        _isWalking = false;
        _runTimer = 0f;
        _isRunTriggered = false;

        foreach (var animator in _linkedAnimators)
        {
            if (animator != null)
            {
                animator.SetBool(IsWalkingHash, false);
            }
        }
    }

    private void HandleDegrounded()
    {
        foreach (var animator in _linkedAnimators)
        {
            if (animator != null)
            {
                animator.SetBool(IsGroundedHash, false);
            }
        }
    }

    private void HandleGrounded()
    {
        foreach (var animator in _linkedAnimators)
        {
            if (animator != null)
            {
                animator.SetBool(IsGroundedHash, true);
            }
        }
    }
}