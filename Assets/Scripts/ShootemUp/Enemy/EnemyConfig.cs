using UnityEngine;

[CreateAssetMenu(fileName = "EnemyConfig", menuName = "Shoot`em up/Enemy Config")]
// Класс для кофигурации врагов
public class EnemyConfig : ScriptableObject
{
    [SerializeField] private EnemyPrefabData[] enemyPrefabs;

    public GameObject GetPrefab(EnemyType enemyType)
    {
        foreach (var data in enemyPrefabs)
        {
            if (data.enemyType == enemyType)
                return data.prefab;
        }

        Debug.LogError($"No prefab found for enemy type: {enemyType}");
        return null;
    }

    public bool HasPrefab(EnemyType enemyType)
    {
        return GetPrefab(enemyType) != null;
    }
}