
// Интерфейс абилки
public interface IAbility
{
    // В будуем можно добавить публичное свойство LinkedUIData (на базе Scriptable object)
    // для отображения абилки в UI
    bool IsAvailable { get; set; }

    void Try2ApplyAbility();
}
