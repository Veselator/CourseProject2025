using System;
using UnityEngine;

public class SignalLineClickHandler : MonoBehaviour
{
    private LinkedManager[] _linkedManagers;

    public void Init()
    {
        _linkedManagers = GetComponent<SignalLineLinkedManagers>().LinkedManagers;
    }

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

    public Signal GetSignal() => linkedManager.GetSignal(ID).GetSignal();
}
