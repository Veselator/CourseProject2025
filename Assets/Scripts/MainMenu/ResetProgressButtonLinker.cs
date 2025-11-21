using UnityEngine;
using UnityEngine.UI;

public class ResetProgressButtonLinker : MonoBehaviour
{
    [SerializeField] private Button _restartButton;

    private void Start()
    {
        _restartButton.onClick.AddListener(ReloadProgress);
    }

    private void OnDestroy()
    {
        _restartButton.onClick.RemoveListener(ReloadProgress);
    }

    private void ReloadProgress()
    {
        GameSaveManager.Instance.CleanUp();
        GameSceneManager.ReloadScene();
    }
}
