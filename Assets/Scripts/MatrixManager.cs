using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class MatrixManager : MonoBehaviour
{
    public static MatrixManager manager;
    [SerializeField]
    private GameObject mino;
    public Mino[,] minos = new Mino[10, 22];
    private GameObject tempMino;

    private void Awake()
    {
        if (manager == null)
        {
            manager = this;
            DontDestroyOnLoad(this);
        }
        else if (manager != this)
            Destroy(gameObject);
    }
    public void InitializeMatrix()
    {
        for(int i = 0; i < 10; i++)
            for(int j = 0; j < 22; j++)
            {
                tempMino = Instantiate(mino, new Vector2(0.5f + i, 0.5f + j), Quaternion.identity);
                minos[i, j] = tempMino.GetComponent<Mino>();
                minos[i, j].Initialized(i, j);
            }
    }
    
    /// <summary>
    /// make blocks of given coordinate available
    /// </summary>
    /// <param name="m">coordinates of blocks to be cleared</param>
    public void ClearBlocks(MatCoor[] m)
    {
        foreach(MatCoor c in m)
            minos[c.x, c.y].SetState(BlockState.available);
        return;
    }
    /// <summary>
    /// update _state of tetrimino into matrix
    /// </summary>
    /// <param name="m">coordinate of the tetrimino to be updated</param>
    /// <exception cref="System.ArgumentException">length of m is not 4</exception>
    /// <exception cref="System.Exception">some block is occupied</exception>
    public void ShowTetriminoBlocks(MatCoor[] m, TetriminoType t)
    {
        if (m.Length != 4)
        {
            Debug.LogError("wrong coordinate array");
            return;
        }
        for (int i = 0; i < 4; i++)
        {
            if (minos[m[i].x, m[i].y].State == BlockState.available || minos[m[i].x, m[i].y].State == BlockState.ghost)
            {
                minos[m[i].x, m[i].y].SetState(BlockState.active, t);
            }
            else
                Debug.LogError($"Wrong coordinate: ({m[i].x}:{m[i].y}) already occupied");
        }
        return ;
    }
/*    /// <summary>
    /// Generate minoCoordinates. Used only in initialization.
    /// </summary>
    /// <param name="c">Target Coordinate</param>
    /// <returns>Generated Mino</returns>
    private GameObject GenerateMinos(MatCoor c)
    {
        return Instantiate(mino, new Vector2(0.5f + c.x, 0.5f + c.y), Quaternion.identity);
    }*/
    /// <summary>
    /// See if block in MatCoor c is available
    /// </summary>
    /// <param name="c">coordinate of target block</param>
    /// <returns>true if available</returns>
    public bool JudgeAvailability(MatCoor c)
    {
        bool isVacant = true;
        if (c.x < 0 || c.x > 9 || c.y < 0 || c.y > 21 || minos[c.x, c.y].State != BlockState.available)
            isVacant = false;
        return isVacant;
    }

    public bool LockTetriminoBlocks(MatCoor[] m, TetriminoType t)
    {
        if (m.Length != 4)
        {
            Debug.LogError("wrong coordinate array");
            return false;
        }
        foreach (MatCoor c in m)
        {
            if (c.x < 0 || c.x >= 10 || c.y < 0)
            {
                Debug.LogError($"invalid lock coodinidate: {c}");
                return false;
            }
            //out of bound, game over
            else if (c.y >= 20)
            {
                return false;
            }
            minos[c.x, c.y].SetState(BlockState.locked, t);
        }
        return true;
    }
    /// <summary>
    /// match pattern and clean lines
    /// </summary>
    public void MatchPatternAndEliminate()
    {
        int x;
        for (int y = 0; y <20; y++)
        {
            for (x = 0; x < 10;  x++)
            {
                if (minos[x, y].State != BlockState.locked)
                    break;
            }
            if (x >= 10)
            {
                for (int z = y; z < 20; z++)
                    for (x = 0; x < 10; x++)
                    {
                        //Debug.Log($"({x}, {z}) get {minos[x, z + 1].State} and {minos[x, z + 1].Type} ");
                        minos[x, z].SetState(minos[x, z + 1].State, minos[x, z + 1].Type);
                    }
                y--;
            }
        }
        
        return;
    }

    public void ChannelGhostPiece(MatCoor[] m)
    {
        foreach (MatCoor c in m)
        {
            if (minos[c.x, c.y].State == BlockState.available)
                minos[c.x, c.y].SetState(BlockState.ghost);
        }
    }
}
