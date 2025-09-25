using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovingPattern
{
    Vector2 StartPosition { get; set; }
    Vector2 EndPosition { get; set; }
    bool IsCompleted { get; }
    void Init();
    Vector2 GetNext();
    void Reset();
    string ToString();
}
