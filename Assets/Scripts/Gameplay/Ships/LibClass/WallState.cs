using System;

[Flags]
public enum WallState
{
    None = 0,
    Up = 1,
    Right = 2,
    Down = 4,
    Left = 8
}
