using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticlesManager : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] _boomParticles;
    [SerializeField] private ParticleSystem _jumpParticle;
    private JumpTracker _jumpTracker;
    private PlayerHealth _playerHealth;

    private void Start()
    {
        _playerHealth = PlayerHealth.Instance;
        _jumpTracker = JumpTracker.Instance;

        _playerHealth.OnPlayerHit += Boom;
        _jumpTracker.OnJumpEnded += EndJump;
    }

    private void OnDestroy()
    {
        _playerHealth.OnPlayerHit -= Boom;
        _jumpTracker.OnJumpEnded -= EndJump;
    }

    private void Boom()
    {
        foreach (var particle in _boomParticles)
        {
            if(particle.gameObject.active) particle.Play();
        }
    }

    private void EndJump()
    {
        _jumpParticle.Play();
    }

}
