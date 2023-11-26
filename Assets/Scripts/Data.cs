using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;


public static class Data
{
    /// <summary>
    /// fallSpeed @ sec/line
    /// </summary>
    public readonly static float[] fallDelay = 
        { 0f, 1.0f, 0.793f, 0.618f, 0.473f, 0.355f, 0.262f, 0.190f, 0.135f,
        0.094f, 0.064f, 0.043f, 0.028f, 0.018f, 0.011f, 0.007f };
    /// <summary>
    /// just in case I wanna add more Tetriminos, like a bomb or sth.
    /// </summary>
    public readonly static int TetriminoCount = 7;

    public readonly static float defaultLockTime = 0.5f;
    /// <summary>
    /// A map from tetrimino type to matrix location of minos of the corresponding type, when facing north, as spawned.
    /// /*maybe it's better to make it a function, so everytime it is called, it creates an different instance to prevent unexpected changes*/
    /// nah, we either deep clone, or we use function
    /// </summary>
    public static Dictionary<TetriminoType, MatCoor[]> spawnLocation = new()
    {
        {TetriminoType.O, new MatCoor[] { new MatCoor(4, 20), new MatCoor(5, 20), new MatCoor(5, 21), new MatCoor(4, 21) } },
        {TetriminoType.I, new MatCoor[] { new MatCoor(3, 20), new MatCoor(4, 20), new MatCoor(5, 20), new MatCoor(6, 20) } },
        {TetriminoType.T, new MatCoor[] { new MatCoor(3, 20), new MatCoor(4, 21), new MatCoor(4, 20), new MatCoor(5, 20) } },
        {TetriminoType.L, new MatCoor[] { new MatCoor(3, 20), new MatCoor(4, 20), new MatCoor(5, 20), new MatCoor(5, 21) } },
        {TetriminoType.J, new MatCoor[] { new MatCoor(3, 20), new MatCoor(4, 20), new MatCoor(5, 20), new MatCoor(3, 21) } },
        {TetriminoType.S, new MatCoor[] { new MatCoor(3, 20), new MatCoor(4, 20), new MatCoor(4, 21), new MatCoor(5, 21) } },
        {TetriminoType.Z, new MatCoor[] { new MatCoor(3, 21), new MatCoor(4, 21), new MatCoor(4, 20), new MatCoor(5, 20) } },
    };
    public static Dictionary<TetriminoType, int> rotationCenterMap = new()
    {
        {TetriminoType.O, 0},
        {TetriminoType.I, 1},
        {TetriminoType.T, 2},
        {TetriminoType.L, 1},
        {TetriminoType.J, 1},
        {TetriminoType.S, 1},
        {TetriminoType.Z, 2},
    };
    /// <summary>
    /// Get the offset data of one type for SRS.
    /// </summary>
    /// <param name="t">type of Tetrimino asked for</param>
    /// <returns>a table of offset data of all orientations and all scenarios</returns>
    public static Dictionary<RotationState, MatCoor[]> GetOffsetData(TetriminoType t)
    {
        return t switch
        {
            TetriminoType.O => new Dictionary<RotationState, MatCoor[]>
            {
                {RotationState.north, new MatCoor[]{new MatCoor(0, 0)}},
                {RotationState.east, new MatCoor[]{new MatCoor(0, -1)}},
                {RotationState.south, new MatCoor[]{new MatCoor(-1, -1)}},
                {RotationState.west, new MatCoor[]{new MatCoor(-1, 0)}},
            },
            TetriminoType.I => new Dictionary<RotationState, MatCoor[]>
            {
                 {RotationState.north, new MatCoor[]{new MatCoor(0, 0), new MatCoor(-1, 0), new MatCoor(2, 0), new MatCoor(-1, 0), new MatCoor(2, 0)}},
                 {RotationState.east, new MatCoor[]{new MatCoor(-1, 0), new MatCoor(0, 0), new MatCoor(0, 0), new MatCoor(0, 1), new MatCoor(0, -2)}},
                 {RotationState.south, new MatCoor[]{new MatCoor(-1, 1), new MatCoor(1, 1), new MatCoor(-2, 1), new MatCoor(1, 0), new MatCoor(-2, 0)}},
                 {RotationState.west, new MatCoor[]{new MatCoor(0, 1), new MatCoor(0, 1), new MatCoor(0, 1), new MatCoor(0, -1), new MatCoor(0, 2)}},
            },
            _ => new Dictionary<RotationState, MatCoor[]>
            {
                 {RotationState.north, new MatCoor[]{new MatCoor(0, 0), new MatCoor(0, 0), new MatCoor(0, 0), new MatCoor(0, 0), new MatCoor(0, 0)}},
                 {RotationState.east, new MatCoor[]{new MatCoor(0, 0), new MatCoor(1, 0), new MatCoor(1, -1), new MatCoor(0, 2), new MatCoor(1, 2)}},
                 {RotationState.south, new MatCoor[]{new MatCoor(0, 0), new MatCoor(0, 0), new MatCoor(0, 0), new MatCoor(0, 0), new MatCoor(0, 0)}},
                 {RotationState.west, new MatCoor[]{new MatCoor(0, 0), new MatCoor(-1, 0), new MatCoor(-1, -1), new MatCoor(0, 2), new MatCoor(-1, 2)}},
            },
        };
    }
    /// <summary>
    /// get pos of tetrimino gameObject in Unity
    /// </summary>
    /// <param name="r">Orientation</param>
    /// <param name="p">minoCoordinates of Mino No.0</param>
    /// <returns>position in Vector2</returns>
    public static Vector2 GetUnityPos(RotationState r, MatCoor p)
    {
        Vector2 v = r switch
        {
            RotationState.north => new Vector2(p.x, p.y),
            RotationState.east => new Vector2(p.x, p.y + 1),
            RotationState.south => new Vector2(p.x + 1, p.y + 1),
            RotationState.west => new Vector2(p.x + 1, p.y),
            _ => throw (new System.Exception("Wrong Tetrimino Orientation in GetUnityPos!")),
        };
        return v;
    }
    /*
    public static MatCoor ShiftPositionFromUnity2Matrix(TetriminoType t, RotationState r, Vector2 p)
    {
        MatCoor posInMatrix = new MatCoor(4, 2);
        int x, y;
        (x, y) = ((int)p.x, (int)p.y);
        switch (t)
        {
            case TetriminoType.O:
                posInMatrix = new int[4, 2] 
                { 
                    {x, y + 1},
                    {x + 1, y + 1}, 
                    {x, y }, 
                    {x + 1, y}
                };
                break;
            case TetriminoType.I:
                posInMatrix = new int[4, 2]
                {
                    {x, y},
                    {x + 1, y},
                    {x + 2, y},
                    {x + 3, y}
                };
                break;

        }
        return posInMatrix;
    }*/
}

public struct MatCoor
{
    public int x;
    public int y;
    public MatCoor(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
    public MatCoor(MatCoor a, int x, int y)
    {
        this.x = a.x + x;
        this.y = a.y + y;
    }
    public MatCoor Rotate90(bool isClockWise)
    {
        if (!isClockWise)
            return new MatCoor(-this.y, this.x);
        else
            return new MatCoor(this.y, -this.x);
    }
    public static MatCoor operator +(MatCoor a, MatCoor b)
    {
        return new MatCoor(a.x + b.x, a.y + b.y);
    }
    public static MatCoor operator -(MatCoor a, MatCoor b)
    {
        return new MatCoor(a.x - b.x, a.y - b.y);
    }
    public void Set(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
    public void Set(MatCoor b)
    {
        this.x = b.x;
        this.y = b.y;
    }
    public override string ToString()
    {
        return $"x: {this.x}, y: {this.y}";
    }
}
public enum TetriminoType
{
    O,
    I,
    T,
    L,
    J,
    S,
    Z,
}
/// <summary>
/// the _state of a single block in the Matrix, either empty, occupied or being used by a moving tetrimino
/// </summary>
public enum BlockState
{
    active,
    available,
    locked,
    ghost,
}
/// <summary>
/// _state of the game
/// </summary>
public enum GameState
{
    defaultPhase,
    generationPhase,
    fallingPhase,
    lockPhase,
    patternPhase,
    eliminationPhase,
    completionPhase,
    welcomePhase,
    pausePhase
}
public enum RotationState
{
    north,
    east,
    south,
    west
}