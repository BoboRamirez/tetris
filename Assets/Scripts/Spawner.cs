using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner
{
    private System.Random rand = new((int)System.DateTime.Now.Ticks);
    /// <summary>
    /// incoming tetrimino gameObjects, in the order of OITLJSZ
    /// </summary>
    [SerializeField]
    private GameObject[] tetriminoInstances;
    /// <summary>
    /// all types in order, shuffled later
    /// </summary>
    private TetriminoType[] types2Spawn = new TetriminoType[Data.TetriminoCount];
    /// <summary>
    /// mapping from tetriminoType to gameObject
    /// </summary>
    Dictionary<TetriminoType, GameObject> instanceMap = new Dictionary<TetriminoType, GameObject>();
    /// <summary>
    /// No. of Tetrimino to spawn in the types2Spawn
    /// </summary>
    private int cur;
    public Spawner()
    {
        int i = 0;
        foreach (var item in System.Enum.GetValues(typeof(TetriminoType)))
            types2Spawn[i++] = (TetriminoType) item;
        instanceMap = BuildTetriminoDictionary(types2Spawn, tetriminoInstances);
        ShuffleSpawnList();
        cur = 0;
    }
    /// <summary>
    /// shuffle types2Spawn and reset cur
    /// </summary>
    public void ShuffleSpawnList()
    {
        int n = Data.TetriminoCount, k;
        TetriminoType tmp;
        cur = 0;
        while (n > 1)
        {
            k = rand.Next(n--);
            tmp = types2Spawn[k];
            types2Spawn[k] = types2Spawn[n];
            types2Spawn[n] = tmp;
        }
    }
    /// <summary>
    /// Spawn the next tetrimino on the list. If the list is ending, shuffle.
    /// </summary>
    public void Spawn(TetriminoType t)
    {
        /*
        if (cur >= Data.TetriminoCount)
            throw new System.ArgumentOutOfRangeException("cur");*/
        
        Instantiate(instanceMap[t], new Vector2(Data.spawnLocation[t].x, Data.spawnLocation[t].y), Quaternion.identity);
        cur++;
        if (cur >= Data.TetriminoCount)
            ShuffleSpawnList();
    }


    private Dictionary<TetriminoType, GameObject> BuildTetriminoDictionary(TetriminoType[] tList, GameObject[] goList)
    {
        var d = new Dictionary<TetriminoType, GameObject>();
        if (tList.Length != Data.TetriminoCount || goList.Length != Data.TetriminoCount)
            return null;
        for (int i = 0; i < Data.TetriminoCount; i++)
        {
            d.Add(tList[i], goList[i]);
        }
        return d;
    }


}


//fuck Tetris Guidelines! https://harddrop.com/wiki/SRS#How_Guideline_SRS_Really_Works is the real deal about SRS.
