using UnityEngine;

public class LineVisualScript : MonoBehaviour
{
    // Чисто для линии босса

    [SerializeField] private Transform[] _points;

    private LineRenderer _lineRenderer;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        if (_points != null && _points.Length >= 2)
        {
            _lineRenderer.positionCount = 2;
        }
    }

    private void Update()
    {
        if (_points == null || _points.Length < 2) return;

        if (_points[0] != null)
        {
            _lineRenderer.SetPosition(0, _points[0].position);
        }

        if (_points[1] != null)
        {
            _lineRenderer.SetPosition(1, _points[1].position);
        }
    }
}
