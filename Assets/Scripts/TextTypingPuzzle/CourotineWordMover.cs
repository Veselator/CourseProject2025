using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D;
using UnityEngine;

public class CourotineWordMover : MonoBehaviour
{
    private SpeedOfText _speedOfText;

    private void Start()
    {
        _speedOfText = SpeedOfText.Instance;
    }

    public IEnumerator MoveWord(TextPiece text, RectTransform rightPos, RectTransform leftPos, float borderOffsetX)
    {
       
        if (text == null) yield break;
        var rt = text.tmpText.GetComponent<RectTransform>();
        if (rt == null) yield break;

        float randomY = UnityEngine.Random.Range(-200f, 200f);
        rt.anchoredPosition = new Vector2(rightPos.anchoredPosition.x, randomY);
        Vector2 center = new Vector2(borderOffsetX, randomY);
        Vector2 end = new Vector2(leftPos.anchoredPosition.x, randomY);

        while (!text.isComplete&&rt != null && Vector3.Distance(rt.anchoredPosition, center) > 0.01f)
        {
            rt.anchoredPosition = Vector3.MoveTowards(rt.anchoredPosition, center, _speedOfText.speedOfTextFly * Time.deltaTime);
            yield return null;
        }

        if (!text.isComplete)
            EventManagerPuzzle.OnMissInv(text);

        while (rt != null && Vector3.Distance(rt.anchoredPosition, end) > 0.01f)
        {
            rt.anchoredPosition = Vector3.MoveTowards(rt.anchoredPosition, end, _speedOfText.speedOfTextFly * Time.deltaTime);
            yield return null;
        }

        if (text.tmpText != null)
            Destroy(text.tmpText.gameObject);
    }

    public IEnumerator MoveFromScreenWinnigScenario(TextPiece textP, RectTransform top)
    {
        if (textP == null || textP.tmpText == null || top == null)
            yield break;

        var rt = textP.tmpText.GetComponent<RectTransform>();
        if (rt == null)
            yield break;


        while (rt != null && top != null && rt.anchoredPosition.y < top.anchoredPosition.y)
        {

            rt.anchoredPosition += new Vector2(0, _speedOfText.speedOfTextFly * Time.deltaTime);
            yield return null;
        }

      

        if (rt != null && rt.gameObject != null)
        {
            Destroy(rt.gameObject);
        }
    }
   


}
