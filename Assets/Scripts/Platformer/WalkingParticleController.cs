using UnityEngine;

public class WalkingParticleController : MonoBehaviour
{
    [SerializeField] private PlayerPlatformerHandler _player;
    private ParticleSystem.EmissionModule _particle;

    private void Start()
    {
        _particle = GetComponent<ParticleSystem>().emission;

        _player.OnPlayerDegrounded += HideParticle;
        _player.OnPlayerGrounded += ShowParticle;
    }

    private void OnDestroy()
    {
        _player.OnPlayerDegrounded -= HideParticle;
        _player.OnPlayerGrounded -= ShowParticle;
    }

    private void HideParticle()
    {
        _particle.enabled = false;
    }

    private void ShowParticle()
    {
        _particle.enabled = true;
    }
}
