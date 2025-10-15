using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxPiece : MonoBehaviour
{
    [SerializeField]
    private int id; // это поле будет видно в инспекторе

    public int Id
    {
        get { return id; }
        set { id = value; }
    }
}
