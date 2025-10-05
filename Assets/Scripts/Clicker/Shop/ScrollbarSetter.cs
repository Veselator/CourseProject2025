using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollbarSetter : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Scrollbar>().value = 1f;
    }
}
