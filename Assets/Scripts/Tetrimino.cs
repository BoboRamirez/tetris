using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetrimino : MonoBehaviour
{
    public RotationState orientation = RotationState.north;
    [SerializeField]
    public TetriminoType type = TetriminoType.O;
    public MatCoor[] minoCoordinates = new MatCoor[4];
    private int rotationCenter = 0;
    private Dictionary<RotationState, MatCoor[]> rotationOffset;
    private MatrixManager matrix;
    private bool _isActive = false;
    public bool IsActive
    {
        get { return _isActive; }
    }
    private float _lockTimer = Data.defaultLockTime;
    public float LockTimer
    {
        get => _lockTimer;
    }
    private int _operationCounter = 15;
    public int OperationCounter
    {
        get => _operationCounter;
    }
    private int _diveDepth;
    private int CurDepth
    {
        get 
        {
            int min = 30;
            foreach (MatCoor c in minoCoordinates)
            {
                if (c.y < min) min = c.y;
            }
            return min;
        }
    }

    private void Start()
    {
        matrix = MatrixManager.manager;
        Console.WriteLine(matrix.ToString());
    }
    public void InitializeTetrimino(TetriminoType t)
    {
        type = t;
        orientation = RotationState.north;
        Data.spawnLocation[type].CopyTo(minoCoordinates, 0);
        rotationOffset = Data.GetOffsetData(t);
        rotationCenter = Data.rotationCenterMap[t];
        matrix.ShowTetriminoBlocks(minoCoordinates);
        _isActive = true;
        _lockTimer = Data.defaultLockTime;
        _operationCounter = 15;
        _diveDepth = 30;
    }
    /// <summary>
    /// get matrix location of drop spot, or ghost piece,  of current tetrimino
    /// </summary>
    /// <returns>location of all 4 minoCoordinates in matrix</returns>
    public MatCoor[] GetDropSpot()
    {
        int dropDepth;
        bool doesHit = false;
        dropDepth = -1;
        while (!doesHit)
        {
            dropDepth++;
            foreach (MatCoor c in minoCoordinates)
            {
                if (c.y - dropDepth -1 < 0 || matrix.minos[c.x, c.y - dropDepth - 1].State == BlockState.locked)
                {
                    doesHit = true; break;
                }
            }
        }
        return new MatCoor[]
        {
            new MatCoor(minoCoordinates[0], 0, -dropDepth),
            new MatCoor(minoCoordinates[1], 0, -dropDepth),
            new MatCoor(minoCoordinates[2], 0, -dropDepth),
            new MatCoor(minoCoordinates[3], 0, -dropDepth),
        };
    }
    public bool HasSpaceToFall()
    {
        for (int i = 0; i < 4; i++)
            if (minoCoordinates[i].y - 1 < 0 || matrix.minos[minoCoordinates[i].x, minoCoordinates[i].y - 1].State == BlockState.locked)
                return false;
        return true;
    }
    /// <summary>
    /// Make Tetrimino fall one block down. Need to judge if this Tetrimino has space to fall before use.
    /// </summary>
    public void Fall()
    {
        if (!HasSpaceToFall())
        {
            //Debug.LogError("Nowhere to fall at Fall()");
            return;
        }
        matrix.ClearBlocks(minoCoordinates);
        for (int i = 0; i < 4; i++)
            minoCoordinates[i].y--;
        matrix.ShowTetriminoBlocks(minoCoordinates);
        //post-fall process
        _lockTimer = Data.defaultLockTime;
        if (CurDepth < _diveDepth)
        {
            _operationCounter = 15;
            _diveDepth = CurDepth;
        }
    }
    public void HardDrop()
    {
        /*Debug.Log("on HardDrop:");
        PrintMinoCoordinates();*/
        matrix.ClearBlocks(minoCoordinates);
        minoCoordinates = GetDropSpot();
/*        Debug.Log("drop Spot:");
        PrintMinoCoordinates();*/
        matrix.ShowTetriminoBlocks(minoCoordinates);
        //gameObject.transform.position = Data.GetUnityPos(orientation, minoCoordinates[0]);
    }
    /// <summary>
    /// lock the current tetrimino. if true, then check for elimination and game continues; else, that means some mino went wild in this tetrimino and game over
    /// </summary>
    /// <returns>if game continues</returns>
    public bool Lock()
    {
        bool doseContinue;
        //Debug.LogWarning("locked!");
        _isActive = false;
        _lockTimer = Data.defaultLockTime;
        _operationCounter = 15;
        doseContinue = matrix.LockTetriminoBlocks(minoCoordinates);
        if (doseContinue) 
        {
            matrix.MatchPatternAndEliminate();
        }
        return doseContinue;
    }
    public void Rotate(bool isClockwise)
    {
        RotationState newOrientation = orientation;
        Dictionary<RotationState, MatCoor[]> offsetData = Data.GetOffsetData(type);
        MatCoor offset;
        MatCoor[] tempCoor = new MatCoor[4];
        bool doInterfere = false;

        matrix.ClearBlocks(minoCoordinates);
        newOrientation += isClockwise ? 1 : -1;
        if (newOrientation > RotationState.west)
            newOrientation = RotationState.north;
        else if (newOrientation < RotationState.north)
            newOrientation = RotationState.west;

        for (int i = 0; i < offsetData[newOrientation].Length; i++)
        {
            offset = offsetData[orientation][i] - offsetData[newOrientation][i];
            doInterfere = false;
            for (int j = 0; j < 4; j++)
            {
                tempCoor[j] = (minoCoordinates[j] - minoCoordinates[rotationCenter]).Rotate90(isClockwise) + minoCoordinates[rotationCenter] + offset;
                doInterfere = !matrix.JudgeAvailability(tempCoor[j]);
                //not working, not this offset, keep looking
                if (doInterfere) break;
            }
            //got it, CAN rotate, update EVERYTHING!
            if (!doInterfere)
            {
                orientation = newOrientation;
                minoCoordinates = tempCoor;
                matrix.ShowTetriminoBlocks(minoCoordinates);
                _lockTimer = Data.defaultLockTime;
                _operationCounter--;
                return;
            }
        }
        //cannot rotate, not this time, show the old Tetrimino
        if (doInterfere)
        {
            matrix.ShowTetriminoBlocks(minoCoordinates);
        }
        return;
    }

    public void Move(bool isLeft)
    {
        MatCoor[] newCoor = new MatCoor[4];
        if (isLeft)
        {
            for (int i = 0; i < 4; i++)
            {
                if (minoCoordinates[i].x <= 0 || matrix.minos[minoCoordinates[i].x - 1, minoCoordinates[i].y].State == BlockState.locked)
                    return;
                newCoor[i] = new MatCoor(minoCoordinates[i], -1, 0);
            }
        }
        else
        {
            for (int i = 0; i < 4; i++)
            {
                if (minoCoordinates[i].x >= 9 || matrix.minos[minoCoordinates[i].x + 1, minoCoordinates[i].y].State == BlockState.locked)
                    return;
                newCoor[i] = new MatCoor(minoCoordinates[i], 1, 0);
            }
        }
        matrix.ClearBlocks(minoCoordinates);
        minoCoordinates = newCoor;
        matrix.ShowTetriminoBlocks(minoCoordinates);
        _lockTimer = Data.defaultLockTime;
        _operationCounter--;
    }
    /// <summary>
    /// get matrix positions of Mino No.1, 2, 3 in matrix from matrix position of Mino No.0
    /// </summary>
    /// <param name="r">Orientation</param>
    /// <param name="p">minoCoordinates of Mino No.0</param>
    /// <returns>matrix position of all Minos, including Mino No.0 </returns>
    public void LockTimerCountDown()
    {
         _lockTimer -= Time.deltaTime;
    }
    public void PrintMinoCoordinates()
    {
        foreach (MatCoor c in minoCoordinates)
            Debug.Log(c);
    }
    /// <summary>
    /// deactivate the current tetrimino. used for exchange holding zone
    /// </summary>
    public void EraseTetrimino()
    {
        matrix.ClearBlocks(minoCoordinates);
        _isActive = false;
    }
}