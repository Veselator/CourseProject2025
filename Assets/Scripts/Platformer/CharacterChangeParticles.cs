using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterChangeParticles : MonoBehaviour
{
    [SerializeField] private ParticleSystem _changeParticles;
    [SerializeField] private PlayerChangerManager _playerChangerManager;

    private void Start()
    {
        _playerChangerManager.OnCharacterChanged += PlayParticles;
    }
    
    private void OnDestroy()
    {
        _playerChangerManager.OnCharacterChanged -= PlayParticles;
    }

    private void PlayParticles(int _)
    {
        _changeParticles.Play();
    }
}
