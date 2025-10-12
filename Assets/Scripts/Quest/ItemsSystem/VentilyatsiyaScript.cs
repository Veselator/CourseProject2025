using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VentilyatsiyaScript : MonoBehaviour
{
    // ��� ���������� ������� ��������, ��
    // [������� ������ ��� ���� ���������� ������� QuestAction]

    private int NumOfCurrentBolts = 4;
    [SerializeField] private QuestAction actionOnBoltsEnded;

    public void CheckBolt()
    {
        NumOfCurrentBolts--;
        if(NumOfCurrentBolts == 0)
        {
            QuestActionProccessor.Instance.ProcessAction(actionOnBoltsEnded, gameObject);
        }
    }
}
