using UnityEngine;

public class PuzzleVisibleOnCharacterChanged : MonoBehaviour
{
    [SerializeField] private PlayerChangerManager _changerManager;
    [SerializeField] private PuzzlesVisibilityManager _visibilityManager;

    private void Start()
    {
        _changerManager.OnCharacterChanged += ChangeVisibility;
    }

    private void OnDestroy()
    {
        _changerManager.OnCharacterChanged -= ChangeVisibility;
    }

    private void ChangeVisibility(int id)
    {
        // Если Борис - отображаем
        if (id == 1) _visibilityManager.SetVisibility(true);
        // Иначе - нет
        else _visibilityManager.SetVisibility(false);
    }
}
