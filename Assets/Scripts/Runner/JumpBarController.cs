using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JumpBarController : MonoBehaviour
{
    [SerializeField] private Slider jumpBarSlider;
    [SerializeField] private GameObject imageIsReadyToJump;
    private float linearInterpolationFactor = 0.5f;
    JumpTracker jumpTracker;

    private void Start()
    {
        jumpTracker = JumpTracker.Instance;
        jumpBarSlider.value = 0f;
        jumpTracker.OnReadyToJump += ShowReadyToJumpIcon;
        jumpTracker.OnJumpAnimationStarted += HideReadyToJumpIcon;
    }

    private void OnDestroy()
    {
        jumpTracker.OnReadyToJump -= ShowReadyToJumpIcon;
        jumpTracker.OnJumpAnimationStarted -= HideReadyToJumpIcon;
    }

    private void ShowReadyToJumpIcon()
    {
        imageIsReadyToJump.SetActive(true);
    }

    private void HideReadyToJumpIcon()
    {
        imageIsReadyToJump.SetActive(false);
    }

    private void Update()
    {
        jumpBarSlider.value = Mathf.Lerp(jumpBarSlider.value, jumpTracker.ProgressToNextJump, linearInterpolationFactor);
    }

}
