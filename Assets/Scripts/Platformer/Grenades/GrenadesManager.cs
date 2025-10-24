using UnityEngine;

public class GrenadesManager : MonoBehaviour
{
    [SerializeField] private GameObject _grenadePrefab;
    [SerializeField] private Transform _grenadeSpawnPoint;
    [SerializeField] private float _thowForceFactor = 3f;

    private int _currentNumOfGrenades = 100; // ВРЕМЕННО
    public int CurrentNumOfGrenades => _currentNumOfGrenades;
    private Vector3 _currentMousePos => Camera.main.ScreenToWorldPoint(Input.mousePosition);
    private Vector2 _currentThrowDirection => (Vector2)(_currentMousePos - _grenadeSpawnPoint.position).normalized;

    public void ThrowGrenade()
    {
        if (_currentNumOfGrenades <= 0) return;
        _currentNumOfGrenades--;

        GameObject grenadeInstance = Instantiate(_grenadePrefab, _grenadeSpawnPoint.position, Quaternion.identity);
        Rigidbody2D tempRigidbody = grenadeInstance.GetComponent<Rigidbody2D>();

        if (tempRigidbody == null)
        {
            Debug.LogError("Компонент Rigidbody2D не найден");
            return;
        }

        tempRigidbody.AddForce(_currentThrowDirection * _thowForceFactor, ForceMode2D.Impulse);
    }
}
