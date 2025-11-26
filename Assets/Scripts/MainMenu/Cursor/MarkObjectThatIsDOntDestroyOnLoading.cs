using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkObjectThatIsDOntDestroyOnLoading : MonoBehaviour
{
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}
