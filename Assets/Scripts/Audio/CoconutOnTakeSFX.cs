using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoconutOnTakeSFX : MonoBehaviour
{
    [SerializeField] private Coconat _linkedCoconut;
    private GameAudioManager _gameAudioManager;
    [SerializeField] private string _coconutTakeSFXName;

    private void Start()
    {
        _gameAudioManager = GameAudioManager.Instance;

        _linkedCoconut.OnCoconutTaken += PlayCoconutTakeSFX;
    }

    private void OnDestroy()
    {
        _linkedCoconut.OnCoconutTaken -= PlayCoconutTakeSFX;
    }

    private void PlayCoconutTakeSFX()
    {
        _gameAudioManager.PlaySound(_coconutTakeSFXName);
    }
}
