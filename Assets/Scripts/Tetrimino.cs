using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetrimino : MonoBehaviour
{
    public RotationState orientation = RotationState.north;
    [SerializeField]
    public TetriminoType type = TetriminoType.O;
    public MatCoor[] minoCoordinates;
    private int rotationCenter = 0;
    private Dictionary<RotationState, MatCoor[]> rotationOffset;
    private readonly MatrixManager matrix = MatrixManager.manager;
    private bool _isActive = false;
    public bool IsActive
    {
        get { return _isActive; }
    }

    private void Update()
    {
        //not even wrong!!!
        /*if (fallCounter >= FallDelay)
        {
            fallCounter -= FallDelay;
            Fall();
            lockTimer = .5f;
        }
        else
        { 
            fallCounter += Time.deltaTime; 
            lockTimer -= Time.deltaTime;
        }*/
    }
    public void InitializeTetrimino(TetriminoType t)
    {
        type = t;
        orientation = RotationState.north;
        minoCoordinates = Data.spawnLocation[type];
        rotationOffset = Data.GetOffsetData(t);
        rotationCenter = Data.rotationCenterMap[t];
        matrix.ShowTetriminoBlocks(minoCoordinates);
        _isActive = true;
    }

    /// <summary>
    /// get matrix location of drop spot, or ghost piece,  of current tetrimino
    /// </summary>
    /// <returns>location of all 4 minoCoordinates in matrix</returns>
    public MatCoor[] GetDropSpot()
    {

        BlockState below;
        List<int> bottomList = new();
        int dropDepth = 0;
        bool doesHit = false;
        for (int i = 0; i < 4; i++)
            if (minoCoordinates[i].y - 1 < 0) return minoCoordinates;
        for (int i = 0; i < 4; i++)
        {
            below = matrix.minos[minoCoordinates[i].x, minoCoordinates[i].y - 1].State;
            if (below < BlockState.active3) continue;
            else if (below == BlockState.locked) return minoCoordinates;
            else bottomList.Add(i);
        }

        while (!doesHit)
        {
            foreach (int i in bottomList)
            {
                dropDepth++;
                if (minoCoordinates[i].y - dropDepth - 1 < 0 || matrix.minos[minoCoordinates[i].x, minoCoordinates[i].y - dropDepth - 1].State == BlockState.locked)
                    doesHit = true;
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
            Debug.LogError("Nowhere to fall at Fall()");
            return;
        }
        matrix.ClearBlocks(minoCoordinates);
        for (int i = 0; i < 4; i++)
            minoCoordinates[i].y--;
        matrix.ShowTetriminoBlocks(minoCoordinates);
    }
    public void HardDrop()
    {
        matrix.ClearBlocks(minoCoordinates);
        minoCoordinates = GetDropSpot();
        matrix.ShowTetriminoBlocks(minoCoordinates);
        //gameObject.transform.position = Data.GetUnityPos(orientation, minoCoordinates[0]);
    }
    //need further attention
    public void Lock()
    {
        _isActive = false;
        matrix.LockTetriminoBlocks(minoCoordinates);
        //should spawn next, or see if game is over?
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
    /// <summary>
    /// get matrix positions of Mino No.1, 2, 3 in matrix from matrix position of Mino No.0
    /// </summary>
    /// <param name="r">Orientation</param>
    /// <param name="p">minoCoordinates of Mino No.0</param>
    /// <returns>matrix position of all Minos, including Mino No.0 </returns>
    public MatCoor[] GetMinoPos(RotationState r, MatCoor p)
    {
        return null;
    }
}