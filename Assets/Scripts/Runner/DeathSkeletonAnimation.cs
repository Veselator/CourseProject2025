using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathSkeletonAnimation : MonoBehaviour
{
    private UIAppearManager uiAppearManager;
    [SerializeField] private Animator _animator;

    private void Start()
    {
        uiAppearManager = UIAppearManager.Instance;
        uiAppearManager.OnAppearAnimationEnded += TriggerAnimation;
    }

    private void OnDestroy()
    {
        uiAppearManager.OnAppearAnimationEnded -= TriggerAnimation;
    }

    private void TriggerAnimation()
    {
        _animator.SetTrigger("TriggerDeathAnim");
    }
}
