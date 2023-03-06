using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tetrimino : MonoBehaviour
{
    protected RotationState orientation = RotationState.north;
    protected TetriminoType type = TetriminoType.O;
    protected MatCoor[] minoCoordinates;
    protected int rotationCenter = 0;
    protected Dictionary<RotationState, MatCoor[]> rotationOffset;
    protected float fallCounter = 0;
    protected float lockTimer = 0.5f;
    protected float FallDelay
    {
        get
        {
            return Data.fallDelay[GameManager.manager.difficulty];
        }
    }
    private readonly MatrixManager matrix = MatrixManager.manager;
    /*
    protected int LeftBorder
    {
        get 
        {
            int min = 9;
            for (int i = 0; i < 4; i++)
            {
                if (minoCoordinates[i,0] < min)
                    min = minoCoordinates[i,0];
            }
            return min;
        }
    }

    protected int RightBorder
    {
        get
        {
            int max = 0;
            for (int i = 0; i < 4; i++)
            {
                if (minoCoordinates[i, 0] > max)
                    max = minoCoordinates[i, 0];
            }
            return max;
        }
    }
    
    protected int BottomBorder
    {
        get
        {
            int min = 9;
            for (int i = 0; i < 4; i++)
            {
                if (minoCoordinates[i, 1] < min)
                    min = minoCoordinates[i, 1];
            }
            return min;
        }
    }*/

    private void Update()
    {
        //not even wrong!!!
        if (fallCounter >= FallDelay)
        {
            fallCounter -= FallDelay;
            Fall();
            lockTimer = .5f;
        }
        else
        { 
            fallCounter += Time.deltaTime; 
            lockTimer -= Time.deltaTime;
        }
    }

    protected void InitializeTetrimino(TetriminoType t)
    {
        type = t;
        orientation = RotationState.north;
        minoCoordinates = GetMinoPos(RotationState.north, Data.spawnLocation[type]);
        rotationOffset = Data.GetOffsetData(t);
        rotationCenter = Data.rotationCenterMap[t];
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
            if (minoCoordinates[i].y - 1 < 0) 
                return false;
        for (int i = 0; i < 4; i++)
            if (matrix.minos[minoCoordinates[i].x, minoCoordinates[i].y - 1].State == BlockState.locked) 
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
        
        for (int i = 0; i < 4; i++)
            matrix.minos[minoCoordinates[i].x, minoCoordinates[i].y].setState(BlockState.available);
        for (int i = 0; i < 4; i++)
            matrix.minos[minoCoordinates[i].x, minoCoordinates[i].y - 1].setState((BlockState) i);
        for (int i = 0; i < 4; i++)
            minoCoordinates[i].y--;
        gameObject.transform.position -= Vector3.down;
    }
    public void HardDrop()
    {
        matrix.ClearBlocks(minoCoordinates);
        minoCoordinates = GetDropSpot();
        matrix.ShowBlocks(minoCoordinates);
        gameObject.transform.position = Data.GetUnityPos(orientation, minoCoordinates[0]);
    }
    public void Lock()
    {
        foreach (var block in minoCoordinates)
        {
            matrix.minos[block.x, block.y].setState(BlockState.locked);
        }
        
    }
    /// <summary>
    /// get matrix positions of Mino No.1, 2, 3 in matrix from matrix position of Mino No.0
    /// </summary>
    /// <param name="r">Orientation</param>
    /// <param name="p">minoCoordinates of Mino No.0</param>
    /// <returns>matrix position of all Minos, including Mino No.0 </returns>
    public abstract MatCoor[] GetMinoPos(RotationState r, MatCoor p);
    public abstract void Rotate(bool isClockwise);
}