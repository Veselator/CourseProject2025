using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowBarsOnEnemyDamaged : MonoBehaviour
{
    private IHealth enemyHealth;
    [SerializeField] private GameObject BarParentGO;

    private void Start()
    {
        enemyHealth = GetComponent<IHealth>();

        enemyHealth.OnDamaged += ShowBars;
    }

    private void OnDestroy()
    {
        enemyHealth.OnDamaged -= ShowBars;
    }

    private void ShowBars()
    {
        if (!BarParentGO.activeInHierarchy) BarParentGO.SetActive(true);
    }
}
