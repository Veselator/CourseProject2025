using UnityEngine;

public class MusicBasedOnLevelManager : MonoBehaviour
{
    // Меняет музыку при смене сцены

    private GameAudioManager _audioManager;
    private GameSceneManager _sceneManager;
    [SerializeField] private string[] _music;

    private void Start()
    {
        _sceneManager = GameSceneManager.Instance;
        _audioManager = GameAudioManager.Instance;

        _sceneManager.OnLoadingStarted += HandleMusicChange;

        HandleMusicChange(-1); // меню
    }

    private void OnDestroy()
    {
        if (_sceneManager == null) return;
        _sceneManager.OnLoadingStarted -= HandleMusicChange;
    }

    private void HandleMusicChange(int id, bool _ = true)
    {
        // -1 - меню
        if (id < -1 || id >= _music.Length) return;
        _audioManager.PlayMusic(_music[id + 1]);
    }
}
