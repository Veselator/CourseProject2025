using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "AudioLibrary", menuName = "Audio/Clip Library")]
public class AudioClipLibrary : ScriptableObject
{
    // SO для библиотеки звуков
    [System.Serializable]
    public class AudioEntry
    {
        public string name;
        public AudioEntryType AET;

        public AudioClip[] clips = new AudioClip[1];

        public AudioCategory category = AudioCategory.SFX;
        [Range(0f, 2f)] public float volume = 1f;
    }

    [SerializeField] private List<AudioEntry> _clips = new List<AudioEntry>();
    private Dictionary<string, AudioEntry> _clipDictionary;

    private void OnEnable()
    {
        BuildDictionary();
    }

    private void BuildDictionary()
    {
        _clipDictionary = new Dictionary<string, AudioEntry>();
        foreach (var entry in _clips)
        {
            if (!string.IsNullOrEmpty(entry.name))
            {
                _clipDictionary[entry.name] = entry;
            }
        }
    }

    public AudioEntry GetEntry(string name)
    {
        if (_clipDictionary == null || _clipDictionary.Count == 0)
        {
            BuildDictionary();
        }

        return _clipDictionary.TryGetValue(name, out var entry) ? entry : null;
    }

    public AudioClip GetClip(string name)
    {
        AudioEntry entry = GetEntry(name);
        return entry?.clips[0];
    }

    public bool HasClip(string name)
    {
        if (_clipDictionary == null || _clipDictionary.Count == 0)
        {
            BuildDictionary();
        }

        return _clipDictionary.ContainsKey(name);
    }
}

public enum AudioEntryType
{
    Normal,
    Random
}