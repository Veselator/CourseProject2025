using UnityEngine;

public class PuzzlesVisibilityManager : MonoBehaviour
{
    // Пришлось удалить анимацию из-за проблес с рендерингом
    [SerializeField] private GameObject _puzzlesParentObject;

    private bool _currentVisibility = false;

    private void Start()
    {
        if (_puzzlesParentObject)
        {
            _puzzlesParentObject.SetActive(_currentVisibility);
        }
        else
        {
            Debug.LogError("Братан, я всё понимаю. Но не забудь, брат, когда добавишь паззлы сюда засунуть компонент родительский всех паззов. Не забудешь, брат? По-братски?");
        }
    }

    public void SetVisibility(bool newState)
    {
        if (_currentVisibility == newState) return;

        _currentVisibility = newState;

        if (_puzzlesParentObject != null)
        {
            _puzzlesParentObject.SetActive(newState);
        }
    }

    public void ToggleVisibility()
    {
        SetVisibility(!_currentVisibility);
    }
}