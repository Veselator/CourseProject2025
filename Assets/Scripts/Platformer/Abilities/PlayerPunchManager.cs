using System.Drawing;
using UnityEngine;

public class PlayerPunchManager : MonoBehaviour
{
    [SerializeField] private Transform _punchTransform;
    [SerializeField] private Vector2 _punchRectangle;
    [SerializeField] private LayerMask _layerPossible2Hit;
    [SerializeField] private float _punchStrength = 2f;
    [SerializeField] private Rigidbody2D _trackingRigidbody;

    private void OnDrawGizmos()
    {
        if (_punchTransform == null) return;
        Gizmos.color = UnityEngine.Color.red;
        Gizmos.DrawWireCube(_punchTransform.position, _punchRectangle);

        Gizmos.color = UnityEngine.Color.yellow;
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
            Rigidbody2D tempRigidbody = collider.GetComponent<Rigidbody2D>();
            if (!tempRigidbody) return;

            tempRigidbody.AddForce(punchDirection * _punchStrength, ForceMode2D.Impulse);
        }
    }

    private Vector2 GetPunchDirection()
    {
        float scaleSign = Mathf.Sign(_punchTransform.lossyScale.x);

        Vector2 direction = _punchTransform.right;

        return direction * scaleSign;
    }
}