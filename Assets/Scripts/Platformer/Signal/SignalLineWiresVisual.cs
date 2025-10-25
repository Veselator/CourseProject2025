using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignalLineWiresVisual : MonoBehaviour
{
    // Делает так, что-бы сегмент отображался нормально

    private LinkedManager[] _linkedManagers;
    [SerializeField] private GameObject[] _wires; // 4
    private Signal _currentSignal;

    public void Init()
    {
        _linkedManagers = GetComponent<SignalLineLinkedManagers>().LinkedManagers;
        _currentSignal = _linkedManagers[0].GetSignal();

        InitWires();
    }

    private void InitWires()
    {
        for (int i = 0; i < _wires.Length; i++)
        {
            _wires[i].SetActive(_currentSignal[i]);
        }
    }
}
