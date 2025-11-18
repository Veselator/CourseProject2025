using UnityEngine;

public class PickupableItem : BaseItem
{
    [Header("Предмет, на который ссылаемся")]
    [SerializeField] private QuestInventoryItem itemData; // то, на какой предмет ссылаемся
    [Header("Действие, которое выполняется при подборе предмета")]
    [SerializeField] private QuestAction additionalActions;

    protected override void Start()
    {
        itemID = itemData.itemId; // Важно
        itemData.worldPickupPosition = transform.position;
        base.Start();
    }

    public override bool CanInteract() => true;

    public override void Interact()
    {
        try
        {
            // Пытаемся добавить (даже если UI упадет, мы это поймаем)
            // ВАЖНО: Перед этим обнови позицию, раз уж ты используешь SO для передачи координат
            itemData.worldPickupPosition = transform.position;
            QuestInventoryManager.Instance.AddItem(itemData);

            if (additionalActions != null)
                QuestActionProccessor.Instance.ProcessAction(additionalActions, this.gameObject);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"CRITICAL ERROR IN PICKUP: {e.Message}\n{e.StackTrace}");
        }
        finally
        {
            Destroy(gameObject);
        }
    }
}
