
public interface ISignalAction
{
    // Действие, когда головоломка решена
    bool IsExecuted { get; }
    void Undo();
    void Execute();
}
