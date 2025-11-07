using System.Collections;
using UnityEngine;

public class SignalLineVisual : MonoBehaviour
{
    private LinkedManager[] _linkedManagers;

    [SerializeField] private GameObject[] _linkedWires;
    private Material[] _linkedMaterials;

    [SerializeField] private float _stateAnimationDuration = 0.3f;
    [SerializeField] private AnimationCurve _stateCurve;

    [SerializeField] private float _rotationAnimationDuration = 0.5f;
    [SerializeField] private AnimationCurve _rotationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private static readonly int currentInterpolationFactor = Shader.PropertyToID("_InterpolationValue");
    private bool _isPlayingRotationAnimation = false;

    private ObjectHighlight _highlight;

    public void Init()
    {
        _linkedManagers = GetComponent<SignalLineLinkedManagers>().LinkedManagers;
        _highlight = GetComponent<ObjectHighlight>();
        InitializeMaterials();

        foreach (var linkedManager in _linkedManagers)
        {
            linkedManager.linkedManager.OnLineRotated += Rotate;
            linkedManager.linkedManager.OnSignalStateChanged += ChangeState;
        }
    }

    private void InitializeMaterials()
    {
        int activeCount = 0;
        foreach (var wire in _linkedWires)
        {
            if (wire != null && wire.activeInHierarchy)
            {
                activeCount++;
            }
        }

        _linkedMaterials = new Material[activeCount];
        int index = 0;

        foreach (var wire in _linkedWires)
        {
            if (wire != null && wire.activeInHierarchy)
            {
                SpriteRenderer spriteRenderer = wire.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    _linkedMaterials[index] = spriteRenderer.material;
                    index++;
                }
            }
        }
    }

    private void OnDestroy()
    {
        foreach (var linkedManager in _linkedManagers)
        {
            linkedManager.linkedManager.OnLineRotated -= Rotate;
            linkedManager.linkedManager.OnSignalStateChanged -= ChangeState;
        }

        if (_linkedMaterials != null)
        {
            foreach (var material in _linkedMaterials)
            {
                if (material != null)
                {
                    Destroy(material);
                }
            }
        }
    }

    private void OnMouseEnter()
    {
        _highlight.SetHighlighted(true);
    }
    private void OnMouseExit()
    {
        _highlight.SetHighlighted(false);
    }

    private void ChangeAllMaterialsInterpolation(float newInterpolation)
    {
        foreach (var material in _linkedMaterials)
        {
            if (material != null)
            {
                material.SetFloat(currentInterpolationFactor, newInterpolation);
            }
        }
    }

    // Проверяем, может ли специфический id 
    // соотвествовать хоть какому-то из мененджеров
    // К которому мы привязаны
    private bool IsValidId(SignalsManager manager, int id)
    {
        foreach (var linkedManager in _linkedManagers)
        {
            if (linkedManager.linkedManager == manager && id == linkedManager.ID) return true;
        }
        return false;
    }

    private void Rotate(SignalsManager manager, SignalLine line, bool isClockwise)
    {
        if (_isPlayingRotationAnimation) return;

        // Проверяем, нас ли вообще вызывают
        if (!IsValidId(manager, line.ID)) return;

        StartCoroutine(RotationAnimation(isClockwise));
    }

    private void ChangeState(SignalsManager manager, SignalLine line, bool state)
    {
        if (!IsValidId(manager, line.ID)) return;

        if (state) StartCoroutine(ChangeStateCoroutine(0f, 1f));
        else StartCoroutine(ChangeStateCoroutine(1f, 0f));
    }

    private void OnDisable()
    {
        _isPlayingRotationAnimation = false;
    }

    private IEnumerator ChangeStateCoroutine(float inValue, float outValue)
    {
        float currentDuration = 0f;
        float t = 0f;

        while (currentDuration < _stateAnimationDuration)
        {
            currentDuration += Time.deltaTime;
            t = currentDuration / _stateAnimationDuration;

            float interpolatedValue = Mathf.Lerp(inValue, outValue, _stateCurve.Evaluate(t));

            ChangeAllMaterialsInterpolation(interpolatedValue);

            yield return null;
        }

        ChangeAllMaterialsInterpolation(outValue);
    }

    private IEnumerator RotationAnimation(bool isClockwise)
    {
        _isPlayingRotationAnimation = true;

        float rotationAngle = isClockwise ? -90f : 90f;

        Quaternion startRotation = transform.localRotation;
        Quaternion targetRotation = startRotation * Quaternion.Euler(0, 0, rotationAngle);

        float currentDuration = 0f;
        float t = 0f;

        while (currentDuration < _rotationAnimationDuration)
        {
            currentDuration += Time.deltaTime;
            t = currentDuration / _rotationAnimationDuration;

            float smoothT = _rotationCurve.Evaluate(t);

            transform.localRotation = Quaternion.Lerp(startRotation, targetRotation, smoothT);

            yield return null;
        }

        transform.localRotation = targetRotation;

        _isPlayingRotationAnimation = false;
    }
}