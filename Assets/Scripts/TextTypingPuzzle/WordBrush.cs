using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WordBrush : MonoBehaviour
{
    static readonly Color32 DEF_COLOR = new Color32(128, 128, 128, 255);
    static readonly Color32 CORRECT_COLOR = new Color32(0, 255, 0, 255);
    static readonly Color32 INCORRECT_COLOR = new Color32(255, 0, 0, 255);
    static public void SetDefaultColor(TextPiece text1)
    {
        var textInfo = text1.tmpText.textInfo;

        for (int i = 0; i < text1.tmpText.text.Length; i++)
        {
            if (i >= textInfo.characterCount) break;

            var c = textInfo.characterInfo[i];
            if (!c.isVisible) continue;

            int meshIndex = c.materialReferenceIndex;
            int vertexIndex = c.vertexIndex;
            Color32[] vertexColors = textInfo.meshInfo[meshIndex].colors32;

            vertexColors[vertexIndex + 0] = DEF_COLOR;
            vertexColors[vertexIndex + 1] = DEF_COLOR;
            vertexColors[vertexIndex + 2] = DEF_COLOR;
            vertexColors[vertexIndex + 3] = DEF_COLOR;
        }

        text1.tmpText.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
    }
    static public void SetCorrectColor(int index, TextPiece text1)
    {

        var textInfo = text1.tmpText.textInfo;
        var c = textInfo.characterInfo[index];
        if (!c.isVisible) return;
        int meshIndex = c.materialReferenceIndex;
        int vertexIndex = c.vertexIndex;
        Color32[] vertexColors = textInfo.meshInfo[meshIndex].colors32;
        vertexColors[vertexIndex + 0] = CORRECT_COLOR;
        vertexColors[vertexIndex + 1] = CORRECT_COLOR;
        vertexColors[vertexIndex + 2] = CORRECT_COLOR;
        vertexColors[vertexIndex + 3] = CORRECT_COLOR;
        text1.tmpText.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
    }

    static public void SetIncorrectColor(int index, TextPiece text1)
    {

        var textInfo = text1.tmpText.textInfo;
        var c = textInfo.characterInfo[index];
        if (!c.isVisible) return;
        int meshIndex = c.materialReferenceIndex;
        int vertexIndex = c.vertexIndex;
        Color32[] vertexColors = textInfo.meshInfo[meshIndex].colors32;
        vertexColors[vertexIndex + 0] = INCORRECT_COLOR;
        vertexColors[vertexIndex + 1] = INCORRECT_COLOR;
        vertexColors[vertexIndex + 2] = INCORRECT_COLOR;
        vertexColors[vertexIndex + 3] = INCORRECT_COLOR;
        text1.tmpText.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
    }
}
