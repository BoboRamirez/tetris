using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private System.Random rand = new((int)System.DateTime.Now.Ticks);
    private readonly TetriminoType[] spawnList = new TetriminoType[Data.TetriminoCount];
    /// <summary>
    /// mapping from tetriminoType to gameObject
    /// </summary>
    //Dictionary<TetriminoType, GameObject> instanceMap = new Dictionary<TetriminoType, GameObject>();
    [SerializeField]    private Tetrimino tetrimino;
    /// <summary>
    /// No. of Tetrimino to spawn in the spawnList
    /// </summary>
    private int cur;
    [SerializeField] private NextInLineControl next;
    private void Start()
    {
        int i = 0;
        foreach (var item in System.Enum.GetValues(typeof(TetriminoType)))
            spawnList[i++] = (TetriminoType)item;
        //instanceMap = BuildTetriminoDictionary(spawnList, tetriminoInstances);
        ShuffleSpawnList();
    }
    /// <summary>
    /// shuffle spawnList and reset cur
    /// </summary>
    public void ShuffleSpawnList()
    {
        int n = Data.TetriminoCount, k;
        TetriminoType tmp;
        cur = 0;
        while (n > 1)
        {
            k = rand.Next(n--);
            tmp = spawnList[k];
            spawnList[k] = spawnList[n];
            spawnList[n] = tmp;
        }
    }
    /// <summary>
    /// Spawn the next tetrimino on the list. If the list is ending, shuffle.
    /// </summary>
    public void Spawn()
    {
        tetrimino.InitializeTetrimino(spawnList[cur]);
        //Debug.Log(spawnList[cur]);
        cur++;
        if (cur >= Data.TetriminoCount)
            ShuffleSpawnList();
        next.UpdateNextZone(spawnList[cur]);
    }

}


//fuck Tetris Guidelines! https://harddrop.com/wiki/SRS#How_Guideline_SRS_Really_Works is the real deal about SRS.
