using UnityEngine;

public class ExplosionVisual : MonoBehaviour
{
    // Отвечает за корректное отображение взрыва

    [SerializeField] private GameObject _explosion;
    private Grenade _linkedGrenade;
    private CameraShake _shaker;
    [SerializeField] private float _shakeIntensity = 3f;

    private void Start()
    {
        _linkedGrenade = GetComponent<Grenade>();
        _linkedGrenade.OnExlode += ExplosionEffect;
    }

    public void Init(CameraShake shaker)
    {
        _shaker = shaker;
    }

    private void OnDestroy()
    {
        _linkedGrenade.OnExlode -= ExplosionEffect;
    }

    private void ExplosionEffect()
    {
        Instantiate(_explosion, transform.position, Quaternion.identity);
        _shaker.StartHitShake(_shakeIntensity);
    }
}
