using UnityEngine;

[CreateAssetMenu(fileName = "BulletConfig", menuName = "Shoot`em up/Bullet Config")]
// Класс для кофигурации пуль
public class BulletConfig : ScriptableObject
{
    [SerializeField] private BulletPrefabData[] bulletPrefabs;

    public GameObject GetPrefab(BulletType bulletType)
    {
        foreach (var data in bulletPrefabs)
        {
            if (data.bulletType == bulletType)
                return data.prefab;
        }

        Debug.LogError($"No prefab found for enemy type: {bulletType}");
        return null;
    }

    public bool HasPrefab(BulletType bulletType)
    {
        return GetPrefab(bulletType) != null;
    }
}