using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAudioManager : MonoBehaviour
{
    // Мененджер аудио
    // Музыка, эффекты, диалоги, UI

    private static GameAudioManager _instance;
    public static GameAudioManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("GameAudioManager не найден в сцене!");
            }
            return _instance;
        }
    }

    [Header("Аудио библиотека")]
    [SerializeField] private AudioClipLibrary _audioLibrary;

    [Header("Каналы")]
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _musicSourceCrossfade;
    [SerializeField] private AudioSource _dialogueSource;
    [SerializeField] private AudioSource _ambientSource;
    [SerializeField] private int _sfxPoolSize = 10;

    [Header("Настройка стандартной высоты тона")]
    [SerializeField] private float _defaultPitch = 1f;

    [Header("Настройки громкости")]
    [SerializeField][Range(0f, 1f)] private float _masterVolume = 1f;
    [SerializeField][Range(0f, 1f)] private float _musicVolume = 1f;
    [SerializeField][Range(0f, 1f)] private float _sfxVolume = 1f;
    [SerializeField][Range(0f, 1f)] private float _dialogueVolume = 1f;

    [Header("Настройки кроссфейда")]
    [SerializeField] private float _defaultCrossfadeTime = 1f;

    [Header("Защита от спама")]
    [SerializeField] private float _duplicateSoundDelay = 0.1f;
    [SerializeField] private int _maxDuplicateSounds = 3;

    private AudioSource[] _sfxPool;
    private int _currentSfxIndex = 0;
    private Coroutine _crossfadeCoroutine;
    private bool _isCrossfading = false;

    private Dictionary<string, float> _lastPlayTime = new Dictionary<string, float>();
    private Dictionary<string, int> _currentlyPlayingCount = new Dictionary<string, int>();
    private Dictionary<string, AudioSource> _loopingSounds = new Dictionary<string, AudioSource>();

    private const string MASTER_VOLUME_KEY = "MasterVolume";
    private const string MUSIC_VOLUME_KEY = "MusicVolume";
    private const string SFX_VOLUME_KEY = "SFXVolume";
    private const string DIALOGUE_VOLUME_KEY = "DialogueVolume";

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        InitializeAudioSources();
        LoadVolumeSettings();

        Debug.Log("GameAudioManager инициализирован");
    }

    private void InitializeAudioSources()
    {
        if (_musicSource == null)
        {
            GameObject musicObj = new GameObject("MusicSource");
            musicObj.transform.SetParent(transform);
            _musicSource = musicObj.AddComponent<AudioSource>();
            _musicSource.loop = true;
            _musicSource.playOnAwake = false;
            _musicSource.spatialBlend = 0f;
        }

        if (_ambientSource == null)
        {
            GameObject ambientObj = new GameObject("AmbientSource");
            ambientObj.transform.SetParent(transform);
            _ambientSource = ambientObj.AddComponent<AudioSource>();
            _ambientSource.loop = true;
            _ambientSource.playOnAwake = false;
            _ambientSource.spatialBlend = 0f;
        }

        if (_musicSourceCrossfade == null)
        {
            GameObject crossfadeObj = new GameObject("MusicSourceCrossfade");
            crossfadeObj.transform.SetParent(transform);
            _musicSourceCrossfade = crossfadeObj.AddComponent<AudioSource>();
            _musicSourceCrossfade.loop = true;
            _musicSourceCrossfade.playOnAwake = false;
            _musicSourceCrossfade.spatialBlend = 0f;
        }

        if (_dialogueSource == null)
        {
            GameObject dialogueObj = new GameObject("DialogueSource");
            dialogueObj.transform.SetParent(transform);
            _dialogueSource = dialogueObj.AddComponent<AudioSource>();
            _dialogueSource.playOnAwake = false;
            _dialogueSource.spatialBlend = 0f;
        }

        _sfxPool = new AudioSource[_sfxPoolSize];
        for (int i = 0; i < _sfxPoolSize; i++)
        {
            GameObject sfxObj = new GameObject($"SFXSource_{i}");
            sfxObj.transform.SetParent(transform);
            _sfxPool[i] = sfxObj.AddComponent<AudioSource>();
            _sfxPool[i].playOnAwake = false;
            _sfxPool[i].spatialBlend = 0f;
        }
    }

    public void PlaySound(string soundName, float volumeMultiplier = 1f)
    {
        if (_audioLibrary == null)
        {
            Debug.LogError("AudioClipLibrary не назначена!");
            return;
        }

        if (!CanPlaySound(soundName))
        {
            return;
        }

        AudioClipLibrary.AudioEntry entry = _audioLibrary.GetEntry(soundName);
        if (entry == null)
        {
            Debug.LogWarning($"Звук '{soundName}' не найден в библиотеке!");
            return;
        }

        AudioClip clipToPlay = GetAudioClip(entry);

        switch (entry.category)
        {
            case AudioCategory.SFX:
            case AudioCategory.UI:
                PlaySFX(clipToPlay, entry.volume * volumeMultiplier, soundName);
                break;
            case AudioCategory.Music:
                PlayMusic(clipToPlay, entry.volume * volumeMultiplier);
                break;
            case AudioCategory.Dialogue:
                PlayDialogue(clipToPlay, entry.volume * volumeMultiplier);
                break;
            case AudioCategory.Ambient:
                PlayAmbient(soundName, volumeMultiplier);
                break;
        }
    }

    public void PlaySFXWithRandomPitch(string soundName, float minPitch, float maxPitch, float volumeMultiplier = 1f)
    {
        if (_audioLibrary == null)
        {
            Debug.LogError("AudioClipLibrary не назначена!");
            return;
        }

        if (!CanPlaySound(soundName))
        {
            return;
        }

        AudioClipLibrary.AudioEntry entry = _audioLibrary.GetEntry(soundName);
        if (entry == null)
        {
            Debug.LogWarning($"Звук '{soundName}' не найден в библиотеке!");
            return;
        }

        AudioClip clipToPlay = GetAudioClip(entry);

        if (clipToPlay == null) return;

        AudioSource source = GetAvailableSFXSource();
        source.clip = clipToPlay;
        source.volume = _masterVolume * _sfxVolume * entry.volume * volumeMultiplier;
        source.pitch = UnityEngine.Random.Range(minPitch, maxPitch);
        source.loop = false;
        source.Play();

        if (!_currentlyPlayingCount.ContainsKey(soundName))
        {
            _currentlyPlayingCount[soundName] = 0;
        }
        _currentlyPlayingCount[soundName]++;

        StartCoroutine(DecrementPlayingCount(soundName, clipToPlay.length));
    }

    public void PlaySFXWithRandomPitch(SoundWithRandomPitchSettings settings)
    {
        PlaySFXWithRandomPitch(settings.SoundId, settings.MinPitch, settings.MaxPitch, settings.IsDefaultVolume ? 1f : settings.VolumeFactor);
    }

    private AudioClip GetAudioClip(AudioClipLibrary.AudioEntry entry)
    {
        return entry.AET == AudioEntryType.Random && entry.clips.Length > 0
            ? entry.clips[UnityEngine.Random.Range(0, entry.clips.Length)]
            : entry.clips[0];
    }

    public void PlayLoopingSound(string soundName, float volumeMultiplier = 1f)
    {
        if (_loopingSounds.ContainsKey(soundName))
        {
            if (_loopingSounds[soundName].isPlaying)
            {
                return;
            }
        }

        if (_audioLibrary == null) return;

        AudioClipLibrary.AudioEntry entry = _audioLibrary.GetEntry(soundName);
        if (entry == null) return;

        AudioClip clipToPlay = GetAudioClip(entry);

        if (clipToPlay == null) return;

        AudioSource source = GetAvailableSFXSource();
        source.clip = clipToPlay;
        source.volume = _masterVolume * _sfxVolume * entry.volume * volumeMultiplier;
        source.loop = true;
        source.Play();

        _loopingSounds[soundName] = source;
    }

    public void StopLoopingSound(string soundName)
    {
        if (_loopingSounds.ContainsKey(soundName))
        {
            _loopingSounds[soundName].Stop();
            _loopingSounds[soundName].loop = false;
            _loopingSounds.Remove(soundName);
        }
    }

    public bool IsLoopingSoundPlaying(string soundName)
    {
        return _loopingSounds.ContainsKey(soundName) && _loopingSounds[soundName].isPlaying;
    }

    public void PlayMusic(string musicName, float fadeTime = -1f)
    {
        Debug.Log($"PlayMusic: {musicName}");
        if (_audioLibrary == null) return;

        AudioClipLibrary.AudioEntry entry = _audioLibrary.GetEntry(musicName);
        if (entry != null && entry.clips[0] != null)
        {
            float crossfadeTime = fadeTime < 0 ? _defaultCrossfadeTime : fadeTime;

            if(_crossfadeCoroutine != null)
            {
                StopCoroutine(_crossfadeCoroutine);
            }

            StartCoroutine(CrossfadeMusic(entry.clips[0], entry.volume, crossfadeTime));
        }
    }

    public void StopMusic(float fadeTime = -1f)
    {
        float crossfadeTime = fadeTime < 0 ? _defaultCrossfadeTime : fadeTime;
        _crossfadeCoroutine = StartCoroutine(FadeOutMusic(crossfadeTime));
    }

    public void PlayAmbient(string musicName, float fadeTime = -1f)
    {
        Debug.Log($"PlayAmbient: {musicName}");
        if (_audioLibrary == null) return;

        AudioClipLibrary.AudioEntry entry = _audioLibrary.GetEntry(musicName);

        if (entry == null) return;

        _ambientSource.clip = entry.clips[0];
        _ambientSource.volume = _masterVolume * _sfxVolume * entry.volume;
        _ambientSource.loop = true;
        _ambientSource.Play();
    }

    public void StopAmbient(float fadeTime = -1f)
    {
        _ambientSource.Stop();
    }

    public void PlayDialogue(string dialogueName, float volumeMultiplier = 1f)
    {
        if (_audioLibrary == null) return;

        AudioClipLibrary.AudioEntry entry = _audioLibrary.GetEntry(dialogueName);
        if (entry != null && entry.clips[0] != null)
        {
            PlayDialogue(entry.clips[0], entry.volume * volumeMultiplier);
        }
    }

    public void StopDialogue()
    {
        _dialogueSource.Stop();
    }

    public bool IsDialoguePlaying()
    {
        return _dialogueSource.isPlaying;
    }

    public void StopAllSounds()
    {
        StopMusic(0f);
        StopDialogue();
        StopAllSFX();
        StopAllLoopingSounds();
    }

    public void StopAllSFX()
    {
        foreach (AudioSource source in _sfxPool)
        {
            source.Stop();
            source.loop = false;
        }
    }

    public void StopAllLoopingSounds()
    {
        foreach (var kvp in _loopingSounds)
        {
            kvp.Value.Stop();
            kvp.Value.loop = false;
        }
        _loopingSounds.Clear();
    }

    public void SetMasterVolume(float volume)
    {
        _masterVolume = Mathf.Clamp01(volume);
        PlayerPrefs.SetFloat(MASTER_VOLUME_KEY, _masterVolume);
        UpdateAllVolumes();
    }

    public void SetMusicVolume(float volume)
    {
        // Внимание - общую громкость будет учитывать при непосредственном воспроизведении
        _musicVolume = Mathf.Clamp01(volume);
        PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, _musicVolume);
        UpdateMusicVolume();
    }

    public void SetSFXVolume(float volume)
    {
        _sfxVolume = Mathf.Clamp01(volume);
        PlayerPrefs.SetFloat(SFX_VOLUME_KEY, _sfxVolume);
    }

    public void SetDialogueVolume(float volume)
    {
        _dialogueVolume = Mathf.Clamp01(volume);
        PlayerPrefs.SetFloat(DIALOGUE_VOLUME_KEY, _dialogueVolume);
        UpdateDialogueVolume();
    }

    public float GetMasterVolume() => _masterVolume;
    public float GetMusicVolume() => _musicVolume;
    public float GetSFXVolume() => _sfxVolume;
    public float GetDialogueVolume() => _dialogueVolume;
    public float GetAmbientVolume() => _musicVolume;

    private bool CanPlaySound(string soundName)
    {
        float currentTime = Time.time;

        if (_lastPlayTime.ContainsKey(soundName))
        {
            float timeSinceLastPlay = currentTime - _lastPlayTime[soundName];
            if (timeSinceLastPlay < _duplicateSoundDelay)
            {
                return false;
            }
        }

        if (!_currentlyPlayingCount.ContainsKey(soundName))
        {
            _currentlyPlayingCount[soundName] = 0;
        }

        if (_currentlyPlayingCount[soundName] >= _maxDuplicateSounds)
        {
            return false;
        }

        _lastPlayTime[soundName] = currentTime;
        return true;
    }

    private void PlaySFX(AudioClip clip, float volumeMultiplier, string soundName)
    {
        if (clip == null) return;

        AudioSource source = GetAvailableSFXSource();
        source.clip = clip;
        source.volume = _masterVolume * _sfxVolume * volumeMultiplier;
        source.loop = false;
        source.pitch = _defaultPitch;
        source.Play();

        if (!_currentlyPlayingCount.ContainsKey(soundName))
        {
            _currentlyPlayingCount[soundName] = 0;
        }
        _currentlyPlayingCount[soundName]++;

        StartCoroutine(DecrementPlayingCount(soundName, clip.length));
    }

    private IEnumerator DecrementPlayingCount(string soundName, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (_currentlyPlayingCount.ContainsKey(soundName))
        {
            _currentlyPlayingCount[soundName]--;
            if (_currentlyPlayingCount[soundName] <= 0)
            {
                _currentlyPlayingCount.Remove(soundName);
            }
        }
    }

    private void PlayMusic(AudioClip clip, float volumeMultiplier)
    {
        if (clip == null) return;

        if (_musicSource.isPlaying && _musicSource.clip == clip)
        {
            return;
        }

        if (_crossfadeCoroutine != null)
        {
            StopCoroutine(_crossfadeCoroutine);
        }

        StartCoroutine(CrossfadeMusic(clip, volumeMultiplier, _defaultCrossfadeTime));
    }

    private void PlayDialogue(AudioClip clip, float volumeMultiplier)
    {
        if (clip == null) return;

        _dialogueSource.clip = clip;
        _dialogueSource.volume = _masterVolume * _dialogueVolume * volumeMultiplier;
        _dialogueSource.Play();
    }

    private AudioSource GetAvailableSFXSource()
    {
        for (int i = 0; i < _sfxPoolSize; i++)
        {
            int index = (_currentSfxIndex + i) % _sfxPoolSize;
            if (!_sfxPool[index].isPlaying)
            {
                _currentSfxIndex = (index + 1) % _sfxPoolSize;
                return _sfxPool[index];
            }
        }

        AudioSource source = _sfxPool[_currentSfxIndex];
        _currentSfxIndex = (_currentSfxIndex + 1) % _sfxPoolSize;
        return source;
    }

    private IEnumerator CrossfadeMusic(AudioClip newClip, float volumeMultiplier, float fadeTime)
    {
        _isCrossfading = true;

        // Определяем, кто сейчас свободен, а кто занят
        // Если _musicSource играет - значит, фейдим ЕГО, а включаем Crossfade. И наоборот.
        AudioSource activeSource = _musicSource.isPlaying ? _musicSource : _musicSourceCrossfade;
        AudioSource newSource = activeSource == _musicSource ? _musicSourceCrossfade : _musicSource;

        // Подготовка нового источника
        newSource.clip = newClip;
        newSource.volume = 0f;
        newSource.Play(); // Теперь играют ОБА

        float elapsedTime = 0f;
        float startVolume = activeSource.volume;
        float targetVolume = _masterVolume * _musicVolume * volumeMultiplier;

        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fadeTime;

            activeSource.volume = Mathf.Lerp(startVolume, 0f, t);
            newSource.volume = Mathf.Lerp(0f, targetVolume, t);

            yield return null;
        }

        // Останавливаем старый
        activeSource.Stop();
        activeSource.volume = 0f;

        // Фиксируем новый
        newSource.volume = targetVolume;

        _isCrossfading = false;
        _crossfadeCoroutine = null;
    }

    private IEnumerator FadeOutMusic(float fadeTime)
    {
        if (_isCrossfading)
        {
            if (_crossfadeCoroutine != null)
            {
                StopCoroutine(_crossfadeCoroutine);
            }
        }

        _isCrossfading = true;

        // 1. Запоминаем, КОГО именно мы сейчас глушим
        AudioSource sourceToStop = _musicSource.isPlaying ? _musicSource : _musicSourceCrossfade;

        // Если оба молчат или оба играют - выберем тот, у кого громкость больше
        if (_musicSource.isPlaying && _musicSourceCrossfade.isPlaying)
        {
            sourceToStop = _musicSource.volume > _musicSourceCrossfade.volume ? _musicSource : _musicSourceCrossfade;
        }

        float startVolume = sourceToStop.volume;
        float elapsedTime = 0f;

        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fadeTime;
            // Глушим только нашего кандидата
            sourceToStop.volume = Mathf.Lerp(startVolume, 0f, t);
            yield return null;
        }

        // 2. В конце останавливаем ТОЛЬКО ЕГО
        sourceToStop.Stop();
        sourceToStop.volume = 0f;

        // Второй источник НЕ ТРОГАЕМ. Если там началась музыка - пусть играет.

        _isCrossfading = false;
        _crossfadeCoroutine = null;
    }

    private void LoadVolumeSettings()
    {
        _masterVolume = PlayerPrefs.GetFloat(MASTER_VOLUME_KEY, 1f);
        _musicVolume = PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY, 1f);
        _sfxVolume = PlayerPrefs.GetFloat(SFX_VOLUME_KEY, 1f);
        _dialogueVolume = PlayerPrefs.GetFloat(DIALOGUE_VOLUME_KEY, 1f);

        UpdateAllVolumes();
    }

    private void UpdateAllVolumes()
    {
        UpdateMusicVolume();
        UpdateDialogueVolume();
        UpdateSFXVolume();
        UpdateLoopingSoundsVolume();
    }

    private void UpdateSFXVolume()
    {
        // Обновляем громкость ВСЕХ играющих SFX
        foreach (AudioSource source in _sfxPool)
        {
            if (source.isPlaying && !source.loop) // Только не-луп звуки
            {
                source.volume = _masterVolume * _sfxVolume;
            }
        }
    }

    private void UpdateLoopingSoundsVolume()
    {
        foreach (var kvp in _loopingSounds)
        {
            if (kvp.Value.isPlaying)
            {
                kvp.Value.volume = _masterVolume * _sfxVolume;
            }
        }
    }

    private void UpdateMusicVolume()
    {
        _musicSource.volume = _musicVolume * _masterVolume;
        _musicSourceCrossfade.volume = _musicVolume * _masterVolume;

        _ambientSource.volume = _musicVolume * _masterVolume; // Да, подвязано к музыке
    }

    private void UpdateDialogueVolume()
    {
        _dialogueSource.volume = _masterVolume * _dialogueVolume;
    }
}

public enum AudioCategory
{
    SFX,
    Music,
    Dialogue,
    UI,
    Ambient
}

[Serializable]
public struct SoundWithRandomPitchSettings
{
    public string SoundId;
    public float MinPitch;
    public float MaxPitch;

    public bool IsDefaultVolume;
    public float VolumeFactor;
}