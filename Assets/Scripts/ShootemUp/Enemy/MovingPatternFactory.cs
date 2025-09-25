using UnityEngine;

[System.Serializable]
public enum MovingPatternType
{
    // Прямо вниз
    Straight,

    // От началальной позиции перемещаемся в центр и вниз
    StraightMiddle,

    // От изначальной точки - к случайной конечной
    Random,

    // От изначальной точки - по кривой к конечной
    RandomCurved,

    // От изначальной - к определённой конечной по кривой
    Curved
}

public static class MovingPatternFactory
{
    public static IMovingPattern CreatePattern(MovingPatternType type, Vector2 start, Vector2 end)
    {
        IMovingPattern pattern = null;

        // Паттерн задаётся при вызове фабрики и не может изменяться в scope lifetime объекта врага
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