using System;

public interface IUpgrade
{
    void ApplyUpgrade();
    string MainText { get; }
    string SecondText { get; }
}
