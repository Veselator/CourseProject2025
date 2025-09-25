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
        // ��� �������� ����� - ������ ���������� �������� ����
        base.HandlePatternCompleted();

        Debug.Log($"Handle pattern completed for regular enemy");
        // ����� �������� ����������� ��������� ��� RegularEnemy
        // ��������, ������� �������� �������� ��� �����������
    }
}