using UnityEngine;

public class RegularEnemy : BaseEnemy
{
    protected override void Start()
    {
        base.Start();
        //Debug.Log($"I am regular enemy with {_movement.Velocity} from {currentMovingPattern.StartPosition} to {currentMovingPattern.EndPosition}");
        //Debug.Log(currentMovingPattern);
    }
    protected override void HandlePatternCompleted()
    {
        // ƒл€ обычного врага - просто продолжить движение вниз
        base.HandlePatternCompleted();

        Debug.Log($"Handle pattern completed for regular enemy");
        // ћожно добавить специфичное поведение дл€ RegularEnemy
        // Ќапример, немного изменить скорость или направление
    }
}