using UnityEngine;

[CreateAssetMenu(fileName = "DialogueCharacterSO", menuName = "Platformer/Dialogue/DialogueCharacterSO")]
public class DialogueCharacterSO : ScriptableObject
{
    // Конфиг лица в диалоге
    public string ID;
    public string Name;
    public Sprite Photo;
}
