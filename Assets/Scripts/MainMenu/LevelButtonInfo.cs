using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "LevelButtonInfo", menuName = "Main Menu/LevelButtonInfo")]
public class LevelButtonInfo : ScriptableObject
{
    public string title;
    public string subtitle;
    public Sprite image;

    public int sceneId; // Для загрузки уровня
    public int levelId; // Для сохранения прогресса
}
