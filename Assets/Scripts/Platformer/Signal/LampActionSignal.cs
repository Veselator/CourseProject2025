using UnityEngine;

public class LampActionSignal : MonoBehaviour, ISignalAction
{
    private bool _isExecuted = false;
    public bool IsExecuted => _isExecuted;
    [SerializeField] private SpriteRenderer _linkedSprite;
    [SerializeField] private Color _activatedColor = Color.white;
    [SerializeField] private Color _unactivatedColor = Color.gray;

    private void Start()
    {
        _linkedSprite.color = _unactivatedColor;
    }

    public void Execute()
    {
        _linkedSprite.color = _activatedColor;
        _isExecuted = true;
    }

    public void Undo()
    {
        _linkedSprite.color = _unactivatedColor;
        _isExecuted = false;
    }
}
