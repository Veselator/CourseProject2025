using UnityEngine;

public class TurnOffDebugs : MonoBehaviour
{
    private void Awake()
    {
        Debug.unityLogger.logEnabled = false;
    }
}
