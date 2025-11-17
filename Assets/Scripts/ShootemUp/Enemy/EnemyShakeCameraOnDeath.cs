using UnityEngine;

public class EnemyShakeCameraOnDeath : MonoBehaviour
{
    private IHealth _linkedHealth;
    private CameraShake _linkedShaker;
    public void Init(CameraShake shaker)
    {
        _linkedHealth = GetComponent<IHealth>();
        _linkedShaker = shaker;
        _linkedHealth.OnDeath += _linkedShaker.StartLightHitShake;
    }

    private void OnDestroy()
    {
        if(_linkedHealth != null)
        {
            _linkedHealth.OnDeath -= _linkedShaker.StartLightHitShake;
        }
    }
}
