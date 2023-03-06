using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatrixManager : MonoBehaviour
{
    public static MatrixManager manager;
    [SerializeField] private GameObject mino;
    public Mino[,] minos = new Mino[10, 20];
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
            minos[c.x, c.y].setState(BlockState.available);
        return;
    }
    /// <summary>
    /// update _state of tetrimino into matrix
    /// </summary>
    /// <param name="m">coordinate of the tetrimino to be updated</param>
    /// <exception cref="System.ArgumentException">length of m is not 4</exception>
    /// <exception cref="System.Exception">some block is occupied</exception>
    public void ShowBlocks(MatCoor[] m)
    {
        for (int i = 0; i < 4; i++)
        {
            if (minos[m[i].x, m[i].y].State == BlockState.available)
            {
                minos[m[i].x, m[i].y].setState((BlockState) i);
            }
            else
                Debug.LogError($"Wrong coordinate: ({m[i].x}:{m[i].y}) already occupied");
        }
        return ;
    }
    /// <summary>
    /// Generate minoCoordinates. Used only in initialization.
    /// </summary>
    /// <param name="c">Target Coordinate</param>
    /// <returns>Generated Mino</returns>
    public GameObject GenerateMinos(MatCoor c)
    {
        return Instantiate(mino, new Vector2(0.5f + c.x, 0.5f + c.y), Quaternion.identity);
    }
}
