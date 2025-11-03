using UnityEngine;

public class PlayerPunchManager : MonoBehaviour
{
    [SerializeField] private Transform _punchTransform;
    [SerializeField] private Vector2 _punchRectangle;
    [SerializeField] private LayerMask _layerPossible2Hit;
    [SerializeField] private float _punchStrength = 2f;
    [SerializeField] private Rigidbody2D _trackingRigidbody;
    [SerializeField] private Damage _punchDamage;

    private void OnDrawGizmos()
    {
        if (_punchTransform == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_punchTransform.position, _punchRectangle);

        Gizmos.color = Color.yellow;
        Vector2 punchDirection = GetPunchDirection();
        Gizmos.DrawRay(_punchTransform.position, punchDirection * 2f);
    }

    public void Punch()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(_punchTransform.position, _punchRectangle, 0f, _layerPossible2Hit);
        if (colliders.Length == 0) return;

        Vector2 punchDirection = GetPunchDirection();
        foreach (Collider2D collider in colliders)
        {
            // Получаем необходимые компоненты
            Rigidbody2D tempRigidbody = collider.GetComponent<Rigidbody2D>();
            IHealth tempHealth = collider.GetComponent<IHealth>();

            // Какие из них не null - с такими и работает
            tempRigidbody?.AddForce(punchDirection * _punchStrength, ForceMode2D.Impulse);
            tempHealth?.TakeDamage(_punchDamage);
        }
    }

    private Vector2 GetPunchDirection()
    {
        float scaleSign = Mathf.Sign(_punchTransform.lossyScale.x);

        Vector2 direction = _punchTransform.right;

        return direction * scaleSign;
    }
}