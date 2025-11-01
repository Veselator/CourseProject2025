using UnityEngine;

public class WalkerAnimations : MonoBehaviour
{
    private PMovingEnemy _currentEnemy;
    private Animator _animator;

    // Кешируем переменные для оптимизации
    private static readonly int IsWalkingHash = Animator.StringToHash("IsWalking");

    private void Awake()
    {
        _currentEnemy = GetComponent<PMovingEnemy>();
        _animator = GetComponent<Animator>();

        _currentEnemy.OnStartedWalking += OnStartedWalking;
        _currentEnemy.OnPointReached += OnStoppedWalking;
    }

    private void OnDestroy()
    {
        _currentEnemy.OnStartedWalking += OnStartedWalking;
        _currentEnemy.OnPointReached += OnStoppedWalking;
    }

    private void OnStartedWalking()
    {
        _animator.SetBool(IsWalkingHash, true);
    }

    private void OnStoppedWalking()
    {
        _animator.SetBool(IsWalkingHash, false);
    }
}
