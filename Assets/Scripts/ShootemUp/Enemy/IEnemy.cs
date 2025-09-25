[System.Serializable]
public enum EnemyType
{
    Regular,
    RegularShooting,
    Armored1,
    Armored2,
    ArmoredShooting,
    Fast,
    Boss
}

public interface IEnemy
{
    float Health { get; }
    bool IsDied { get; }
    IMovingPattern currentMovingPattern { get; set; }
}
