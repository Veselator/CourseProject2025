using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpParticlesController : MonoBehaviour
{
    [SerializeField] private PlayerPlatformerHandler _player;
    private ParticleSystem _particle;

    private void Start()
    {
        _particle = GetComponent<ParticleSystem>();

        _player.OnPlayerJumped += PlayParticle;
    }

    private void OnDestroy()
    {
        _player.OnPlayerJumped -= PlayParticle;
    }

    private void PlayParticle(float _)
    {
        _particle.Play();
    }
}
