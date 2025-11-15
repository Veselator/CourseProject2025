using UnityEngine;

public class ClickerShopItemAudio : MonoBehaviour
{
    private BaseClickerShopItem _itemHandler;
    private GameAudioManager _audioManager;

    [SerializeField] private string _buySound;
    [SerializeField] private string _failedSound;

    private void Start()
    {
        _audioManager = GameAudioManager.Instance;
        _itemHandler = GetComponent<BaseClickerShopItem>();

        _itemHandler.OnItemPurchased += PlayBuySound;
        _itemHandler.OnFailedToBuyItem += PlayFailedSound;
    }

    private void OnDestroy()
    {
        _itemHandler.OnItemPurchased -= PlayBuySound;
        _itemHandler.OnFailedToBuyItem -= PlayFailedSound;
    }

    private void PlayBuySound(IClickerShopItem _)
    {
        _audioManager.PlaySound(_buySound);
    }

    private void PlayFailedSound(IClickerShopItem _)
    {
        _audioManager.PlaySound(_failedSound);
    }
}
