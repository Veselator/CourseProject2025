using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestVisibilityUIManager : MonoBehaviour
{
    [SerializeField] private GameObject UIHolder;

    public static QuestVisibilityUIManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void HideUI()
    {
        UIHolder.SetActive(false);
    }

    public void ShowUI()
    {
        UIHolder.SetActive(true);
    }
}
