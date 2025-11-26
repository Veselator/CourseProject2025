using UnityEngine;

public class CursorManager : MonoBehaviour
{
    // Мененджер состояния курсора

    public static CursorManager Instance { get; private set; }
    private bool _isDynamicCursor = true;
    public bool IsDynamicCursor
    {
        get { return _isDynamicCursor; }
        set 
        { 
            _isDynamicCursor = value;
            OnCursorTypeChanged(value);
        }
    }

    // Ссылки на скрипты курсора
    [SerializeField] private CursorRotation _cursorRotation;
    [SerializeField] private CursorClickAnimation _cursorClickAnimation;

    private void Awake()
    {
        Instance = this;
    }

    public void ToggleDynamicCursor()
    {
        IsDynamicCursor = !IsDynamicCursor;
    }

    private void OnCursorTypeChanged(bool newState)
    {
        if (newState)
        {
            _cursorRotation.Resume();
            _cursorClickAnimation.Resume();
        }
        else
        {
            _cursorRotation.StopAndReset();
            _cursorClickAnimation.StopAndReset();
        }
    }
}
