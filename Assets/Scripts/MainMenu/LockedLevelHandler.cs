using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedLevelHandler : MonoBehaviour
{
    // Да, лучше было сделать отдельную систему закрытых и открытых уровней
    // НО:
    // - она используется один раз.
    // - из-за отключений света нету времени на рефакторинг таких мелочей, когда надо
    //   работать над геймплеем

    private void Start()
    {
        if(GameSaveManager.Instance.CompletedLevelCount == 7)
        {
            gameObject.SetActive(false);
        }
    }
}
