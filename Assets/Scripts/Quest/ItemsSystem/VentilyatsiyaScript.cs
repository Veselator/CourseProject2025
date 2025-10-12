using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VentilyatsiyaScript : MonoBehaviour
{
    // Это такоооооой ужасный говнокод, но
    // [причина почему мне лень длписывать систему QuestAction]

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
