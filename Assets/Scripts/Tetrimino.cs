using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tetrimino : MonoBehaviour
{
    protected GameObject tetriminoPiece = null;
    protected RotationState orientation = RotationState.north;
    protected TetriminoType type = TetriminoType.O;
    protected MatCoor[] minos;
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
    /*
    protected int LeftBorder
    {
        get 
        {
            int min = 9;
            for (int i = 0; i < 4; i++)
            {
                if (minos[i,0] < min)
                    min = minos[i,0];
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
                if (minos[i, 0] > max)
                    max = minos[i, 0];
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
                if (minos[i, 1] < min)
                    min = minos[i, 1];
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
        minos = GetMinoPos(RotationState.north, Data.spawnLocation[type]);
        rotationOffset = Data.GetOffsetData(t);
        rotationCenter = Data.rotationCenterMap[t];
        tetriminoPiece = this.gameObject;
    }

    /// <summary>
    /// get matrix location of drop spot, or ghost piece,  of current tetrimino
    /// </summary>
    /// <returns>location of all 4 minos in matrix</returns>
    public MatCoor[] GetDropSpot()
    {

        BlockState below;
        List<int> bottomList = new();
        int dropDepth = 0;
        bool doesHit = false;
        for (int i = 0; i < 4; i++)
            if (minos[i].y - 1 < 0) return minos;
        for (int i = 0; i < 4; i++)
        {
            below = MatrixManager.manager.matrix[minos[i].x, minos[i].y - 1];
            if (below < BlockState.active3) continue;
            else if (below == BlockState.locked) return minos;
            else bottomList.Add(i);
        }

        while (!doesHit)
        {
            foreach (int i in bottomList)
            {
                dropDepth++;
                if (minos[i].y - dropDepth - 1 < 0 || MatrixManager.manager.matrix[minos[i].x, minos[i].y - dropDepth - 1] == BlockState.locked)
                    doesHit = true;
            }
        }

        return new MatCoor[]
        {
            new MatCoor(minos[0], 0, -dropDepth),
            new MatCoor(minos[1], 0, -dropDepth),
            new MatCoor(minos[2], 0, -dropDepth),
            new MatCoor(minos[3], 0, -dropDepth),
        };
    }
    public bool HasSpaceToFall()
    {
        for (int i = 0; i < 4; i++)
            if (minos[i].y - 1 < 0) 
                return false;
        for (int i = 0; i < 4; i++)
            if (MatrixManager.manager.matrix[minos[i].x, minos[i].y - 1] == BlockState.locked) 
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
            MatrixManager.manager.matrix[minos[i].x, minos[i].y] = BlockState.available;
        for (int i = 0; i < 4; i++)
            MatrixManager.manager.matrix[minos[i].x, minos[i].y - 1] = (BlockState) i;
        for (int i = 0; i < 4; i++)
            minos[i].y--;
        tetriminoPiece.transform.position -= Vector3.down;
    }
    public void HardDrop()
    {
        MatrixManager.manager.ClearBlocks(minos);
        minos = GetDropSpot();
        MatrixManager.manager.UpdateTetrimino(minos);
        tetriminoPiece.transform.position = Data.GetUnityPos(orientation, minos[0]);
    }
    public void LockAndDestroy()
    {
        foreach (var block in minos)
        {
            MatrixManager.manager.matrix[block.x, block.y] = BlockState.locked;
            MatrixManager.manager.GenerateMinos();
        }
        
    }
    /// <summary>
    /// get matrix positions of Mino No.1, 2, 3 in matrix from matrix position of Mino No.0
    /// </summary>
    /// <param name="r">Orientation</param>
    /// <param name="p">minos of Mino No.0</param>
    /// <returns>matrix position of all Minos, including Mino No.0 </returns>
    public abstract MatCoor[] GetMinoPos(RotationState r, MatCoor p);
    public abstract void Rotate(bool isClockwise);
}