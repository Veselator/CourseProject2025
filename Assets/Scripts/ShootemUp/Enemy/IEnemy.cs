[System.Serializable]
public enum EnemyType
{
    Regular,
    Armored1,
    Armored2,
    Heavy,
    Fast,
    Boss
}

public interface IEnemy
{
    float Health { get; }
    bool IsDied { get; }
    IMovingPattern currentMovingPattern { get; set; }
}
