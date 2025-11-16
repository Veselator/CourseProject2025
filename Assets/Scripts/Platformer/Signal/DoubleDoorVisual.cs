using System.Collections;
using UnityEngine;

public class DoubleDoorVisual : MonoBehaviour
{
    [SerializeField] private Door _linkedDoor;
    [SerializeField] private Transform[] _doorParts;
    private Vector3[] _doorStartPoints;
    [SerializeField] private Transform[] _doorOpenPoints;

    [SerializeField] private float _animationDuration = 0.5f;
    [SerializeField] private AnimationCurve _animationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private Coroutine _currentAnimation;

    private void Awake()
    {
        InitializeStartPoints();
        _linkedDoor.OnStateChanged += HoldDoorStateChange;
    }

    private void OnDestroy()
    {
        _linkedDoor.OnStateChanged -= HoldDoorStateChange;
    }

    private void InitializeStartPoints()
    {
        _doorStartPoints = new Vector3[_doorParts.Length];

        for (int i = 0; i < _doorParts.Length; i++)
        {
            if (_doorParts[i] != null)
            {
                _doorStartPoints[i] = _doorParts[i].position;
            }
        }
    }

    private void HoldDoorStateChange(bool newState, bool _)
    {
        if (_currentAnimation != null)
        {
            StopCoroutine(_currentAnimation);
        }

        if (newState)
        {
            _currentAnimation = StartCoroutine(AnimateDoor(true));
        }
        else
        {
            _currentAnimation = StartCoroutine(AnimateDoor(false));
        }
    }

    private IEnumerator AnimateDoor(bool isOpening)
    {
        float currentDuration = 0f;

        Vector3[] startPositions = new Vector3[_doorParts.Length];
        Vector3[] targetPositions = new Vector3[_doorParts.Length];

        for (int i = 0; i < _doorParts.Length; i++)
        {
            if (_doorParts[i] != null)
            {
                startPositions[i] = _doorParts[i].position;
                targetPositions[i] = isOpening ? _doorOpenPoints[i].position : _doorStartPoints[i];
            }
        }

        while (currentDuration < _animationDuration)
        {
            currentDuration += Time.deltaTime;
            float t = currentDuration / _animationDuration;
            float smoothT = _animationCurve.Evaluate(t);

            for (int i = 0; i < _doorParts.Length; i++)
            {
                if (_doorParts[i] != null)
                {
                    _doorParts[i].position = Vector3.Lerp(startPositions[i], targetPositions[i], smoothT);
                }
            }

            yield return null;
        }

        for (int i = 0; i < _doorParts.Length; i++)
        {
            if (_doorParts[i] != null)
            {
                _doorParts[i].position = targetPositions[i];
            }
        }

        _currentAnimation = null;
    }
}