using UnityEngine;

public class SoundOnHealthDamaged : MonoBehaviour
{
    private IHealth _health;
    [SerializeField] private string _soundIfDamaged = "hit_metal";
    [SerializeField] private float _minPitch = 0.6f;
    [SerializeField] private float _maxPitch = 1.2f;
    [SerializeField] private float _volume = 0.44f;

    private void Start()
    {
        _health = GetComponent<IHealth>();
        _health.OnDamaged += HandleDamaged;
    }

    private void OnDestroy()
    {
        _health.OnDamaged -= HandleDamaged;
    }

    private void HandleDamaged()
    {
        GameAudioManager.Instance.PlaySFXWithRandomPitch(_soundIfDamaged, _minPitch, _maxPitch, _volume);
    }
}
