using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineCreator : MonoBehaviour
{
    private LineRenderer line;
    private Collider2D CurrentHit;
    private Collider2D LastHit;
    private bool isDraw;
    private void Awake()
    {
       
        line = GetComponent<LineRenderer>();
        if (line == null)
            line = gameObject.AddComponent<LineRenderer>();
        GetBasicParams();
        line.startWidth = 0.1f;
        line.endWidth = 0.1f;
        line.startColor = Color.red;
        line.endColor = Color.blue;
        line.useWorldSpace = true; // для 2D обычно true
    }
    private void GetBasicParams() 
    {
        line.positionCount = 0;
        isDraw = false;
        CurrentHit = null;
        LastHit = null;
    }
    public void StartLineRenderCourotine() // Да, курутины - не лучший способ рисовать линии, но я скорее просто попрактиковать курутины хотел
    {
        StartCoroutine(RenderLine());
    }
    private IEnumerator RenderLine()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        GetBasicParams();
        CurrentHit = Physics2D.OverlapCircle(mousePos, 0.01f);
        line.positionCount = 1;
        line.SetPosition(0, mousePos);
        isDraw = true;

        if (CurrentHit)
        {
            BoxPuzzleEventManager.ShowSelected(CurrentHit); //Подсветка и просчет правильныъ, как понятно
            BoxPuzzleEventManager.OnRigthSelected();
        }
        while (isDraw)
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;

            line.positionCount++;
            line.SetPosition(line.positionCount - 1, mousePos);

            Collider2D hit = Physics2D.OverlapCircle(mousePos, 0.01f);
            if (hit && hit != CurrentHit) // Пересечение с другими колайдерами
            {
                LastHit = CurrentHit;
                CurrentHit = hit;
                BoxPuzzleEventManager.ShowSelected(CurrentHit);

                isDraw = CheckCorrection.Check(LastHit, CurrentHit); // Проверяем стоит ли дальше рендерить
                if (!isDraw)
                {
                    BoxPuzzleEventManager.RetrunToNormOp();
                    GetBasicParams();
                    yield break;
                }

                BoxPuzzleEventManager.OnRigthSelected();
            }

            if (Input.GetMouseButtonUp(0))
            {
                BoxPuzzleEventManager.RetrunToNormOp();
              
                GetBasicParams();
                yield break;
            }

            yield return null;
        }
    }
    public void ResetLine()
    {
      
        StopAllCoroutines();
        GetBasicParams();
    }
    private void OnEnable()
    {
        BoxPuzzleEventManager.OnLevelChange += ResetLine;

    }
    private void OnDisable()
    {
        BoxPuzzleEventManager.OnLevelChange -= ResetLine;

    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isDraw) // Вызываем один раз курутину
        {
            StartLineRenderCourotine();
        }
    }
}
