using System;

[Serializable]
public enum TypeBlock
{
    Addition,
    Subtraction,
    Multiplication,
    Division,
    Start,
    Finish,
    Condition,
    CycleStart,
    CycleEnd,
    BlackBox,
    Storage,
    Transition
}