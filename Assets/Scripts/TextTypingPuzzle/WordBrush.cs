using TMPro;
using UnityEngine;

public class WordBrush : MonoBehaviour
{
    static readonly Color32 DEF_COLOR = new Color32(128, 128, 128, 255);
    static readonly Color32 CORRECT_COLOR = new Color32(0, 255, 0, 255);
    static readonly Color32 INCORRECT_COLOR = new Color32(255, 0, 0, 255);

    // Оригинальные методы для TextPiece
    static public void SetDefaultColor(TextPiece text1)
    {
        SetDefaultColor(text1.tmpText);
    }

    static public void SetCorrectColor(int index, TextPiece text1)
    {
        SetCorrectColor(index, text1.tmpText);
    }

    static public void SetIncorrectColor(int index, TextPiece text1)
    {
        SetIncorrectColor(index, text1.tmpText);
    }

    // Перегрузки для TMP_Text
    static public void SetDefaultColor(TMP_Text tmpText)
    {
        var textInfo = tmpText.textInfo;
        for (int i = 0; i < tmpText.text.Length; i++)
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
        tmpText.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
    }

    static public void SetCorrectColor(int index, TMP_Text tmpText)
    {
        var textInfo = tmpText.textInfo;
        if (index >= textInfo.characterCount) return;

        var c = textInfo.characterInfo[index];
        if (!c.isVisible) return;

        int meshIndex = c.materialReferenceIndex;
        int vertexIndex = c.vertexIndex;
        Color32[] vertexColors = textInfo.meshInfo[meshIndex].colors32;

        vertexColors[vertexIndex + 0] = CORRECT_COLOR;
        vertexColors[vertexIndex + 1] = CORRECT_COLOR;
        vertexColors[vertexIndex + 2] = CORRECT_COLOR;
        vertexColors[vertexIndex + 3] = CORRECT_COLOR;

        tmpText.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
    }

    static public void SetIncorrectColor(int index, TMP_Text tmpText)
    {
        var textInfo = tmpText.textInfo;
        if (index >= textInfo.characterCount) return;

        var c = textInfo.characterInfo[index];
        if (!c.isVisible) return;

        int meshIndex = c.materialReferenceIndex;
        int vertexIndex = c.vertexIndex;
        Color32[] vertexColors = textInfo.meshInfo[meshIndex].colors32;

        vertexColors[vertexIndex + 0] = INCORRECT_COLOR;
        vertexColors[vertexIndex + 1] = INCORRECT_COLOR;
        vertexColors[vertexIndex + 2] = INCORRECT_COLOR;
        vertexColors[vertexIndex + 3] = INCORRECT_COLOR;

        tmpText.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
    }
}