using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivesManagerGui : MonoBehaviour
{
    [SerializeField] private LivesManager _livesManager;
    [SerializeField] private GameObject[] _linkedHearts;

    private void Awake()
    {
        _livesManager.OnValueChanged += HandleLivesChanged;
    }

    private void OnDestroy()
    {
        _livesManager.OnValueChanged -= HandleLivesChanged;
    }

    private void HandleLivesChanged(float lives)
    {
        for (int i = 0; i < _linkedHearts.Length; i++)
        {
            if (_linkedHearts[i].activeInHierarchy && i >= lives) _linkedHearts[i].SetActive(false);
            else if (!_linkedHearts[i].activeInHierarchy && i < lives) _linkedHearts[i].SetActive(true);
        }
    }
}
