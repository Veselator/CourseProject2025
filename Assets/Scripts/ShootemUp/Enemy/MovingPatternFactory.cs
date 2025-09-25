using UnityEngine;

[System.Serializable]
public enum MovingPatternType
{
    // ����� ����
    Straight,

    // �� ����������� ������� ������������ � ����� � ����
    StraightMiddle,

    // �� ����������� ����� - � ��������� ��������
    Random,

    // �� ����������� ����� - �� ������ � ��������
    RandomCurved,

    // �� ����������� - � ����������� �������� �� ������
    Curved
}

public static class MovingPatternFactory
{
    public static IMovingPattern CreatePattern(MovingPatternType type, Vector2 start, Vector2 end)
    {
        IMovingPattern pattern = null;

        // ������� ������� ��� ������ ������� � �� ����� ���������� � scope lifetime ������� �����
        switch (type)
        {
            case MovingPatternType.Straight:
                pattern = new StraightMovingPattern();
                break;

            case MovingPatternType.StraightMiddle:
                pattern = new StraightMiddleMovingPattern();
                break;

            case MovingPatternType.Random:
                pattern = new RandomMovingPattern();
                break;

            case MovingPatternType.RandomCurved:
                Vector2 randomControl1 = start + Random.insideUnitCircle * 3f;
                Vector2 randomControl2 = end + Random.insideUnitCircle * 3f;
                pattern = new CurvedMovingPattern(randomControl1, randomControl2);
                break;

            case MovingPatternType.Curved:
                Vector2 control1 = new Vector2(start.x - 2f, start.y - 2f);
                Vector2 control2 = new Vector2(end.x + 2f, end.y + 2f);
                pattern = new CurvedMovingPattern(control1, control2);
                break;

            default:
                pattern = new StraightMovingPattern();
                break;
        }

        pattern.StartPosition = start;
        pattern.EndPosition = end;

        return pattern;
    }
}