using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CheckCorrection
{
    // Чего 
    public static bool Check(Collider2D lp, Collider2D cp) 
    {
        if (!lp) return true;
        BoxPiece lastPiece = lp.GetComponent<BoxPiece>();
        BoxPiece currentPiece = cp.GetComponent<BoxPiece>();
        if (currentPiece.Id - lastPiece.Id != 1)
        {
            return false;
        }
        return true;

       
    }
}
