using System.Collections;
using UnityEngine;

public class DamageReceivingAnimation : MonoBehaviour
{
    [Header("Настройки анимации")]
    [SerializeField] private GameObject[] _damageObjects;
    [SerializeField] private float _animationDuration = 0.5f;
    [SerializeField] private float _flickerSpeed = 0.1f;
    [SerializeField] private Health _linkedHealth;

    private MaterialPropertyBlock _propBlock;
    private static readonly int ColorInterpolationValueID = Shader.PropertyToID("_ColorInterpolationValue");

    private Coroutine _currentAnimation;

    private void Awake()
    {
        _propBlock = new MaterialPropertyBlock();
    }

    private void Start()
    {
        _linkedHealth.OnDamaged += ShowAnimation;
    }

    private void OnDestroy()
    {
        _linkedHealth.OnDamaged -= ShowAnimation;
    }

    public void ShowAnimation()
    {
        if (_currentAnimation != null)
        {
            StopCoroutine(_currentAnimation);
        }

        _currentAnimation = StartCoroutine(AnimationRoutine());
    }

    private IEnumerator AnimationRoutine()
    {
        float elapsed = 0f;

        while (elapsed < _animationDuration)
        {
            SetColorInterpolationValue(0.99f);
            yield return new WaitForSeconds(_flickerSpeed);

            SetColorInterpolationValue(0f);
            yield return new WaitForSeconds(_flickerSpeed);

            elapsed += _flickerSpeed * 2f;
        }

        SetColorInterpolationValue(0f);

        _currentAnimation = null;
    }

    private void SetColorInterpolationValue(float value)
    {
        foreach (var obj in _damageObjects)
        {
            if (obj == null) continue;

            var spriteRenderer = obj.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.GetPropertyBlock(_propBlock);
                _propBlock.SetFloat(ColorInterpolationValueID, value);
                spriteRenderer.SetPropertyBlock(_propBlock);
            }
        }
    }

    public void ResetAnimation()
    {
        if (_currentAnimation != null)
        {
            StopCoroutine(_currentAnimation);
            _currentAnimation = null;
        }

        SetColorInterpolationValue(0f);
    }
}