using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedLevelHandler : MonoBehaviour
{
    // Разблокирование уровня

    // Да, лучше было сделать отдельную систему закрытых и открытых уровней
    // НО:
    // - она используется один раз.
    // - из-за отключений света нету времени на рефакторинг таких мелочей, когда надо
    //   работать над геймплеем

    [SerializeField] private ScaleIfHighlightedUI _scaleIfHighlightedUI;

    private void Start()
    {
        if(GameSaveManager.Instance.CompletedLevelCount == 7)
        {
            _scaleIfHighlightedUI.enabled = true;
            gameObject.SetActive(false);
        }
    }
}
