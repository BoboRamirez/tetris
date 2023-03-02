using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatrixManager : MonoBehaviour
{
    public static MatrixManager manager;
    public BlockState[,] matrix = new BlockState[10, 22];
    [SerializeField] private GameObject mino;
    private GameObject[,] minos = new GameObject[10, 20];

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
                matrix[i, j] = BlockState.available;
                minos[i, j] = Instantiate(mino, new Vector2(0.5f + c.x, 0.5f + c.y), Quaternion.identity);
                minos[i, j].
            }
                
    }
    
    /// <summary>
    /// make blocks of given coordinate available
    /// </summary>
    /// <param name="m">coordinates of blocks to be cleared</param>
    public void ClearBlocks(MatCoor[] m)
    {
        foreach(MatCoor c in m)
        {
            matrix[c.x, c.y] = BlockState.available;
        }
        return;
    }
    /// <summary>
    /// update state of tetrimino into matrix
    /// </summary>
    /// <param name="m">coordinate of the tetrimino to be updated</param>
    /// <exception cref="System.ArgumentException">length of m is not 4</exception>
    /// <exception cref="System.Exception">some block is occupied</exception>
    public void UpdateTetrimino(MatCoor[] m)
    {
        if(m.Length != 4)
        {
            Debug.LogError("Wrong Coordinate Length!");
            return;
        }
        for(int i = 0; i < 4; i++)
        {
            if(matrix[m[i].x, m[i].y] == BlockState.available)
                matrix[m[i].x, m[i].y] = (BlockState)i;
            else
            {
                Debug.LogError("Wrong coordinate: already occupied");
            }
        }
        return ;
    }
    public GameObject GenerateMinos(MatCoor c)
    {
        return Instantiate(mino, new Vector2(0.5f + c.x, 0.5f + c.y), Quaternion.identity);
    }
}
