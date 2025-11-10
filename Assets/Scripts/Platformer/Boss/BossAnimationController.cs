using UnityEngine;

public class BossAnimationController : MonoBehaviour
{
    [SerializeField] private Sprite[] _emotions;
    [SerializeField] private SpriteRenderer _emotionSprite;

    public void SetEmotion(BossEmotion emotion)
    {
        switch (emotion)
        {
            case BossEmotion.Happy:
                _emotionSprite.sprite = _emotions[0];
                break;
            case BossEmotion.Unhappy:
                _emotionSprite.sprite = _emotions[1];
                break;
        }
    }
}

public enum BossEmotion
{
    Happy,
    Unhappy
}