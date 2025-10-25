using System;
using UnityEngine;
public class SignalLine
{
    // Класс, отвечающий за логику линии - основного элемента головоломки платформера

    public int ID {  get; private set; }
    public Signal CurrentSignal { get; private set; }
    public SignalDirection CurrentDirection { get; private set; }
    public SignalLine NextSignalLine { get; private set; }
    public bool IsLineActive = false;

    public SignalLine(int id)
    {
        ID = id;
        CurrentSignal = new Signal(true, false, true, false);
        CurrentDirection = SignalDirection.top;
    }

    public SignalLine(int id, Signal currentSignal, SignalDirection currentDirection, SignalLine nextLine)
    {
        ID = id;
        CurrentSignal = currentSignal;
        CurrentDirection = currentDirection;
        NextSignalLine = nextLine;
    }

    public Signal GetSignal() => CurrentSignal; // Получаем весь сигнал

    // Получаем сигналы по специфическим направлениям
    public bool GetSpecificSignal(SignalDirection direction) => CurrentSignal[direction];
    public bool GetSpecificSignal(int direction) => CurrentSignal[direction];

    public void Rotate(bool isClockwise)
    {
        if (isClockwise) CurrentSignal = new Signal(CurrentSignal.left, CurrentSignal.top, CurrentSignal.right, CurrentSignal.bottom);//CurrentSignal.RotateClockwise();
        else CurrentSignal = new Signal(CurrentSignal.right, CurrentSignal.bottom, CurrentSignal.left, CurrentSignal.top);
        //Debug.Log($"Signal {ID} is rotated! New signal is {CurrentSignal}");
    }

    public override string ToString()
    {
        string activeString = IsLineActive ? "active" : "not active";
        return $"Hi! I`m the signal line with ID {ID}. My direction is {(int)CurrentDirection}. " +
            $"My signals are: top={CurrentSignal.top}, right={CurrentSignal.right}, bottom={CurrentSignal.bottom}, left={CurrentSignal.left}. " +
            $"I`m currently {activeString}! Have a nice day!";
    }
}

public enum SignalDirection
{
    top,
    right,
    bottom,
    left
}

[Serializable]
public struct Signal
{
    public bool top, right, bottom, left;

    public bool this[SignalDirection direction]
    {
        get
        {
            switch (direction)
            {
                case SignalDirection.top:
                    return top;
                case SignalDirection.right:
                    return right;
                case SignalDirection.bottom:
                    return bottom;
                case SignalDirection.left:
                    return left;
            }

            return false;
        }
    }

    public bool this[int direction]
    {
        get
        {
            int normalizedDirection = direction % 4;
            return this[(SignalDirection)normalizedDirection];
        }
    }

    public Signal(bool top, bool right, bool bottom, bool left)
    {
        this.top = top;
        this.right = right;
        this.bottom = bottom;
        this.left = left;
    }
}
