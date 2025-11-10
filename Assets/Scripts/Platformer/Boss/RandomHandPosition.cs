using UnityEngine;

public class RandomHandPosition : MonoBehaviour
{
    [SerializeField] private float _size = 1f;
    [SerializeField] private float _moveSpeed = 2f;
    [SerializeField] private float _minChangeInterval = 1f;
    [SerializeField] private float _maxChangeInterval = 3f;

    private Vector3 _centerPosition;
    private Vector3 _targetPosition;
    private float _timer = 0f;
    private float _currentInterval;

    private void Start()
    {
        _centerPosition = transform.localPosition;
        SetRandomTarget();
        _currentInterval = Random.Range(_minChangeInterval, _maxChangeInterval);
    }

    private void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= _currentInterval)
        {
            SetRandomTarget();
            _timer = 0f;
            _currentInterval = Random.Range(_minChangeInterval, _maxChangeInterval);
        }

        transform.localPosition = Vector3.Lerp(transform.localPosition, _targetPosition, Time.deltaTime * _moveSpeed);
    }

    private void SetRandomTarget()
    {
        float halfSize = _size * 0.5f;
        float randomX = Random.Range(-halfSize, halfSize);
        float randomY = Random.Range(-halfSize, halfSize);

        _targetPosition = _centerPosition + new Vector3(randomX, randomY, 0f);
    }
}