using System;

public interface IReadableValue
{
    // Интерфейс для значения, которое мы можем считать
    // и, к примеру, отобразить в интерфейсе

    public float Value { get; } // Только от 0 до 1!

    public event Action<float> OnValueChanged; 
}
