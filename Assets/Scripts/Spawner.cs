using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private System.Random rand = new((int)System.DateTime.Now.Ticks);
    /// <summary>
    /// incoming tetrimino gameObjects, in the order of OITLJSZ
    /// </summary>
    [SerializeField]
    private GameObject tetriminoControl;
    /// <summary>
    /// all types in order, shuffled later
    /// </summary>
    private readonly TetriminoType[] spawnList = new TetriminoType[Data.TetriminoCount];
    /// <summary>
    /// mapping from tetriminoType to gameObject
    /// </summary>
    //Dictionary<TetriminoType, GameObject> instanceMap = new Dictionary<TetriminoType, GameObject>();
    private Tetrimino tetrimino;
    /// <summary>
    /// No. of Tetrimino to spawn in the spawnList
    /// </summary>
    private int cur;
    private NextInLineControl next;
    private void Start()
    {
        int i = 0;
        foreach (var item in System.Enum.GetValues(typeof(TetriminoType)))
            spawnList[i++] = (TetriminoType)item;
        //instanceMap = BuildTetriminoDictionary(spawnList, tetriminoInstances);
        ShuffleSpawnList();
        tetrimino = tetriminoControl.GetComponent<Tetrimino>();
        next = GetComponentInChildren<NextInLineControl>();
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
        Debug.Log(spawnList[cur]);
        cur++;
        if (cur >= Data.TetriminoCount)
            ShuffleSpawnList();
        next.UpdateNextZone(spawnList[cur]);
    }
    /*private Dictionary<TetriminoType, GameObject> BuildTetriminoDictionary(TetriminoType[] tList, GameObject[] goList)
    {
        var d = new Dictionary<TetriminoType, GameObject>();
        if (tList.Length != Data.TetriminoCount || goList.Length != Data.TetriminoCount)
            return null;
        for (int i = 0; i < Data.TetriminoCount; i++)
        {
            d.Add(tList[i], goList[i]);
        }
        return d;
    }*/
    

}


//fuck Tetris Guidelines! https://harddrop.com/wiki/SRS#How_Guideline_SRS_Really_Works is the real deal about SRS.
