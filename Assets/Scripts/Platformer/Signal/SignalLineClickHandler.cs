using System;
using UnityEngine;

public class SignalLineClickHandler : MonoBehaviour
{
    [SerializeField] private LinkedManager[] _linkedManagers;

    private void OnMouseDown()
    {
        //_linkedManager.RotateLine(ID);
        foreach (LinkedManager signalManager in _linkedManagers)
        {
            signalManager.linkedManager.RotateLine(signalManager.ID);
        }
    }
}

[Serializable]
public struct LinkedManager
{
    public SignalsManager linkedManager;
    public int ID;
}
