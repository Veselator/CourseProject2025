using UnityEngine;

public class PlayerHealthLinker : MonoBehaviour
{
    // Нужен для реализации синглтона для здоровья игрока на основе IHealth
    public static IHealth PlayerHealth { get; private set; }

    private void Awake()
    {
        PlayerHealth = GetComponent<IHealth>();
    }
}
