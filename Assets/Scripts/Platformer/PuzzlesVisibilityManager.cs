using UnityEngine;

public class PuzzlesVisibilityManager : MonoBehaviour
{
    // Пришлось удалить анимацию из-за проблес с рендерингом
    [SerializeField] private GameObject[] _puzzlesParentObjects;

    private bool _currentVisibility = false;
    private bool _isLoadedFailed = false;

    private void Start()
    {
        if(_puzzlesParentObjects == null || _puzzlesParentObjects.Length == 0)
        {
            Debug.LogError("Братан, я всё понимаю. Но не забудь, брат, когда добавишь паззлы сюда засунуть компонент родительский всех паззов. Не забудешь, брат? По-братски?");
            _isLoadedFailed = true;
            return;
        }

        foreach(GameObject puzzle in _puzzlesParentObjects)
        {
            puzzle.SetActive(_currentVisibility);
        }
    }

    public void SetVisibility(bool newState)
    {
        if (_currentVisibility == newState) return;

        _currentVisibility = newState;

        if (_isLoadedFailed) return;

        foreach (GameObject puzzle in _puzzlesParentObjects)
        {
            puzzle.SetActive(_currentVisibility);
        }
    }

    public void ToggleVisibility()
    {
        SetVisibility(!_currentVisibility);
    }
}