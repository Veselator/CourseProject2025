using UnityEngine;

[CreateAssetMenu(fileName = "AbilityUIData", menuName = "Platformer/AbilityUIData")]
public class AbilityUIData : ScriptableObject
{
    public string Title;
    [TextArea(4, 5)]
    public string Description;

    public Sprite AbilitySprite;
}
