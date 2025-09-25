using System;
using System.Collections;
using UnityEngine;

public class JumpAnimator : MonoBehaviour
{
    [SerializeField] private float jumpHeight = 5f;
    [SerializeField] private float jumpDuration = 1f;
    private float maxRotationAngle = 50f;

    public bool IsJumping { get; private set; } = false;
    public Action OnJumpAnimationEnded;

    public static JumpAnimator Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void Jump()
    {
        if (IsJumping) return;
        StartCoroutine(AnimateJump());
    }

    private IEnumerator AnimateJump()
    {
        IsJumping = true;
        float startLocalY = transform.localPosition.y;
        float startLocalZ = transform.localEulerAngles.z;
        float elapsedTime = 0f;

        //Debug.Log(startLocalY);
        while (elapsedTime < jumpDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / jumpDuration;

            // Плавная парабола для прыжка (вверх и вниз)
            float heightOffset = Math.Max(4 * jumpHeight * progress * (1 - progress), 0f);

            // Обновляем локальную Y координату
            Vector3 currentLocalPos = transform.localPosition;
            currentLocalPos.y = startLocalY + heightOffset;
            transform.localPosition = currentLocalPos;

            // Поворот по Z оси (локальный)
            Vector3 currentLocalRotation = transform.localEulerAngles;
            currentLocalRotation.z = startLocalZ + (maxRotationAngle * progress * (1 - progress));
            transform.localEulerAngles = currentLocalRotation;
            //Debug.Log($"currentLocalPos = {currentLocalPos}, currentLocalRotation = {currentLocalRotation}, progress = {progress}");
            yield return null;
        }

        // Возвращаем на начальные локальные позицию и поворот
        Vector3 finalLocalPos = transform.localPosition;
        finalLocalPos.y = startLocalY;
        transform.localPosition = finalLocalPos;

        Vector3 finalLocalRotation = transform.localEulerAngles;
        finalLocalRotation.z = startLocalZ;
        transform.localEulerAngles = finalLocalRotation;

        IsJumping = false;
        OnJumpAnimationEnded?.Invoke();
    }
}