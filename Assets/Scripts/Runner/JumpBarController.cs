using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JumpBarController : MonoBehaviour
{
    [SerializeField] private Image jumpBarSlider;
    [SerializeField] private GameObject imageIsReadyToJump;
    private float linearInterpolationFactor = 0.5f;
    JumpTracker jumpTracker;

    private void Start()
    {
        jumpTracker = JumpTracker.Instance;
        jumpBarSlider.fillAmount = 0f;
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
        jumpBarSlider.fillAmount = Mathf.Lerp(jumpBarSlider.fillAmount, jumpTracker.ProgressToNextJump, linearInterpolationFactor);
    }

}
