using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyTracker : MonoBehaviour
{
    public int Money = 0;
    public event Action OnMoneyChanged;

    public static MoneyTracker Instance;
    [SerializeField] private ParticleSystem _moneyParticle;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Money"))
        {
            _moneyParticle.Play();
            Destroy(other);
            Money++;
            OnMoneyChanged?.Invoke();
        }
    }
}
