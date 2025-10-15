using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightRegion:MonoBehaviour
{
    private static readonly Color32 HLColor = new Color32(123, 120, 120, 255);
    private static readonly Color32 WhiteColor = new Color32(255, 255, 255, 255);
    private static List<Collider2D> highlitedPic = new List<Collider2D>();
    public static void HighlighPic(Collider2D currPiece) 
    { 
        highlitedPic.Add(currPiece);
    var sprite = currPiece.GetComponent<SpriteRenderer>();      
        sprite.color = HLColor;
    }
    public static void ReturnStandartColor() 
    {
        for (int i = highlitedPic.Count - 1; i >= 0; i--)
        {
            var c = highlitedPic[i];
            if (c == null)
            {
                highlitedPic.RemoveAt(i);
                continue;
            }
            var sprite = c.GetComponent<SpriteRenderer>();
            if (sprite != null)
                sprite.color = WhiteColor;
        }

    }
    private static void ClearList()
    {
        highlitedPic.Clear();
    }

    private void OnEnable()
    {
        BoxPuzzleEventManager.OnSelected += HighlighPic;
        BoxPuzzleEventManager.OnReturnToNormalOpacity += ReturnStandartColor;
        BoxPuzzleEventManager.OnReturnToNormalOpacity += ClearList;
    }
    private void OnDisable()
    {
        BoxPuzzleEventManager.OnSelected -= HighlighPic;
        BoxPuzzleEventManager.OnReturnToNormalOpacity -= ReturnStandartColor;
        BoxPuzzleEventManager.OnReturnToNormalOpacity -= ClearList;
    }

}
