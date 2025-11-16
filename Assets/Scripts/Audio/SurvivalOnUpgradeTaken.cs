using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivalOnUpgradeTaken : MonoBehaviour
{
    [SerializeField] private BaseUpgrade _linkedUpgrade;
    private GameAudioManager _gameAudioManager;
    [SerializeField] private string _upgradeTakenSFX = "bonus2";

    private void Start()
    {
        _gameAudioManager = GameAudioManager.Instance;
        if(_linkedUpgrade == null) _linkedUpgrade = GetComponent<BaseUpgrade>();

        _linkedUpgrade.OnDamageUpgradeTaken += PlaySFX;
    }

    private void OnDestroy()
    {
        _linkedUpgrade.OnDamageUpgradeTaken -= PlaySFX;
    }

    private void PlaySFX()
    {
        _gameAudioManager.PlaySound(_upgradeTakenSFX);
    }
}
