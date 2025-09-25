using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradesManager : MonoBehaviour
{
    private WavesManager _wavesManager;

    private void Awake()
    {
        _wavesManager = WavesManager.Instance;
    }
}
