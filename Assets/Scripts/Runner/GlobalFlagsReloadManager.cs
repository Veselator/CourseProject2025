using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalFlagsReloadManager : MonoBehaviour
{
    void Start()
    {
        GlobalFlags.ResetAllFlags();
    }
}
