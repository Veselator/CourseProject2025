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

    public void Rotate(bool isCLockwise)
    {
        if (isCLockwise) CurrentSignal.RotateClockwise();
        else CurrentSignal.RotateAnticlockwise();
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

    public void RotateClockwise()
    {
        bool temp = top;
        top = left;
        left = bottom;
        bottom = right;
        right = temp;
    }

    public void RotateAnticlockwise()
    {
        bool temp = top;
        top = right;
        bottom = left;
        right = bottom;
        left = temp;
    }
}
