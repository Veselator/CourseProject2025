using UnityEngine;

[CreateAssetMenu(fileName = "DialogueSO", menuName = "Platformer/Dialogue/DialogueSO")]
public class DialogueSO : ScriptableObject
{
    // Диалог - коллекция реплик
    public string Id;
    public DialogueNodeSO[] Nodes;
}
