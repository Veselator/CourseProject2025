using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BoxPuzzleEventManager 
{
    public static Action<Collider2D> OnSelected;
    public static Action OnReturnToNormalOpacity;
    public static Action OnRigthSelected;
    public static Action OnLevelChange;

    public static void ShowSelected(Collider2D col) 
    { 
        OnSelected?.Invoke(col);
    }

    public static void RetrunToNormOp() 
    {
        OnReturnToNormalOpacity?.Invoke();
    }
    public static void RigthSelected()
    {
        OnRigthSelected?.Invoke();
    }
  
    public static void LevelChange() 
    { 
        OnLevelChange?.Invoke();
    }
}
